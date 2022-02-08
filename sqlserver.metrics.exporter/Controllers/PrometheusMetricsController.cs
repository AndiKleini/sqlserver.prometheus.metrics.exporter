using Microsoft.AspNetCore.Mvc;
using SqlServer.Metrics.Provider;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlserver.Metrics.Exporter.Controllers
{
    [ApiController]
    [Route("metrics")]
    public class PrometheusMetricsController : Controller
    {
        private static DateTime? lastCollect;
        private readonly IStoredProcedureMetricsProvider metricsProvider;

        public PrometheusMetricsController(IStoredProcedureMetricsProvider metricsProvider)
        {
            this.metricsProvider = metricsProvider;
        }

        [HttpGet]
        public async Task<string> GetMetrics()
        {
            if (lastCollect == null)
            {
                lastCollect = DateTime.Now.AddMinutes(-5);
            } 

            var metricItems = await this.metricsProvider.Collect(lastCollect.Value);
            lastCollect = DateTime.Now;
            return metricItems.Aggregate(
                new StringBuilder(), 
                (aggregate, current) => aggregate.Append($"{current.Name} {current.Value}\n")).ToString();
        }
    }
}
