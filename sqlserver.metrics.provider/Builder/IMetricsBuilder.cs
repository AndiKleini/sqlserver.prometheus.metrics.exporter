using System.Linq;
using System.Collections.Generic;

namespace SqlServer.Metrics.Provider.Builder
{
    public interface IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> grouping);
    }
}