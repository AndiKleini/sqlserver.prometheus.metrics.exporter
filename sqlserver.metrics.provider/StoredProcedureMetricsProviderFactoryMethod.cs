using SqlServer.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;
using Sqlserver.Metrics.Provider.Builder;

namespace SqlServer.Metrics.Provider
{
    public class StoredProcedureMetricsProviderFactoryMethod
    {
        public static IStoredProcedureMetricsProvider Create(IPlanCacheRepository planCacheRepository, IPreviousItemCache @object, params BuilderTypes[] buildersInUse)
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
                    case BuilderTypes.ExecutionCountMetricsBuilder:
                    {
                            metricsBuilder.Include(new ExecutionCountMetricsBuilder(@object));
                            break;
                    }
                    case BuilderTypes.LastElapsedTimeMetricsBuilder:
                        {
                            metricsBuilder.Include(new LastElapsedTimeMetricsBuilder());
                            break;
                        }
                }
            });
            return new StoredProcedureMetricsProvider(planCacheRepository, metricsBuilder);
        }
    }
}