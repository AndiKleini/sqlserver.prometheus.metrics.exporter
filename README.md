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

### Execution Count

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
