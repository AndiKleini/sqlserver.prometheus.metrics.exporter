# sqlserver.prometheus.metrics.exporter

Under construction

## Purpose
Collects metrics from some SQL Server instance for stored procedures and exposes them to a prometheus compatible REST endpoint.
Intention is in tracking and analyzing performance within the database, without the necessisity any adaption in the client code.  

## Features
Supported metrics are:


The system is open for adding or customizing new metrics.

## Architecture

 by exploring execution statistics (managed view: sys.dm_exec_procedure_stats stats) nad sourcing particular extended events.


## Repository structure

## Testing
