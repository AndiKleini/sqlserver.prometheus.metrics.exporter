using SqlServer.Metrics.Provider.Tests.Builder.Exposals;

namespace SqlServer.Metrics.Provider.Tests.Builder
{
    internal class MetricItemFactoryMethod
    {
        public static MetricItem GetMetricItem(string storedProcedureName, string metricName, int metricsValue)
        {
            return new MetricItem()
            {
                Name = new MetricBuilderBaseExposal().GetMetricsName(storedProcedureName, metricName),
                Value = metricsValue
            };
        }
    }
}