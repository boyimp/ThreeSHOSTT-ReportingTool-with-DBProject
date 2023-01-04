CREATE TABLE [dbo].[States] (
    [Id]         INT             IDENTITY (4, 1) NOT NULL,
    [GroupName]  NVARCHAR (4000) NULL,
    [StateType]  INT             NOT NULL,
    [Color]      NVARCHAR (4000) NULL,
    [Name]       NVARCHAR (4000) NULL,
    [StateOrder] INT             CONSTRAINT [DF_States_StateOrder] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.States] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

