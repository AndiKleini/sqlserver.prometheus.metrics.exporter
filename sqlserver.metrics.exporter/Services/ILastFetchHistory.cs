using System;

namespace Sqlserver.Metrics.Exporter.Services
{
    public interface ILastFetchHistory
    {
        HistoricalFetch GetPreviousFetch();
        void SetPreviousFetchTo(HistoricalFetch historicalFetch);
    }
}