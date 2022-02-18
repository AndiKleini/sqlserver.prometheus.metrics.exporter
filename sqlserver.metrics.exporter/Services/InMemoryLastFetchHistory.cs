using Sqlserver.Metrics.Exporter.Services;
using System;

namespace Sqlserver.Metrics.Exporter.Services
{
    public class InMemoryLastFetchHistory : ILastFetchHistory
    {
        private DateTime? callTimeStamp = null;

        public DateTime? GetPreviousFetchAndResetToNow()
        {
            var previousTimeStamp = this.callTimeStamp;
            this.callTimeStamp = DateTime.Now;
            return previousTimeStamp;
        }
    }
}