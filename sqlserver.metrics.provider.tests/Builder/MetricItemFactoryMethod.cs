namespace SqlServer.Metrics.Provider.Tests.Builder
{
    internal class MetricItemFactoryMethod
    {
        public static MetricItem GetMetricItem(string storedProcedureName, string metricName, int averageElapsed)
        {
            return new MetricItem()
            {
                Name = $"MSSQL_{metricName}{{\"storedprocedure={storedProcedureName}\"}}",
                Value = averageElapsed
            };
        }
    }
}