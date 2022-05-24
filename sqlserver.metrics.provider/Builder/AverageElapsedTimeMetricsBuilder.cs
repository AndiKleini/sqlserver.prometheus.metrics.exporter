using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Builder
{
    public class AverageElapsedTimeMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, "AverageElapsedTime"),
                Value = groupedPlanCacheItems.Sum(p => p.ExecutionStatistics.ElapsedTime.Total)
                        /
                        groupedPlanCacheItems.Sum(p => p.ExecutionStatistics.GeneralStats.ExecutionCount)
            };
        }
    }
}