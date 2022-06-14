using FluentAssertions;
using Moq;
using NUnit.Framework;
using SqlServer.Metrics.Provider.Builder;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Tests.Builder
{
    [TestFixture]
    public class EstimatedExecutionCountMetricsBuilderTests
    {
        [Test]
        public void Build_PreviousPlanCacheItemNotSet_EmitsNoMetrics()
        {
            const string spName = "somesp";
            PlanCacheItem planCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = spName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = 2 }
                }
            };
            Mock<IPreviousItemCache> mockery = new Mock<IPreviousItemCache>();
            mockery.Setup(s => s.GetPreviousCacheItem(spName)).Returns(default(PlanCacheItem));
            mockery.Setup(s => s.StorePreviousCacheItem(spName, planCacheItem));
            IMetricsBuilder instanceUnderTest = new EstimatedExecutionCountMetricsBuilder(mockery.Object);
          
            IGrouping<string, PlanCacheItem> groupedPlanCacheItem = 
                new List<PlanCacheItem>() { planCacheItem }.GroupBy(s => s.SpName).First();

            var yield = instanceUnderTest.Build(groupedPlanCacheItem);

            yield.Should().BeEmpty();
            mockery.VerifyAll();
        }

        [Test]
        public void Build_PreviousPlanCacheItemSetAndCurrentPlanCacheItemWithHigherExecutionCount_EmitsDifferenceOfExecutionCounts()
        {
            const string spName = "somesp";
            const int currentExecutionCount = 4;
            PlanCacheItem planCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = spName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = currentExecutionCount }
                }
            };
            const int previousExecutionCount = 2;
            PlanCacheItem previousPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = spName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = previousExecutionCount }
                }
            };
            Mock<IPreviousItemCache> mockery = new Mock<IPreviousItemCache>();
            mockery.Setup(s => s.GetPreviousCacheItem(spName)).Returns(previousPlanCacheItem);
            mockery.Setup(s => s.StorePreviousCacheItem(spName, planCacheItem));
            IMetricsBuilder instanceUnderTest = new EstimatedExecutionCountMetricsBuilder(mockery.Object);
            MetricItem expectedMetricItem = MetricItemFactoryMethod.GetMetricItem(spName, "EstimatedExecutionCount", currentExecutionCount - previousExecutionCount);

            IGrouping<string, PlanCacheItem> groupedPlanCacheItem =
                new List<PlanCacheItem>() { planCacheItem }.GroupBy(s => s.SpName).First();

            var yield = instanceUnderTest.Build(groupedPlanCacheItem);

            yield.Should().ContainEquivalentOf(expectedMetricItem);
            mockery.VerifyAll();
        }

        [Test]
        public void Build_PreviousPlanCacheItemSetAndCurrentPlanCacheItemWithLowerExecutionCount_EmitsCurrentExecutionCount()
        {
            const string spName = "somesp";
            const int currentExecutionCount = 1;
            PlanCacheItem planCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = spName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = currentExecutionCount }
                }
            };
            const int previousExecutionCount = 2;
            PlanCacheItem previousPlanCacheItem = new PlanCacheItem()
            {
                RemovedFromCacheAt = null,
                SpName = spName,
                ExecutionStatistics = new ProcedureExecutionStatistics()
                {
                    GeneralStats = new GeneralStats() { ExecutionCount = previousExecutionCount }
                }
            };
            Mock<IPreviousItemCache> mockery = new Mock<IPreviousItemCache>();
            mockery.Setup(s => s.GetPreviousCacheItem(spName)).Returns(previousPlanCacheItem);
            mockery.Setup(s => s.StorePreviousCacheItem(spName, planCacheItem));
            IMetricsBuilder instanceUnderTest = new EstimatedExecutionCountMetricsBuilder(mockery.Object);
            MetricItem expectedMetricItem = MetricItemFactoryMethod.GetMetricItem(spName, "EstimatedExecutionCount", currentExecutionCount);

            IGrouping<string, PlanCacheItem> groupedPlanCacheItem =
                new List<PlanCacheItem>() { planCacheItem }.GroupBy(s => s.SpName).First();

            var yield = instanceUnderTest.Build(groupedPlanCacheItem);

            yield.Should().ContainEquivalentOf(expectedMetricItem);
            mockery.VerifyAll();
        }
    }
}
