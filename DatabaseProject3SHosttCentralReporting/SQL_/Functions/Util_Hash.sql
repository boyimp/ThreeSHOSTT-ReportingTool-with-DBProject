CREATE FUNCTION [SQL#].[Util_Hash]
(@Algorithm NVARCHAR (50) NULL, @BaseData VARBINARY (MAX) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[UTILITY].[Hash]


GO
