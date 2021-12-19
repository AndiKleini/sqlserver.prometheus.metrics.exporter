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
            var items = this.planCacheRepository.GetPlanCache(from, to);

            IEnumerable<IGrouping<string, PlanCacheItem>> groupedBySpName = items.GroupBy(p => p.SpName);
            IEnumerable<MetricItem> commonMetrics = groupedBySpName.SelectMany(metricsBuilder.Build);
            IEnumerable<MetricItem> deltaMetrics = groupedBySpName
                            .Join(
                                this.planCacheRepository.GetPreviousPlanCacheItems(),
                                p => p.Key,
                                p => p.SpName,
                                metricsBuilder.BuildDeltas)
                            .SelectMany(s => s);
            return commonMetrics.Concat(deltaMetrics);

        }
    }
}