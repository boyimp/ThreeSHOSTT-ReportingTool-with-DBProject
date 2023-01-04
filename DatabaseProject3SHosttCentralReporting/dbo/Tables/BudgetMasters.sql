CREATE TABLE [dbo].[BudgetMasters] (
    [Id]        INT             IDENTITY (7, 1) NOT NULL,
    [StartDate] DATETIME        NOT NULL,
    [EndDate]   DATETIME        NOT NULL,
    [Name]      NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo_BudgetMasters] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

