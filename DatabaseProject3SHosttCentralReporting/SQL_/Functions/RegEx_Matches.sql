CREATE FUNCTION [SQL#].[RegEx_Matches]
(@ExpressionToValidate NVARCHAR (MAX) NULL, @RegularExpression NVARCHAR (MAX) NULL, @StartAt INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS 
     TABLE (
        [MatchNum] INT            NULL,
        [Value]    NVARCHAR (MAX) NULL,
        [StartPos] INT            NULL,
        [EndPos]   INT            NULL,
        [Length]   INT            NULL)
AS
 EXTERNAL NAME [SQL#].[REGEX].[Matches]


GO

