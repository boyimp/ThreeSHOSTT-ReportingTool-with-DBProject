CREATE FUNCTION [SQL#].[RegEx_MatchLength]
(@ExpressionToValidate NVARCHAR (MAX) NULL, @RegularExpression NVARCHAR (MAX) NULL, @StartAt INT NULL, @Length INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SQL#].[REGEX].[MatchLength]


GO

