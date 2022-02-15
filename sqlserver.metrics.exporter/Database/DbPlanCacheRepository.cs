﻿
using Dapper;
using SqlServer.Metrics.Exporter.Database.Entities;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Exporter.Database
{
    public class DbPlanCacheRepository : IDbPlanCacheRepository
    {
        private readonly string connectionString;

        public DbPlanCacheRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<PlanCacheItem>> GetCurrentPlanCache(DateTime from)
        {
            using var connection = new SqlConnection(this.connectionString);
            var result = await connection.QueryAsync<DbCacheItem>(
                "monitoring.getStoredProcedureMetricsFromCache",
                new { fromUtc = from },
                commandType: CommandType.StoredProcedure);
            return result.Select(r => r.ToPlanCacheItem()).ToList();
        }

        public async Task<List<PlanCacheItem>> GetHistoricalPlanCache(DateTime from)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();
            var result = await connection.QueryAsync<DbHistoricalCacheItem>(
                "monitoring.getStoredProcedureMetricsHistorical",
                new { fromUtc = from },
                commandType: CommandType.StoredProcedure);
            return result.Select(r => r.ToPlanCacheItem()).ToList();
        }

        public async Task<Dictionary<int, string>> GetObjectIdAndProcedureNames()
        {
            using var connection = new SqlConnection(this.connectionString);
            var procedures = await connection.QueryAsync(
                "monitoring.getProcedures",
                commandType: CommandType.StoredProcedure);
            return procedures.ToDictionary(item => (int)item.object_id, item => (string)item.Name);
        }
    }
}