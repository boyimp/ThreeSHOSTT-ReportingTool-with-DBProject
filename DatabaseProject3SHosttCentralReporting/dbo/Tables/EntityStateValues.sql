CREATE TABLE [dbo].[EntityStateValues] (
    [Id]           INT             IDENTITY (103, 1) NOT NULL,
    [EntityId]     INT             NOT NULL,
    [EntityStates] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.EntityStateValues] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

