using FluentAssertions;
using Moq;
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
        public void Build_NoHistoricalItemButPlanCacheItemButPreviousPlanCacheItemAvailable_ReturnsDiffBetweenCurrentAndPreviousItem()
        {
            const string storedProcedureName = "SpName";
            const int executionCountOfCache = 5;
            const int previousExecutionCount = 2;
            int overallExecutions = executionCountOfCache - previousExecutionCount;
            PlanCacheItem currentPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = storedProcedureName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = executionCountOfCache }
                }
            };
            PlanCacheItem previousPlanCacheItem =
                new PlanCacheItem()
                {
                    RemovedFromCacheAt = null,
                    SpName = storedProcedureName,
                    ExecutionStatistics = new ProcedureExecutionStatistics()
                    {
                        GeneralStats = new GeneralStats() { ExecutionCount = previousExecutionCount }
                    }
                };
            var previousItemCache = new Mock<IPreviousItemCache>();
            previousItemCache.Setup(s => s.StorePreviousCacheItem(storedProcedureName, It.Is<PlanCacheItem>(item => item.ExecutionStatistics.GeneralStats.ExecutionCount == overallExecutions)));
            previousItemCache.Setup(s => s.GetPreviousCacheItem(storedProcedureName)).Returns(previousPlanCacheItem);
            var instanceUnderTest = new ExecutionCountMetricsBuilder(previousItemCache.Object);
            var groupedPlanCacheItems = (new List<PlanCacheItem>() { currentPlanCacheItem }).GroupBy(p => p.SpName).First();
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ExecutionCount",
                        Value = overallExecutions
                    }
              };

            IEnumerable<MetricItem> result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
            previousItemCache.VerifyAll();
        }

        [Test]
        public void Build_HistoricalItemAndNoPreviousPlanCacheItemAndNoCurrentPlacnCacheItemAvailable_ReturnsNoMetricResultStoresZeroItemAsPrevious()
        {
            const string storedProcedureName = "SpName";
            const int executionCountOfCache = 5;
            DateTime removedFromCacheAt = DateTime.Parse("2021-12-12 17:34:04");
            var historicalPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = removedFromCacheAt,
                SpName = storedProcedureName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = executionCountOfCache }
                }
            };
            var zeroPlanCacheItem = new PlanCacheItem() { ExecutionStatistics = new ProcedureExecutionStatistics() { GeneralStats = new GeneralStats() { ExecutionCount = 0 } } };
            var previousItemCache = new Mock<IPreviousItemCache>();
            previousItemCache.Setup(s => s.StorePreviousCacheItem(storedProcedureName, It.Is<PlanCacheItem>(item => item.ExecutionStatistics.GeneralStats.ExecutionCount == 0)));
            previousItemCache.Setup(s => s.GetPreviousCacheItem(storedProcedureName)).Returns(default(PlanCacheItem));
            var instanceUnderTest = new ExecutionCountMetricsBuilder(previousItemCache.Object);
            var groupedPlanCacheItems = (new List<PlanCacheItem>() { historicalPlanCacheItem }).GroupBy(p => p.SpName).First();

            IEnumerable<MetricItem> result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEmpty();
            previousItemCache.VerifyAll();
        }

        [Test]
        public void Build_HistoricalItemWithLastExecutionTimeAfterPreviousPlanCacheItem_ReturnsDiffOfPreviousPlanCacheItemAndCurrentPlanCacheItem()
        {
            const string storedProcedureName = "SpName";
            const int executionCountOfCache = 5;
            const int previousExecutionCount = 1;
            const int historicalExecutionCount = 2;
            int overallExecutions = executionCountOfCache + historicalExecutionCount - previousExecutionCount;
            DateTime removedFromCacheAt = DateTime.Parse("2021-12-12 17:34:04");
            PlanCacheItem currentPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = storedProcedureName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = executionCountOfCache }
                }
            };
            DateTime lastExecutionTimeHistorical = DateTime.Parse("2021-12-12 17:32:00");
            PlanCacheItem historicalPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = removedFromCacheAt,
                SpName = storedProcedureName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = historicalExecutionCount, LastExecutionTime = lastExecutionTimeHistorical }
                }
            };
            DateTime lastExecutionTimePrevious = DateTime.Parse("2021-12-12 17:30:00");
            PlanCacheItem previousPlanCacheItem =
                new PlanCacheItem()
                {
                    RemovedFromCacheAt = null,
                    SpName = storedProcedureName,
                    ExecutionStatistics = new ProcedureExecutionStatistics()
                    {
                        GeneralStats = new GeneralStats() { ExecutionCount = previousExecutionCount, LastExecutionTime = lastExecutionTimePrevious }
                    }
                };
            var previousItemCache = new Mock<IPreviousItemCache>();
            previousItemCache.Setup(s => s.StorePreviousCacheItem(storedProcedureName, It.Is<PlanCacheItem>(item => item.ExecutionStatistics.GeneralStats.ExecutionCount == overallExecutions)));
            previousItemCache.Setup(s => s.GetPreviousCacheItem(storedProcedureName)).Returns(previousPlanCacheItem);
            var instanceUnderTest = new ExecutionCountMetricsBuilder(previousItemCache.Object);
            var groupedPlanCacheItems = (new List<PlanCacheItem>() { currentPlanCacheItem, historicalPlanCacheItem }).GroupBy(p => p.SpName).First();
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ExecutionCount",
                        Value = overallExecutions
                    }
              };

            IEnumerable<MetricItem> result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
            previousItemCache.VerifyAll();
        }
    }
}
