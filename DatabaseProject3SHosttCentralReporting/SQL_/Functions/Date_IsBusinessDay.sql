CREATE FUNCTION [SQL#].[Date_IsBusinessDay]
(@TheDate DATETIME NULL, @ExcludeDaysMask BIGINT NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#].[DATE].[IsBusinessDay]


GO

