using Sqlserver.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Provider
{
    public interface IStoredProcedureMetricsProvider
    {
        Task<IEnumerable<MetricItem>> Collect(DateTime from);
        List<IMetricsBuilder> GetMetricBuilders();
    }
}