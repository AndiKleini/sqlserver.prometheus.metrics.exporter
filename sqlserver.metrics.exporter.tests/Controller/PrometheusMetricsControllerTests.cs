using FluentAssertions;
using Moq;
using NUnit.Framework;
using Serilog;
using Sqlserver.Metrics.Exporter.Services;
using Sqlserver.Metrics.Provider;
using SqlServer.Metrics.Exporter.Controllers;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Exporter.Tests.Controller
{
    [TestFixture]
    public class PrometheusMetricsControllerTests
    {
        [Test]
        public async Task GetMetrics_MetricsAreEmittedByProvider_RetunsPrometheusFormat()
        {
            const string elapedTimeMaxName = "MySP_ElapsedTimeMax";
            const int elapsedTimeMaxValue = 1;
            const string logiocalReadsMaxName = "MySP_LogicalReadsMax";
            const int logicalReadsMaxValue = 2;
            const string maxSpillsName = "MySP_SpillsMax";
            const int maxSpillsValue = 3;
            const string prometheusFormatLineSeperator = "\n";
            var expectedMetricItems =
                $"{elapedTimeMaxName} {elapsedTimeMaxValue}" + prometheusFormatLineSeperator +
                $"{logiocalReadsMaxName} {logicalReadsMaxValue}" + prometheusFormatLineSeperator +
                $"{maxSpillsName} {maxSpillsValue}" + prometheusFormatLineSeperator;
            HistoricalFetch previousFetch = new HistoricalFetch() { LastFetchTime = DateTime.Now.AddMinutes(-5), IncludedHistoricalItemsUntil = DateTime.Now.AddMinutes(-6) };
            var providerMock = new Mock<IStoredProcedureMetricsProvider>();
            List<MetricItem> yieldMetricItems = new List<MetricItem>()
                {
                    new MetricItem() { Name = elapedTimeMaxName , Value = elapsedTimeMaxValue },
                    new MetricItem() { Name = logiocalReadsMaxName , Value = logicalReadsMaxValue },
                    new MetricItem() { Name = maxSpillsName , Value = maxSpillsValue },
                };
            DateTime includedHistoricalItemUntil = DateTime.Now.AddMinutes(-1);
            providerMock.Setup(
                s => s.Collect(previousFetch.LastFetchTime.Value, previousFetch.IncludedHistoricalItemsUntil.Value)).
                ReturnsAsync(new MetricsResult() { Items = yieldMetricItems, NewestHistoricalItemConsidered = includedHistoricalItemUntil });
            var lastFetchHistory = new Mock<ILastFetchHistory>();
            lastFetchHistory.Setup(s => s.GetPreviousFetch()).Returns(previousFetch);
            lastFetchHistory.Setup(s => s.SetPreviousFetchTo(It.Is<HistoricalFetch>(hist => hist.IncludedHistoricalItemsUntil == includedHistoricalItemUntil)));
            PrometheusMetricsController instanceUnderTest = CreateInstanceUnderTest(providerMock.Object, lastFetchHistory.Object);

            var metricsFormat = await instanceUnderTest.GetMetrics();

            metricsFormat.Should().Be(expectedMetricItems);
            lastFetchHistory.VerifyAll();
        }

       

        [Test]
        public async Task GetMetrics_FirstFetchAfterRestart_EmitsNoMetrics()
        {
            const string elapedTimeMaxName = "MySP_ElapsedTimeMax";
            const int elapsedTimeMaxValue = 1;
            const string logiocalReadsMaxName = "MySP_LogicalReadsMax";
            const int logicalReadsMaxValue = 2;
            const string maxSpillsName = "MySP_SpillsMax";
            const int maxSpillsValue = 3;
            var expectedMetricItems = String.Empty;
            var providerMock = new Mock<IStoredProcedureMetricsProvider>();
            List<MetricItem> yieldMetricItems = new List<MetricItem>()
                {
                    new MetricItem() { Name = elapedTimeMaxName , Value = elapsedTimeMaxValue },
                    new MetricItem() { Name = logiocalReadsMaxName , Value = logicalReadsMaxValue },
                    new MetricItem() { Name = maxSpillsName , Value = maxSpillsValue },
                };
            DateTime includedHistoricalItemUntil = DateTime.Now.AddMinutes(-1);
            providerMock.Setup(s => s.Collect(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new MetricsResult() { Items = yieldMetricItems, NewestHistoricalItemConsidered = includedHistoricalItemUntil });
            var lastFetchHistory = new Mock<ILastFetchHistory>();
            lastFetchHistory.Setup(s => s.GetPreviousFetch()).Returns(default(HistoricalFetch));
            lastFetchHistory.Setup(s => s.SetPreviousFetchTo(It.Is<HistoricalFetch>(hist => hist.IncludedHistoricalItemsUntil == includedHistoricalItemUntil)));
            var instanceUnderTest = CreateInstanceUnderTest(providerMock.Object, lastFetchHistory.Object);

            var metricsFormat = await instanceUnderTest.GetMetrics();

            metricsFormat.Should().Be(expectedMetricItems);
            lastFetchHistory.VerifyAll();
        }

        [Test]
        public async Task GetMetrics_NewestHistoricalItemConsideredNull_SetsNewestHistoricalItemConsideredToMaxDispatchLatency()
        {
            HistoricalFetch previousFetch = new HistoricalFetch() { LastFetchTime = DateTime.Now.AddMinutes(-5), IncludedHistoricalItemsUntil = DateTime.Now.AddMinutes(-6) };
            var providerMock = new Mock<IStoredProcedureMetricsProvider>();
            List<MetricItem> yieldMetricItems = new List<MetricItem>();
            DateTime? includedHistoricalItemUntilReturned = null;
            const int maximumDispatchLatency = -30;
            DateTime? includedHistoricalItemUntilCalculated = DateTime.Now.AddSeconds(maximumDispatchLatency);
            HistoricalFetch historicalFetchSupplied = null;
            providerMock.Setup(
                s => s.Collect(previousFetch.LastFetchTime.Value, previousFetch.IncludedHistoricalItemsUntil.Value)).
                ReturnsAsync(new MetricsResult() { Items = yieldMetricItems, NewestHistoricalItemConsidered = includedHistoricalItemUntilReturned });
            var lastFetchHistory = new Mock<ILastFetchHistory>();
            lastFetchHistory.Setup(s => s.GetPreviousFetch()).Returns(previousFetch);

            lastFetchHistory.Setup(s => s.SetPreviousFetchTo(It.IsAny<HistoricalFetch>())).Callback<HistoricalFetch>(h => historicalFetchSupplied = h);
            var instanceUnderTest = CreateInstanceUnderTest(providerMock.Object, lastFetchHistory.Object);

            var metricsFormat = await instanceUnderTest.GetMetrics();

            historicalFetchSupplied.Should().NotBeNull();
            historicalFetchSupplied.IncludedHistoricalItemsUntil.Should().BeCloseTo(includedHistoricalItemUntilCalculated.Value, TimeSpan.FromSeconds(2));
            lastFetchHistory.VerifyAll();
        }

        private static PrometheusMetricsController CreateInstanceUnderTest(IStoredProcedureMetricsProvider providerMock, ILastFetchHistory lastFetchHistory)
        {
            return new PrometheusMetricsController(providerMock, lastFetchHistory, new Mock<ILogger>().Object);
        }
    }
}
