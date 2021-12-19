using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class StandardMetricsBuilder : IMetricsBuilder
    {
        List<IMetricsBuilder> metricItems = new List<IMetricsBuilder>();

        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            return this.metricItems.SelectMany(b => b.Build(groupedPlanCacheItems));
        }

        public void Include(IMetricsBuilder metricsBuilder)
        {
            metricItems.Add(metricsBuilder);
        }
    }
}