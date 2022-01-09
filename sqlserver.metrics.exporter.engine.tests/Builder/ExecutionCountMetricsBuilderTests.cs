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
    public class ExecutionCountMetricsBuilderTests
    {
        [Test]
        public void Build_HistoricalAndCachedFiguresForExecutionCountSupplied_ReturnsExecutionCountMetricItems()
        {
            string storedProcedureName = "MySp";
            const int executionCountOfCache = 5;
            const int executionCountHistorical1 = 3;
            const int executionCountHistorical2 = 7;
            const int previousExecutionCount = 2;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            int overallExecutions = executionCountOfCache + executionCountHistorical1 + executionCountHistorical2 - previousExecutionCount;
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ExecutionCount",
                        Value = overallExecutions
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
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountOfCache }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountHistorical1 }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            GeneralStats = new GeneralStats() { ExecutionCount = executionCountHistorical2 }
                        }
                    }
                }).GroupBy(p => p.SpName).First();
            var previousPlanCachItem = 
                new PlanCacheItem()
                {
                    RemovedFromCacheAt = null,
                    SpName = storedProcedureName,
                    ExecutionStatistics = new ProcedureExecutionStatistics()
                    {
                        GeneralStats = new GeneralStats() { ExecutionCount = previousExecutionCount }
                    }
                };

            ExecutionCountMetricsBuilder instanceUnderTest = new ExecutionCountMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems, previousPlanCachItem);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
