CREATE TABLE [dbo].[PeriodicConsumptions] (
    [Id]             INT             IDENTITY (14, 1) NOT NULL,
    [WorkPeriodId]   INT             NOT NULL,
    [StartDate]      DATETIME        NOT NULL,
    [EndDate]        DATETIME        NOT NULL,
    [LastUpdateTime] DATETIME        NOT NULL,
    [Name]           NVARCHAR (4000) NULL,
    [Synced]         BIT             CONSTRAINT [DF_PeriodicConsumptions_Synced] DEFAULT ((0)) NOT NULL,
    [SyncID]         INT             CONSTRAINT [DF_PeriodicConsumptions_SyncID] DEFAULT ((0)) NOT NULL,
    [SyncOutletId]   INT             DEFAULT ((0)) NOT NULL,
    [Finalized]      BIT             CONSTRAINT [DF_PeriodicConsumptions_Finalized] DEFAULT ((0)) NOT NULL,
    [Synced2]        BIT             CONSTRAINT [DF_PeriodicConsumptions_Synced2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.PeriodicConsumptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

