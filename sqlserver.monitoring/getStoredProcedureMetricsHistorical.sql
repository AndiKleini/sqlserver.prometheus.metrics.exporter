create PROCEDURE [monitoring].[getStoredProcedureMetricsHistorical]  
   @fromUtc datetime2,
   @xEventPath nvarchar(50)
AS 
Begin

	SELECT
		timestamp_utc,
		CAST(event_data AS VARCHAR(MAX)) as event_data
	FROM sys.fn_xe_file_target_read_file(@xEventPath, NULL, NULL, NULL)
	where CONVERT(datetime2, timestamp_utc) > @fromUtc;

End
