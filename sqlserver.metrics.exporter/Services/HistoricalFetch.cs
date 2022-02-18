using System;

namespace Sqlserver.Metrics.Exporter.Services
{
    public struct HistoricalFetch
    {
        public DateTime? LastFetchTime { get; set; }
        public DateTime? IncludedHistoricalItemsUntil { get; set; }
    }
}