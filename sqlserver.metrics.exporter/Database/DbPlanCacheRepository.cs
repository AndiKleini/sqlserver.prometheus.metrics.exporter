
using Dapper;
using Sqlserver.Metrics.Exporter.Database.Entities;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sqlserver.Metrics.Exporter.Database
{
    public class DbPlanCacheRepository
    {
        private readonly string connectionString;

        public DbPlanCacheRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<PlanCacheItem>> GetCurrentPlanCache(DateTime from)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();
            var result = await connection.QueryAsync<DbCacheItem>(
                "monitoring.getStoredProcedureMetricsFromCache", 
                new { fromUtc = from }, 
                commandType: CommandType.StoredProcedure);
            return result.Select(r => r.ToPlanCacheItem()).ToList();
        }

        public async Task<List<PlanCacheItem>> GetHistoricalPlanCache(DateTime from)
        {
            throw new NotImplementedException();
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();
            var result = await connection.QueryAsync<DbHistoricalCacheItem>(
                "monitoring.getStoredProcedureMetricsHistorical",
                new { fromUtc = from },
                commandType: CommandType.StoredProcedure);
            return null; //  result.Select(r => r.ToPlanCacheItem()).ToList();
        }
    }
}