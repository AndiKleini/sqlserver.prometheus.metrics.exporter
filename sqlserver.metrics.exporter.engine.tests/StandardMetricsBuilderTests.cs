using FluentAssertions;
using NUnit.Framework;
using Sqlserver.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Tests
{
    [TestFixture]
    public class StandardMetricsBuilderTests
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
                    },
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_ElapsedTimeMin",
                        Value = minElapsedTime
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

            StandardMetricsBuilder instanceUnderTest = new StandardMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems);
        }

        [Test]
        public void Build_MultipleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            string storedProcedureName = "MySp";
            int maxElapsedTime = 150;
            int minElapsedTime = 10;
            int betweenMaxElapsedTime = 70;
            int betweenMinElapsedTime = 30;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
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
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = betweenMaxElapsedTime, Min = betweenMinElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Max = betweenMaxElapsedTime, Min = betweenMinElapsedTime }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            StandardMetricsBuilder instanceUnderTest = new StandardMetricsBuilder();

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
