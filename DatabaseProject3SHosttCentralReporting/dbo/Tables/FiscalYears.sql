CREATE TABLE [dbo].[FiscalYears] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [StartDate] DATETIME       NULL,
    [EndDate]   DATETIME       NULL,
    [name]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_FiscalYear] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

