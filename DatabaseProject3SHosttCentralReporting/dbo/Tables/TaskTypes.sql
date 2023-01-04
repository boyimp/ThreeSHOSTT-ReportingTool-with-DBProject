CREATE TABLE [dbo].[TaskTypes] (
    [Id]   INT             IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.TaskTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

