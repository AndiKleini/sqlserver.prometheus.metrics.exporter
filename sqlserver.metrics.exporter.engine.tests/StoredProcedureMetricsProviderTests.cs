using NUnit.Framework;
using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using FluentAssertions;
using System;
using Moq;
using System.Linq;
using Sqlserver.Metrics.Provider;
using Sqlserver.Metrics.Provider.Builder;

namespace sqlserver.metrics.exporter.engine.tests
{
    [TestFixture]
    public class StoredProcedureMetricsProviderTest
    {
        [Test]
        public void Export_RetrieveStatisticsOnlyFromCache_ReturnsMetrics()
        {
            const string storedProcedureName = "MySp";
            const int maxElapsedTime = 12;
            const int minElapsedTime = 2;
            const int lastElapsedTime = 6;
            List<MetricItem> expectedItems = 
                new List<MetricItem>() 
                { 
                    new MetricItem() 
                    { 
                        Name = $"{storedProcedureName}_ElapsedTimeMax", 
                        Value = maxElapsedTime 
                    },
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ElapsedTimeMin",
                        Value = minElapsedTime
                    }

                };
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            DateTime to = DateTime.Parse("2021-12-12 13:05");
            var mockeryPlanCache = new Mock<IPlanCacheRepository>();
            mockeryPlanCache
                .Setup(s => s.GetPlanCache(from, to))
                .Returns(new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt =  null,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = maxElapsedTime, Min = minElapsedTime, Last = lastElapsedTime }
                        }
                    }
                });
            IPlanCacheRepository planCacherepository = mockeryPlanCache.Object;
            var mockeryMetricsBuilder = new Mock<IMetricsBuilder>();
            mockeryMetricsBuilder
                .Setup(b => b.Build(It.IsAny<IGrouping<string, PlanCacheItem>>()))
                .Returns(expectedItems);
            var metricsBuilder = mockeryMetricsBuilder.Object;
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository, metricsBuilder);

            var items = instanceUnderTest.Collect(from, to);

            items.Should().BeEquivalentTo(expectedItems);
        }
    }
}
