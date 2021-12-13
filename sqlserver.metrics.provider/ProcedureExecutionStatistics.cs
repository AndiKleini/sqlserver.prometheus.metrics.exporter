using System;
using System.Xml.Serialization;

namespace Sqlserver.Metrics.Provider
{
    public class ProcedureExecutionStatistics
	{ 
		[XmlElement(ElementName = "GeneralStats")]
		public GeneralStats GeneralStats { get; set; }

		[XmlElement(ElementName = "WorkerTime")]
		public WorkerTime WorkerTime { get; set; }

		[XmlElement(ElementName = "ElapsedTime")]
		public ElapsedTime ElapsedTime { get; set; }

		[XmlElement(ElementName = "LogicalWrites")]
		public LogicalWrites LogicalWrites { get; set; }

		[XmlElement(ElementName = "PageServerReads")]
		public PageServerReads PageServerReads { get; set; }

		[XmlElement(ElementName = "PhysicalReads")]
		public PhysicalReads PhysicalReads { get; set; }

		[XmlElement(ElementName = "LogicalReads")]
		public LogicalReads LogicalReads { get; set; }
	}

	[XmlRoot(ElementName = "GeneralStats")]
	public class GeneralStats
	{

		[XmlAttribute(AttributeName = "ExecutionCount")]
		public int ExecutionCount { get; set; }

		[XmlAttribute(AttributeName = "LastExecutionTime")]
		public DateTime LastExecutionTime { get; set; }

		[XmlAttribute(AttributeName = "CachedTime")]
		public DateTime CachedTime { get; set; }
	}

	[XmlRoot(ElementName = "WorkerTime")]
	public class WorkerTime
	{

		[XmlAttribute(AttributeName = "Total")]
		public int Total { get; set; }

		[XmlAttribute(AttributeName = "Last")]
		public int Last { get; set; }

		[XmlAttribute(AttributeName = "Min")]
		public int Min { get; set; }

		[XmlAttribute(AttributeName = "Max")]
		public int Max { get; set; }
	}

	[XmlRoot(ElementName = "ElapsedTime")]
	public class ElapsedTime
	{

		[XmlAttribute(AttributeName = "Total")]
		public int Total { get; set; }

		[XmlAttribute(AttributeName = "Last")]
		public int Last { get; set; }

		[XmlAttribute(AttributeName = "Min")]
		public int Min { get; set; }

		[XmlAttribute(AttributeName = "Max")]
		public int Max { get; set; }
	}

	[XmlRoot(ElementName = "LogicalWrites")]
	public class LogicalWrites
	{

		[XmlAttribute(AttributeName = "Total")]
		public int Total { get; set; }

		[XmlAttribute(AttributeName = "Last")]
		public int Last { get; set; }

		[XmlAttribute(AttributeName = "Min")]
		public int Min { get; set; }

		[XmlAttribute(AttributeName = "Max")]
		public int Max { get; set; }
	}

	[XmlRoot(ElementName = "PageServerReads")]
	public class PageServerReads
	{

		[XmlAttribute(AttributeName = "Total")]
		public int Total { get; set; }

		[XmlAttribute(AttributeName = "Last")]
		public int Last { get; set; }

		[XmlAttribute(AttributeName = "Min")]
		public int Min { get; set; }

		[XmlAttribute(AttributeName = "Max")]
		public int Max { get; set; }
	}

	[XmlRoot(ElementName = "PhysicalReads")]
	public class PhysicalReads
	{

		[XmlAttribute(AttributeName = "Total")]
		public int Total { get; set; }

		[XmlAttribute(AttributeName = "Last")]
		public int Last { get; set; }

		[XmlAttribute(AttributeName = "Min")]
		public int Min { get; set; }

		[XmlAttribute(AttributeName = "Max")]
		public int Max { get; set; }
	}

	[XmlRoot(ElementName = "LogicalReads")]
	public class LogicalReads
	{

		[XmlAttribute(AttributeName = "Total")]
		public int Total { get; set; }

		[XmlAttribute(AttributeName = "Last")]
		public int Last { get; set; }

		[XmlAttribute(AttributeName = "Min")]
		public int Min { get; set; }

		[XmlAttribute(AttributeName = "Max")]
		public int Max { get; set; }
	}

}
