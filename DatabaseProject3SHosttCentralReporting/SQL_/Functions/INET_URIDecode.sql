CREATE FUNCTION [SQL#].[INET_URIDecode]
(@EncodedURI NVARCHAR (4000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#.Network].[INET].[URIDecode]


GO

