CREATE TABLE [dbo].[PaidItems] (
    [Id]       INT             IDENTITY (15, 1) NOT NULL,
    [TicketId] INT             NOT NULL,
    [Key]      NVARCHAR (4000) NULL,
    [Quantity] NUMERIC (16, 3) NOT NULL,
    CONSTRAINT [PK_dbo.PaidItems] PRIMARY KEY CLUSTERED ([Id] ASC, [TicketId] ASC),
    CONSTRAINT [FK_dbo.PaidItems_dbo.Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [dbo].[Tickets] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_TicketId]
    ON [dbo].[PaidItems]([TicketId] ASC);


GO

