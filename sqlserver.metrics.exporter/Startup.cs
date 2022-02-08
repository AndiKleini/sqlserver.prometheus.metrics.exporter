using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sqlserver.Metrics.Exporter.Adapters;
using Sqlserver.Metrics.Exporter.Database;
using Sqlserver.Metrics.Provider;
using Sqlserver.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sqlserver.metrics.exporter
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
            services.AddSingleton<IDbPlanCacheRepository>(s => new DbPlanCacheRepository(s.GetService<IConfiguration>().GetConnectionString("SqlServerToMonitor")));
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "sqlserver.metrics.exporter", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "sqlserver.metrics.exporter v1"));
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
