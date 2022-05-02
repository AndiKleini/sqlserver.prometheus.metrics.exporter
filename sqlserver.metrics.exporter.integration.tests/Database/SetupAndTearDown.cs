using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Sqlserver.Metrics.Exporter.Integration.Tests.Database
{
    public class SetupAndTearDown
    {
        public async Task<int> PrepareTestDatabase()
        {
            await this.ExecuteScript(Configuration.SetupScript);
            return await this.GetObjectIdOfProcedure(Configuration.TestProcedureName);
        }

        public async Task CleanUpDataBase()
        {
            await this.ExecuteScript(Configuration.TearDownScript);
        }

        private async Task ExecuteScript(string scriptFile)
        {
            string script = File.ReadAllText($"{Configuration.ScriptPath}{scriptFile}");
            using var connection = new SqlConnection(TestConfig.MasterDbConnectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            foreach (var batch in script.Split(new string[] { "GO", "go", "Go" }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                command.CommandText = batch;
                await command.ExecuteScalarAsync();
            }
        }

        private async Task<int> GetObjectIdOfProcedure(string procedure)
        {
            string queryObjectIdToStoredProcedure = $"select OBJECT_ID from sys.objects where name = '{Configuration.TestProcedureName}'";
            using var connection = new SqlConnection(Configuration.ConnectionString);
            connection.Open();
            return await connection.ExecuteScalarAsync<int>(queryObjectIdToStoredProcedure);
        }
    }
}
