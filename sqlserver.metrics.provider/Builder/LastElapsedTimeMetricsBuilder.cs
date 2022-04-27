using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Builder
{
    public class LastElapsedTimeMetricsBuilder : IMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = $"{groupedPlanCacheItems.Key}_ElapsedTimeLast",
                Value = groupedPlanCacheItems.OrderBy(s => s.RemovedFromCacheAt).Take(1).Single().ExecutionStatistics.ElapsedTime.Last
            };
        }
    }
}