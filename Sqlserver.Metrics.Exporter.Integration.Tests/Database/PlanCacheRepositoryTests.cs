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
    public class PlanCacheRepositoryTests
    {
        private const string ConnectionString = "Data Source=.;Initial Catalog=restifysp;Integrated Security=True;";

        [Test]
        public async Task GetPlanCache_DatabaseAvaliable()
        {
            var instanceUnderTest = new PlanCacheRepository(ConnectionString);
            DateTime from = DateTime.Now;
            this.ExecuteStoredProceduresToFillUpCache();
            
            List<PlanCacheItem> items = await instanceUnderTest.GetPlanCache(from);

            items.Should().Contain(p => p.SpName == "getStoredProcedureMetricsFromCache");
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
