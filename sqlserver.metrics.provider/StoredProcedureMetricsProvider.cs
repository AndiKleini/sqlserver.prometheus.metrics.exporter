using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider
{
    public class StoredProcedureMetricsProvider
    {
        private readonly IPlanCacheRepository planCacherepository;

        public StoredProcedureMetricsProvider(IPlanCacheRepository planCacherepository)
        {
            this.planCacherepository = planCacherepository;
        }

        public IEnumerable<MetricItem> Collect(DateTime from, DateTime to)
        {
            var items = this.planCacherepository.GetPlanCache(from, to);
            return items.Select(
                p => new MetricItem()
                {
                    Name = $"{p.SpName}_ExecutionCount",
                    Value = p.ExecutionStatistics.GeneralStats.ExecutionCount
                });
        }
    }
}