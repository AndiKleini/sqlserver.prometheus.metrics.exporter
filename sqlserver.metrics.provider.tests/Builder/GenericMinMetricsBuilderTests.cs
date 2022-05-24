using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Tests.Builder
{
    [TestFixture]
    public class GenericMinMetricsBuilderTests
    {
        [Test]
        public void Build_MultipleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            const string metricsName = "ElapsedTimeMin";
            string storedProcedureName = "MySp";
            int minElapsedTime = 15;
            int aboveMaxElapsedTime = 70;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    MetricItemFactoryMethod.GetMetricItem(storedProcedureName, metricsName, minElapsedTime)
              };
            var groupedPlanCacheItems =
                (new List<PlanCacheItem>() {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = null,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Min = minElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Min = aboveMaxElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Min = aboveMaxElapsedTime }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            GenericMinMetricsBuilder instanceUnderTest = new GenericMinMetricsBuilder(metricsName, p => p.ExecutionStatistics.ElapsedTime.Min);

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
