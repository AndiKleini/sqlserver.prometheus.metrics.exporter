using SqlServer.Metrics.Exporter.Database;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Exporter.Adapters
{
    public class PlanCacheRepositoryAdapter : IPlanCacheRepository
    {
        private IDbPlanCacheRepository planCacheRepository;

        public PlanCacheRepositoryAdapter(IDbPlanCacheRepository planCacheRepository)
        {
            this.planCacheRepository = planCacheRepository;
        }

        public async Task<IEnumerable<PlanCacheItem>> GetPlanCache(DateTime collectMetricsFrom, DateTime includeHistoricalItemsFrom)
        {
            var lookUp = await this.planCacheRepository.GetObjectIdAndProcedureNames();

            return
                (await this.planCacheRepository.GetCurrentPlanCache(collectMetricsFrom))
                .Concat(await this.planCacheRepository.GetHistoricalPlanCache(includeHistoricalItemsFrom))
                .Where(p => IsNotInternalObject(p))
                .Where(p => ProcedureInSysObjects(p, lookUp))
                .Select(p => { p.SpName = lookUp[p.ObjectId]; return p; });

            bool IsNotInternalObject(PlanCacheItem p) => p.ObjectId > 0;

            bool ProcedureInSysObjects(PlanCacheItem panCacheItem, Dictionary<int, string> lookUp) => lookUp.ContainsKey(panCacheItem.ObjectId);
        }

        public Task<IEnumerable<PlanCacheItem>> GetPreviousPlanCacheItems()
        {
            throw new NotImplementedException();
        }
    }
}