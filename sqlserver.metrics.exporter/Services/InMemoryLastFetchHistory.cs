using Sqlserver.Metrics.Exporter.Services;
using System;

namespace Sqlserver.Metrics.Exporter.Services
{
    public class InMemoryLastFetchHistory : ILastFetchHistory
    {
        private HistoricalFetch previousFetch;

        public HistoricalFetch GetPreviousFetch()
        {
            return this.previousFetch;
        }

        public void SetPreviousFetchTo(HistoricalFetch historicalFetch)
        {
            this.previousFetch = historicalFetch;
        }
    }
}