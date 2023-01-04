CREATE FUNCTION [SQL#].[RegEx_Index]
(@ExpressionToValidate NVARCHAR (MAX) NULL, @RegularExpression NVARCHAR (MAX) NULL, @StartAt INT NULL, @Length INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[REGEX].[Index]


GO

