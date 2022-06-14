using SqlServer.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;
using Sqlserver.Metrics.Provider.Builder;

namespace SqlServer.Metrics.Provider
{
    public class StoredProcedureMetricsProviderFactoryMethod
    {
        public static IStoredProcedureMetricsProvider Create(IPlanCacheRepository planCacheRepository, IPreviousItemCache previousItemCache, params BuilderTypes[] buildersInUse)
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
                        metricsBuilder.Include(new ExecutionCountMetricsBuilder(previousItemCache));
                        break;
                    }
                    case BuilderTypes.LastElapsedTimeMetricsBuilder:
                    {
                        metricsBuilder.Include(new LastElapsedTimeMetricsBuilder());
                        break;
                    }
                    case BuilderTypes.MaxPhysicalReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMaxMetricsBuilder("PhysicalReadsMax", s => s.ExecutionStatistics.PhysicalReads.Max));
                        break;
                    }
                    case BuilderTypes.MinPhysicalReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMinMetricsBuilder("PhysicalReadsMin", s => s.ExecutionStatistics.PhysicalReads.Min));
                        break;
                    }
                    case BuilderTypes.LastPhysicalReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericLastMetricsBuilder("PhysicalReadsLast", s => s.ExecutionStatistics.PhysicalReads.Last));
                        break;
                    }
                    case BuilderTypes.AveragePhysicalRreadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericAverageMetricsBuilder("PhysicalReadsAverage", s => s.ExecutionStatistics.PhysicalReads.Total));
                        break;
                    }
                    case BuilderTypes.MaxLogicalReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMaxMetricsBuilder("LogicalReadsMax", s => s.ExecutionStatistics.LogicalReads.Max));
                        break;
                    }
                    case BuilderTypes.MinLogicalReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMinMetricsBuilder("LogicalReadsMin", s => s.ExecutionStatistics.LogicalReads.Min));
                        break;
                    }
                    case BuilderTypes.LastLogicalReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericLastMetricsBuilder("LogicalReadsLast", s => s.ExecutionStatistics.LogicalReads.Last));
                        break;
                    }
                    case BuilderTypes.AverageLogicalRreadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericAverageMetricsBuilder("LogicalReadsAverage", s => s.ExecutionStatistics.LogicalReads.Total));
                        break;
                    }
                    case BuilderTypes.MaxPageServerReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMaxMetricsBuilder("PageServerReadsMax", s => s.ExecutionStatistics.PageServerReads.Max));
                        break;
                    }
                    case BuilderTypes.MinPageServerReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMinMetricsBuilder("PageServerReadsMin", s => s.ExecutionStatistics.PageServerReads.Min));
                        break;
                    }
                    case BuilderTypes.LastPageServerReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericLastMetricsBuilder("PageServerReadsLast", s => s.ExecutionStatistics.PageServerReads.Last));
                        break;
                    }
                    case BuilderTypes.AveragePageServerReadsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericAverageMetricsBuilder("PageServerReadsAverage", s => s.ExecutionStatistics.PageServerReads.Total));
                        break;
                    }
                    case BuilderTypes.MaxLogicalWritesMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMaxMetricsBuilder("LogicalWritesMax", s => s.ExecutionStatistics.LogicalWrites.Max));
                        break;
                    }
                    case BuilderTypes.MinLogicalWritesMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMinMetricsBuilder("LogicalWritesMin", s => s.ExecutionStatistics.LogicalWrites.Min));
                        break;
                    }
                    case BuilderTypes.LastLogicalWritesMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericLastMetricsBuilder("LogicalWritesLast", s => s.ExecutionStatistics.LogicalWrites.Last));
                        break;
                    }
                    case BuilderTypes.AverageLogicalWritesMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericAverageMetricsBuilder("LogicalWritesAverage", s => s.ExecutionStatistics.LogicalWrites.Total));
                        break;
                    }
                    case BuilderTypes.MaxWorkerTimeMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMaxMetricsBuilder("WorkerTimeMax", s => s.ExecutionStatistics.WorkerTime.Max));
                        break;
                    }
                    case BuilderTypes.MinWorkerTimeMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMinMetricsBuilder("WorkerTimeMin", s => s.ExecutionStatistics.WorkerTime.Min));
                        break;
                    }
                    case BuilderTypes.LastWorkerTimeMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericLastMetricsBuilder("WorkerTimeLast", s => s.ExecutionStatistics.WorkerTime.Last));
                        break;
                    }
                    case BuilderTypes.AverageWorkerTimeMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericAverageMetricsBuilder("WorkerTimeAverage", s => s.ExecutionStatistics.WorkerTime.Total));
                        break;
                    }
                    case BuilderTypes.LastPageSpillsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericLastMetricsBuilder("PageSpillsLast", s => s.ExecutionStatistics.PageSpills.Last));
                        break;
                    }
                    case BuilderTypes.MaxPageSpillsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMaxMetricsBuilder("PageSpillsMax", s => s.ExecutionStatistics.PageSpills.Max));
                        break;
                    }
                    case BuilderTypes.MinPageSpillsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericMinMetricsBuilder("PageSpillsMin", s => s.ExecutionStatistics.PageSpills.Min));
                        break;
                    }
                    case BuilderTypes.AveragePageSpillsMetricsBuilder:
                    {
                        metricsBuilder.Include(new GenericAverageMetricsBuilder("PageSpillsAverage", s => s.ExecutionStatistics.PageSpills.Total));
                        break;
                    }
                    case BuilderTypes.EstimatedExecutionCountBuilder:
                    {
                        metricsBuilder.Include(new EstimatedExecutionCountMetricsBuilder(previousItemCache));
                        break;
                    }
                }
            });
            return new StoredProcedureMetricsProvider(planCacheRepository, metricsBuilder);
        }
    }
}