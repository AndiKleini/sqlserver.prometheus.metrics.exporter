using System;

namespace SqlServer.Metrics.Provider
{
    public class ProcedureExecutionStatistics
	{ 
		public GeneralStats GeneralStats { get; set; }

		public WorkerTime WorkerTime { get; set; }

		public ElapsedTime ElapsedTime { get; set; }

		public LogicalWrites LogicalWrites { get; set; }

		public PageServerReads PageServerReads { get; set; }

		public PhysicalReads PhysicalReads { get; set; }

		public LogicalReads LogicalReads { get; set; }
        public PageSpills PageSpills { get; set; }
    }

	public class GeneralStats
	{
		public long ExecutionCount { get; set; }

        public DateTime LastExecutionTime { get; set; }

		public DateTime CachedTime { get; set; }
	}

	public class WorkerTime
	{
		public long Total { get; set; }

		public long Last { get; set; }

		public long Min { get; set; }

		public long Max { get; set; }
	}

	public class ElapsedTime
	{
		public long Total { get; set; }

		public long Last { get; set; }

		public long Min { get; set; }

		public long Max { get; set; }
	}

	public class LogicalWrites
	{
		public long Total { get; set; }

		public long Last { get; set; }

		public long Min { get; set; }

		public long Max { get; set; }
	}

	public class PageServerReads
	{
		public long Total { get; set; }

		public long Last { get; set; }

		public long Min { get; set; }

		public long Max { get; set; }
	}

	public class PhysicalReads
	{
		public long Total { get; set; }

		public long Last { get; set; }

		public long Min { get; set; }

		public long Max { get; set; }
	}

	public class LogicalReads
	{
		public long Total { get; set; }

		public long Last { get; set; }

		public long Min { get; set; }

		public long Max { get; set; }
	}

	public class PageSpills
	{
		public long Last { get; set; }
		public long Max { get; set; }
		public long Min { get; set; }
		public long Total { get; set; }
	}
}
