CREATE FUNCTION [SQL#].[String_Count]
(@StringValue NVARCHAR (MAX) NULL, @Search NVARCHAR (MAX) NULL, @StartAt INT NULL, @ComparisonType INT NULL, @CountOverlap BIT NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[STRING].[Count]


GO

