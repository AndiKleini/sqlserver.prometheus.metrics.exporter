using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider
{
    public class EstimatedExecutionCountMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        private IPreviousItemCache previousItemCache;

        public EstimatedExecutionCountMetricsBuilder(IPreviousItemCache previousItemCache)
        {
            this.previousItemCache = previousItemCache;
        }

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            PlanCacheItem previousCacheItem = this.previousItemCache.GetPreviousCacheItem(groupedPlanCacheItems.Key);
            PlanCacheItem currentPlanCacheItem = groupedPlanCacheItems.First();
            this.previousItemCache.StorePreviousCacheItem(groupedPlanCacheItems.Key, currentPlanCacheItem);
            if (IsFirstAttemptOrNoChangesOfExecutionCount(previousCacheItem, currentPlanCacheItem))
            {
                yield break;
            }
            else
            {
                yield return new MetricItem()
                {
                    Name = this.GetMetricsName(groupedPlanCacheItems.Key, "EstimatedExecutionCount"),
                    Value = GetDifferenceIfPositiveOtherwiseCurrentValue(
                        currentPlanCacheItem.ExecutionStatistics.GeneralStats.ExecutionCount,
                        previousCacheItem.ExecutionStatistics.GeneralStats.ExecutionCount)
                };
            }

            long GetDifferenceIfPositiveOtherwiseCurrentValue(long currentExecutionCount, long previousExecutionCount)
            {
                return currentExecutionCount > previousExecutionCount ? currentExecutionCount - previousExecutionCount : currentExecutionCount;
            }

            static bool IsFirstAttemptOrNoChangesOfExecutionCount(PlanCacheItem previousCacheItem, PlanCacheItem currentPlanCacheItem)
            {
                return previousCacheItem == null || currentPlanCacheItem.ExecutionStatistics.GeneralStats.ExecutionCount == previousCacheItem.ExecutionStatistics.GeneralStats.ExecutionCount;
            }
        }
    }
}