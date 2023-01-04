CREATE TABLE [dbo].[AccountTransactionDocumentAccountMaps] (
    [Id]                               INT             IDENTITY (1, 1) NOT NULL,
    [AccountTransactionDocumentTypeId] INT             NOT NULL,
    [AccountId]                        INT             NOT NULL,
    [AccountName]                      NVARCHAR (4000) NULL,
    [MappedAccountId]                  INT             NOT NULL,
    [MappedAccountName]                NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.AccountTransactionDocumentAccountMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountTransactionDocumentAccountMaps_dbo.AccountTransactionDocumentTypes_AccountTransactionDocumentTypeId] FOREIGN KEY ([AccountTransactionDocumentTypeId]) REFERENCES [dbo].[AccountTransactionDocumentTypes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionDocumentTypeId]
    ON [dbo].[AccountTransactionDocumentAccountMaps]([AccountTransactionDocumentTypeId] ASC);


GO

