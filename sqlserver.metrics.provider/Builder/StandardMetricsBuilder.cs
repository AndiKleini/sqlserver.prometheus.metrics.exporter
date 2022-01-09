using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class StandardMetricsBuilder : IMetricsBuilder, IDeltaMetricsBuilder
    {
        List<IMetricsBuilder> metricItems = new List<IMetricsBuilder>();
        List<IDeltaMetricsBuilder> deltaMetricsBuilders = new List<IDeltaMetricsBuilder>();

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            return this.metricItems.SelectMany(b => b.Build(groupedPlanCacheItems));
        }

        public IEnumerable<MetricItem> BuildDeltas(IGrouping<string, PlanCacheItem> grouping, PlanCacheItem previousPlanCacheItem)
        {
            return this.deltaMetricsBuilders.SelectMany(b => b.BuildDeltas(grouping, previousPlanCacheItem));
        }

        public void Include(IMetricsBuilder metricsBuilder)
        {
            this.metricItems.Add(metricsBuilder);
        }

        public void Include(IDeltaMetricsBuilder metricsBuilder)
        {
            this. deltaMetricsBuilders.Add(metricsBuilder);
        }
    }
}