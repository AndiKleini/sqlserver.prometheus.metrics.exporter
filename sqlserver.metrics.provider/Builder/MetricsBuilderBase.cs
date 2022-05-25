namespace SqlServer.Metrics.Provider.Builder
{
    public class MetricsBuilderBase
    {

        protected string GetMetricsName(string procedureName, string metricsName) => $"MSSQL_{metricsName}{{storedprocedure=\"{procedureName}\"}}";
    }
}