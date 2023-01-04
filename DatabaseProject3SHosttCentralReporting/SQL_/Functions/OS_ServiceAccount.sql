CREATE FUNCTION [SQL#].[OS_ServiceAccount]
( )
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#.OS].[WindowsEnvironment].[ServiceAccount]


GO

