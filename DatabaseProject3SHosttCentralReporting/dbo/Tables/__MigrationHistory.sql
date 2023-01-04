CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId]    NVARCHAR (255) NOT NULL,
    [Model]          IMAGE          NOT NULL,
    [ProductVersion] NVARCHAR (32)  NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
);


GO

