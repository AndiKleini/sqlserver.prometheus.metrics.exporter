using SqlServer.Metrics.Provider;
using System.Linq;
using System.Collections.Generic;

namespace Sqlserver.Metrics.Provider.Builder
{
    public interface IMetricsBuilder
    {
        IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> grouping);
    }
}