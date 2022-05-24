using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlServer.Metrics.Provider.Builder
{
    public class GenericMaxMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        private Func<PlanCacheItem, long> selector;

        public string MetricsName { get; private set; }

        public GenericMaxMetricsBuilder(string metricsName, Expression<Func<PlanCacheItem, long>> selector)
        {
            this.MetricsName = metricsName;
            this.selector = selector.Compile();
        }

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, this.MetricsName),
                Value = groupedPlanCacheItems.Max(this.selector)
            };
        }
    }
}