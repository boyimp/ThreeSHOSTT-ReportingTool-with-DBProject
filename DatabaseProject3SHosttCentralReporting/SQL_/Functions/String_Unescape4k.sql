CREATE FUNCTION [SQL#].[String_Unescape4k]
(@EscapedString NVARCHAR (4000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[STRING].[Unescape]


GO

