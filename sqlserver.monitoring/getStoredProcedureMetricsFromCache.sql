create PROCEDURE [monitoring].[getStoredProcedureMetricsFromCache]  
   @fromUtc datetime2
AS 
Begin
	select 
	    obj.name,
		cached_time,
		last_execution_time,
		execution_count,
		total_elapsed_time,
		last_elapsed_time,
		min_elapsed_time,
		max_elapsed_time,
		total_worker_time,
		last_worker_time,
		min_worker_time,
		max_worker_time,
		total_logical_reads,
		last_logical_reads,
		min_logical_reads,
		max_logical_reads,
		total_physical_reads,
		last_physical_reads,
		min_physical_reads,
		max_physical_reads,
		total_logical_writes,
		last_logical_writes,
		min_logical_writes,
		max_logical_writes,
		total_spills,
		last_spills,
		min_spills,
		max_spills,
		total_page_server_reads,
		last_page_server_reads,
		min_page_server_reads,
		max_page_server_reads
	from sys.dm_exec_procedure_stats stats
	inner join sys.objects obj
		on obj.object_id = stats.object_id
	where stats.last_execution_time >= @fromUtc 
	and stats.type = 'P';
End
