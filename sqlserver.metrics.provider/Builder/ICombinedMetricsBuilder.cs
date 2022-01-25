using System.Collections.Generic;

namespace Sqlserver.Metrics.Provider.Builder
{
    public interface ICombinedMetricsBuilder : IDeltaMetricsBuilder, IMetricsBuilder
    {
        List<IMetricsBuilder> MetricItems { get; }

        List<IDeltaMetricsBuilder> DeltaMetricItems { get; }
    }
}
