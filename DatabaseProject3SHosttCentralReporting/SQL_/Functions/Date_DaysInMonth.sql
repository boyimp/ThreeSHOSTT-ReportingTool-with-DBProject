CREATE FUNCTION [SQL#].[Date_DaysInMonth]
(@Year INT NULL, @Month INT NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[DATE].[DaysInMonth]


GO

