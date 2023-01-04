CREATE TABLE [dbo].[HHTSync] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [SyncRequired] BIT CONSTRAINT [DF_HHTSync_SyncRequired] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_HHTSync] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

