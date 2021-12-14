using NUnit.Framework;
using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using FluentAssertions.Collections;
using FluentAssertions;
using System;
using Moq;
using Sqlserver.Metrics.Provider;

namespace sqlserver.metrics.exporter.engine.tests
{
    [TestFixture]
    public class StoredProcedureMetricsProviderTest
    {
        [Test]
        public void Export_RetreieveStatisticsOnlyFromPlanCache_MetricsEmitted()
        {
            const string storedProcedureName = "MySp";
            const int executionCountValue = 12;
            List<MetricItem> expectedItems = new List<MetricItem>() { new MetricItem() { Name = $"{storedProcedureName}_ExecutionCount", Value = executionCountValue } };
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            DateTime to = DateTime.Parse("2021-12-12 13:05");
            var mockery = new Mock<IPlanCacheRepository>();
            mockery
                .Setup(s => s.GetPlanCache(from, to))
                .Returns(new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountValue }
                        }
                    }
                });
            IPlanCacheRepository planCacherepository = mockery.Object;
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository);

            var items = instanceUnderTest.Collect(from, to);

            items.Should().BeEquivalentTo(expectedItems);
        }

        [Test]
        public void Export_RetreieveStatisticsOnlyFrom_MetricsEmitted()
        {
            const string storedProcedureName = "MySp";
            const int executionCountValue = 12;
            List<MetricItem> expectedItems = new List<MetricItem>() { new MetricItem() { Name = $"{storedProcedureName}_ExecutionCount", Value = executionCountValue } };
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            DateTime to = DateTime.Parse("2021-12-12 13:05");
            var mockery = new Mock<IPlanCacheRepository>();
            mockery
                .Setup(s => s.GetPlanCache(from, to))
                .Returns(new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountValue }
                        }
                    }
                });
            IPlanCacheRepository planCacherepository = mockery.Object;
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository);

            var items = instanceUnderTest.Collect(from, to);

            items.Should().BeEquivalentTo(expectedItems);
        }
    }
}
