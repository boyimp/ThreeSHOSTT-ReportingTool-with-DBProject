CREATE TABLE [dbo].[TicketTags] (
    [Id]               INT             IDENTITY (4, 1) NOT NULL,
    [TicketTagGroupId] INT             NOT NULL,
    [Name]             NVARCHAR (4000) NULL,
    [Message]          NVARCHAR (4000) CONSTRAINT [DF_TicketTags_Message] DEFAULT (N'') NOT NULL,
    CONSTRAINT [PK_dbo.TicketTags] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TicketTags_dbo.TicketTagGroups_TicketTagGroupId] FOREIGN KEY ([TicketTagGroupId]) REFERENCES [dbo].[TicketTagGroups] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_TicketTagGroupId]
    ON [dbo].[TicketTags]([TicketTagGroupId] ASC);


GO

