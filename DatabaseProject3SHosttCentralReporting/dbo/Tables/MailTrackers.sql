CREATE TABLE [dbo].[MailTrackers] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (4000) NOT NULL,
    [ToEMailAddress]    NVARCHAR (4000) NOT NULL,
    [Subject]           NVARCHAR (4000) NOT NULL,
    [SMTPServer]        NVARCHAR (4000) NOT NULL,
    [CCEmailAddress]    NVARCHAR (4000) NULL,
    [FromEmail]         NVARCHAR (4000) NOT NULL,
    [FromEmailPassword] NVARCHAR (4000) NOT NULL,
    [MsgBody]           NVARCHAR (MAX)  NOT NULL,
    [LastUpdateTime]    DATETIME        NOT NULL,
    [Status]            BIT             CONSTRAINT [DF_MailTracker_Delivered] DEFAULT ((0)) NOT NULL,
    [SMTPPort]          INT             NULL,
    [EnableSSL]         BIT             CONSTRAINT [DF_MailTrackers_EnableSSL] DEFAULT ((0)) NOT NULL,
    [StatusMessage]     NVARCHAR (400)  NULL,
    [AtthachmentFile]   NVARCHAR (400)  NULL,
    CONSTRAINT [PK_dbo_MailTracker] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

