CREATE FUNCTION [SQL#].[Math_Constant]
(@ConstantName NVARCHAR (4000) NULL)
RETURNS FLOAT (53)
AS
 EXTERNAL NAME [SQL#].[MATH].[Constant]


GO
