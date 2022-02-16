using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sqlserver.Metrics.Exporter;
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
            var providerMock = new Mock<IStoredProcedureMetricsProvider>();
            List<MetricItem> yieldMetricItems = new List<MetricItem>()
                {
                    new MetricItem() { Name = elapedTimeMaxName , Value = elapsedTimeMaxValue },
                    new MetricItem() { Name = logiocalReadsMaxName , Value = logicalReadsMaxValue },
                    new MetricItem() { Name = maxSpillsName , Value = maxSpillsValue },
                };
            providerMock.Setup(s => s.Collect(It.IsAny<DateTime>())).ReturnsAsync(yieldMetricItems);
            var lastFetchHistory = new Mock<ILastFetchHistory>();
            lastFetchHistory.Setup(s => s.GetPreviousFetchAndResetToNow()).Returns(DateTime.Now.AddMinutes(-5));
            var instanceUnderTest = new PrometheusMetricsController(providerMock.Object, lastFetchHistory.Object);

            var metricsFormat = await instanceUnderTest.GetMetrics();

            metricsFormat.Should().Be(expectedMetricItems);
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
            providerMock.Setup(s => s.Collect(It.IsAny<DateTime>())).ReturnsAsync(yieldMetricItems);
            var lastFetchHistory = new Mock<ILastFetchHistory>();
            lastFetchHistory.Setup(s => s.GetPreviousFetchAndResetToNow()).Returns(default(DateTime?));
            var instanceUnderTest = new PrometheusMetricsController(providerMock.Object, lastFetchHistory.Object);

            var metricsFormat = await instanceUnderTest.GetMetrics();

            metricsFormat.Should().Be(expectedMetricItems);
        }
    }
}
