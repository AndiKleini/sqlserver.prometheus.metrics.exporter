using System;

namespace SqlServer.Metrics.Exporter.Database.Entities
{
    public class DbCacheItem
    {
		public DateTime cached_time;
		public DateTime last_execution_time;
		public long execution_count;
		public long total_elapsed_time;
		public long last_elapsed_time;
		public long min_elapsed_time;
		public long max_elapsed_time;
		public long total_worker_time;
		public long last_worker_time;
		public long min_worker_time;
		public long max_worker_time;
		public long total_logical_reads;
		public long last_logical_reads;
		public long min_logical_reads;
		public long max_logical_reads;
		public long total_physical_reads;
		public long last_physical_reads;
		public long min_physical_reads;
		public long max_physical_reads;
		public long total_logical_writes;
		public long last_logical_writes;
		public long min_logical_writes;
		public long max_logical_writes;
		public long total_spills;
		public long last_spills;
		public long min_spills;
		public long max_spills;
		public long total_page_server_reads;
		public long last_page_server_reads;
		public long min_page_server_reads;
		public long max_page_server_reads;
        public int object_Id;
    }
}
