﻿using System;

namespace Sqlserver.Metrics.Exporter.Services
{
    public class HistoricalFetch
    {
        public DateTime? LastFetchTime { get; set; }
        public DateTime? IncludedHistoricalItemsFrom { get; set; }
    }
}