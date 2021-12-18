using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider
{
    public class StoredProcedureMetricsProvider
    {
        private readonly IPlanCacheRepository planCacherepository;
        private readonly IMetricsBuilder metricsBuilder;

        public StoredProcedureMetricsProvider(IPlanCacheRepository planCacherepository, IMetricsBuilder metricsBuilder = null)
        {
            this.planCacherepository = planCacherepository;
            this.metricsBuilder = metricsBuilder;
        }

        public IEnumerable<MetricItem> Collect(DateTime from, DateTime to)
        {
            var items = this.planCacherepository.GetPlanCache(from, to);
            return items.GroupBy(p => p.SpName)
                .SelectMany(g => this.metricsBuilder != null ? this.metricsBuilder.Build(g) : ElapsedTimeMetrics.GetElapsedTimeMetrics(g));
        }
    }
}