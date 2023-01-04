CREATE FUNCTION [SQL#].[Util_GenerateFloatRange]
(@StartNum FLOAT (53) NULL, @EndNum FLOAT (53) NULL, @Step FLOAT (53) NULL)
RETURNS 
     TABLE (
        [FloatNum] INT        NULL,
        [FloatVal] FLOAT (53) NULL)
AS
 EXTERNAL NAME [SQL#].[UTILITY].[GenerateFloatRange]


GO

