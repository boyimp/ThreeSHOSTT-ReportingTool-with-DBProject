CREATE FUNCTION [SQL#].[String_StartsWith]
(@StringValue NVARCHAR (MAX) NULL, @SearchValue NVARCHAR (4000) NULL, @ComparisonType INT NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#].[STRING].[StartsWith]


GO

