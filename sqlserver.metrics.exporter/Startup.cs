using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SqlServer.Metrics.Exporter.Adapters;
using SqlServer.Metrics.Exporter.Database;
using SqlServer.Metrics.Provider;
using Sqlserver.Metrics.Exporter.Services;
using Serilog;

namespace SqlServer.metrics.exporter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<IPreviousItemCache>(new InMemoryPreviousItemCacheAdapter());
            services.AddSingleton<ILastFetchHistory>(new InMemoryLastFetchHistory());
            services.AddSingleton<IDbPlanCacheRepository>(s => new DbPlanCacheRepository(
                s.GetService<IConfiguration>().GetConnectionString("SqlServerToMonitor"),
                s.GetService<IConfiguration>().GetValue<string>("XEventPath")));
            services.AddSingleton<IPlanCacheRepository>(s => new PlanCacheRepositoryAdapter(s.GetService<IDbPlanCacheRepository>()));
            services.AddSingleton(
                s => StoredProcedureMetricsProviderFactoryMethod.Create(
                    s.GetService<IPlanCacheRepository>(),
                    s.GetService<IPreviousItemCache>(),
                    new BuilderTypes[]
                    {
                        BuilderTypes.EstimatedExecutionCountBuilder,
                        // BuilderTypes.ExecutionCountMetricsBuilder,
                        // Elapsed Time
                        BuilderTypes.AverageElapsedTimeMetricsBuilder,
                        BuilderTypes.MaxElapsedTimeMetricsBuilder,
                        BuilderTypes.MinElapsedTimeMetricsBuilder,
                        BuilderTypes.LastElapsedTimeMetricsBuilder,
                        // physical reads
                        BuilderTypes.MaxPhysicalReadsMetricsBuilder,
                        BuilderTypes.MinPhysicalReadsMetricsBuilder,
                        BuilderTypes.LastPhysicalReadsMetricsBuilder,
                        BuilderTypes.AveragePhysicalRreadsMetricsBuilder,
                        // worker time
                        BuilderTypes.AverageWorkerTimeMetricsBuilder,
                        BuilderTypes.MaxWorkerTimeMetricsBuilder,
                        BuilderTypes.MinWorkerTimeMetricsBuilder,
                        BuilderTypes.LastWorkerTimeMetricsBuilder,
                        // page server reads
                        BuilderTypes.MinPageServerReadsMetricsBuilder,
                        BuilderTypes.MaxPageServerReadsMetricsBuilder,
                        BuilderTypes.AveragePageServerReadsMetricsBuilder,
                        BuilderTypes.LastPageServerReadsMetricsBuilder,
                        // logical reads
                        BuilderTypes.MaxLogicalReadsMetricsBuilder,
                        BuilderTypes.MinLogicalReadsMetricsBuilder,
                        BuilderTypes.AverageLogicalRreadsMetricsBuilder,
                        BuilderTypes.LastLogicalReadsMetricsBuilder,
                        // logical writes
                        BuilderTypes.MaxLogicalWritesMetricsBuilder,
                        BuilderTypes.MinLogicalWritesMetricsBuilder,
                        BuilderTypes.AverageLogicalWritesMetricsBuilder,
                        BuilderTypes.LastLogicalWritesMetricsBuilder,
                        // page spills
                        BuilderTypes.LastPageSpillsMetricsBuilder,
                        BuilderTypes.MaxPageSpillsMetricsBuilder,
                        BuilderTypes.MinPageSpillsMetricsBuilder,
                        BuilderTypes.AveragePageSpillsMetricsBuilder
                     }));
            services.AddSingleton<ILogger>(s => new LoggerConfiguration().WriteTo.Console().CreateLogger());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SqlServer.metrics.exporter", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SqlServer.metrics.exporter v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
