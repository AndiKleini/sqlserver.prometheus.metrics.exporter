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
    public class GenericAverageMetricsBuilderTests
    {
        [Test]
        public void Build_MultipleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            const string metricsName = "ElapsedTimeAverage";
            string storedProcedureName = "MySp";
            const int totalElapsedTime1 = 100;
            const int totalElapsedTime2 = 200;
            const int totalElapsedTime3 = 300;
            const int executionCount1 = 2; 
            const int executionCount2 = 4; 
            const int executionCount3 = 5;
            int expectedAverageTime = 
                (totalElapsedTime1 + totalElapsedTime2 + totalElapsedTime3) /
                (executionCount1 + executionCount2 + executionCount3);
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_{metricsName}",
                        Value = expectedAverageTime
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
                            ElapsedTime = new ElapsedTime() { Total = totalElapsedTime1 },
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCount1 }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Total = totalElapsedTime2 },
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCount2 }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Total = totalElapsedTime3 },
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCount3 }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            GenericAverageMetricsBuilder instanceUnderTest = new GenericAverageMetricsBuilder(metricsName, p => p.ExecutionStatistics.ElapsedTime.Total);

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
