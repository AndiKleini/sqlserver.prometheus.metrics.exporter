using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Provider
{
    public interface IPlanCacheRepository
    {
        Task<IEnumerable<PlanCacheItem>> GetPlanCache(DateTime from);
        Task<IEnumerable<PlanCacheItem>> GetPreviousPlanCacheItems();
    }
}