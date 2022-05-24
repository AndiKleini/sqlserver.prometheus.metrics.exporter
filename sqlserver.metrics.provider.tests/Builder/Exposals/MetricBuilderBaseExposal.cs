using SqlServer.Metrics.Provider.Builder;

namespace SqlServer.Metrics.Provider.Tests.Builder.Exposals
{
    public class MetricBuilderBaseExposal : MetricsBuilderBase
    {
        internal new string GetMetricsName(string procedureName, string metricsName) => base.GetMetricsName(procedureName, metricsName);
    }
}
