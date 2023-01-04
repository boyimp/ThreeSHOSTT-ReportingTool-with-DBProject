CREATE FUNCTION [SQL#].[String_LastIndexOf4k]
(@StringValue NVARCHAR (4000) NULL, @SearchValue NVARCHAR (4000) NULL, @StartIndex INT NULL, @ComparisonType INT NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[STRING].[LastIndexOf]


GO

