using System;

namespace SqlServer.Metrics.Exporter.Database.Entities
{
    public class DbCacheItem
    {
		public DateTime cached_time;
		public DateTime last_execution_time;
		public int execution_count;
		public int total_elapsed_time;
		public int last_elapsed_time;
		public int min_elapsed_time;
		public int max_elapsed_time;
		public int total_worker_time;
		public int last_worker_time;
		public int min_worker_time;
		public int max_worker_time;
		public int total_logical_reads;
		public int last_logical_reads;
		public int min_logical_reads;
		public int max_logical_reads;
		public int total_physical_reads;
		public int last_physical_reads;
		public int min_physical_reads;
		public int max_physical_reads;
		public int total_logical_writes;
		public int last_logical_writes;
		public int min_logical_writes;
		public int max_logical_writes;
		public int total_spills;
		public int last_spills;
		public int min_spills;
		public int max_spills;
		public int total_page_server_reads;
		public int last_page_server_reads;
		public int min_page_server_reads;
		public int max_page_server_reads;
        public int object_Id;
    }
}
