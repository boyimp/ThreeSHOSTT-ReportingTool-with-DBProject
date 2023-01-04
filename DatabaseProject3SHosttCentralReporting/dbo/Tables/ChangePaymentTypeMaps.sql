CREATE TABLE [dbo].[ChangePaymentTypeMaps] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [ChangePaymentTypeId] INT NOT NULL,
    [TerminalId]          INT NOT NULL,
    [DepartmentId]        INT NOT NULL,
    [UserRoleId]          INT NOT NULL,
    [TicketTypeId]        INT NOT NULL,
    CONSTRAINT [PK_dbo.ChangePaymentTypeMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ChangePaymentTypeMaps_dbo.ChangePaymentTypes_ChangePaymentTypeId] FOREIGN KEY ([ChangePaymentTypeId]) REFERENCES [dbo].[ChangePaymentTypes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_ChangePaymentTypeId]
    ON [dbo].[ChangePaymentTypeMaps]([ChangePaymentTypeId] ASC);


GO

