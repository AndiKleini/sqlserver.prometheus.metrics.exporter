﻿using System.Collections.Generic;

namespace SqlServer.Metrics.Provider
{
    public class QueryCacheRemovalStatistics
    {
		public string Timestamp { get; set; }

		public List<Data> DataItems { get; set; }
	}

	public class Data
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}
}
