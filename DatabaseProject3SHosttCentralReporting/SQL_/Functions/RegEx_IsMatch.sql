CREATE FUNCTION [SQL#].[RegEx_IsMatch]
(@ExpressionToValidate NVARCHAR (MAX) NULL, @RegularExpression NVARCHAR (MAX) NULL, @StartAt INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#].[REGEX].[IsMatch]


GO

