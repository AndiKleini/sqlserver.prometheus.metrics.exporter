using Sqlserver.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider
{
    public class StoredProcedureMetricsProvider
    {
        private readonly IPlanCacheRepository planCacheRepository;
        private readonly ICombinedMetricsBuilder metricsBuilder;

        public StoredProcedureMetricsProvider(IPlanCacheRepository planCacherepository, ICombinedMetricsBuilder metricsBuilder = null)
        {
            this.planCacheRepository = planCacherepository;
            this.metricsBuilder = metricsBuilder;
        }

        public IEnumerable<MetricItem> Collect(DateTime from, DateTime to)
        {
            var groupedBySpName = this.planCacheRepository.GetPlanCache(from, to).GroupBy(p => p.SpName);
            return groupedBySpName.SelectMany(metricsBuilder.Build).Concat(groupedBySpName
                            .Join(
                                this.planCacheRepository.GetPreviousPlanCacheItems(),
                                p => p.Key,
                                p => p.SpName,
                                metricsBuilder.BuildDeltas)
                            .SelectMany(s => s));
        }
    }
}