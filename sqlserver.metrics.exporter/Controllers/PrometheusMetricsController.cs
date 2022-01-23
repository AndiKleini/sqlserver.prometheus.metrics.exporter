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
        private readonly IStoredProcedureMetricsProvider metricsProvider;

        public PrometheusMetricsController(IStoredProcedureMetricsProvider metricsProvider)
        {
            this.metricsProvider = metricsProvider;
        }

        public async Task<string> GetMetrics()
        {
            var metricItems = await this.metricsProvider.Collect(DateTime.Now);
            return metricItems.Aggregate(
                new StringBuilder(), 
                (aggregate, current) => aggregate.AppendLine($"{current.Name} {current.Value}")).ToString();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
