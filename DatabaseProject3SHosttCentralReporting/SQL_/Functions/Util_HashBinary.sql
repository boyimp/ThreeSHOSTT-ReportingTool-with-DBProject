CREATE FUNCTION [SQL#].[Util_HashBinary]
(@Algorithm NVARCHAR (50) NULL, @BaseData VARBINARY (MAX) NULL)
RETURNS VARBINARY (8000)
AS
 EXTERNAL NAME [SQL#].[UTILITY].[HashBinary]


GO

