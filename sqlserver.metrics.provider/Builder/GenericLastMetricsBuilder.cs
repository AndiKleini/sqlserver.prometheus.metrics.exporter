using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class GenericLastMetricsBuilder
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
                Name = $"{groupedPlanCacheItems.Key}_{this.MetricsName}",
                Value = youngestValue
            };
        }
    }
}