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
                        BuilderTypes.AverageElapsedTimeMetricsBuilder,
                        // BuilderTypes.ExecutionCountMetricsBuilder,
                        BuilderTypes.MaxElapsedTimeMetricsBuilder,
                        BuilderTypes.MinElapsedTimeMetricsBuilder
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
