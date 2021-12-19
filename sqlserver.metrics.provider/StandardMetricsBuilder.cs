using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider
{
    public class StandardMetricsBuilder : IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            return new MaxElapsedTimeMetricsBuilder().Build(groupedPlanCacheItems).Concat(
                   new MinElapsedTimeMetricsBuilder().Build(groupedPlanCacheItems));
        }
    }
}