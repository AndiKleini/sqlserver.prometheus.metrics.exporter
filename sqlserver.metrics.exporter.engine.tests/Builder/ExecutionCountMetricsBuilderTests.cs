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
     

        public void Build_NoHistoricalItemButPlanCacheItemAvailable_ReturnDiffToPreviousItem()
        {

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

            result.Should().BeNull();
            previousItemCache.VerifyAll();
        }

        [Test]
        public void Build_NoHistoricalItemAndNoPreviousPlanCacheItemButCurrentPlacnCacheItemAvailable_ReturnsNoMetricResultStoresCurrentItemAsPrevious()
        {
            const string storedProcedureName = "SpName";
            const int executionCountOfCache = 5;
            PlanCacheItem currentPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = storedProcedureName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = executionCountOfCache }
                }
            };
            var previousItemCache = new Mock<IPreviousItemCache>();
            previousItemCache.Setup(s => s.StorePreviousCacheItem(storedProcedureName, currentPlanCacheItem));
            previousItemCache.Setup(s => s.GetPreviousCacheItem(storedProcedureName)).Returns(default(PlanCacheItem));
            var instanceUnderTest = new ExecutionCountMetricsBuilder(previousItemCache.Object);

            var groupedPlanCacheItems =
                (new List<PlanCacheItem>() { currentPlanCacheItem }).GroupBy(p => p.SpName).First();

            IEnumerable<MetricItem> result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeNull();
            previousItemCache.VerifyAll();
        }

        public void Build_HistoricalItemWithLastExecutionTimeBeforePreviousPlanCacheItem_ReturnsDiffOfPreviousPlanCacheItemAndCurrentPlanCacheItem()
        {
            //const string storedProcedureName = "SpName";
            //const int executionCountOfCache = 5;
            //const int executionCountHistorical1 = 3;
            //const int executionCountHistorical2 = 7;
            //const int previousExecutionCount = 2;
            //DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            //DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            //PlanCacheItem previousPlanCacheItem =
            //    new PlanCacheItem()
            //    {
            //        RemovedFromCacheAt = null,
            //        SpName = storedProcedureName,
            //        ExecutionStatistics = new ProcedureExecutionStatistics()
            //        {
            //            GeneralStats = new GeneralStats() { ExecutionCount = previousExecutionCount }
            //        }
            //    };
            //var previousItemCache = new Mock<IPreviousItemCache>();
            //previousItemCache.Setup(s => s.GetPreviousCacheItem(storedProcedureName)).Returns(previousPlanCacheItem);
            //var instanceUnderTest = new ExecutionCountMetricsBuilder(previousItemCache.Object);
            //var groupedPlanCacheItems =
            //    (new List<PlanCacheItem>() {
            //        new PlanCacheItem()
            //        {
            //            RemovedFromCacheAt = null,
            //            SpName = storedProcedureName,
            //            ExecutionStatistics = new ProcedureExecutionStatistics()
            //            {
            //                GeneralStats = new GeneralStats() { ExecutionCount = executionCountOfCache }
            //            }
            //        },
            //        new PlanCacheItem()
            //        {
            //            RemovedFromCacheAt = removedFromCacheAt1,
            //            SpName = storedProcedureName,
            //            ExecutionStatistics = new ProcedureExecutionStatistics()
            //            {
            //                GeneralStats = new GeneralStats() { ExecutionCount = executionCountHistorical1 }
            //            }
            //        },
            //        new PlanCacheItem()
            //        {
            //            RemovedFromCacheAt = removedFormCacheAt2,
            //            SpName = storedProcedureName,
            //            ExecutionStatistics = new ProcedureExecutionStatistics()
            //            {
            //                GeneralStats = new GeneralStats() { ExecutionCount = executionCountHistorical2 }
            //            }
            //        }
            //    }).GroupBy(p => p.SpName).First();
            //int overallExecutions = executionCountOfCache + executionCountHistorical1 + executionCountHistorical2 - previousExecutionCount;
            //List<MetricItem> expectedItems =
            //  new List<MetricItem>()
            //  {
            //        new MetricItem()
            //        {
            //            Name = $"{storedProcedureName}_ExecutionCount",
            //            Value = overallExecutions
            //        }
            //  };

            //IEnumerable<MetricItem> result = instanceUnderTest.Build(groupedPlanCacheItems);

            //result.Should().BeEquivalentTo(expectedItems);
        }

        public void Build_HistoricalItemWithLastExecutionTimeAfterPreviousPlanCacheItem_ReturnsDiffOfPreviousPlanCacheItemAndHistoricalPlanCacheItem()
        {

        }
    }
}
