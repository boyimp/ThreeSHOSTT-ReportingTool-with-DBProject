CREATE FUNCTION [SQL#].[Date_GetIntDate]
(@TheDate DATETIME NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[DATE].[GetIntDate]


GO

