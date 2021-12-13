using System;

namespace SqlServer.Metrics.Provider
{
    public class StoredProcedureMetricsProvider
    {
        public StoredProcedureMetricsProvider(IPlanCacheRepository planCacherepository)
        {
        }

        public System.Collections.Generic.List<MetricItem> Collect(DateTime dateTime, DateTime dateTime1)
        {
            throw new NotImplementedException();
        }
    }
}