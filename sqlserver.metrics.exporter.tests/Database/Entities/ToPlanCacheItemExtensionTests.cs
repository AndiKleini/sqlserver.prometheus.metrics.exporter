using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Exporter.Database.Entities;
using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SqlServer.Metrics.Exporter.Tests.Database.Entities
{
    [TestFixture]
    public class ToPlanCacheItemExtensionTests
    {
        [Test]
        public void ToPlancCacheItem_GivenDbCacheItem_ReturnsCorrepondingPlanCacheItem()
        {
            DateTime cachedTime = DateTime.Now;
            const long executionCount = 12;
            DateTime lastExecutionTime = DateTime.Now;
            const long totalElapsed = 7867;
            const long lastElapsed = 71;
            const long minElapsed = 11;
            const long maxElapsed = 781;
            const long totalLogicalReads = 1132;
			const long minLogicalReads = 11;
			const long maxLogicalReads = 112;
			const long lastLogicalReads = 78;
            const long totalPageReads = 3423;
			const long minPageReads = 34;
			const long maxPageReads = 342;
			const long lastPageReads = 234;
            const long totalLogicalWrites = 2345;
			const long minLogicalWrites = 12;
			const long maxLogicalWrites = 234;
			const long lastLogicalWrites = 123;
            const long totalPhysicalReads = 34456;
			const long lastPhysicalReads = 34;
			const long minPhysicalReads = 20;
			const long maxPhysicalReads = 341;
            const long totalWorkerTime = 2314;
			const long maxWorkerTime = 56;
			const long minWorkerTime = 23;
			const long lastWorkerTime = 231;
			const int totalPageSpills = 213;
			const int lastPageSpills = 143;
			const int minPageSpills = 567;
			const int maxPageSpills = 897;
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
					total_spills= totalPageSpills,
					last_spills= lastPageSpills,
					min_spills= minPageSpills,
					max_spills= maxPageSpills,
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
					},
					PageSpills = new Provider.PageSpills() 
					{
						Last = lastPageSpills,
						Max = maxPageSpills,
						Min = minPageSpills,
						Total = totalPageSpills
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
			const long executionCount = 12;
			DateTime lastExecutionTime = DateTime.Now;
			const long totalElapsed = 7867;
			const long lastElapsed = 71;
			const long minElapsed = 11;
			const long maxElapsed = 781;
			const long totalLogicalReads = 1132;
			const long minLogicalReads = 11;
			const long maxLogicalReads = 112;
			const long lastLogicalReads = 78;
			const long totalPageReads = 3423;
			const long minPageReads = 34;
			const long maxPageReads = 342;
			const long lastPageReads = 234;
			const long totalLogicalWrites = 2345;
			const long minLogicalWrites = 12;
			const long maxLogicalWrites = 234;
			const long lastLogicalWrites = 123;
			const long totalPhysicalReads = 34456;
			const long lastPhysicalReads = 34;
			const long minPhysicalReads = 20;
			const long maxPhysicalReads = 341;
			const long totalWorkerTime = 2314;
			const long maxWorkerTime = 56;
			const long minWorkerTime = 23;
			const long lastWorkerTime = 231;
            DateTime timestampOfEnCache = DateTime.Now;
            const string queryCacheRemoveStatisticsDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";
            const string procedureExecutionStatisticsDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            string executionStatisticsXml =
				"<ProcedureExecutionStats>" +
				$"<GeneralStats ExecutionCount=\"{executionCount}\" LastExecutionTime=\"{lastExecutionTime.ToString(procedureExecutionStatisticsDateTimeFormat)}\" CachedTime=\"{cachedTime.ToString(procedureExecutionStatisticsDateTimeFormat)}\"/>" +
				$"<WorkerTime Total =\"{totalWorkerTime}\" Last=\"{lastWorkerTime}\" Min=\"{minWorkerTime}\" Max=\"{maxWorkerTime}\"/>" +
				$"<ElapsedTime Total =\"{totalElapsed}\" Last=\"{lastElapsed}\" Min=\"{minElapsed}\" Max=\"{maxElapsed}\"/>" +
				$"<LogicalWrites Total =\"{totalLogicalWrites}\" Last=\"{lastLogicalWrites}\" Min=\"{minLogicalWrites}\" Max=\"{maxLogicalWrites}\"/>" +
				$"<PageServerReads Total =\"{totalPageReads}\" Last=\"{lastPageReads}\" Min=\"{minPageReads}\" Max=\"{maxPageReads}\"/>" +
				$"<PhysicalReads Total =\"{totalPhysicalReads}\" Last=\"{lastPhysicalReads}\" Min=\"{minPhysicalReads}\" Max=\"{maxPhysicalReads}\"/>" +
				$"<LogicalReads Total =\"{totalLogicalReads}\" Last=\"{lastLogicalReads}\" Min=\"{minLogicalReads}\" Max=\"{maxLogicalReads}\"/>" +
				"</ProcedureExecutionStats>";
			string queryCacheRemovalStatisticsXml =
				$"<event name=\"query_cache_removal_statistics\" package=\"SqlServer\" timestamp=\"{removedFromCacheTime.ToString(queryCacheRemoveStatisticsDateTimeFormat)}\">" +
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
					},
					PageSpills = new PageSpills() { /* Page Spills will always be empty as these values are not included by the Extended Events XML.*/ }
				}
			};

			PlanCacheItem result = ToPlanCacheItemExtension.ToPlanCacheItem(dbHistoricalCacheItem);

			result.Should().BeEquivalentTo(
				expectedResult,
				options => options.Using<DateTime>(ctx =>
                {
                    TimeSpan precisionBelowOnEMillisecond = TimeSpan.FromTicks(9999);
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, precisionBelowOnEMillisecond);
                }).WhenTypeIs<DateTime>());
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
