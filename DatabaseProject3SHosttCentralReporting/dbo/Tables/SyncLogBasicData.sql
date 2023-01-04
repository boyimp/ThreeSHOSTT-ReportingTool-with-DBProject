CREATE TABLE [dbo].[SyncLogBasicData] (
    [BasicDataName] VARCHAR (MAX) NOT NULL,
    [OutletId]      INT           NULL,
    [Synced]        INT           NOT NULL,
    [Synced2]       BIT           CONSTRAINT [DF_SyncLogBasicData_Synced2] DEFAULT ((0)) NOT NULL
);


GO

