CREATE TABLE [dbo].[ProgramSettingValues] (
    [Id]    INT             IDENTITY (1, 1) NOT NULL,
    [Value] NVARCHAR (250)  NULL,
    [Name]  NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.ProgramSettingValues] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

