CREATE FUNCTION [SQL#].[Convert_BinaryToHexString]
(@BinaryValue VARBINARY (MAX) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SQL#].[CONVERT].[BinaryToHexString]


GO

