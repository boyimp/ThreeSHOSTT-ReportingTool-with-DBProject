CREATE FUNCTION [SQL#].[DB_WaitForDelay]
(@MillisecondsToSleep INT NULL, @NonDeterminiser VARBINARY (4000) NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#.DB].[FunctionHelpers].[WaitForDelay]


GO

