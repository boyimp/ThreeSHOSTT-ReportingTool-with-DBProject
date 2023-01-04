CREATE TABLE [dbo].[WarehouseConsumptions] (
    [Id]                              INT IDENTITY (51, 1) NOT NULL,
    [PeriodicConsumptionId]           INT NOT NULL,
    [WarehouseId]                     INT NOT NULL,
    [IsSyncedToSTAR]                  BIT NULL,
    [IsTransactionsForMasterDataDone] BIT CONSTRAINT [DF_WarehouseConsumptions_IsTransactionsForMasterDataDone] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.WarehouseConsumptions] PRIMARY KEY CLUSTERED ([Id] ASC, [PeriodicConsumptionId] ASC),
    CONSTRAINT [FK_dbo.WarehouseConsumptions_dbo.PeriodicConsumptions_PeriodicConsumptionId] FOREIGN KEY ([PeriodicConsumptionId]) REFERENCES [dbo].[PeriodicConsumptions] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_PeriodicConsumptionId]
    ON [dbo].[WarehouseConsumptions]([PeriodicConsumptionId] ASC);


GO

