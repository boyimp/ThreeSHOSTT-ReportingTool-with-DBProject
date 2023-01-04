CREATE FUNCTION [SQL#].[Math_RoundToEvenFloat]
(@BaseNumber FLOAT (53) NULL, @DecimalPlaces TINYINT NULL)
RETURNS FLOAT (53)
AS
 EXTERNAL NAME [SQL#].[MATH].[RoundToEvenFloat]


GO

