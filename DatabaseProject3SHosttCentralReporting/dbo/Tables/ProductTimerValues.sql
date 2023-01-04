CREATE TABLE [dbo].[ProductTimerValues] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [ProductTimerId] INT             NOT NULL,
    [PriceType]      INT             NOT NULL,
    [PriceDuration]  NUMERIC (16, 2) NOT NULL,
    [MinTime]        NUMERIC (16, 2) NOT NULL,
    [TimeRounding]   NUMERIC (16, 2) NOT NULL,
    [Start]          DATETIME        NOT NULL,
    [End]            DATETIME        NOT NULL,
    CONSTRAINT [PK_dbo.ProductTimerValues] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

