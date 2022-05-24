using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class GenericLastMetricsBuilder : MetricsBuilderBase, IMetricsBuilder
    {
        private Func<PlanCacheItem, long> selector;
        public string MetricsName { get; private set; }

        public GenericLastMetricsBuilder(string metricsName, Func<PlanCacheItem, long> selector)
        {
            this.MetricsName = metricsName;
            this.selector = selector;
        }

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            long youngestValue = 0;
            var planCacheItemFromCache = groupedPlanCacheItems.Where(s => s.RemovedFromCacheAt == null).FirstOrDefault();
            if (planCacheItemFromCache != null)
            {
                youngestValue = this.selector(planCacheItemFromCache);
            }
            else
            {
                youngestValue = this.selector(groupedPlanCacheItems.OrderByDescending(s => s.RemovedFromCacheAt).First());
            }
            yield return new MetricItem()
            {
                Name = this.GetMetricsName(groupedPlanCacheItems.Key, this.MetricsName),
                Value = youngestValue
            };
        }
    }
}