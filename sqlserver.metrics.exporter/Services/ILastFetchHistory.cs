using System;

namespace Sqlserver.Metrics.Exporter.Services
{
    public interface ILastFetchHistory
    {
        DateTime? GetPreviousFetchAndResetToNow();
        HistoricalFetch GetPreviousFetch();
        void SetPreviousFetchTo(HistoricalFetch historicalFetch);
    }
}