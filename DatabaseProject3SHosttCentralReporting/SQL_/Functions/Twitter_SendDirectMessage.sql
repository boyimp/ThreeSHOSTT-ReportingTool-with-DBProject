CREATE FUNCTION [SQL#].[Twitter_SendDirectMessage]
(@ConsumerKey NVARCHAR (100) NULL, @ConsumerSecret NVARCHAR (100) NULL, @AccessToken NVARCHAR (100) NULL, @AccessTokenSecret NVARCHAR (100) NULL, @Message NVARCHAR (4000) NULL, @Recipient NVARCHAR (20) NULL)
RETURNS BIGINT
AS
 EXTERNAL NAME [SQL#.Twitterizer].[TWITTER].[SendDirectMessage]


GO

