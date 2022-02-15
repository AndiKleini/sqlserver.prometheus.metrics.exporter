using SqlServer.Metrics.Provider;

namespace SqlServer.Metrics.Provider
{
    public interface IPreviousItemCache
    {
        void StorePreviousCacheItem(string storedProcedureName, PlanCacheItem previousPlanCacheItem);
        PlanCacheItem GetPreviousCacheItem(string storedProcedureName);
    }
}