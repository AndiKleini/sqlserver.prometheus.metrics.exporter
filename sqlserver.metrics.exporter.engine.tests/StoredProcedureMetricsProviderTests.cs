using NUnit.Framework;
using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using FluentAssertions;
using System;
using Moq;
using System.Linq;
using SqlServer.Metrics.Provider.Builder;
using System.Threading.Tasks;
using Sqlserver.Metrics.Provider;

namespace SqlServer.metrics.exporter.engine.tests
{
    [TestFixture]
    public class StoredProcedureMetricsProviderTest
    {
        [Test]
        public async Task Export_RetrieveStatisticsOnlyFromCache_ReturnsMetrics()
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
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            var mockeryPlanCache = new Mock<IPlanCacheRepository>();
            DateTime newestHistoricalItemRemovedAt = DateTime.Parse("2021-12-12 11:00");
            DateTime olderHistoricalItemRemovedAt = DateTime.Parse("2021-12-12 10:00");
            mockeryPlanCache
                .Setup(s => s.GetPlanCache(from))
                .ReturnsAsync(new List<PlanCacheItem>()
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
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = olderHistoricalItemRemovedAt,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = maxElapsedTime, Min = minElapsedTime, Last = lastElapsedTime },
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCount }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = newestHistoricalItemRemovedAt,
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
                .ReturnsAsync(new List<PlanCacheItem>()
                {
                    previousPlanCacheItem
                });
            IPlanCacheRepository planCacherepository = mockeryPlanCache.Object;
            var mockeryCombinedMetricsBuilder = new Mock<ICombinedMetricsBuilder>();
            mockeryCombinedMetricsBuilder
                .Setup(b => b.Build(It.IsAny<IGrouping<string, PlanCacheItem>>()))
                .Returns(expectedItemsFromBuildMethod);
            MetricsResult expectedResult = new MetricsResult() { Items = expectedItemsFromBuildMethod, NewestHistoricalItemConsidered = newestHistoricalItemRemovedAt };
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository, mockeryCombinedMetricsBuilder.Object);

            MetricsResult yieldResult = await instanceUnderTest.Collect(from);

            yieldResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task Export_RetrieveStatisticsOnlyFromCache_NoHistoricalItemsContained_ReturnsMetrics()
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
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            var mockeryPlanCache = new Mock<IPlanCacheRepository>();
            mockeryPlanCache
                .Setup(s => s.GetPlanCache(from))
                .ReturnsAsync(new List<PlanCacheItem>()
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
                .ReturnsAsync(new List<PlanCacheItem>()
                {
                    previousPlanCacheItem
                });
            IPlanCacheRepository planCacherepository = mockeryPlanCache.Object;
            var mockeryCombinedMetricsBuilder = new Mock<ICombinedMetricsBuilder>();
            mockeryCombinedMetricsBuilder
                .Setup(b => b.Build(It.IsAny<IGrouping<string, PlanCacheItem>>()))
                .Returns(expectedItemsFromBuildMethod);
            MetricsResult expectedResult = new MetricsResult() { Items = expectedItemsFromBuildMethod, NewestHistoricalItemConsidered = null };
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository, mockeryCombinedMetricsBuilder.Object);

            MetricsResult yieldResult = await instanceUnderTest.Collect(from);

            yieldResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
