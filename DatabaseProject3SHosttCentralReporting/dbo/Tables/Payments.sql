CREATE TABLE [dbo].[Payments] (
    [Id]                                              INT             IDENTITY (1821, 1) NOT NULL,
    [TicketId]                                        INT             NOT NULL,
    [PaymentTypeId]                                   INT             NOT NULL,
    [Name]                                            NVARCHAR (4000) NULL,
    [Date]                                            DATETIME        NOT NULL,
    [AccountTransactionId]                            INT             NOT NULL,
    [Amount]                                          DECIMAL (16, 4) NULL,
    [AccountTransaction_Id]                           INT             NULL,
    [AccountTransaction_AccountTransactionDocumentId] INT             NULL,
    [Remarks]                                         NVARCHAR (4000) CONSTRAINT [DF_Payments_Remarks] DEFAULT (N'') NOT NULL,
    [CustomData]                                      NTEXT           NULL,
    [PaymentTypeName]                                 NVARCHAR (4000) CONSTRAINT [DF_Payments_PaymentTypeName] DEFAULT ('') NOT NULL,
    [TenderedAmount]                                  DECIMAL (18, 4) CONSTRAINT [DF_Payments_TenderedAmount] DEFAULT ((0)) NOT NULL,
    [ProcessedAmount]                                 DECIMAL (18, 4) CONSTRAINT [DF_Payments_ProcessedAmount] DEFAULT ((0)) NOT NULL,
    [ChangeAmount]                                    DECIMAL (18, 4) CONSTRAINT [DF_Payments_ChangeAmount] DEFAULT ((0)) NOT NULL,
    [SelectedQuantity]                                INT             CONSTRAINT [DF_Payments_SelectedQuantity] DEFAULT ((0)) NOT NULL,
    [RemainingAmount]                                 DECIMAL (18, 4) CONSTRAINT [DF_Payments_RemainingAmount] DEFAULT ((0)) NOT NULL,
    [SplitCount]                                      INT             CONSTRAINT [DF_Payments_SplitCount] DEFAULT ((1)) NOT NULL,
    [UserId]                                          INT             NULL,
    [WorkPeriodId]                                    INT             NULL,
    [ShiftId]                                         INT             NULL,
    [TerminalId]                                      INT             NULL,
    CONSTRAINT [PK_dbo.Payments] PRIMARY KEY CLUSTERED ([Id] ASC, [TicketId] ASC),
    CONSTRAINT [FK_dbo.Payments_dbo.Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [dbo].[Tickets] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payments_AccountTransactionDocuments] FOREIGN KEY ([AccountTransaction_AccountTransactionDocumentId]) REFERENCES [dbo].[AccountTransactionDocuments] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_TicketId]
    ON [dbo].[Payments]([TicketId] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransaction_Id_AccountTransaction_AccountTransactionDocumentId]
    ON [dbo].[Payments]([AccountTransaction_Id] ASC, [AccountTransaction_AccountTransactionDocumentId] ASC);


GO

