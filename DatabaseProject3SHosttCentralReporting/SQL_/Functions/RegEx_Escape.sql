CREATE FUNCTION [SQL#].[RegEx_Escape]
(@ExpressionToEscape NVARCHAR (MAX) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SQL#].[REGEX].[Escape]


GO

