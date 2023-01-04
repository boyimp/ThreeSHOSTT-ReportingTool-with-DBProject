CREATE TABLE [dbo].[TicketTagMaps] (
    [Id]               INT IDENTITY (2, 1) NOT NULL,
    [TicketTagGroupId] INT NOT NULL,
    [TerminalId]       INT NOT NULL,
    [DepartmentId]     INT NOT NULL,
    [UserRoleId]       INT NOT NULL,
    [TicketTypeId]     INT NOT NULL,
    CONSTRAINT [PK_dbo.TicketTagMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TicketTagMaps_dbo.TicketTagGroups_TicketTagGroupId] FOREIGN KEY ([TicketTagGroupId]) REFERENCES [dbo].[TicketTagGroups] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_TicketTagGroupId]
    ON [dbo].[TicketTagMaps]([TicketTagGroupId] ASC);


GO

