CREATE FUNCTION [SQL#].[Date_NewDateTime]
(@Year INT NULL, @Month INT NULL, @Day INT NULL, @Hour INT NULL, @Minute INT NULL, @Second INT NULL, @Millisecond INT NULL)
RETURNS DATETIME
AS
 EXTERNAL NAME [SQL#].[DATE].[NewDateTime]


GO

