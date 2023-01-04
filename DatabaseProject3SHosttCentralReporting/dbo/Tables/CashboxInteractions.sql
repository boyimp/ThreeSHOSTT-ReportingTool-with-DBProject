CREATE TABLE [dbo].[CashboxInteractions] (
    [Id]                       INT             IDENTITY (1, 1) NOT NULL,
    [Name]                     NVARCHAR (4000) NULL,
    [SourceTerminalId]         INT             NULL,
    [TargetTerminalId]         INT             NULL,
    [DocumentTypeId]           INT             NULL,
    [Amount]                   DECIMAL (18, 4) NULL,
    [CashboxInteractionStatus] INT             NULL,
    [LastUpdateTime]           DATETIME        NULL,
    [UserId]                   INT             NULL,
    [WorkPeriodId]             INT             NULL,
    [ShiftId]                  INT             NULL,
    CONSTRAINT [PK_dbo.CashboxInteractions] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

