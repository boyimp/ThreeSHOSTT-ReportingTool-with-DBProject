CREATE FUNCTION [SQL#].[RegEx_Escape4k]
(@ExpressionToEscape NVARCHAR (4000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[REGEX].[Escape]


GO
