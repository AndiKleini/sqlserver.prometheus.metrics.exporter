using FluentAssertions;
using Moq;
using NUnit.Framework;
using SqlServer.Metrics.Exporter.Adapters;
using SqlServer.Metrics.Exporter.Database;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Exporter.Tests.Database
{
    [TestFixture]
    public class DbCacheRepositoryAdapterTests
    {
        private const int ObjectID_A = 1234;
        private const int ObjectID_B = 123434567;
        private const string ProcedureName_A = "MySp_A";
        private const string procedureName_B = "MySp_B";
        const int executionCount = 12;
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
        private DateTime CachedTime_Procedure_A = DateTime.Parse("14/01/22 12:00:34");
        private DateTime CachedTime_Procedure_B = DateTime.Parse("14/01/22 12:01:34");
        private DateTime CachedTime_Historical_Procedure_A = DateTime.Parse("14/01/22 12:03:34");
        private DateTime CachedTime_Historical_Procedure_B = DateTime.Parse("14/01/22 12:04:34");
        private DateTime LastExecutionTime_Procedure_A = DateTime.Parse("14/01/22 12:00:34");
        private DateTime LastExecutionTime_Procedure_B = DateTime.Parse("14/01/22 12:01:34");
        private DateTime LastExecutionTime_Historical_Procedure_A = DateTime.Parse("14/01/22 12:03:34");
        private DateTime LastExecutionTime_Historical_Procedure_B = DateTime.Parse("14/01/22 12:04:34");

        [Test]
        public async Task GetPlanCache_Returns_Historical_And_CurrentItems()
        {
            var planCacheRepositoryMock = new Mock<IDbPlanCacheRepository>();
            planCacheRepositoryMock.Setup(s => s.GetCurrentPlanCache(It.IsAny<DateTime>())).ReturnsAsync(this.GetCurrentPlanCacheItems());
            planCacheRepositoryMock.Setup(s => s.GetHistoricalPlanCache(It.IsAny<DateTime>())).ReturnsAsync(this.GetHistoricalPlanCacheItems());
            planCacheRepositoryMock.Setup(s => s.GetObjectIdAndProcedureNames()).ReturnsAsync(GetObjectIds());

            var instanceUnderTest = new PlanCacheRepositoryAdapter(planCacheRepositoryMock.Object);

            IEnumerable<PlanCacheItem> combinedPlanCacheItems = await instanceUnderTest.GetPlanCache(DateTime.Now);

            combinedPlanCacheItems.Should().BeEquivalentTo(
                this.GetCurrentPlanCacheItemsWithResolvedName().Concat(this.GetHistoricalPlanCacheItemsWithResolvedName()));
        }

        private static Dictionary<int, string> GetObjectIds()
        {
            return new Dictionary<int, string>()
                {
                    { ObjectID_A, ProcedureName_A },
                    { ObjectID_B, procedureName_B }
                };
        }

        private List<PlanCacheItem> GetCurrentPlanCacheItems()
        {
            return new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        SpName = ProcedureName_A,
                        ObjectId = ObjectID_A,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Procedure_A,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Procedure_A
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
                    },
                    new PlanCacheItem()
                    {
                        SpName = procedureName_B,
                        ObjectId = ObjectID_B,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Procedure_B,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Procedure_B
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
                    }
                };
        }

        private List<PlanCacheItem> GetCurrentPlanCacheItemsWithResolvedName()
        {
            return new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        SpName = ProcedureName_A,
                        ObjectId = ObjectID_A,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Procedure_A,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Procedure_A
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
                    },
                    new PlanCacheItem()
                    {
                        SpName = procedureName_B,
                        ObjectId = ObjectID_B,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Procedure_B,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Procedure_B
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
                    }
                };
        }

        private List<PlanCacheItem> GetHistoricalPlanCacheItems()
        {
            return new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        ObjectId = ObjectID_A,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Historical_Procedure_A,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Historical_Procedure_A
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
                    },
                    new PlanCacheItem()
                    {
                        ObjectId = ObjectID_B,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Historical_Procedure_B,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Historical_Procedure_B
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
                    }
                };
        }

        private List<PlanCacheItem> GetHistoricalPlanCacheItemsWithResolvedName()
        {
           
            return new List<PlanCacheItem>()
                {
                    new PlanCacheItem()
                    {
                        SpName = ProcedureName_A,
                        ObjectId = ObjectID_A,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Historical_Procedure_A,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Historical_Procedure_A
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
                    },
                    new PlanCacheItem()
                    {
                        SpName = procedureName_B,
                        ObjectId = ObjectID_B,
                        ExecutionStatistics = new Provider.ProcedureExecutionStatistics()
                        {
                            GeneralStats = new Provider.GeneralStats()
                            {
                                CachedTime = CachedTime_Historical_Procedure_B,
                                ExecutionCount = executionCount,
                                LastExecutionTime = LastExecutionTime_Historical_Procedure_B
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
                    }
                };
        }
    }
}
