using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Provider
{
    public interface IPlanCacheRepository
    {
        Task<IEnumerable<PlanCacheItem>> GetPlanCache(DateTime collectMetricsFrom, DateTime includeHistoricalItemsFrom);
        Task<IEnumerable<PlanCacheItem>> GetPreviousPlanCacheItems();
    }
}