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
            var previousFetch = history.GetPreviousFetch();
            var metricsResult = 
                await this.metricsProvider.Collect(
                    previousFetch.LastFetchTime.GetValueOrDefault(DateTime.Now.AddMinutes(-5)), 
                    previousFetch.IncludedHistoricalItemsUntil.GetValueOrDefault(DateTime.Now.AddMinutes(-5).AddSeconds(-30)));
            this.history.SetPreviousFetchTo(
                new HistoricalFetch()
                {
                    LastFetchTime = DateTime.Now,
                    IncludedHistoricalItemsUntil = metricsResult.NewestHistoricalItemConsidered
                });
            return previousFetch.LastFetchTime == null ? 
                String.Empty : 
                metricsResult.Items.Aggregate(
                    new StringBuilder(), 
                    (aggregate, current) => aggregate.Append($"{current.Name} {current.Value}\n")).ToString();
        }
    }
}
