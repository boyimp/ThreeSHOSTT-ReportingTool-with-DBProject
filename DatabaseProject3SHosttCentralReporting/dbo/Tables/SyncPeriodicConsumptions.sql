CREATE TABLE [dbo].[SyncPeriodicConsumptions] (
    [Id]             INT             IDENTITY (14, 1) NOT NULL,
    [WorkPeriodId]   INT             NOT NULL,
    [StartDate]      DATETIME        NOT NULL,
    [EndDate]        DATETIME        NOT NULL,
    [LastUpdateTime] DATETIME        NOT NULL,
    [Name]           NVARCHAR (4000) NULL,
    [Synced]         BIT             CONSTRAINT [DF_SyncPeriodicConsumptions_Synced] DEFAULT ((0)) NOT NULL,
    [SyncID]         INT             CONSTRAINT [DF_SyncPeriodicConsumptions_SyncID] DEFAULT ((0)) NOT NULL,
    [SyncOutletId]   INT             NULL,
    [Synced2]        BIT             CONSTRAINT [DF_SyncPeriodicConsumptions_Synced2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.SyncPeriodicConsumptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

