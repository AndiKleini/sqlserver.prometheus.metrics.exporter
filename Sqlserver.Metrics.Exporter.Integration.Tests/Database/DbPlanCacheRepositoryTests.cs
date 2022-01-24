using Dapper;
using FluentAssertions;
using NUnit.Framework;
using Sqlserver.Metrics.Exporter.Database;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sqlserver.Metrics.Exporter.Integration.Tests.Database
{
    [TestFixture]
    public class DbPlanCacheRepositoryTests
    {
        private const string ConnectionString = "Data Source=.;Initial Catalog=restifysp;Integrated Security=True;";

        [Test]
        public async Task GetCurrentPlanCache()
        {
            var instanceUnderTest = new DbPlanCacheRepository(ConnectionString);
            DateTime from = DateTime.Now;
            this.ExecuteStoredProceduresToFillUpCache();
            
            List<PlanCacheItem> items = await instanceUnderTest.GetCurrentPlanCache(from);

            // TODO: make better integration tests here
            // the object_id should be extracted automatically
            items.Should().Contain(p => p.ObjectId == 978102525);
        }

        [Test]
        [Ignore("Not yet implemented")]
        public async Task GetHistoricalPlanCache()
        {
            var instanceUnderTest = new DbPlanCacheRepository(ConnectionString);
            DateTime from = DateTime.Now;
            this.ExecuteStoredProceduresToFillUpCache();
            await Task.Delay(TimeSpan.FromMinutes(1));

            List<PlanCacheItem> items = await instanceUnderTest.GetHistoricalPlanCache(from);

            // TODO create dedciated stored procedure for this kind of test
            items.Should().Contain(p => p.SpName == "getStoredProcedureMetricsFromCache");
        }

        [Test]
        public async Task GetObjectIdAndProcedureNames()
        {
            var instanceUnderTest = new DbPlanCacheRepository(ConnectionString);

            var items = await instanceUnderTest.GetObjectIdAndProcedureNames();

            items.Should().ContainValue("getProcedures");
        }

        private void ExecuteStoredProceduresToFillUpCache()
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@fromUtc", DateTime.Now, DbType.DateTime2, ParameterDirection.Input);

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var affectedRows = connection.Execute("monitoring.getStoredProcedureMetricsFromCache", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
