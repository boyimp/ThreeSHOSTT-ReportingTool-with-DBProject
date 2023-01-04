CREATE FUNCTION [SQL#].[Date_DaysInMonthFromDateTime]
(@TheDate DATETIME NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[DATE].[DaysInMonthFromDateTime]


GO

