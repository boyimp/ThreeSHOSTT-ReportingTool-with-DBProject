CREATE TABLE [dbo].[CentralWebSync] (
    [DepartmentID]   INT           NOT NULL,
    [CollectionName] NVARCHAR (50) NOT NULL,
    [Synced]         SMALLINT      NOT NULL,
    [LastSyncTime]   DATETIME      NULL,
    [Synced2]        BIT           CONSTRAINT [DF_CentralWebSync_Synced2] DEFAULT ((0)) NOT NULL
);


GO

