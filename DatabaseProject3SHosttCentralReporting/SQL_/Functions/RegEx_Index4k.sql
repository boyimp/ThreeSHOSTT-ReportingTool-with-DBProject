CREATE FUNCTION [SQL#].[RegEx_Index4k]
(@ExpressionToValidate NVARCHAR (4000) NULL, @RegularExpression NVARCHAR (4000) NULL, @StartAt INT NULL, @Length INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[REGEX].[Index]


GO

