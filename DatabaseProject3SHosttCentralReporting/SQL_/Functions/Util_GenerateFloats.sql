CREATE FUNCTION [SQL#].[Util_GenerateFloats]
(@StartNum FLOAT (53) NULL, @TotalNums INT NULL, @Step FLOAT (53) NULL)
RETURNS 
     TABLE (
        [FloatNum] INT        NULL,
        [FloatVal] FLOAT (53) NULL)
AS
 EXTERNAL NAME [SQL#].[UTILITY].[GenerateFloats]


GO

