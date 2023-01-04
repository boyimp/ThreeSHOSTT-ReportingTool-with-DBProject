CREATE TABLE [dbo].[TaskTypeEntityTypes] (
    [TaskType_Id]   INT NOT NULL,
    [EntityType_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.TaskTypeEntityTypes] PRIMARY KEY CLUSTERED ([TaskType_Id] ASC, [EntityType_Id] ASC),
    CONSTRAINT [FK_dbo.TaskTypeEntityTypes_dbo.EntityTypes_EntityType_Id] FOREIGN KEY ([EntityType_Id]) REFERENCES [dbo].[EntityTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TaskTypeEntityTypes_dbo.TaskTypes_TaskType_Id] FOREIGN KEY ([TaskType_Id]) REFERENCES [dbo].[TaskTypes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_EntityType_Id]
    ON [dbo].[TaskTypeEntityTypes]([EntityType_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_TaskType_Id]
    ON [dbo].[TaskTypeEntityTypes]([TaskType_Id] ASC);


GO

