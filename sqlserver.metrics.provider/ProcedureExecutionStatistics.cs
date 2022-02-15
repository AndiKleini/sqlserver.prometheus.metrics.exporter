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
	}

	public class GeneralStats
	{
		public int ExecutionCount { get; set; }

        public DateTime LastExecutionTime { get; set; }

		public DateTime CachedTime { get; set; }
	}

	public class WorkerTime
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public int Min { get; set; }

		public int Max { get; set; }
	}

	public class ElapsedTime
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public int Min { get; set; }

		public int Max { get; set; }
	}

	public class LogicalWrites
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public int Min { get; set; }

		public int Max { get; set; }
	}

	public class PageServerReads
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public int Min { get; set; }

		public int Max { get; set; }
	}

	public class PhysicalReads
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public int Min { get; set; }

		public int Max { get; set; }
	}

	public class LogicalReads
	{
		public int Total { get; set; }

		public int Last { get; set; }

		public int Min { get; set; }

		public int Max { get; set; }
	}
}
