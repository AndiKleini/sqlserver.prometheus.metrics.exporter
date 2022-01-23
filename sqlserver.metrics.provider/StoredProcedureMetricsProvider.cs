using Sqlserver.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Provider
{
    public class StoredProcedureMetricsProvider : IStoredProcedureMetricsProvider
    {
        private readonly IPlanCacheRepository planCacheRepository;
        private readonly ICombinedMetricsBuilder metricsBuilder;

        public StoredProcedureMetricsProvider(IPlanCacheRepository planCacheRepository, ICombinedMetricsBuilder metricsBuilder = null)
        {
            this.planCacheRepository = planCacheRepository;
            this.metricsBuilder = metricsBuilder;
        }

        public async Task<IEnumerable<MetricItem>> Collect(DateTime from)
        {
            var groupedBySpName = this.planCacheRepository.GetPlanCache(from).GroupBy(p => p.SpName);
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