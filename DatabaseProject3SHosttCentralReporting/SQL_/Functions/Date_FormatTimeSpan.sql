CREATE FUNCTION [SQL#].[Date_FormatTimeSpan]
(@StartDate DATETIME NULL, @EndDate DATETIME NULL, @OutputFormat NVARCHAR (4000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[DATE].[FormatTimeSpan]


GO

