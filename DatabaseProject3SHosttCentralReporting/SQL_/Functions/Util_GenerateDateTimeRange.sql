CREATE FUNCTION [SQL#].[Util_GenerateDateTimeRange]
(@StartDateTime DATETIME NULL, @EndDateTime DATETIME NULL, @Step INT NULL, @StepType NVARCHAR (4000) NULL)
RETURNS 
     TABLE (
        [DatetimeNum] INT      NULL,
        [DatetimeVal] DATETIME NULL)
AS
 EXTERNAL NAME [SQL#].[UTILITY].[GenerateDateTimeRange]


GO

