CREATE FUNCTION [SQL#].[RegEx_GetCacheSize]
( )
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[REGEX].[GetCacheSize]


GO

