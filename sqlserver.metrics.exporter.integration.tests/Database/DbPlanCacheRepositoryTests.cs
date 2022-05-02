using Dapper;
using FluentAssertions;
using NUnit.Framework;
using Sqlserver.Metrics.Exporter.Integration.Tests.Database;
using SqlServer.Metrics.Exporter.Database;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Exporter.Integration.Tests.Database
{
    [TestFixture]
    public class DbPlanCacheRepositoryTests
    {
        private const int MaxDispatchLatency = 30;
        private static int testProcedureObjectId;
        private static SetupAndTearDown setupAndTearDown = new SetupAndTearDown();

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            testProcedureObjectId = await setupAndTearDown.PrepareTestDatabase();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await setupAndTearDown.CleanUpDataBase();
        }

        [Test]
        public async Task GetCurrentPlanCache()
        {
            var instanceUnderTest = new DbPlanCacheRepository(Configuration.ConnectionString, Configuration.XEventPath);
            DateTime from = DateTime.Now;
            CancellationTokenSource cancelTestProcedureExecutions = new CancellationTokenSource();
            var executeTestProcedureInBackground = 
                new TaskFactory().StartNew(
                    async () =>
                    {
                        while (!cancelTestProcedureExecutions.Token.IsCancellationRequested)
                        {
                            await this.ExecuteStoredProceduresToFillUpCache();
                        };
                    }, 
                    cancelTestProcedureExecutions.Token);

            List<PlanCacheItem> items = await instanceUnderTest.GetCurrentPlanCache(from);

            cancelTestProcedureExecutions.Cancel();
            items.Should().Contain(p => p.ObjectId == testProcedureObjectId);
        }

        [Test]
        public async Task GetHistoricalPlanCache()
        {
            var instanceUnderTest = new DbPlanCacheRepository(Configuration.ConnectionString, Configuration.XEventPath);
            await this.ExecuteStoredProceduresToFillUpCache();
            await ClearAndWaitForCacheRemoveXEvent(MaxDispatchLatency);
            const int DispatchLatencyOffset = 2;
            DateTime from = DateTime.Now.AddSeconds(- MaxDispatchLatency - DispatchLatencyOffset);

            List<PlanCacheItem> items = await instanceUnderTest.GetHistoricalPlanCache(from);

            items.Should().Contain(p => p.ObjectId == testProcedureObjectId);
        }

        [Test]
        public async Task GetObjectIdAndProcedureNames()
        {
            var instanceUnderTest = new DbPlanCacheRepository(Configuration.ConnectionString, Configuration.XEventPath);

            var items = await instanceUnderTest.GetObjectIdAndProcedureNames();

            items.Should().ContainValue(Configuration.TestProcedureName);
        }

        private async Task ExecuteStoredProceduresToFillUpCache()
        {
            using var connection = new SqlConnection(Configuration.ConnectionString);
            connection.Open();
            var affectedRows = await connection.ExecuteAsync($"{Configuration.TestProcedureSchema}.{Configuration.TestProcedureName}", commandType: CommandType.StoredProcedure);
        }

        private async Task ClearAndWaitForCacheRemoveXEvent(int maxDispatchLatency)
        {
            using var connection = new SqlConnection(Configuration.ConnectionString);
            connection.Open();
            var affectedRows = await connection.ExecuteAsync("DBCC FREEPROCCACHE", commandType: CommandType.Text);
            await Task.Delay(TimeSpan.FromSeconds(maxDispatchLatency));
        }
    }
}
