CREATE FUNCTION [SQL#].[Math_Truncate]
(@BaseNumber FLOAT (53) NULL, @DecimalPlaces TINYINT NULL)
RETURNS FLOAT (53)
AS
 EXTERNAL NAME [SQL#].[MATH].[Truncate]


GO

