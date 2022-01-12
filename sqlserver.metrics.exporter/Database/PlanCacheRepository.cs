
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
    public class PlanCacheRepository
    {
        private string connectionString = "Data Source=.;Initial Catalog=restifysp;Integrated Security=True;";

        public PlanCacheRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<PlanCacheItem>> GetPlanCache(DateTime from)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();
            var result = await connection.QueryAsync<DbCacheItem>(
                "monitoring.getStoredProcedureMetricsFromCache", 
                new { fromUtc = from }, 
                commandType: CommandType.StoredProcedure);
            return result.Select(r => r.ToPlanCacheItem()).ToList();
        }
    }
}