using Sqlserver.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;

namespace Sqlserver.Metrics.Provider
{
    public class StoredProcedureMetricsProviderFactoryMethod
    {
        public static IStoredProcedureMetricsProvider Create(IPlanCacheRepository planCacheRepository, params BuilderTypes[] buildersInUse)
        {
            var metricsBuilder = new StandardMetricsBuilder();
            Array.ForEach(buildersInUse, b =>
            {
                switch (b)
                {
                    case BuilderTypes.AverageElapsedTimeMetricsBuilder:
                    {
                            metricsBuilder.Include(new AverageElapsedTimeMetricsBuilder());
                            break;
                    }
                    case BuilderTypes.MaxElapsedTimeMetricsBuilder:
                    {
                            metricsBuilder.Include(new MaxElapsedTimeMetricsBuilder());
                            break;
                    }
                    case BuilderTypes.MinElapsedTimeMetricsBuilder:
                    {
                            metricsBuilder.Include(new MinElapsedTimeMetricsBuilder());
                            break;
                    }
                }
            });
            return new StoredProcedureMetricsProvider(planCacheRepository, metricsBuilder);
        }
    }
}