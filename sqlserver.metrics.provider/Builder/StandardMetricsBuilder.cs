using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class StandardMetricsBuilder : IMetricsBuilder, IDeltaMetricsBuilder
    {
        List<IMetricsBuilder> metricItems = new List<IMetricsBuilder>();

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            return this.metricItems.SelectMany(b => b.Build(groupedPlanCacheItems));
        }

        public IEnumerable<MetricItem> BuildDeltas(IGrouping<string, PlanCacheItem> grouping, PlanCacheItem planCacheItem)
        {
            throw new System.NotImplementedException();
        }

        public void Include(IMetricsBuilder metricsBuilder)
        {
            metricItems.Add(metricsBuilder);
        }
    }
}