CREATE FUNCTION [SQL#].[Twitter_xAuth]
(@ConsumerKey NVARCHAR (100) NULL, @ConsumerSecret NVARCHAR (100) NULL, @UserName NVARCHAR (100) NULL, @Password NVARCHAR (100) NULL)
RETURNS 
     TABLE (
        [AccessToken]       NVARCHAR (100) NULL,
        [AccessTokenSecret] NVARCHAR (100) NULL)
AS
 EXTERNAL NAME [SQL#.Twitterizer].[TWITTER].[xAuth]


GO

