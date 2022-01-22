CREATE PROCEDURE [monitoring].[getProcedures]  
   
AS 
Begin
	select 
	    object_id,
		Name
	from sys.objects obj
	where obj.type = 'P'
End
