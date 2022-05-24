using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Builder
{
    public class MaxElapsedTimeMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, "ElapsedTimeMax"),
                Value = groupedPlanCacheItems.Max(p => p.ExecutionStatistics.ElapsedTime.Max)
            };
        }
    }
}