CREATE TABLE [dbo].[SyncWorkPeriods] (
    [Id]               INT             IDENTITY (96, 1) NOT NULL,
    [StartDate]        DATETIME        NOT NULL,
    [EndDate]          DATETIME        NOT NULL,
    [StartDescription] NVARCHAR (4000) NULL,
    [EndDescription]   NVARCHAR (4000) NULL,
    [Name]             NVARCHAR (4000) NULL,
    [Synced]           BIT             CONSTRAINT [DF_SyncWorkPeriods_Synced] DEFAULT ((0)) NOT NULL,
    [IsSyncedToSTAR]   BIT             NULL,
    [SyncID]           INT             CONSTRAINT [DF_SyncWorkPeriods_SyncID] DEFAULT ((0)) NOT NULL,
    [SyncOutletId]     INT             NULL,
    [Synced2]          BIT             CONSTRAINT [DF_SyncWorkPeriods_Synced2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo_SyncWorkPeriods] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

