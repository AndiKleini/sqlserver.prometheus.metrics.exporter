using Sqlserver.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
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

        public async Task<MetricsResult> Collect(DateTime from)
        {
            var planCacheItems = await this.planCacheRepository.GetPlanCache(from);
            var groupedBySpName = planCacheItems.GroupBy(p => p.SpName);
            var metricItems = groupedBySpName.SelectMany(metricsBuilder.Build);
            return 
                new MetricsResult() 
                { 
                    Items = metricItems, 
                    NewestHistoricalItemConsidered = planCacheItems.Max(item => item.RemovedFromCacheAt)
                };
        }

        public List<IMetricsBuilder> GetMetricBuilders()
        {
            return this.metricsBuilder.MetricItems;
        }
    }
}