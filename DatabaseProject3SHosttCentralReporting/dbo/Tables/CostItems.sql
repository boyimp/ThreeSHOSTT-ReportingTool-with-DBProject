CREATE TABLE [dbo].[CostItems] (
    [Id]                     INT             IDENTITY (207, 1) NOT NULL,
    [WarehouseConsumptionId] INT             NOT NULL,
    [PeriodicConsumptionId]  INT             NOT NULL,
    [MenuItemId]             INT             NOT NULL,
    [PortionId]              INT             NOT NULL,
    [PortionName]            NVARCHAR (4000) NULL,
    [Quantity]               NUMERIC (16, 3) NOT NULL,
    [CostPrediction]         NUMERIC (16, 2) NOT NULL,
    [Cost]                   NUMERIC (16, 2) NOT NULL,
    [Name]                   NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.CostItems] PRIMARY KEY CLUSTERED ([Id] ASC, [WarehouseConsumptionId] ASC, [PeriodicConsumptionId] ASC),
    CONSTRAINT [FK_dbo.CostItems_dbo.WarehouseConsumptions_PeriodicConsumptionId_WarehouseConsumptionId] FOREIGN KEY ([PeriodicConsumptionId], [WarehouseConsumptionId]) REFERENCES [dbo].[WarehouseConsumptions] ([Id], [PeriodicConsumptionId]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_PeriodicConsumptionId_WarehouseConsumptionId]
    ON [dbo].[CostItems]([PeriodicConsumptionId] ASC, [WarehouseConsumptionId] ASC);


GO

