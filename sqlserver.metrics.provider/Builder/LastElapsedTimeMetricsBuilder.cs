using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class LastElapsedTimeMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, "ElapsedTimeLast"),
                Value = groupedPlanCacheItems.OrderBy(s => s.RemovedFromCacheAt).Take(1).Single().ExecutionStatistics.ElapsedTime.Last
            };
        }
    }
}