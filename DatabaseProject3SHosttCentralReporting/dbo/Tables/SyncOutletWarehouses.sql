CREATE TABLE [dbo].[SyncOutletWarehouses] (
    [OutletId]    INT NOT NULL,
    [WarehouseId] INT NOT NULL,
    CONSTRAINT [FK_SyncOutletWarehouse_SyncOutlets] FOREIGN KEY ([OutletId]) REFERENCES [dbo].[SyncOutlets] ([Id]),
    CONSTRAINT [FK_SyncOutletWarehouse_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO

