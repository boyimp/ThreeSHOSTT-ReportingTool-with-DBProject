CREATE FUNCTION [SQL#].[RegEx_ReplaceIfMatched4k]
(@ExpressionToValidate NVARCHAR (4000) NULL, @RegularExpression NVARCHAR (4000) NULL, @Replacement NVARCHAR (4000) NULL, @NotFoundReplacement NVARCHAR (4000) NULL, @Count INT NULL, @StartAt INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[REGEX].[ReplaceIfMatched]


GO

