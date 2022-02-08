using System.Collections.Generic;

namespace Sqlserver.Metrics.Provider.Builder
{
    public interface ICombinedMetricsBuilder : IMetricsBuilder
    {
        List<IMetricsBuilder> MetricItems { get; }
    }
}
