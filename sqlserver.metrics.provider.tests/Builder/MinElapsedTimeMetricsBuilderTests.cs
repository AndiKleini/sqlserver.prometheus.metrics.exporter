using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Tests.Builder
{
    [TestFixture]
    public class MinElapsedTimeMetricsBuilderTests
    {
        [Test]
        public void Build_SingleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            string storedProcedureName = "MySp";
            int minElapsedTime = 10;
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    MetricItemFactoryMethod.GetMetricItem(storedProcedureName, "ElapsedTimeMin", minElapsedTime)
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
                    }}).GroupBy(p => p.SpName).First();

            MinElapsedTimeMetricsBuilder instanceUnderTest = new MinElapsedTimeMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }

        [Test]
        public void Build_MultipleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            string storedProcedureName = "MySp";
            int minElapsedTime = 15;
            int aboveMinElapsedTime = 70;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                   MetricItemFactoryMethod.GetMetricItem(storedProcedureName, "ElapsedTimeMin", minElapsedTime)
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
                            ElapsedTime = new ElapsedTime() { Min = aboveMinElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Min = aboveMinElapsedTime }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            MinElapsedTimeMetricsBuilder instanceUnderTest = new MinElapsedTimeMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
