using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class GenericAverageMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        private Func<PlanCacheItem, long> selector;

        public string MetricsName { get; private set; }

        public GenericAverageMetricsBuilder(string metricsName, Func<PlanCacheItem, long> selector)
        {
            this.MetricsName = metricsName;
            this.selector = selector;
        }

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, this.MetricsName),
                Value = groupedPlanCacheItems.Sum(this.selector)
                       /
                       groupedPlanCacheItems.Sum(p => p.ExecutionStatistics.GeneralStats.ExecutionCount)
            };
        }
    }
}