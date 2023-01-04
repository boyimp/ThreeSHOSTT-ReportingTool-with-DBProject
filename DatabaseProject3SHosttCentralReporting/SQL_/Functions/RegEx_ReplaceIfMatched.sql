CREATE FUNCTION [SQL#].[RegEx_ReplaceIfMatched]
(@ExpressionToValidate NVARCHAR (MAX) NULL, @RegularExpression NVARCHAR (MAX) NULL, @Replacement NVARCHAR (4000) NULL, @NotFoundReplacement NVARCHAR (MAX) NULL, @Count INT NULL, @StartAt INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SQL#].[REGEX].[ReplaceIfMatched]


GO

