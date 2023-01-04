CREATE FUNCTION [SQL#].[RegEx_Split4k]
(@ExpressionToValidate NVARCHAR (4000) NULL, @RegularExpression NVARCHAR (4000) NULL, @Count INT NULL, @StartAt INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS 
     TABLE (
        [MatchNum] INT             NULL,
        [Value]    NVARCHAR (4000) NULL,
        [StartPos] INT             NULL,
        [EndPos]   INT             NULL,
        [Length]   INT             NULL)
AS
 EXTERNAL NAME [SQL#].[REGEX].[Split]


GO

