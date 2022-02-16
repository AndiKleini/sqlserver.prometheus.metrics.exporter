using SqlServer.Metrics.Provider;
using System.Collections.Generic;

namespace SqlServer.Metrics.Exporter.Adapters
{
    public class InMemoryPreviousItemCacheAdapter : IPreviousItemCache
    {
        private Dictionary<string, PlanCacheItem> cache = new Dictionary<string, PlanCacheItem>();

        public PlanCacheItem GetPreviousCacheItem(string storedProcedureName) => this.cache.GetValueOrDefault(storedProcedureName);
       
        public void StorePreviousCacheItem(string storedProcedureName, PlanCacheItem previousPlanCacheItem) 
            => this.cache[storedProcedureName] = previousPlanCacheItem;
    }
}