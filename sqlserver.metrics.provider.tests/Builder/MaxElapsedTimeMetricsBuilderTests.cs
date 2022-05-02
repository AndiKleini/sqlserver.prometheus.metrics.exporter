using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Tests.Builder
{
    [TestFixture]
    public class MaxElapsedTimeMetricsBuilderTests
    {
        [Test]
        public void Build_SingleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            string storedProcedureName = "MySp";
            int maxElapsedTime = 150;
            int minElapsedTime = 10;
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ElapsedTimeMax",
                        Value = maxElapsedTime
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
                            ElapsedTime = new ElapsedTime() { Max = maxElapsedTime, Min = minElapsedTime }
                        }
                    }}).GroupBy(p => p.SpName).First();

            MaxElapsedTimeMetricsBuilder instanceUnderTest = new MaxElapsedTimeMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }

        [Test]
        public void Build_MultipleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            string storedProcedureName = "MySp";
            int maxElapsedTime = 150;
            int betweenMaxElapsedTime = 70;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ElapsedTimeMax",
                        Value = maxElapsedTime
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
                            ElapsedTime = new ElapsedTime() { Max = maxElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = betweenMaxElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = betweenMaxElapsedTime }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            MaxElapsedTimeMetricsBuilder instanceUnderTest = new MaxElapsedTimeMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
