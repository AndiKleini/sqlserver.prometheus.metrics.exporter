﻿using SqlServer.Metrics.Provider;
using System;

namespace SqlServer.Metrics.Provider
{
    public class PlanCacheItem
    {
        public ProcedureExecutionStatistics ExecutionStatistics { get; set; }
        public string SpName { get; set; }
        public DateTime? RemovedFromCacheAt { get; set; }
        public int ObjectId { get; set; }

        public static PlanCacheItem Zero()
        {
            return new PlanCacheItem() { ExecutionStatistics = new ProcedureExecutionStatistics() { GeneralStats = new GeneralStats() { ExecutionCount = 0 } } };

        }
    }
}