using Microsoft.AspNetCore.Mvc;
using Serilog;
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
    public partial class PrometheusMetricsController : Controller
    {
        private readonly IStoredProcedureMetricsProvider metricsProvider;
        private readonly ILastFetchHistory history;
        private readonly ILogger logger;

        public PrometheusMetricsController(
            IStoredProcedureMetricsProvider metricsProvider, 
            ILastFetchHistory history,
            ILogger logger)
        {
            this.metricsProvider = metricsProvider;
            this.history = history;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<string> GetMetrics()
        {
            this.logger.Information("metrics collected");
            var previousFetch = history.GetPreviousFetch();
            var collectRange = GetCollectionRangeFrom(previousFetch);
            this.logger.Information(collectRange.ToString());
            var metricsResult = await this.metricsProvider.Collect(collectRange.LastFetchTime, collectRange.IncludedHistoricalItemsFrom);
            this.history.SetPreviousFetchTo(
                new HistoricalFetch()
                {
                    LastFetchTime = DateTime.UtcNow,
                    IncludedHistoricalItemsFrom = metricsResult.NewestHistoricalItemConsidered.GetValueOrDefault(DateTime.UtcNow.AddSeconds(-30))
                });
            return previousFetch == null ?
                String.Empty :
                metricsResult.Items.Aggregate(
                    new StringBuilder(),
                    (aggregate, current) => aggregate.Append($"{current.Name} {current.Value}\n")).ToString();

            CollectionRange GetCollectionRangeFrom(HistoricalFetch previousFetch)
            {
                if (previousFetch == null)
                {
                    return new CollectionRange()
                    {
                        LastFetchTime = DateTime.UtcNow.AddMinutes(-5),
                        IncludedHistoricalItemsFrom = DateTime.UtcNow.AddMinutes(-5).AddSeconds(-30)
                    };
                } 
                else
                {
                    return new CollectionRange()
                    {
                        LastFetchTime = previousFetch.LastFetchTime.Value,
                        IncludedHistoricalItemsFrom = previousFetch.IncludedHistoricalItemsFrom.Value
                    };
                }
            }
        }
    }
}
