using Microsoft.AspNetCore.Mvc;
using Sqlserver.Metrics.Exporter.Services;
using SqlServer.Metrics.Provider;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer.Metrics.Exporter.Controllers
{
    [ApiController]
    [Route("metrics")]
    public class PrometheusMetricsController : Controller
    {
        private static DateTime? lastCollect;
        private readonly IStoredProcedureMetricsProvider metricsProvider;
        private readonly ILastFetchHistory history;

        public PrometheusMetricsController(IStoredProcedureMetricsProvider metricsProvider, ILastFetchHistory history)
        {
            this.metricsProvider = metricsProvider;
            this.history = history;
        }

        [HttpGet]
        public async Task<string> GetMetrics()
        {
            lastCollect = history.GetPreviousFetchAndResetToNow();
            var metricsResult = await this.metricsProvider.Collect(lastCollect.GetValueOrDefault(DateTime.Now.AddMinutes(-5)));
            return lastCollect == null ? 
                String.Empty : 
                metricsResult.Items.Aggregate(
                    new StringBuilder(), 
                    (aggregate, current) => aggregate.Append($"{current.Name} {current.Value}\n")).ToString();
        }
    }
}
