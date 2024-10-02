namespace Sqlserver.Metrics.Exporter.Integration.Tests.Database
{
    public static class Configuration
    {
        public const string MasterDbConnectionString =
            "Server=localhost;Database=master;User Id=<your user>;Password=<your password>; TrustServerCertificate=True;";
        public const string ConnectionString = "Server=localhost;Database=monitoringtesting;Id=<your user>;Password=<your password>; TrustServerCertificate=True; ";
        public const string XEventPath = "/tmp/*.xel*";
        public const string TestProcedureSchema = "metrics";
        public const string TestProcedureName = "doSomething1";
        public const string ScriptPath = "./Database/Scripts/";
        public const string SetupScript = "SetUpTestDataBase.txt";
        public const string TearDownScript = "DropTestDataBase.txt";
    }
}
