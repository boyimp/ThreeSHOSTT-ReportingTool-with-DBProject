CREATE FUNCTION [SQL#].[Date_IsLeapYear]
(@Year INT NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#].[DATE].[IsLeapYear]


GO
