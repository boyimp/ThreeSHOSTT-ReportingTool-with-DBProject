CREATE TABLE [dbo].[MetaTags] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [MetaTagType]    INT            NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [LastUpdateTime] DATETIME       NULL,
    CONSTRAINT [PK_MetaTag] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

