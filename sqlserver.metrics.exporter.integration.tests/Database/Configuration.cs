namespace Sqlserver.Metrics.Exporter.Integration.Tests.Database
{
    public static class Configuration
    {
        public const string ConnectionString = "Data Source=.;Initial Catalog=monitoringtesting;Integrated Security=True;";
        public const string XEventPath = "C:\\temp\\*.xel";
        public const string TestProcedureSchema = "metrics";
        public const string TestProcedureName = "doSomething1";
        public const string ScriptPath = "./Database/Scripts/";
        public const string SetupScript = "SetUpTestDataBase.txt";
        public const string TearDownScript = "DropTestDataBase.txt";
    }
}
