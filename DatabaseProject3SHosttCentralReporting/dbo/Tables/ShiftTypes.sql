CREATE TABLE [dbo].[ShiftTypes] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (4000) NULL,
    [StartTime] TIME (7)        NULL,
    [EndTime]   TIME (7)        NULL,
    CONSTRAINT [PK_dbo.ShiftTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

