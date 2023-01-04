CREATE TABLE [dbo].[AccountTransactions] (
    [Id]                           INT             IDENTITY (4686, 1) NOT NULL,
    [AccountTransactionDocumentId] INT             NOT NULL,
    [Amount]                       DECIMAL (16, 4) NULL,
    [ExchangeRate]                 DECIMAL (16, 4) NULL,
    [AccountTransactionTypeId]     INT             NOT NULL,
    [SourceAccountTypeId]          INT             NOT NULL,
    [TargetAccountTypeId]          INT             NOT NULL,
    [IsReversed]                   BIT             NOT NULL,
    [Reversable]                   BIT             NOT NULL,
    [Name]                         NVARCHAR (4000) NULL,
    [Synced]                       BIT             CONSTRAINT [DF_AccountTransactions_Synced] DEFAULT ((0)) NOT NULL,
    [SyncID]                       INT             CONSTRAINT [DF_AccountTransactions_SyncID] DEFAULT ((0)) NOT NULL,
    [SyncOutletId]                 INT             DEFAULT ((0)) NOT NULL,
    [Synced2]                      BIT             CONSTRAINT [DF_AccountTransactions_Synced2] DEFAULT ((0)) NOT NULL,
    [UserId]                       INT             NULL,
    [WorkPeriodId]                 INT             NULL,
    [ShiftId]                      INT             NULL,
    [SourceTerminalId]             INT             NULL,
    [TargetTerminalId]             INT             NULL,
    CONSTRAINT [PK_AccountTransactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountTransactions_dbo.AccountTransactionDocuments_AccountTransactionDocumentId] FOREIGN KEY ([AccountTransactionDocumentId]) REFERENCES [dbo].[AccountTransactionDocuments] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionDocumentId]
    ON [dbo].[AccountTransactions]([AccountTransactionDocumentId] ASC);


GO

