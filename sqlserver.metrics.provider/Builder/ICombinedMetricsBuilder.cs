using System.Collections.Generic;

namespace SqlServer.Metrics.Provider.Builder
{
    public interface ICombinedMetricsBuilder : IMetricsBuilder
    {
        List<IMetricsBuilder> MetricItems { get; }
    }
}
