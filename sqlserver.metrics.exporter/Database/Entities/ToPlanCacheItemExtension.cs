﻿using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Exporter.Datebase.Entities;
using System.IO;
using System.Xml.Serialization;

namespace SqlServer.Metrics.Exporter.Database.Entities
{
    public static class ToPlanCacheItemExtension
    {
	    private static readonly XmlSerializer SerializerRemovalStats;
	    private static readonly XmlSerializer SerializerExecutionStats;

	    static ToPlanCacheItemExtension()
	    {
		    var attributes = new XmlAttributes();
		    attributes.Xmlns = false;
		    var attributesOverrides = new XmlAttributeOverrides();
		    attributesOverrides.Add(typeof(QueryCacheRemovalStatisticsXmlItem), attributes);
		    SerializerRemovalStats = 
			    new XmlSerializer(
				    typeof(QueryCacheRemovalStatisticsXmlItem),
				    attributesOverrides,
				    null,
				    new XmlRootAttribute("event"),
				    null);
		    
		    var attributesOverridesExecutionStats = new XmlAttributeOverrides();
		    attributesOverridesExecutionStats.Add(typeof(ProcedureExecutionStatisticsXmlItem), attributes);
		    SerializerExecutionStats = 
			    new XmlSerializer(
				    typeof(ProcedureExecutionStatisticsXmlItem),
				    attributesOverridesExecutionStats,
				    null,
				    new XmlRootAttribute("ProcedureExecutionStats"),
				    null);
	    }
	    
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
					},
					PageSpills = new Provider.PageSpills()
                    {
						Last = dbCacheItem.last_spills,
						Max = dbCacheItem.max_spills,
						Min = dbCacheItem.min_spills,
						Total = dbCacheItem.total_spills
					}
				}
			};
		}

        public static PlanCacheItem ToPlanCacheItem(this DbHistoricalCacheItem dbHistoricalCacheItem)
        {
			
			using var textReaderRemovalStats = new StringReader(dbHistoricalCacheItem.event_data);
			var removalStatistics = (QueryCacheRemovalStatisticsXmlItem)SerializerRemovalStats.Deserialize(textReaderRemovalStats);
			
			using var textReaderExecutionStats = new StringReader(removalStatistics.Data[7].Value);
			var executionStatisticsXml = (ProcedureExecutionStatisticsXmlItem)SerializerExecutionStats.Deserialize(textReaderExecutionStats);
			return new PlanCacheItem()
			{
				ExecutionStatistics = executionStatisticsXml.ToExecutionStatistics(),
				ObjectId = int.Parse(removalStatistics.Data[2].Value),
				RemovedFromCacheAt = dbHistoricalCacheItem.timestamp_utc
			};
        }

		private static ProcedureExecutionStatistics ToExecutionStatistics(this ProcedureExecutionStatisticsXmlItem executionStatisticsXml)
        {
			return new Provider.ProcedureExecutionStatistics()
			{
				GeneralStats = new Provider.GeneralStats()
				{
					CachedTime = executionStatisticsXml.GeneralStats.CachedTime,
					ExecutionCount = executionStatisticsXml.GeneralStats.ExecutionCount,
					LastExecutionTime = executionStatisticsXml.GeneralStats.LastExecutionTime
				},
				ElapsedTime = new Provider.ElapsedTime()
				{
					Last = executionStatisticsXml.ElapsedTime.Last,
					Max = executionStatisticsXml.ElapsedTime.Max,
					Min = executionStatisticsXml.ElapsedTime.Min,
					Total = executionStatisticsXml.ElapsedTime.Total
				},
				LogicalReads = new Provider.LogicalReads()
				{
					Last = executionStatisticsXml.LogicalReads.Last,
					Max = executionStatisticsXml.LogicalReads.Max,
					Min = executionStatisticsXml.LogicalReads.Min,
					Total = executionStatisticsXml.LogicalReads.Total
				},
				PageServerReads = new Provider.PageServerReads()
				{
					Last = executionStatisticsXml.PageServerReads.Last,
					Max = executionStatisticsXml.PageServerReads.Max,
					Min = executionStatisticsXml.PageServerReads.Min,
					Total = executionStatisticsXml.PageServerReads.Total
				},
				LogicalWrites = new Provider.LogicalWrites()
				{
					Last = executionStatisticsXml.LogicalWrites.Last,
					Max = executionStatisticsXml.LogicalWrites.Max,
					Min = executionStatisticsXml.LogicalWrites.Min,
					Total = executionStatisticsXml.LogicalWrites.Total
				},
				PhysicalReads = new Provider.PhysicalReads()
				{
					Last = executionStatisticsXml.PhysicalReads.Last,
					Max = executionStatisticsXml.PhysicalReads.Max,
					Min = executionStatisticsXml.PhysicalReads.Min,
					Total = executionStatisticsXml.PhysicalReads.Total
				},
				WorkerTime = new Provider.WorkerTime()
				{
					Last = executionStatisticsXml.WorkerTime.Last,
					Max = executionStatisticsXml.WorkerTime.Max,
					Min = executionStatisticsXml.WorkerTime.Min,
					Total = executionStatisticsXml.WorkerTime.Total
				},
				PageSpills = new Provider.PageSpills()
			};
		}
	}
}