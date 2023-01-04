CREATE TABLE [dbo].[TerminalUserActivities] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (MAX) NULL,
    [UserId]                INT            NULL,
    [WorkPeriodId]          INT            NULL,
    [ShiftId]               INT            NULL,
    [TerminalId]            INT            NULL,
    [TerminalOperationType] INT            CONSTRAINT [DF_TerminalUserActivities_TerminalOperationType] DEFAULT ((0)) NOT NULL,
    [LastUpdateTime]        DATETIME       NULL,
    CONSTRAINT [PK_dbo.TerminalUserActivities] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

