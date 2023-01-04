CREATE TABLE [dbo].[KitchenScreenMaps] (
    [Id]                INT IDENTITY (8, 1) NOT NULL,
    [KitchenScreenId]   INT NOT NULL,
    [TerminalId]        INT NOT NULL,
    [DepartmentId]      INT NOT NULL,
    [UserRoleId]        INT NOT NULL,
    [PrinterTemplateId] INT NOT NULL,
    [StatesID]          INT NOT NULL,
    [TicketTypeId]      INT NOT NULL,
    CONSTRAINT [PK_dbo.KitchenScreenMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo_KitchenScreenMaps_dbo_KitchenScreens_KitchenScreenId] FOREIGN KEY ([KitchenScreenId]) REFERENCES [dbo].[KitchenScreens] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_KitchenScreenId]
    ON [dbo].[KitchenScreenMaps]([KitchenScreenId] ASC);


GO

GRANT UPDATE
    ON OBJECT::[dbo].[KitchenScreenMaps] TO PUBLIC
    AS [dbo];


GO

GRANT INSERT
    ON OBJECT::[dbo].[KitchenScreenMaps] TO PUBLIC
    AS [dbo];


GO

GRANT SELECT
    ON OBJECT::[dbo].[KitchenScreenMaps] TO PUBLIC
    AS [dbo];


GO

GRANT DELETE
    ON OBJECT::[dbo].[KitchenScreenMaps] TO PUBLIC
    AS [dbo];


GO

GRANT REFERENCES
    ON OBJECT::[dbo].[KitchenScreenMaps] TO PUBLIC
    AS [dbo];


GO

