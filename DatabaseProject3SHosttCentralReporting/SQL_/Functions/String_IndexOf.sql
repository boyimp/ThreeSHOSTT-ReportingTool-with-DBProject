CREATE FUNCTION [SQL#].[String_IndexOf]
(@StringValue NVARCHAR (MAX) NULL, @SearchValue NVARCHAR (4000) NULL, @StartIndex INT NULL, @ComparisonType INT NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[STRING].[IndexOf]


GO

