CREATE FUNCTION [SQL#].[Math_RandomRange]
(@Seed INT NULL, @LowerBound INT NULL, @UpperBound INT NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[MATH].[RandomRange]


GO

