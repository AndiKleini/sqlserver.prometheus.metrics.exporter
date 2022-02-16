using System;

namespace Sqlserver.Metrics.Exporter
{
    public interface ILastFetchHistory
    {
        DateTime? GetPreviousFetchAndResetToNow();
    }
}