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
    public class GenericLastMetricsBuilderTests
    {
        [Test]
        public void Build_MultipleFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            const string metricsName = "ElapsedTimeLast";
            string storedProcedureName = "MySp";
            int lastElapsedTime = 150;
            int beforeLastElapsedTime = 70;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_{metricsName}",
                        Value = lastElapsedTime
                    }
              };
            var groupedPlanCacheItems =
                (new List<PlanCacheItem>() {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt =  null,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Last = lastElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Last = beforeLastElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Last = beforeLastElapsedTime }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            GenericLastMetricsBuilder instanceUnderTest = new GenericLastMetricsBuilder(metricsName, p => p.ExecutionStatistics.ElapsedTime.Last);

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }

        [Test]
        public void Build_MultipleHistoricalFiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
        {
            const string metricsName = "ElapsedTimeLast";
            string storedProcedureName = "MySp";
            int lastElapsedTime = 150;
            int beforeLastElapsedTime = 70;
            DateTime removedFromCacheAt1 = DateTime.Parse("2021-12-12 17:34:04");
            DateTime removedFormCacheAt2 = DateTime.Parse("2021-12-12 17:30:04");
            List<MetricItem> expectedItems =
              new List<MetricItem>()
              {
                    new MetricItem()
                    {
                        Name = $"{storedProcedureName}_{metricsName}",
                        Value = lastElapsedTime
                    }
              };
            var groupedPlanCacheItems =
                (new List<PlanCacheItem>() {
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFromCacheAt1,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Last = lastElapsedTime }
                        }
                    },
                    new PlanCacheItem()
                    {
                        RemovedFromCacheAt = removedFormCacheAt2,
                        SpName = storedProcedureName,
                        ExecutionStatistics = new ProcedureExecutionStatistics()
                        {
                            ElapsedTime = new ElapsedTime() { Last = beforeLastElapsedTime }
                        }
                    }
                }).GroupBy(p => p.SpName).First();

            GenericLastMetricsBuilder instanceUnderTest = new GenericLastMetricsBuilder(metricsName, p => p.ExecutionStatistics.ElapsedTime.Last);

            var result = instanceUnderTest.Build(groupedPlanCacheItems);

            result.Should().BeEquivalentTo(expectedItems);
        }
    }
}
