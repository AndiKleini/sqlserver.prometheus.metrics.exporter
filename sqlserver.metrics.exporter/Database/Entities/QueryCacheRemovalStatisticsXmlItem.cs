using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sqlserver.Metrics.Exporter.Datebase.Entities
{
	[XmlRoot(Namespace = "some")]
    public class QueryCacheRemovalStatisticsXmlItem
	{
        [XmlAttribute(AttributeName = "timestamp")]
		public string Timestamp { get; set; }
		[XmlElement(ElementName = "data")]
		public List<Data> Data { get; set; }
	}

	[XmlRoot(ElementName = "data")]
	public class Data
	{
		[XmlElement(ElementName = "value")]
		public string Value { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "text")]
		public string Text { get; set; }
	}
}
