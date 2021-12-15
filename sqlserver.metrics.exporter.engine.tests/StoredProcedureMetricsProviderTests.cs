using NUnit.Framework;
using SqlServer.Metrics.Provider;
using System.Collections.Generic;
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
                    /*
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ElapsedTimeLast",
                        Value = lastElapsedTime
                    },*/
                };
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            DateTime to = DateTime.Parse("2021-12-12 13:05");
            var mockery = new Mock<IPlanCacheRepository>();
            mockery
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
            IPlanCacheRepository planCacherepository = mockery.Object;
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository);

            var items = instanceUnderTest.Collect(from, to);

            items.Should().BeEquivalentTo(expectedItems);
        }

        [Test]
        public void Export_RetreieveStatisticsOfHistoricalItemsOnly_ReturnsMetrics()
        {
            const string encacheDateStringified = "2021-12-12 13:00:34";
            DateTime encachedDate = DateTime.Parse(encacheDateStringified);
            const string storedProcedureName = "MySp";
            const int maxElapsedTime = 12;
            List<MetricItem> expectedItems = new List<MetricItem>() { new MetricItem() { Name = $"{storedProcedureName}_ElapsedTimeMax", Value = maxElapsedTime } };
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            DateTime to = DateTime.Parse("2021-12-12 13:05");
            var mockery = new Mock<IPlanCacheRepository>();
            mockery
                .Setup(s => s.GetPlanCache(from, to))
                .Returns(new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = encachedDate,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                             ElapsedTime = new ElapsedTime() { Max = maxElapsedTime }
                        }
                    }
                }); ;
            IPlanCacheRepository planCacherepository = mockery.Object;
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository);

            var items = instanceUnderTest.Collect(from, to);

            // TODO find better assertion here
            items.Should().ContainEquivalentOf(expectedItems[0]);
        }

        [Test]
        public void Export_RetreieveStatisticsFromCachdeAndHistoricalItems_ReturnsAggregatedMetrics()
        {
            const string encacheDateStringified = "2021-12-12 13:00:34";
            DateTime encachedDate = DateTime.Parse(encacheDateStringified);
            const string storedProcedureName = "MySp";
            const int maxElapsedTime1 = 12;
            const int maxElapsedTime2 = 10;
            int maxExlapsedTime = Math.Max(maxElapsedTime1, maxElapsedTime2);
            List<MetricItem> expectedItems = 
                new List<MetricItem>() 
                { 
                    new MetricItem() 
                    { 
                        Name = $"{storedProcedureName}_ElapsedTimeMax", 
                        Value = maxExlapsedTime
                    } 
                };
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            DateTime to = DateTime.Parse("2021-12-12 13:05");
            var mockery = new Mock<IPlanCacheRepository>();
            mockery
                .Setup(s => s.GetPlanCache(from, to))
                .Returns(new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = encachedDate,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                             ElapsedTime = new ElapsedTime() { Max = maxElapsedTime1 }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt =  null,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = maxElapsedTime2 }
                        }
                    }
                }); ;
            IPlanCacheRepository planCacherepository = mockery.Object;
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository);

            var items = instanceUnderTest.Collect(from, to);

            // TODO find better assertion here
            items.Should().ContainEquivalentOf(expectedItems[0]);
        }
    }
}
