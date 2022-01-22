using FluentAssertions;
using NUnit.Framework;
using Sqlserver.Metrics.Exporter.Database.Entities;
using Sqlserver.Metrics.Provider;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using static Sqlserver.Metrics.Exporter.QueryCacheRemovalStatistics;

namespace Sqlserver.Metrics.Exporter.Tests.Database.Entities
{
    [TestFixture]
    public class ToPlanCacheItemExtensionTests
    {
		[Test]
        public void ToPlancCacheItem_GivenDbCacheItem_ReturnsCorrepondingPlanCacheItem()
        {
            DateTime cachedTime = DateTime.Now;
            const int executionCount = 12;
            DateTime lastExecutionTime = DateTime.Now;
            const int totalElapsed = 7867;
            const int lastElapsed = 71;
            const int minElapsed = 11;
            const int maxElapsed = 781;
            const int totalLogicalReads = 1132;
			const int minLogicalReads = 11;
			const int maxLogicalReads = 112;
			const int lastLogicalReads = 78;
            const int totalPageReads = 3423;
			const int minPageReads = 34;
			const int maxPageReads = 342;
			const int lastPageReads = 234;
            const int totalLogicalWrites = 2345;
			const int minLogicalWrites = 12;
			const int maxLogicalWrites = 234;
			const int lastLogicalWrites = 123;
            const int totalPhysicalReads = 34456;
			const int lastPhysicalReads = 34;
			const int minPhysicalReads = 20;
			const int maxPhysicalReads = 341;
            const int totalWorkerTime = 2314;
			const int maxWorkerTime = 56;
			const int minWorkerTime = 23;
			const int lastWorkerTime = 231;
            const int objecId = 1234;
			DbCacheItem dbCacheItem = 
                new DbCacheItem()
                {
					object_Id = objecId,
					cached_time= cachedTime,
					last_execution_time= lastExecutionTime,
					execution_count= executionCount,
					total_elapsed_time= totalElapsed,
					last_elapsed_time= lastElapsed,
					min_elapsed_time= minElapsed,
					max_elapsed_time= maxElapsed,
					total_worker_time= totalWorkerTime,
					last_worker_time= lastWorkerTime,
					min_worker_time= minWorkerTime,
					max_worker_time= maxWorkerTime,
					total_logical_reads= totalLogicalReads,
					last_logical_reads= lastLogicalReads,
					min_logical_reads= minLogicalReads,
					max_logical_reads= maxLogicalReads,
					total_physical_reads= totalPhysicalReads,
					last_physical_reads= lastPhysicalReads,
					min_physical_reads= minPhysicalReads,
					max_physical_reads= maxPhysicalReads,
					total_logical_writes= totalLogicalWrites,
					last_logical_writes= lastLogicalWrites,
					min_logical_writes= minLogicalWrites,
					max_logical_writes= maxLogicalWrites,
					total_spills= 123,
					last_spills= 143,
					min_spills= 567,
					max_spills= 897,
					total_page_server_reads= totalPageReads,
					last_page_server_reads= lastPageReads,
					min_page_server_reads= minPageReads,
					max_page_server_reads = maxPageReads
				};
			PlanCacheItem expectedResult = new PlanCacheItem()
			{
				ObjectId = objecId,
				ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
				{
					GeneralStats = new Provider.GeneralStats()
					{
						CachedTime = cachedTime,
						ExecutionCount = executionCount,
						LastExecutionTime = lastExecutionTime
					},
					ElapsedTime = new Provider.ElapsedTime()
					{
						Last = lastElapsed,
						Max = maxElapsed,
						Min = minElapsed,
						Total = totalElapsed
					},
					LogicalReads = new Provider.LogicalReads()
					{
						Last = lastLogicalReads,
						Max = maxLogicalReads,
						Min = minLogicalReads,
						Total = totalLogicalReads
					},
					PageServerReads = new Provider.PageServerReads()
					{
						Last = lastPageReads,
						Max = maxPageReads,
						Min = minPageReads,
						Total = totalPageReads
					},
					LogicalWrites = new Provider.LogicalWrites()
					{
						Last = lastLogicalWrites,
						Max = maxLogicalWrites,
						Min = minLogicalWrites,
						Total = totalLogicalWrites
					},
					PhysicalReads = new Provider.PhysicalReads()
					{
						Last = lastPhysicalReads,
						Max = maxPhysicalReads,
						Min = minPhysicalReads,
						Total = totalPhysicalReads
					},
					WorkerTime = new Provider.WorkerTime()
					{
						Last = lastWorkerTime,
						Max = maxWorkerTime,
						Min = minWorkerTime,
						Total = totalWorkerTime
					}
				}
			};

            PlanCacheItem result = ToPlanCacheItemExtension.ToPlanCacheItem(dbCacheItem);

			result.Should().BeEquivalentTo(expectedResult);
        }

		[Test]
		public void ToPlancCacheItem_GivenHistoricalCacheItem_ReturnsCorrepondingPlanCacheItem()
        {
			const int objectId = 1234;
			DateTime cachedTime = DateTime.Now;
			DateTime removedFromCacheTime = DateTime.Now;
			const int executionCount = 12;
			DateTime lastExecutionTime = DateTime.Now;
			const int totalElapsed = 7867;
			const int lastElapsed = 71;
			const int minElapsed = 11;
			const int maxElapsed = 781;
			const int totalLogicalReads = 1132;
			const int minLogicalReads = 11;
			const int maxLogicalReads = 112;
			const int lastLogicalReads = 78;
			const int totalPageReads = 3423;
			const int minPageReads = 34;
			const int maxPageReads = 342;
			const int lastPageReads = 234;
			const int totalLogicalWrites = 2345;
			const int minLogicalWrites = 12;
			const int maxLogicalWrites = 234;
			const int lastLogicalWrites = 123;
			const int totalPhysicalReads = 34456;
			const int lastPhysicalReads = 34;
			const int minPhysicalReads = 20;
			const int maxPhysicalReads = 341;
			const int totalWorkerTime = 2314;
			const int maxWorkerTime = 56;
			const int minWorkerTime = 23;
			const int lastWorkerTime = 231;
            DateTime timestampOfEnCache = DateTime.Now;
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffff";
            string executionStatisticsXml =
				"<ProcedureExecutionStats>" +
				$"<GeneralStats ExecutionCount=\"{executionCount}\" LastExecutionTime=\"{lastExecutionTime.ToString(dateTimeFormat)}\" CachedTime=\"{cachedTime.ToString(dateTimeFormat)}\"/>" +
				$"<WorkerTime Total =\"{totalWorkerTime}\" Last=\"{lastWorkerTime}\" Min=\"{minWorkerTime}\" Max=\"{maxWorkerTime}\"/>" +
				$"<ElapsedTime Total =\"{totalElapsed}\" Last=\"{lastElapsed}\" Min=\"{minElapsed}\" Max=\"{maxElapsed}\"/>" +
				$"<LogicalWrites Total =\"{totalLogicalWrites}\" Last=\"{lastLogicalWrites}\" Min=\"{minLogicalWrites}\" Max=\"{maxLogicalWrites}\"/>" +
				$"<PageServerReads Total =\"{totalPageReads}\" Last=\"{lastPageReads}\" Min=\"{minPageReads}\" Max=\"{maxPageReads}\"/>" +
				$"<PhysicalReads Total =\"{totalPhysicalReads}\" Last=\"{lastPhysicalReads}\" Min=\"{minPhysicalReads}\" Max=\"{maxPhysicalReads}\"/>" +
				$"<LogicalReads Total =\"{totalLogicalReads}\" Last=\"{lastLogicalReads}\" Min=\"{minLogicalReads}\" Max=\"{maxLogicalReads}\"/>" +
				"</ProcedureExecutionStats>";
			string queryCacheRemovalStatisticsXml =
				$"<event name=\"query_cache_removal_statistics\" package=\"sqlserver\" timestamp=\"{removedFromCacheTime.ToString(dateTimeFormat)}\">" +
                    "<data name=\"recompile_count\">" +
                        "<value>0</value>" +
					"</data>" +
					"<data name=\"compiled_object_type\">" +
						"<value>2</value> "  +
					"</data>" +
					"<data name=\"compiled_object_id\">" +
						$"<value>{objectId}</value>" +
					"</data>" +
					"<data name=\"begin_offset\">" +
						"<value>0</value>" +
					"</data>" +
					"<data name=\"end_offset\">" +
						"<value>-1</value>" +
					"</data>" +
					"<data name=\"plan_handle\">" +
						"<value>0500ff7f5436f6cb50015294de01000001000000000000000000000000000000000000000000000000000000</value>" +
					"</data>" +
					"<data name=\"sql_handle\">" +
						"<value>0400ff7f5436f6cb010000000000000000000000000000000000000000000000000000000000000000000000</value>" +
					"</data>" +
					"<data name=\"execution_statistics\">" +
						$"<value><![CDATA[{executionStatisticsXml}]]></value>" +
					"</data>" +
                "</event>";
			DbHistoricalCacheItem dbHistoricalCacheItem = new DbHistoricalCacheItem()
			{
				timestamp_utc = timestampOfEnCache,
				event_data = queryCacheRemovalStatisticsXml
            };
			PlanCacheItem expectedResult = new PlanCacheItem()
			{
				RemovedFromCacheAt = timestampOfEnCache,
				SpName = null,
				ObjectId = objectId,
				ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
				{
					GeneralStats = new Provider.GeneralStats()
					{
						CachedTime = cachedTime,
						ExecutionCount = executionCount,
						LastExecutionTime = lastExecutionTime
					},
					ElapsedTime = new Provider.ElapsedTime()
					{
						Last = lastElapsed,
						Max = maxElapsed,
						Min = minElapsed,
						Total = totalElapsed
					},
					LogicalReads = new Provider.LogicalReads()
					{
						Last = lastLogicalReads,
						Max = maxLogicalReads,
						Min = minLogicalReads,
						Total = totalLogicalReads
					},
					PageServerReads = new Provider.PageServerReads()
					{
						Last = lastPageReads,
						Max = maxPageReads,
						Min = minPageReads,
						Total = totalPageReads
					},
					LogicalWrites = new Provider.LogicalWrites()
					{
						Last = lastLogicalWrites,
						Max = maxLogicalWrites,
						Min = minLogicalWrites,
						Total = totalLogicalWrites
					},
					PhysicalReads = new Provider.PhysicalReads()
					{
						Last = lastPhysicalReads,
						Max = maxPhysicalReads,
						Min = minPhysicalReads,
						Total = totalPhysicalReads
					},
					WorkerTime = new Provider.WorkerTime()
					{
						Last = lastWorkerTime,
						Max = maxWorkerTime,
						Min = minWorkerTime,
						Total = totalWorkerTime
					}
				}
			};

			PlanCacheItem result = ToPlanCacheItemExtension.ToPlanCacheItem(dbHistoricalCacheItem);

			result.Should().BeEquivalentTo(expectedResult);
        }

		[Test]
		[Ignore("This is only for debugging XL Serialization purposes")]
		public void TestXml()
        {
			/*
            var item = new QueryCacheRemovalStatistics()
            {
                Timestamp = DateTime.Now.ToString(),
                Data = new List<Data>()
                {
                    new Data() { Name = "Andi", Value = "Hubert "},
                    new Data() { Name = "Andi", Value = "Hubert "},
                    new Data() { Name = "Andi", Value = "Hubert "},
                    new Data() { Name = "Andi", Value = "Hubert "},
                }
            };

            XmlSerializer ser = new XmlSerializer(typeof(QueryCacheRemovalStatistics), new XmlRootAttribute("event"));
            FileStream stream = File.OpenWrite("./myresult1");
            ser.Serialize(stream, item);
            stream.Close();
			*/

			var att = new XmlAttributes();
			att.Xmlns = false;
			var attOverrides = new XmlAttributeOverrides();
			attOverrides.Add(typeof(ProcedureExecutionStatistics), att);
            XmlSerializer ser2 = 
				new XmlSerializer(
					typeof(ProcedureExecutionStatistics),
					attOverrides, null,
					new XmlRootAttribute("ProcedureExecutionStats"), 
					null);
			FileStream stream2 = File.OpenRead("./myresult");
			var stats = (ProcedureExecutionStatistics)ser2.Deserialize(stream2);
			stats.Should().NotBeNull();
		}
	}
}
