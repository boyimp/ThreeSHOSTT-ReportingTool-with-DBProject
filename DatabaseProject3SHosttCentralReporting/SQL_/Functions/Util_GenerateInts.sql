CREATE FUNCTION [SQL#].[Util_GenerateInts]
(@StartNum INT NULL, @TotalNums INT NULL, @Step INT NULL)
RETURNS 
     TABLE (
        [IntNum] INT NULL,
        [IntVal] INT NULL)
AS
 EXTERNAL NAME [SQL#].[UTILITY].[GenerateInts]


GO

