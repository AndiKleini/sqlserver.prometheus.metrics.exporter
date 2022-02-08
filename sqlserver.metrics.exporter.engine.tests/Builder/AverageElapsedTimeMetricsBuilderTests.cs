using FluentAssertions;
using NUnit.Framework;
using Sqlserver.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Tests.Builder
{
    [TestFixture]
    public class AverageElapsedTimeMetricsBuilderTests
    {
        [Test]
        public void Build_HistoricalAndCachedFiguresForElapsedTimeSupplied_ReturnsAverageElapsedTimeMetricItems()
        {
            string storedProcedureName = "MySp";
            const int executionCountOfCache = 5;
            const int executionCountHistorical1 = 3;
            const int executionCountHistorical2 = 7;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            const int elapsedTimeOfItemInCache = 400;
            const int elapsedTimeOfHistoricalItem1 = 600;
            const int elapsedTimeOfHistoricalItem2 = 300;
            int averageElapsed =
                (
                    elapsedTimeOfHistoricalItem1 +
                    elapsedTimeOfItemInCache +
                    elapsedTimeOfHistoricalItem2
                ) 
                /
                (
                    executionCountHistorical1 +
                    executionCountHistorical2 +
                    executionCountOfCache
                );
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_AverageElapsedTime",
                        Value = averageElapsed
                    }
              };

            var groupedPlanCacheItems =
                (new List<PlanCacheItem>() {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = null,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountOfCache },
                            ElapsedTime = new ElapsedTime() { Total = elapsedTimeOfItemInCache }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountHistorical1 },
                            ElapsedTime = new ElapsedTime() { Total = elapsedTimeOfHistoricalItem1 }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountHistorical2 },
                            ElapsedTime = new ElapsedTime() { Total = elapsedTimeOfHistoricalItem2 }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            var instanceUnderTest = new AverageElapsedTimeMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
