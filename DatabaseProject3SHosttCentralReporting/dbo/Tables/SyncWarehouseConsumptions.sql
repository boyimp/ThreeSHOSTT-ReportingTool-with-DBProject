CREATE TABLE [dbo].[SyncWarehouseConsumptions] (
    [Id]                    INT IDENTITY (51, 1) NOT NULL,
    [PeriodicConsumptionId] INT NOT NULL,
    [WarehouseId]           INT NOT NULL,
    [IsSyncedToSTAR]        BIT NULL,
    [SyncOutletId]          INT NULL,
    CONSTRAINT [PK_dbo.SyncWarehouseConsumptions] PRIMARY KEY CLUSTERED ([Id] ASC, [PeriodicConsumptionId] ASC),
    CONSTRAINT [FK_dbo.SyncWarehouseConsumptions_dbo.SyncPeriodicConsumptions_PeriodicConsumptionId] FOREIGN KEY ([PeriodicConsumptionId]) REFERENCES [dbo].[SyncPeriodicConsumptions] ([Id]) ON DELETE CASCADE
);


GO

