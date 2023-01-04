CREATE TABLE [dbo].[Notifications] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (400) NOT NULL,
    [MessageId]           NVARCHAR (400) NOT NULL,
    [NotificationMessage] NVARCHAR (MAX) NOT NULL,
    [OrganizationName]    NVARCHAR (400) NOT NULL,
    [Transaction]         NVARCHAR (MAX) NOT NULL,
    [LastUpdateTime]      DATETIME       NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

