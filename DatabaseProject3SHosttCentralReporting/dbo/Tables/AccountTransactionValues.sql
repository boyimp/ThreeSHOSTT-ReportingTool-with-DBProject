CREATE TABLE [dbo].[AccountTransactionValues] (
    [Id]                           INT             IDENTITY (9371, 1) NOT NULL,
    [AccountTransactionId]         INT             NOT NULL,
    [AccountTransactionDocumentId] INT             NOT NULL,
    [AccountTypeId]                INT             NOT NULL,
    [AccountId]                    INT             NOT NULL,
    [Date]                         DATETIME        NOT NULL,
    [Debit]                        DECIMAL (16, 4) NULL,
    [Credit]                       DECIMAL (16, 4) NULL,
    [Exchange]                     DECIMAL (16, 4) NULL,
    [Name]                         NVARCHAR (4000) NULL,
    [UserId]                       INT             NULL,
    [WorkPeriodId]                 INT             NULL,
    [ShiftId]                      INT             NULL,
    [TerminalId]                   INT             NULL,
    CONSTRAINT [PK_AccountTransactionValues] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountTransactionValues_AccountTransactionDocuments] FOREIGN KEY ([AccountTransactionDocumentId]) REFERENCES [dbo].[AccountTransactionDocuments] ([Id]),
    CONSTRAINT [FK_AccountTransactionValues_AccountTransactions] FOREIGN KEY ([AccountTransactionId]) REFERENCES [dbo].[AccountTransactions] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionId_AccountTransactionDocumentId]
    ON [dbo].[AccountTransactionValues]([AccountTransactionId] ASC, [AccountTransactionDocumentId] ASC);


GO

