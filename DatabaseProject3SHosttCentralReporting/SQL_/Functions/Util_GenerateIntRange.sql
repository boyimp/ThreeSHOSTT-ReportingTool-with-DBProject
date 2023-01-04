CREATE FUNCTION [SQL#].[Util_GenerateIntRange]
(@StartNum INT NULL, @EndNum INT NULL, @Step INT NULL)
RETURNS 
     TABLE (
        [IntNum] INT NULL,
        [IntVal] INT NULL)
AS
 EXTERNAL NAME [SQL#].[UTILITY].[GenerateIntRange]


GO

