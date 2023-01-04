CREATE TABLE [dbo].[OrderTagTemplates] (
    [Id]   INT             IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.OrderTagTemplates] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

