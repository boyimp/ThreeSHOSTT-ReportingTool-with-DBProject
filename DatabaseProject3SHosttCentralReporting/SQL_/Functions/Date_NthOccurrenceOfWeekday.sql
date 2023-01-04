CREATE FUNCTION [SQL#].[Date_NthOccurrenceOfWeekday]
(@Occurrence SMALLINT NULL, @Weekday NVARCHAR (10) NULL, @StartDate DATETIME NULL)
RETURNS DATETIME
AS
 EXTERNAL NAME [SQL#].[DATE].[NthOccurrenceOfWeekday]


GO

