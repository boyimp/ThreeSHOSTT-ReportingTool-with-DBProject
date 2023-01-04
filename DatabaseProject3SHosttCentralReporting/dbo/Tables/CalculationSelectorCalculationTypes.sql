CREATE TABLE [dbo].[CalculationSelectorCalculationTypes] (
    [CalculationSelector_Id] INT NOT NULL,
    [CalculationType_Id]     INT NOT NULL,
    CONSTRAINT [PK_dbo.CalculationSelectorCalculationTypes] PRIMARY KEY CLUSTERED ([CalculationSelector_Id] ASC, [CalculationType_Id] ASC),
    CONSTRAINT [FK_dbo.CalculationSelectorCalculationTypes_dbo.CalculationSelectors_CalculationSelector_Id] FOREIGN KEY ([CalculationSelector_Id]) REFERENCES [dbo].[CalculationSelectors] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CalculationSelectorCalculationTypes_dbo.CalculationTypes_CalculationType_Id] FOREIGN KEY ([CalculationType_Id]) REFERENCES [dbo].[CalculationTypes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_CalculationType_Id]
    ON [dbo].[CalculationSelectorCalculationTypes]([CalculationType_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_CalculationSelector_Id]
    ON [dbo].[CalculationSelectorCalculationTypes]([CalculationSelector_Id] ASC);


GO

