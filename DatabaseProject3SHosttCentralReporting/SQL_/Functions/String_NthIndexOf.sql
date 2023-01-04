CREATE FUNCTION [SQL#].[String_NthIndexOf]
(@StringValue NVARCHAR (MAX) NULL, @Search NVARCHAR (MAX) NULL, @StartAt INT NULL, @NthOccurance INT NULL, @ComparisonType INT NULL, @CountOverlap BIT NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQL#].[STRING].[NthIndexOf]


GO

