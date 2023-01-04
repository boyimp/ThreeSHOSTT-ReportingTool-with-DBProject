CREATE FUNCTION [SQL#].[Twitter_Update]
(@ConsumerKey NVARCHAR (100) NULL, @ConsumerSecret NVARCHAR (100) NULL, @AccessToken NVARCHAR (100) NULL, @AccessTokenSecret NVARCHAR (100) NULL, @Message NVARCHAR (4000) NULL, @InReplyToStatusID BIGINT NULL, @Latitude FLOAT (53) NULL, @Longitude FLOAT (53) NULL)
RETURNS BIGINT
AS
 EXTERNAL NAME [SQL#.Twitterizer].[TWITTER].[Update]


GO

