CREATE FUNCTION [SQL#].[String_Replace]
(@Expression NVARCHAR (MAX) NULL, @Find NVARCHAR (MAX) NULL, @Replacement NVARCHAR (MAX) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SQL#].[STRING].[Replace]


GO

