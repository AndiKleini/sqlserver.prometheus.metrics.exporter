using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider
{
    public interface IMetricsBuilder
    {
        IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> grouping);
    }
}