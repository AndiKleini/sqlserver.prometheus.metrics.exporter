using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Builder
{
    public class ExecutionCountMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        private IPreviousItemCache previousItemCache;

        public ExecutionCountMetricsBuilder(IPreviousItemCache previousItemCache)
        {
            this.previousItemCache = previousItemCache;
        }

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            var previousPlanCacheItem = this.previousItemCache.GetPreviousCacheItem(groupedPlanCacheItems.Key);
            if (previousPlanCacheItem is null)
            {
                var currentPlanCacheItem = groupedPlanCacheItems.SingleOrDefault(i => i.RemovedFromCacheAt == null);
                previousItemCache.StorePreviousCacheItem(
                    groupedPlanCacheItems.Key,
                    currentPlanCacheItem ?? PlanCacheItem.Zero());
                yield break;
            }

            long currentExecutionCount =
               groupedPlanCacheItems.
               Sum(p => p.ExecutionStatistics.GeneralStats.ExecutionCount)
               - previousPlanCacheItem.ExecutionStatistics.GeneralStats.ExecutionCount;

            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, "ExecutionCount"),
                Value = currentExecutionCount
            };
            previousItemCache.StorePreviousCacheItem(
                 groupedPlanCacheItems.Key, new PlanCacheItem() { ExecutionStatistics = new ProcedureExecutionStatistics() { GeneralStats = new GeneralStats() { ExecutionCount = currentExecutionCount } } });
        }
    }
}