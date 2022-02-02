using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class ExecutionCountMetricsBuilder
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
                    currentPlanCacheItem ?? new PlanCacheItem() { ExecutionStatistics = new ProcedureExecutionStatistics() { GeneralStats = new GeneralStats() { ExecutionCount = 0 } } } );
                yield break;
            }

            int currentExecutionCount = groupedPlanCacheItems.Sum(p => p.ExecutionStatistics.GeneralStats.ExecutionCount) - previousPlanCacheItem.ExecutionStatistics.GeneralStats.ExecutionCount;
            yield return new MetricItem()
            {
                Name = $"{groupedPlanCacheItems.Key}_ExecutionCount",
                Value = currentExecutionCount
            };
            previousItemCache.StorePreviousCacheItem(
                 groupedPlanCacheItems.Key, new PlanCacheItem() { ExecutionStatistics = new ProcedureExecutionStatistics() { GeneralStats = new GeneralStats() { ExecutionCount = currentExecutionCount } } });
        }
    }
}