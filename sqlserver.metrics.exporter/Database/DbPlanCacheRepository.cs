
using Dapper;
using SqlServer.Metrics.Exporter.Database.Entities;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace SqlServer.Metrics.Exporter.Database
{
    public class DbPlanCacheRepository : IDbPlanCacheRepository
    {
        private readonly string getCurrentPlanCacheQuery = "select object_Id, cached_time, last_execution_time, execution_count, total_elapsed_time, last_elapsed_time, min_elapsed_time, max_elapsed_time, total_worker_time, last_worker_time, min_worker_time, max_worker_time, total_logical_reads,	last_logical_reads,	min_logical_reads, max_logical_reads, total_physical_reads,	last_physical_reads, min_physical_reads, max_physical_reads, total_logical_writes, last_logical_writes, min_logical_writes,	max_logical_writes,	total_spills, last_spills, min_spills, max_spills, total_page_server_reads,	last_page_server_reads,	min_page_server_reads, max_page_server_reads from sys.dm_exec_procedure_stats stats	where stats.last_execution_time>=@fromUtc and stats.type='P';";
        private readonly string getHistoricalPlanCacheQuery = "select timestamp_utc, CAST(event_data AS VARCHAR(MAX)) as event_data FROM sys.fn_xe_file_target_read_file(@xEventPath, NULL, NULL, NULL) where CONVERT(datetime2, timestamp_utc) > dateadd(minute, datediff(minute, getdate(), getutcdate()), @fromUtc);";
        private readonly string getStoredProcedures = "select object_id, Name from sys.objects obj where obj.type='P'";
        private readonly string connectionString;
        private readonly string xEventPath;

        public DbPlanCacheRepository(string connectionString, string xEventPath)
        {
            this.connectionString = connectionString;
            this.xEventPath = xEventPath;
        }

        public async Task<List<PlanCacheItem>> GetCurrentPlanCache(DateTime from)
        {
            using var connection = new SqlConnection(this.connectionString);
            var result = await connection.QueryAsync<DbCacheItem>(
                getCurrentPlanCacheQuery,
                new { fromUtc = from },
                commandType: CommandType.Text);
            return result.Select(r => r.ToPlanCacheItem()).ToList();
        }

        public async Task<List<PlanCacheItem>> GetHistoricalPlanCache(DateTime from)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();
            var result = await connection.QueryAsync<DbHistoricalCacheItem>(
                getHistoricalPlanCacheQuery,
                new { fromUtc = from, xEventPath = this.xEventPath },
                commandType: CommandType.Text);
            return result.Select(r => r.ToPlanCacheItem()).ToList();
        }

        public async Task<Dictionary<int, string>> GetObjectIdAndProcedureNames()
        {
            using var connection = new SqlConnection(this.connectionString);
            var procedures = await connection.QueryAsync(
                getStoredProcedures,
                commandType: CommandType.Text);
            return procedures.ToDictionary(item => (int)item.object_id, item => (string)item.Name);
        }
    }
}