CREATE FUNCTION [SQL#].[Util_IsValidConvert]
(@ValueToConvert NVARCHAR (MAX) NULL, @ConvertToDataTypes NVARCHAR (4000) NULL, @DecimalPrecision SMALLINT NULL, @DecimalScale SMALLINT NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#].[UTILITY].[IsValidConvert]


GO

