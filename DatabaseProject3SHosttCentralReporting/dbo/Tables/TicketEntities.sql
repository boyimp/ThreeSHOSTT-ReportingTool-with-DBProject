CREATE TABLE [dbo].[TicketEntities] (
    [Id]               INT             IDENTITY (3383, 1) NOT NULL,
    [EntityTypeId]     INT             NOT NULL,
    [EntityId]         INT             NOT NULL,
    [AccountId]        INT             NOT NULL,
    [AccountTypeId]    INT             NOT NULL,
    [EntityName]       NVARCHAR (4000) NULL,
    [EntityCustomData] NTEXT           NULL,
    [Ticket_Id]        INT             NULL,
    CONSTRAINT [PK_dbo.TicketEntities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TicketEntities_dbo.Tickets_Ticket_Id] FOREIGN KEY ([Ticket_Id]) REFERENCES [dbo].[Tickets] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_Ticket_Id]
    ON [dbo].[TicketEntities]([Ticket_Id] ASC);


GO

