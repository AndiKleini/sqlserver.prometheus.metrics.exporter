# sqlserver.prometheus.metrics.exporter

Under construction

## Purpose
Collects metrics from some SQL Server instance for stored procedures and exposes them to a prometheus compatible REST endpoint.
Intention is in tracking and analyzing performance within the database, without the necessisity any adaption in the client code.  

## Features
Supported metrics are:
* MaxElapsedTime 
* MinElapsedTime
* AverageElapsedTime
* ExecutionCount

The system is open for customizations and adding new metrics.

### Max Elapsed Time
The total elapsed time, in microseconds, for completed executions of this stored procedure.

### Min Elapsed Time
The minimum elapsed time, in microseconds, for completed executions of this stored procedure.

### Average Elapsed Time
The average elapsed time, in microseconds, for completed executions of this stored procedure.

### Average Elapsed Time
The last (youngest) elapsed time, in microseconds, of completed execution of this stored procedure.

### Max Physical Reads
The minimum number of physical reads that this stored procedure has ever performed during a single execution.

### Min Physical Reads
The minimum number of physical reads that this stored procedure has ever performed during a single execution.

### Average Physical Reads
The average number of physical reads that this stored procedure has performed during a single execution.

### Last Physical Reads
The number of physical reads performed the last time the stored procedure was executed.

### Max Logical Reads
The minimum number of logical reads that this stored procedure has ever performed during a single execution.

### Min Logical Reads
The minimum number of logical reads that this stored procedure has ever performed during a single execution.

### Average Logical Reads
The average number of logical reads that this stored procedure has performed during a single execution.

### Last Logical Reads
The number of logical reads performed the last time the stored procedure was executed.

### Max Logical Writes
The minimum number of logical writes that this stored procedure has ever performed during a single execution.

### Min Logical Writes
The minimum number of logical writes that this stored procedure has ever performed during a single execution.

### Average Logical Writes
The average number of logical writes that this stored procedure has performed during a single execution.

### Last Logical Writes
The number of buffer pool pages dirtied the last time the plan was executed. If a page is already dirty (modified) no writes are counted.

### Max Page Server Reads
The maximum number of page server reads that this stored procedure has ever performed during a single execution.

### Min Page Server Reads
The minimum number of page server reads that this stored procedure has ever performed during a single execution.

### Average Page Server Reads
The average number of page server reads that this stored procedure has performed during a single execution.

### Last Page Server Reads
The number of buffer pool pages dirtied the last time the plan was executed. If a page is already dirty (modified) no writes are counted.

### Execution Count
Counts the number of executions between now an the last fetch.

Please compare documentation about managed view sys.dm_exec_procedure_stats:
https://docs.microsoft.com/en-us/sql/relational-databases/system-dynamic-management-views/sys-dm-exec-procedure-stats-transact-sql?view=sql-server-ver15

## Architecture



### Datasource
Basically all required information is already provided by the SQL Server, so that all metrics can be calculated from two basic sources:
* execution statistics (sys.dm_exec_procedure_stats stats)
* extended event session

Statistics for stored procedures are retrieved from the management view sys.dm_exec_procedure_stats stats. As the metrics are collected periodically (compare prometheus scrape job strategy) and statistics are maybe kept in the cache only for a short time, the sourcing of the extended event query_cache_removal_statistics is required to get consistent data. Putting both sources together, one yields a full history of statistics. 

### REST endpoint
The entry point of the application is a prometheus compatible https://myapipath/metrics endpoint (HTTP Method GET without any further parameters). Whenever the GEt is triggered it runs through following scenario:
1. Fetch statistical records from datasource and enrich these with the name of the stored procedures.
2. Pass the yield records to the provider engine for calculation.
3. Format the retrieved response into the prometheus metrics and emit.

### State
Please be aware that the application is not stateless. It has to store some characteristics (e.g.: timestamp, execution count) of the previous fetch avoiding unwanted multiple consideration of items. As a consequence you can not scale up the instance anyway. Additionally you have to avoid setting up any calls at the metrics endpoint except those comming from prometheus job.

## Repository structure

## Testing

## Configuration

## Deployment
Scalability
## Extensibility

### Add a new metric
