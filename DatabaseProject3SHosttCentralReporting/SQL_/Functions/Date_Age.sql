CREATE FUNCTION [SQL#].[Date_Age]
(@StartDate DATETIME NULL, @EndDate DATETIME NULL, @LeapYearHandling NVARCHAR (4000) NULL, @IncludeDays BIT NULL)
RETURNS FLOAT (53)
AS
 EXTERNAL NAME [SQL#].[DATE].[Age]


GO

