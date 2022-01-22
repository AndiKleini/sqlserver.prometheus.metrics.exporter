﻿using Sqlserver.Metrics.Provider;
using SqlServer.Metrics.Provider;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Sqlserver.Metrics.Exporter.Database.Entities
{
    public static class ToPlanCacheItemExtension
    {
        public static PlanCacheItem ToPlanCacheItem(this DbCacheItem dbCacheItem)
        {
			return new PlanCacheItem()
			{
				ObjectId = dbCacheItem.object_Id,
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

        public static PlanCacheItem ToPlanCacheItem(this DbHistoricalCacheItem dbHistoricalCacheItem)
        {
			var attributes = new XmlAttributes();
			attributes.Xmlns = false;
			var attributesOverrides = new XmlAttributeOverrides();
			attributesOverrides.Add(typeof(QueryCacheRemovalStatistics), attributes);
			var serializerRemovalStats = 
				new XmlSerializer(
					typeof(QueryCacheRemovalStatistics),
					attributesOverrides,
					null,
					new XmlRootAttribute("event"),
					null);
			using var textReaderRemovalStats = new StringReader(dbHistoricalCacheItem.event_data);
			var removalStatistics = (QueryCacheRemovalStatistics)serializerRemovalStats.Deserialize(textReaderRemovalStats);
			var attributesOverridesExecutionStats = new XmlAttributeOverrides();
			attributesOverridesExecutionStats.Add(typeof(ProcedureExecutionStatistics), attributes);
			var serializerExecutionStats = 
				new XmlSerializer(
					typeof(ProcedureExecutionStatistics),
					attributesOverridesExecutionStats,
					null,
					new XmlRootAttribute("ProcedureExecutionStats"),
					null);
			using var textReaderExecutionStats = new StringReader(removalStatistics.Data[7].Value);
			var executionStatistics = (ProcedureExecutionStatistics)serializerExecutionStats.Deserialize(textReaderExecutionStats);
			return new PlanCacheItem()
			{
				ExecutionStatistics = executionStatistics,
				ObjectId = int.Parse(removalStatistics.Data[2].Value),
				RemovedFromCacheAt = dbHistoricalCacheItem.timestamp_utc
			};
        }
    }
}