CREATE TABLE [dbo].[ForeignCurrencies] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [CurrencySymbol] NVARCHAR (4000) NULL,
    [ExchangeRate]   NUMERIC (18, 2) NOT NULL,
    [Rounding]       NUMERIC (18, 2) NOT NULL,
    [Name]           NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.ForeignCurrencies] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

