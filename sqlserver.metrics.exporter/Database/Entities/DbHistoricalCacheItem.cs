using System;

namespace Sqlserver.Metrics.Exporter.Database.Entities
{
    public class DbHistoricalCacheItem
    {
        public DateTime timestamp_utc;
        public string event_data;
    }
}
