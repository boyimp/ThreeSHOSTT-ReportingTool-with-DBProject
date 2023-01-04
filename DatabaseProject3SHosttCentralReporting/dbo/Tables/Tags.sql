CREATE TABLE [dbo].[Tags] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [TagType]        INT            NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [InsertedDate]   DATETIME       NULL,
    [InsertedBy]     INT            NULL,
    [EditedDate]     DATETIME       NULL,
    [EditedBy]       INT            NULL,
    [LastUpdateTime] DATETIME       NULL,
    CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

