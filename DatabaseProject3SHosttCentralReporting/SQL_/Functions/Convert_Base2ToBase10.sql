CREATE FUNCTION [SQL#].[Convert_Base2ToBase10]
(@Base2Value NVARCHAR (64) NULL)
RETURNS BIGINT
AS
 EXTERNAL NAME [SQL#].[CONVERT].[Base2ToBase10]


GO

