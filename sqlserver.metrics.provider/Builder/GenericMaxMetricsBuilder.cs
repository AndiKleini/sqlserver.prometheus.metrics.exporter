using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlServer.Metrics.Provider.Builder
{
    public class GenericMaxMetricsBuilder : IMetricsBuilder
    {
        private Func<PlanCacheItem, long> selector;

        public GenericMaxMetricsBuilder(string metricsName, Expression<Func<PlanCacheItem, long>> selector)
        {
            this.MetricsName = metricsName;
            this.selector = selector.Compile();
        }

        public string MetricsName { get; private set; }

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = $"{groupedPlanCacheItems.Key}_{this.MetricsName}",
                Value = groupedPlanCacheItems.Max(this.selector)
            };
        }
    }
}