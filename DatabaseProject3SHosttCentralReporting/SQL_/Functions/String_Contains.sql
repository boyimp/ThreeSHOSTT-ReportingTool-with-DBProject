CREATE FUNCTION [SQL#].[String_Contains]
(@StringValue NVARCHAR (MAX) NULL, @SearchValue NVARCHAR (MAX) NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#].[STRING].[Contains]


GO

