﻿using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider
{
    public class MaxElapsedTimeMetricsBuilder
    {
        public IEnumerable<MetricItem> Build(IGrouping<string, PlanCacheItem> groupedPlanCacheItems)
        {
            yield return new MetricItem()
            {
                Name = $"{groupedPlanCacheItems.Key}_ElapsedTimeMax",
                Value = groupedPlanCacheItems.Max(p => p.ExecutionStatistics.ElapsedTime.Max)
            };
        }
    }
}