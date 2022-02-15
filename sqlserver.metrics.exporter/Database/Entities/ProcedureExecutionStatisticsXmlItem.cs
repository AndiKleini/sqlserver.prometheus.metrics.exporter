using System;
using System.Globalization;
using System.Xml.Serialization;

namespace SqlServer.Metrics.Exporter
{
	

	public class ProcedureExecutionStatisticsXmlItem
	{ 
		[XmlElement(ElementName = "GeneralStats")]
		public GeneralStatsXmlItem GeneralStats { get; set; }

		[XmlElement(ElementName = "WorkerTime")]
		public WorkerTimeXmlItem WorkerTime { get; set; }

		[XmlElement(ElementName = "ElapsedTime")]
		public ElapsedTimeXmlItem ElapsedTime { get; set; }

		[XmlElement(ElementName = "LogicalWrites")]
		public LogicalWritesXmlItem LogicalWrites { get; set; }

		[XmlElement(ElementName = "PageServerReads")]
		public PageServerReadsXmlItem PageServerReads { get; set; }

		[XmlElement(ElementName = "PhysicalReads")]
		public PhysicalReadsXmlItem PhysicalReads { get; set; }

		[XmlElement(ElementName = "LogicalReads")]
		public LogicalReadsXmlItem LogicalReads { get; set; }
	}

	[XmlRoot(ElementName = "GeneralStats")]
	public class GeneralStatsXmlItem
	{
		const string procedureExecutionStatisticsDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

		[XmlAttribute(AttributeName = "ExecutionCount")]
		public int ExecutionCount { get; set; }

		[XmlIgnore]
        public DateTime LastExecutionTime { get; set; }

        [XmlAttribute(AttributeName = "LastExecutionTime")]
		public string LastExecutionTimeStringified 
		{
			get
            {
				return LastExecutionTime.ToString(procedureExecutionStatisticsDateTimeFormat);
            }
			set
            {
				LastExecutionTime = DateTime.ParseExact(value, procedureExecutionStatisticsDateTimeFormat, CultureInfo.InvariantCulture);

			}
		}

		[XmlIgnore]
		public DateTime CachedTime { get; set; }

		[XmlAttribute(AttributeName = "CachedTime")]
		public string CachedTimeStringified 
		{
			get
			{
				return CachedTime.ToString(procedureExecutionStatisticsDateTimeFormat);
			}
			set
			{
				CachedTime = DateTime.ParseExact(value, procedureExecutionStatisticsDateTimeFormat, CultureInfo.InvariantCulture);
			}
		}
	}

	[XmlRoot(ElementName = "WorkerTime")]
	public class WorkerTimeXmlItem
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
	public class ElapsedTimeXmlItem
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
	public class LogicalWritesXmlItem
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
	public class PageServerReadsXmlItem
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
	public class PhysicalReadsXmlItem
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
	public class LogicalReadsXmlItem
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
