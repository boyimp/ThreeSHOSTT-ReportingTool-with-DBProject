CREATE FUNCTION [SQL#].[Math_RoundToEvenDecimal]
(@BaseNumber DECIMAL (28, 10) NULL, @DecimalPlaces TINYINT NULL)
RETURNS DECIMAL (28, 10)
AS
 EXTERNAL NAME [SQL#].[MATH].[RoundToEvenDecimal]


GO

