using System.Collections.Generic;
using System.Xml.Serialization;

namespace SqlServer.Metrics.Provider
{
    public class QueryCacheRemovalStatistics
    {
        [XmlAttribute(AttributeName = "timestamp")]
		public string Timestamp { get; set; }

		[XmlElement(ElementName = "data")]
		public List<Data> DataItems { get; set; }
	}

	public class Data
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlElement(ElementName = "value")]
		public string Value { get; set; }
	}
}
