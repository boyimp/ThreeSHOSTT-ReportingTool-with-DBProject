CREATE FUNCTION [SQL#].[String_ToTitleCase4k]
(@StringValue NVARCHAR (4000) NULL, @CultureNameOrLCID NVARCHAR (100) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[STRING].[ToTitleCase]


GO

