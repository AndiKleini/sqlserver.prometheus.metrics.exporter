using NUnit.Framework;
using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Tests
{
    [TestFixture]
    public class StandardMetricsBuilderTests
    {
        [Test]
        public void Build_FiguresForElapsedTimeSupplied_ReturnsElapsedMetricItems()
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
    }
}
