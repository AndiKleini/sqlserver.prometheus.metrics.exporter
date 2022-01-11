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
            const int executionCount = 37;
            const int previousExecutionCount = 27;
            List<MetricItem> expectedItemsFromBuildMethod = 
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
            List<MetricItem> expectedItemsFromBuildDeltaMethod = 
                new List<MetricItem>()
                {  
                    new MetricItem() 
                    { 
                        Name = $"{storedProcedureName}_ExecutionCount", 
                        Value = executionCount - previousExecutionCount 
                    }
                };

            DateTime from = DateTime.Parse("2021-12-12 13:00");
            var mockeryPlanCache = new Mock<IPlanCacheRepository>();
            mockeryPlanCache
                .Setup(s => s.GetPlanCache(from))
                .Returns(new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt =  null,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = maxElapsedTime, Min = minElapsedTime, Last = lastElapsedTime }, 
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCount }
                        }
                    }
                });
            PlanCacheItem previousPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = storedProcedureName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    ElapsedTime = new ElapsedTime() { Max = maxElapsedTime, Min = minElapsedTime, Last = lastElapsedTime },
                    GeneralStats = new GeneralStats() { ExecutionCount = previousExecutionCount }
                }
            };
            mockeryPlanCache
                .Setup(s => s.GetPreviousPlanCacheItems())
                .Returns(new List<PlanCacheItem>()
                {
                    previousPlanCacheItem
                });
            IPlanCacheRepository planCacherepository = mockeryPlanCache.Object;
            var mockeryCombinedMetricsBuilder = new Mock<ICombinedMetricsBuilder>();
            mockeryCombinedMetricsBuilder
                .Setup(b => b.Build(It.IsAny<IGrouping<string, PlanCacheItem>>()))
                .Returns(expectedItemsFromBuildMethod);
            mockeryCombinedMetricsBuilder
                .Setup(s => s.BuildDeltas(It.IsAny<IGrouping<string, PlanCacheItem>>(), previousPlanCacheItem))
                .Returns(expectedItemsFromBuildDeltaMethod);
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository, mockeryCombinedMetricsBuilder.Object);

            var items = instanceUnderTest.Collect(from);

            items.Should().BeEquivalentTo(expectedItemsFromBuildMethod.Concat(expectedItemsFromBuildDeltaMethod));
        }
    }
}
