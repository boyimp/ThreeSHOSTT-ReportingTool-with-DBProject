CREATE FUNCTION [SQL#].[Date_LastDayOfMonth]
(@TheDate DATETIME NULL, @NewHour INT NULL, @NewMinute INT NULL, @NewSecond INT NULL, @NewMillisecond INT NULL)
RETURNS DATETIME
AS
 EXTERNAL NAME [SQL#].[DATE].[LastDayOfMonth]


GO

