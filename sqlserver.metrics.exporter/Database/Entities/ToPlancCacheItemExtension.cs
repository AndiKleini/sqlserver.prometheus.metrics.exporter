using SqlServer.Metrics.Provider;

namespace Sqlserver.Metrics.Exporter.Database.Entities
{
    public static class ToPlancCacheItemExtension
    {
        public static PlanCacheItem ToPlanCacheItem(this DbCacheItem dbCacheItem)
        {
			return new PlanCacheItem()
			{
				SpName = dbCacheItem.name,
				ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
				{
					GeneralStats = new Provider.GeneralStats()
					{
						CachedTime = dbCacheItem.cached_time,
						ExecutionCount = dbCacheItem.execution_count,
						LastExecutionTime = dbCacheItem.last_execution_time
					},
					ElapsedTime = new Provider.ElapsedTime()
					{
						Last = dbCacheItem.last_elapsed_time,
						Max = dbCacheItem.max_elapsed_time,
						Min = dbCacheItem.min_elapsed_time,
						Total = dbCacheItem.total_elapsed_time
					},
					LogicalReads = new Provider.LogicalReads()
					{
						Last = dbCacheItem.last_logical_reads,
						Max = dbCacheItem.max_logical_reads,
						Min = dbCacheItem.min_logical_reads,
						Total = dbCacheItem.total_logical_reads
					},
					PageServerReads = new Provider.PageServerReads()
					{
						Last = dbCacheItem.last_page_server_reads,
						Max = dbCacheItem.max_page_server_reads,
						Min = dbCacheItem.min_page_server_reads,
						Total = dbCacheItem.total_page_server_reads
					},
					LogicalWrites = new Provider.LogicalWrites()
					{
						Last = dbCacheItem.last_logical_writes,
						Max = dbCacheItem.max_logical_writes,
						Min = dbCacheItem.min_logical_writes,
						Total = dbCacheItem.total_logical_writes
					},
					PhysicalReads = new Provider.PhysicalReads()
					{
						Last = dbCacheItem.last_physical_reads,
						Max = dbCacheItem.max_physical_reads,
						Min = dbCacheItem.min_physical_reads,
						Total = dbCacheItem.total_physical_reads
					},
					WorkerTime = new Provider.WorkerTime()
					{
						Last = dbCacheItem.last_worker_time,
						Max = dbCacheItem.max_worker_time,
						Min = dbCacheItem.min_worker_time,
						Total = dbCacheItem.total_worker_time
					}
				}
			};
		}
    }
}