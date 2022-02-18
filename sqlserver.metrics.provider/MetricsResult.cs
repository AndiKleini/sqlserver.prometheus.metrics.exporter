using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;

namespace Sqlserver.Metrics.Provider
{
    public class MetricsResult
    {
        public IEnumerable<MetricItem> Items { get; set; }
        public DateTime? NewestHistoricalItemConsidered { get; set; }
    }
}