using Sqlserver.Metrics.Provider;
using SqlServer.Metrics.Provider.Builder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Provider
{
    public interface IStoredProcedureMetricsProvider
    {
        Task<MetricsResult> Collect(DateTime from);
        List<IMetricsBuilder> GetMetricBuilders();
    }
}