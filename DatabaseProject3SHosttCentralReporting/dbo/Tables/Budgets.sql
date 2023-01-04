CREATE TABLE [dbo].[Budgets] (
    [Id]             INT             IDENTITY (44, 1) NOT NULL,
    [BudgetMasterId] INT             NOT NULL,
    [Account_Id]     INT             NOT NULL,
    [AccountName]    NVARCHAR (MAX)  NULL,
    [Budget]         DECIMAL (16, 4) NULL,
    CONSTRAINT [PK_dbo_Budgets] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo_Budgets_dbo_Accounts_Account_Id] FOREIGN KEY ([Account_Id]) REFERENCES [dbo].[Accounts] ([Id]),
    CONSTRAINT [FK_dbo_Budgets_dbo_Accounts_BudgetMaster_Id] FOREIGN KEY ([BudgetMasterId]) REFERENCES [dbo].[BudgetMasters] ([Id]) ON DELETE CASCADE
);


GO

