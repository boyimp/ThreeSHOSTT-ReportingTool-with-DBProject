CREATE FUNCTION [SQL#].[Math_Factorial]
(@BaseNumber INT NULL)
RETURNS FLOAT (53)
AS
 EXTERNAL NAME [SQL#].[MATH].[Factorial]


GO

