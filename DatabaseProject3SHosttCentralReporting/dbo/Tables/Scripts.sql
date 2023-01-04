CREATE TABLE [dbo].[Scripts] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [HandlerName] NVARCHAR (4000) NULL,
    [Code]        NTEXT           NULL,
    [Name]        NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.Scripts] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

