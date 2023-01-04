CREATE TABLE [dbo].[TaskTokens] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [TaskId]          INT             NOT NULL,
    [Caption]         NVARCHAR (4000) NULL,
    [Value]           NVARCHAR (4000) NULL,
    [Type]            INT             NOT NULL,
    [ReferenceTypeId] INT             NOT NULL,
    [ReferenceId]     INT             NOT NULL,
    CONSTRAINT [PK_dbo.TaskTokens] PRIMARY KEY CLUSTERED ([Id] ASC, [TaskId] ASC),
    CONSTRAINT [FK_dbo.TaskTokens_dbo.Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_TaskId]
    ON [dbo].[TaskTokens]([TaskId] ASC);


GO

