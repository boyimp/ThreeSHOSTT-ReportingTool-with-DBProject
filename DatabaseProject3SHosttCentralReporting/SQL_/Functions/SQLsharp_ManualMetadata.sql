CREATE FUNCTION [SQL#].[SQLsharp_ManualMetadata]
( )
RETURNS 
     TABLE (
        [Key]      NVARCHAR (50) NULL,
        [Value]    SQL_VARIANT   NULL,
        [Datatype] NVARCHAR (50) NULL)
AS
 EXTERNAL NAME [SQL#].[SQLsharp].[ManualMetadata]


GO

