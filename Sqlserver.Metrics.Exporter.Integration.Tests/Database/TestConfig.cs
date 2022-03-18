namespace Sqlserver.Metrics.Exporter.Integration.Tests
{
    internal class TestConfig
    {
        private static string testDbName = "monitoringtesting";

        private static string testDBConnectionString = $"Data Source=.;Initial Catalog={testDbName};Integrated Security=True;";

        private static string masterDBConnectionString = $"Data Source=.;Initial Catalog=master;Integrated Security=True;";

        public static string TestDbConnectionString => testDBConnectionString;

        public static string MasterDbConnectionString => masterDBConnectionString;
    }
}