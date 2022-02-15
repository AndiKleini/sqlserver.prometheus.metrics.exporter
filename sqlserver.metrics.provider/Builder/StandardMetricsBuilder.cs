using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider.Builder
{
    public class StandardMetricsBuilder : ICombinedMetricsBuilder
    {
        private List<IMetricsBuilder> metricItems = new List<IMetricsBuilder>();
        public List<IMetricsBuilder> MetricItems { get => metricItems; }


        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            return this.metricItems.SelectMany(b => b.Build(groupedPlanCacheItems));
        }

        public void Include(IMetricsBuilder metricsBuilder)
        {
            this.metricItems.Add(metricsBuilder);
        }
    }
}