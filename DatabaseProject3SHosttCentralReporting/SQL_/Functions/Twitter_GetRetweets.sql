CREATE FUNCTION [SQL#].[Twitter_GetRetweets]
(@ConsumerKey NVARCHAR (100) NULL, @ConsumerSecret NVARCHAR (100) NULL, @AccessToken NVARCHAR (100) NULL, @AccessTokenSecret NVARCHAR (100) NULL, @StatusID BIGINT NULL, @OptionalParameters [SQL#].[Type_HashTable] NULL)
RETURNS 
     TABLE (
        [StatusID]                       BIGINT          NULL,
        [Created]                        DATETIME        NULL,
        [InReplyToStatusID]              BIGINT          NULL,
        [InReplyToUserID]                BIGINT          NULL,
        [IsFavorited]                    BIT             NULL,
        [IsTruncated]                    BIT             NULL,
        [Source]                         NVARCHAR (200)  NULL,
        [StatusText]                     NVARCHAR (4000) NULL,
        [RecipientID]                    BIGINT          NULL,
        [TimeZone]                       NVARCHAR (100)  NULL,
        [ScreenName]                     NVARCHAR (100)  NULL,
        [UserName]                       NVARCHAR (100)  NULL,
        [UserID]                         BIGINT          NULL,
        [Location]                       NVARCHAR (100)  NULL,
        [PlaceID]                        NVARCHAR (50)   NULL,
        [PlaceName]                      NVARCHAR (500)  NULL,
        [PlaceFullName]                  NVARCHAR (500)  NULL,
        [PlaceType]                      NVARCHAR (500)  NULL,
        [PlaceCountry]                   NVARCHAR (500)  NULL,
        [PlaceLatitude]                  FLOAT (53)      NULL,
        [PlaceLongitude]                 FLOAT (53)      NULL,
        [RateLimit]                      INT             NULL,
        [RateLimitRemaining]             INT             NULL,
        [RateLimitReset]                 DATETIME        NULL,
        [PlaceCountryCode]               NVARCHAR (2)    NULL,
        [PlaceAttributes]                XML             NULL,
        [PlaceBoundingBox]               XML             NULL,
        [PlaceURL]                       NVARCHAR (4000) NULL,
        [FavoriteCount]                  INT             NULL,
        [FilterLevel]                    NVARCHAR (50)   NULL,
        [InReplyToScreenName]            NVARCHAR (100)  NULL,
        [Language]                       NVARCHAR (20)   NULL,
        [PossiblySensitive]              BIT             NULL,
        [QuotedStatusID]                 BIGINT          NULL,
        [RetweetCount]                   INT             NULL,
        [Retweeted]                      BIT             NULL,
        [WithheldCopyright]              BIT             NULL,
        [WithheldScope]                  NVARCHAR (20)   NULL,
        [WithheldInCountries]            NVARCHAR (4000) NULL,
        [Entities]                       XML             NULL,
        [RateLimitResetLocalTime]        DATETIME        NULL,
        [StatusType]                     NVARCHAR (20)   NULL,
        [QuotedStatus]                   XML             NULL,
        [QuotedStatusCreatedAt]          DATETIME        NULL,
        [QuotedStatusCreatedAtLocalTime] DATETIME        NULL,
        [CreatedLocalTime]               DATETIME        NULL)
AS
 EXTERNAL NAME [SQL#.Twitterizer].[TWITTER].[GetRetweets]


GO

