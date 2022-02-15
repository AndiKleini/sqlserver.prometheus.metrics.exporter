using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Exporter.Database
{
    public interface IDbPlanCacheRepository
    {
        Task<List<PlanCacheItem>> GetCurrentPlanCache(DateTime from);
        Task<List<PlanCacheItem>> GetHistoricalPlanCache(DateTime from);
        Task<Dictionary<int, string>> GetObjectIdAndProcedureNames();
    }
}