CREATE FUNCTION [SQL#].[Date_FromUNIXTime]
(@UNIXDate FLOAT (53) NULL)
RETURNS DATETIME
AS
 EXTERNAL NAME [SQL#].[DATE].[FromUNIXTime]


GO

