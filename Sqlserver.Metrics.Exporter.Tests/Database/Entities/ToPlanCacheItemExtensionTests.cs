using FluentAssertions;
using NUnit.Framework;
using Sqlserver.Metrics.Exporter.Database.Entities;
using SqlServer.Metrics.Provider;
using System;

namespace Sqlserver.Metrics.Exporter.Tests.Database.Entities
{
    [TestFixture]
    public class ToPlanCacheItemExtensionTests
    {
		const string spName = "MySP";

		private const string historicalItemXml = 
			"<event name=\"query_cache_removal_statistics\" package=\"sqlserver\" timestamp=\"2022 - 01 - 12T05:28:42.992Z\">"+
			"<data name=\"recompile_count\"><value>0</value></data><data name=\"compiled_object_type\"><value>2</value><text>"+"" +
			"<![CDATA[Stored Procedure]]></text></data><data name=\"compiled_object_id\"><value>-524454186</value></data><data name=\"begin_offset\">"+"" +
			"<value>0</value></data><data name=\"end_offset\"><value>-1</value></data><data name=\"plan_handle\">"+"" +
			"<value>0500ff7fd676bde050e11d7c2202000001000000000000000000000000000000000000000000000000000000</value></data>"+"" +
			"<data name=\"sql_handle\"><value>0400ff7fd676bde0010000000000000000000000000000000000000000000000000000000000000000000000</value></data>"+"" +
			"<data name=\"execution_statistics\"><value>"+"" +
			"<![CDATA[<ProcedureExecutionStats><GeneralStats ExecutionCount=\"2\" LastExecutionTime=\"2022 - 01 - 12 06:28:10.793\" CachedTime=\"2022 - 01 - 12 06:28:10.690\"/>"+"" +
			"<WorkerTime Total=\"4505\" Last=\"1715\" Min=\"1715\" Max=\"2790\"/><ElapsedTime Total=\"8062\" Last=\"3392\" Min=\"3392\" Max=\"4670\"/>"+"" +
			"<LogicalWrites Total=\"0\" Last=\"0\" Min=\"0\" Max=\"0\"/><PageServerReads Total=\"0\" Last=\"0\" Min=\"0\" Max=\"0\"/>"+"" +
			"<PhysicalReads Total=\"0\" Last=\"0\" Min=\"0\" Max=\"0\"/><LogicalReads Total=\"88\" Last=\"44\" Min=\"44\" Max=\"44\"/>"+"" +
			"</ProcedureExecutionStats>]]></value></data></event>";

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
			DbCacheItem dbCacheItem = 
                new DbCacheItem()
                {
					name= spName,
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
				SpName = spName,
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
		[Ignore("Not yet implemented")]
		public void ToPlancCacheItem_GivenHistoricalCacheItem_ReturnsCorrepondingPlanCacheItem()
        {
			const string spName = "MySP";
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
            DateTime timestampOfEnCache = DateTime.Now;
			DbHistoricalCacheItem dbHistoricalCacheItem = new DbHistoricalCacheItem()
			{
				timestamp_utc = timestampOfEnCache,
				event_data = historicalItemXml
            };
			PlanCacheItem expectedResult = new PlanCacheItem()
			{
				SpName = spName,
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

	}
}
