
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
        public PlanCacheRepository()
        {
        }

        public async Task<List<PlanCacheItem>> GetPlanCache(DateTime from)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@fromUtc", DateTime.Now, DbType.DateTime2, ParameterDirection.Input);

            using var connection = new SqlConnection("Data Source=.;Initial Catalog=restifysp;Integrated Security=True;");
            connection.Open();
            var result = await connection.QueryAsync<DbCacheItem>("monitoring.getStoredProcedureMetricsFromCache", new { fromUtc = from }, commandType: CommandType.StoredProcedure);
            return result.Select(r => r.ToPlanCacheItem()).ToList();
        }
    }
}