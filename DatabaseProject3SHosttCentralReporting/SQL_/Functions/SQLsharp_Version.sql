CREATE FUNCTION [SQL#].[SQLsharp_Version]
( )
RETURNS NVARCHAR (50)
AS
 EXTERNAL NAME [SQL#].[SQLsharp].[Version]


GO
