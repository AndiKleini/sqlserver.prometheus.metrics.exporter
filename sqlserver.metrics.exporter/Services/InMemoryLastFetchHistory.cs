using Sqlserver.Metrics.Exporter.Services;
using System;

namespace Sqlserver.Metrics.Exporter.Services
{
    public class InMemoryLastFetchHistory : ILastFetchHistory
    {
        private DateTime? callTimeStamp = null;

        public HistoricalFetch GetPreviousFetch()
        {
            throw new NotImplementedException();
        }

        public DateTime? GetPreviousFetchAndResetToNow()
        {
            var previousTimeStamp = this.callTimeStamp;
            this.callTimeStamp = DateTime.Now;
            return previousTimeStamp;
        }

        public void SetPreviousFetchTo(HistoricalFetch historicalFetch)
        {
            throw new NotImplementedException();
        }
    }
}