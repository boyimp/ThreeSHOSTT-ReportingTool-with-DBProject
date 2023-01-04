CREATE TABLE [dbo].[InventoryTransactionDocuments] (
    [Id]                        INT             IDENTITY (7, 1) NOT NULL,
    [Date]                      DATETIME        NOT NULL,
    [Name]                      NVARCHAR (4000) NULL,
    [Synced]                    BIT             CONSTRAINT [DF_InventoryTransactionDocuments_Synced] DEFAULT ((0)) NOT NULL,
    [SyncID]                    INT             CONSTRAINT [DF_InventoryTransactionDocuments_SyncID] DEFAULT ((0)) NOT NULL,
    [SyncOutletId]              INT             DEFAULT ((0)) NOT NULL,
    [ServerUpdateTime]          DATETIME        NULL,
    [IsProduction]              BIT             CONSTRAINT [DF_InventoryTransactionDocuments_IsProduction] DEFAULT ((0)) NOT NULL,
    [Synced2]                   BIT             CONSTRAINT [DF_InventoryTransactionDocuments_Synced2] DEFAULT ((0)) NOT NULL,
    [ServerUpdateTime2]         DATETIME        NULL,
    [MasterDataID]              INT             NULL,
    [MasterDataTransactionType] INT             NULL,
    CONSTRAINT [PK_dbo.InventoryTransactionDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

