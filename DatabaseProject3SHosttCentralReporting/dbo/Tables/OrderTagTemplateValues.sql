CREATE TABLE [dbo].[OrderTagTemplateValues] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [OrderTagTemplateId] INT NOT NULL,
    [OrderTagGroup_Id]   INT NULL,
    [OrderTag_Id]        INT NULL,
    CONSTRAINT [PK_dbo.OrderTagTemplateValues] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.OrderTagTemplateValues_dbo.OrderTagGroups_OrderTagGroup_Id] FOREIGN KEY ([OrderTagGroup_Id]) REFERENCES [dbo].[OrderTagGroups] ([Id]),
    CONSTRAINT [FK_dbo.OrderTagTemplateValues_dbo.OrderTags_OrderTag_Id] FOREIGN KEY ([OrderTag_Id]) REFERENCES [dbo].[OrderTags] ([Id]),
    CONSTRAINT [FK_dbo.OrderTagTemplateValues_dbo.OrderTagTemplates_OrderTagTemplateId] FOREIGN KEY ([OrderTagTemplateId]) REFERENCES [dbo].[OrderTagTemplates] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_OrderTagTemplateId]
    ON [dbo].[OrderTagTemplateValues]([OrderTagTemplateId] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_OrderTagGroup_Id]
    ON [dbo].[OrderTagTemplateValues]([OrderTagGroup_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_OrderTag_Id]
    ON [dbo].[OrderTagTemplateValues]([OrderTag_Id] ASC);


GO

