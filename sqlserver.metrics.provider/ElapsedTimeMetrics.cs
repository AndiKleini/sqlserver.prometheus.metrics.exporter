using System.Collections.Generic;
using System.Linq;

namespace SqlServer.Metrics.Provider
{
    public class ElapsedTimeMetrics
    {

        public static IEnumerable<MetricItem> GetElapsedTimeMetrics(IGrouping<string, PlanCacheItem> grouping)
        {
            yield return new MetricItem()
            {
                Name = $"{grouping.Key}_ElapsedTimeMax",
                Value = grouping.Max(p => p.ExecutionStatistics.ElapsedTime.Max)
            };
            yield return new MetricItem()
            {
                Name = $"{grouping.Key}_ElapsedTimeMin",
                Value = grouping.Min(p => p.ExecutionStatistics.ElapsedTime.Min)
            };
            /*
            yield return new MetricItem()
            {
                Name = $"{grouping.Key}_ElapsedTimeLast",
                Value = grouping.
                    OrderBy(p => p.ExecutionStatistics.GeneralStats.LastExecutionTime).First().ExecutionStatistics.ElapsedTime.Last
            };
            */
        }
    }
}