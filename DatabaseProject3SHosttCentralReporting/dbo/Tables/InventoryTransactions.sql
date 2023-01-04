CREATE TABLE [dbo].[InventoryTransactions] (
    [Id]                             INT             IDENTITY (44, 1) NOT NULL,
    [InventoryTransactionDocumentId] INT             NOT NULL,
    [InventoryTransactionTypeId]     INT             NOT NULL,
    [SourceWarehouseId]              INT             NOT NULL,
    [TargetWarehouseId]              INT             NOT NULL,
    [Date]                           DATETIME        NOT NULL,
    [Unit]                           NVARCHAR (4000) NULL,
    [Multiplier]                     INT             NOT NULL,
    [Quantity]                       DECIMAL (16, 4) NULL,
    [Price]                          DECIMAL (16, 4) NULL,
    [InventoryItem_Id]               INT             NULL,
    [Produced]                       BIT             CONSTRAINT [DF_InventoryTransactions_Produced] DEFAULT ((0)) NOT NULL,
    [RawMaterial]                    BIT             CONSTRAINT [DF_InventoryTransactions_RawMaterial] DEFAULT ((0)) NOT NULL,
    [MasterDataID]                   INT             NULL,
    [MasterDataTransactionType]      INT             NULL,
    CONSTRAINT [PK_dbo.InventoryTransactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.InventoryTransactions_dbo.InventoryItems_InventoryItem_Id] FOREIGN KEY ([InventoryItem_Id]) REFERENCES [dbo].[InventoryItems] ([Id]),
    CONSTRAINT [FK_dbo.InventoryTransactions_dbo.InventoryTransactionDocuments_InventoryTransactionDocumentId] FOREIGN KEY ([InventoryTransactionDocumentId]) REFERENCES [dbo].[InventoryTransactionDocuments] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_InventoryTransactionDocumentId]
    ON [dbo].[InventoryTransactions]([InventoryTransactionDocumentId] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_InventoryItem_Id]
    ON [dbo].[InventoryTransactions]([InventoryItem_Id] ASC);


GO

