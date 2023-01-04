CREATE TABLE [dbo].[ChangePayments] (
    [Id]                                              INT             IDENTITY (1, 1) NOT NULL,
    [TicketId]                                        INT             NOT NULL,
    [ChangePaymentTypeId]                             INT             NOT NULL,
    [Name]                                            NVARCHAR (4000) NULL,
    [Date]                                            DATETIME        NOT NULL,
    [AccountTransactionId]                            INT             NOT NULL,
    [Amount]                                          NUMERIC (18, 2) NOT NULL,
    [AccountTransaction_Id]                           INT             NULL,
    [AccountTransaction_AccountTransactionDocumentId] INT             NULL,
    [SplitCount]                                      INT             CONSTRAINT [DF_ChangePayments_SplitCount] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbo.ChangePayments] PRIMARY KEY CLUSTERED ([Id] ASC, [TicketId] ASC),
    CONSTRAINT [FK_dbo.ChangePayments_dbo.Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [dbo].[Tickets] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_TicketId]
    ON [dbo].[ChangePayments]([TicketId] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransaction_Id_AccountTransaction_AccountTransactionDocumentId]
    ON [dbo].[ChangePayments]([AccountTransaction_Id] ASC, [AccountTransaction_AccountTransactionDocumentId] ASC);


GO

