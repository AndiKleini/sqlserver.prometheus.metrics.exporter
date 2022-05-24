using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Builder
{
    public class MinElapsedTimeMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, "ElapsedTimeMin"),
                Value = groupedPlanCacheItems.Min(p => p.ExecutionStatistics.ElapsedTime.Min)
            };
        }
    }
}