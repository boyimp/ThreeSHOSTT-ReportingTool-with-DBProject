CREATE TABLE [dbo].[ChangePaymentTypes] (
    [Id]                        INT             IDENTITY (1, 1) NOT NULL,
    [SortOrder]                 INT             NOT NULL,
    [Name]                      NVARCHAR (4000) NULL,
    [AccountTransactionType_Id] INT             NULL,
    [Account_Id]                INT             NULL,
    CONSTRAINT [PK_dbo.ChangePaymentTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ChangePaymentTypes_dbo.Accounts_Account_Id] FOREIGN KEY ([Account_Id]) REFERENCES [dbo].[Accounts] ([Id]),
    CONSTRAINT [FK_dbo.ChangePaymentTypes_dbo.AccountTransactionTypes_AccountTransactionType_Id] FOREIGN KEY ([AccountTransactionType_Id]) REFERENCES [dbo].[AccountTransactionTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionType_Id]
    ON [dbo].[ChangePaymentTypes]([AccountTransactionType_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_Account_Id]
    ON [dbo].[ChangePaymentTypes]([Account_Id] ASC);


GO

