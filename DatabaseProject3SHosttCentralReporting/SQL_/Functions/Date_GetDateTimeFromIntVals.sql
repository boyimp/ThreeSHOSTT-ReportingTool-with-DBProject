CREATE FUNCTION [SQL#].[Date_GetDateTimeFromIntVals]
(@IntDate INT NULL, @IntTime INT NULL)
RETURNS DATETIME
AS
 EXTERNAL NAME [SQL#].[DATE].[GetDateTimeFromIntVals]


GO

