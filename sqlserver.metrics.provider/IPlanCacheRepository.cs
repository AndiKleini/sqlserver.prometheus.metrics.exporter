using System;
using System.Collections.Generic;

namespace SqlServer.Metrics.Provider
{
    public interface IPlanCacheRepository
    {
        List<PlanCacheItem> GetPlanCache(DateTime from, DateTime to);
    }
}