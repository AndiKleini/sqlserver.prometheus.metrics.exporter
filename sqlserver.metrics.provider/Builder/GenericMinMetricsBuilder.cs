using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class GenericMinMetricsBuilder : IMetricsBuilder
    {
        private Func<PlanCacheItem, long> selector;

        public string MetricsName { get; private set; }

        public GenericMinMetricsBuilder(string metricsName, Func<PlanCacheItem, long> selector)
        {
            this.MetricsName = metricsName;
            this.selector = selector;
        }

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = $"{groupedPlanCacheItems.Key}_{this.MetricsName}",
                Value = groupedPlanCacheItems.Min(this.selector)
            };
        }
    }
}