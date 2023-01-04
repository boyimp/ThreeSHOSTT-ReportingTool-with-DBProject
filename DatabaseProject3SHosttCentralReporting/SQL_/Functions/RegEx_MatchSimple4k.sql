CREATE FUNCTION [SQL#].[RegEx_MatchSimple4k]
(@ExpressionToValidate NVARCHAR (4000) NULL, @RegularExpression NVARCHAR (4000) NULL, @StartAt INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[REGEX].[MatchSimple]


GO

