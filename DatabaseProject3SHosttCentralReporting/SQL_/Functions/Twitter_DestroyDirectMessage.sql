CREATE FUNCTION [SQL#].[Twitter_DestroyDirectMessage]
(@ConsumerKey NVARCHAR (100) NULL, @ConsumerSecret NVARCHAR (100) NULL, @AccessToken NVARCHAR (100) NULL, @AccessTokenSecret NVARCHAR (100) NULL, @StatusID BIGINT NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#.Twitterizer].[TWITTER].[DestroyDirectMessage]


GO

