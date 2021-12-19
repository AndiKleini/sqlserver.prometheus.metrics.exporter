using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class StandardMetricsBuilder : IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            return new MaxElapsedTimeMetricsBuilder().Build(groupedPlanCacheItems).Concat(
                   new MinElapsedTimeMetricsBuilder().Build(groupedPlanCacheItems)); /*.Concat(
                   new ExecutionCountMetricsBuilder().Build.groupedPlanCacheItems, ;*/
        }
    }
}