CREATE FUNCTION [SQL#].[Twitter_FollowUser]
(@ConsumerKey NVARCHAR (100) NULL, @ConsumerSecret NVARCHAR (100) NULL, @AccessToken NVARCHAR (100) NULL, @AccessTokenSecret NVARCHAR (100) NULL, @ScreenName NVARCHAR (20) NULL)
RETURNS 
     TABLE (
        [UserID]                         BIGINT          NULL,
        [ScreenName]                     NVARCHAR (100)  NULL,
        [UserName]                       NVARCHAR (100)  NULL,
        [IsProtected]                    BIT             NULL,
        [IsVerified]                     BIT             NULL,
        [Description]                    NVARCHAR (4000) NULL,
        [CreatedOn]                      DATETIME        NULL,
        [Location]                       NVARCHAR (500)  NULL,
        [TimeZone]                       NVARCHAR (100)  NULL,
        [UTCOffset]                      INT             NULL,
        [ProfileImageUri]                NVARCHAR (2048) NULL,
        [ProfileUri]                     NVARCHAR (2048) NULL,
        [FriendsCount]                   INT             NULL,
        [NumberOfFollowers]              INT             NULL,
        [NumberOfStatuses]               INT             NULL,
        [StatusText]                     NVARCHAR (4000) NULL,
        [RateLimit]                      INT             NULL,
        [RateLimitRemaining]             INT             NULL,
        [RateLimitReset]                 DATETIME        NULL,
        [Language]                       NVARCHAR (50)   NULL,
        [NumberOfPublicListMemberships]  INT             NULL,
        [IsGeoEnabled]                   BIT             NULL,
        [Following]                      BIT             NULL,
        [Muting]                         BIT             NULL,
        [ContributorsEnabled]            BIT             NULL,
        [IsTranslator]                   BIT             NULL,
        [FollowRequestSent]              BIT             NULL,
        [NumberOfFavorites]              INT             NULL,
        [ProfileImageUriHttps]           NVARCHAR (2048) NULL,
        [ProfileBackgroundColor]         NVARCHAR (20)   NULL,
        [ProfileTextColor]               NVARCHAR (20)   NULL,
        [ProfileLinkColor]               NVARCHAR (20)   NULL,
        [ProfileSidebarFillColor]        NVARCHAR (20)   NULL,
        [ProfileSidebarBorderColor]      NVARCHAR (20)   NULL,
        [ProfileBackgroundImageUri]      NVARCHAR (2048) NULL,
        [ProfileBackgroundImageUriHttps] NVARCHAR (2048) NULL,
        [ProfileUseBackgroundImage]      BIT             NULL,
        [ProfileBackgroundTile]          BIT             NULL,
        [DefaultProfile]                 BIT             NULL,
        [DefaultProfileImage]            BIT             NULL,
        [PreviousCursor]                 NVARCHAR (50)   NULL,
        [NextCursor]                     NVARCHAR (50)   NULL,
        [RateLimitResetLocalTime]        DATETIME        NULL)
AS
 EXTERNAL NAME [SQL#.Twitterizer].[TWITTER].[FollowUser]


GO

