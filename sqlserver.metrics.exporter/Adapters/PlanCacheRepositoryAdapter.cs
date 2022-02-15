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

        public async Task<IEnumerable<PlanCacheItem>> GetPlanCache(DateTime from)
        {
            var lookUp = await this.planCacheRepository.GetObjectIdAndProcedureNames();

            return
                (await this.planCacheRepository.GetCurrentPlanCache(from))
                .Concat(await this.planCacheRepository.GetHistoricalPlanCache(from))
                .Where(p => p.ObjectId > 0) // we have to ignore the internal objectIds 
                .Select(p => { p.SpName = lookUp[p.ObjectId] /* TODO: ignore if no value is emitted an write proper warning */; return p; });
        }

        public Task<IEnumerable<PlanCacheItem>> GetPreviousPlanCacheItems()
        {
            throw new NotImplementedException();
        }
    }
}