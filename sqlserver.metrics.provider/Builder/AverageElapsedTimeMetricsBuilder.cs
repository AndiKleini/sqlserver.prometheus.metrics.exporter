using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class AverageElapsedTimeMetricsBuilder : IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = $"{groupedPlanCacheItems.Key}_AverageElapsedTime",
                Value = groupedPlanCacheItems.Sum(p => p.ExecutionStatistics.ElapsedTime.Total)
                        /
                        groupedPlanCacheItems.Sum(p => p.ExecutionStatistics.GeneralStats.ExecutionCount)
            };
        }
    }
}