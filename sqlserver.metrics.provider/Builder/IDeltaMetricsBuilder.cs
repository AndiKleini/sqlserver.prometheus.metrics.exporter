using SqlServer.Metrics.Provider;
using System.Linq;
using System.Collections.Generic;

namespace Sqlserver.Metrics.Provider.Builder
{
    public interface IDeltaMetricsBuilder
    {
        IEnumerable<MetricItem> BuildDeltas(IGrouping<string, PlanCacheItem> grouping, PlanCacheItem planCacheItem);
    }
}