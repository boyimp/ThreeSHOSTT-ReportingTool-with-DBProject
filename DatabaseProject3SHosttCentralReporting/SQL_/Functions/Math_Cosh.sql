CREATE FUNCTION [SQL#].[Math_Cosh]
(@BaseNumber FLOAT (53) NULL)
RETURNS FLOAT (53)
AS
 EXTERNAL NAME [SQL#].[MATH].[Cosh]


GO
