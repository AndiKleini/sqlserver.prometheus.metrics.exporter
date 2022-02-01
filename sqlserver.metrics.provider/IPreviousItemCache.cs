using SqlServer.Metrics.Provider;

namespace Sqlserver.Metrics.Provider
{
    public interface IPreviousItemCache
    {
        void StorePreviousCacheItem(string storedProcedureName, PlanCacheItem previousPlanCacheItem);
        PlanCacheItem GetPreviousCacheItem(string storedProcedureName);
    }
}