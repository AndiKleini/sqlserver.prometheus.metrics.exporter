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
            CollectionRange collectRange = From(previousFetch);
            var metricsResult = await this.metricsProvider.Collect(collectRange.LastFetchTime, collectRange.IncludedHistoricalItemsUntil);
            this.history.SetPreviousFetchTo(
                new HistoricalFetch()
                {
                    LastFetchTime = DateTime.Now,
                    IncludedHistoricalItemsUntil = metricsResult.NewestHistoricalItemConsidered
                });
            return previousFetch == null ?
                String.Empty :
                metricsResult.Items.Aggregate(
                    new StringBuilder(),
                    (aggregate, current) => aggregate.Append($"{current.Name} {current.Value}\n")).ToString();

            CollectionRange From(HistoricalFetch previousFetch)
            {
                return new CollectionRange()
                {
                    LastFetchTime = previousFetch?.LastFetchTime ?? DateTime.Now.AddMinutes(-5),
                    IncludedHistoricalItemsUntil = previousFetch?.IncludedHistoricalItemsUntil ?? DateTime.Now.AddMinutes(-5).AddSeconds(-30)
                };
            }
        }

        private class CollectionRange
        {
            public DateTime LastFetchTime { get; set; }
            public DateTime IncludedHistoricalItemsUntil { get; set; }
        }
    }
}
