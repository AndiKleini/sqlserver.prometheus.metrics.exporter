using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider
{
    public class ExecutionCountMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems, PlanCacheItem previousPlanCacheItem)
        {
            yield return new MetricItem()
            {
                Name = $"{groupedPlanCacheItems.Key}_ExecutionCount",
                Value = 
                    groupedPlanCacheItems.Sum(p => p.ExecutionStatistics.GeneralStats.ExecutionCount) - 
                    previousPlanCacheItem.ExecutionStatistics.GeneralStats.ExecutionCount
            };
        }
    }
}