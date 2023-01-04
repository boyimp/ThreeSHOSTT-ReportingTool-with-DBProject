CREATE TABLE [dbo].[Tasks] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [TaskTypeId]     INT             NOT NULL,
    [Content]        NVARCHAR (4000) NULL,
    [StartDate]      DATETIME        NOT NULL,
    [EndDate]        DATETIME        NOT NULL,
    [Completed]      BIT             NOT NULL,
    [LastUpdateTime] DATETIME        NOT NULL,
    [Name]           NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.Tasks] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

