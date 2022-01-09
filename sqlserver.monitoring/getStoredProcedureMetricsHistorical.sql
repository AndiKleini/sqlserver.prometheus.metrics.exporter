create PROCEDURE [monitoring].[getStoredProcedureMetricsHistorical]  
   @fromUtc datetime2
AS 
Begin

    declare @ExtendedEvents_Path nvarchar(50) = 'C:\temp\ExtendedEvents*.xel';

	SELECT
		timestamp_utc,
		CAST(event_data AS VARCHAR(MAX)) as event_data
	FROM sys.fn_xe_file_target_read_file(@ExtendedEvents_Path, NULL, NULL, NULL)
	where CONVERT(datetime2, timestamp_utc) > @fromUtc;
End
