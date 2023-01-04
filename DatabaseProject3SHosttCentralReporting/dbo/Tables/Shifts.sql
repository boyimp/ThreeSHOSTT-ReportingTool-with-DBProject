CREATE TABLE [dbo].[Shifts] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (4000) NULL,
    [StartDate]        DATETIME        NOT NULL,
    [EndDate]          DATETIME        NOT NULL,
    [StartDescription] NVARCHAR (4000) NULL,
    [EndDescription]   NVARCHAR (4000) NULL,
    [ShiftTypeId]      INT             NULL,
    [WorkPeriodId]     INT             NULL,
    CONSTRAINT [PK_dbo.Shifts] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

