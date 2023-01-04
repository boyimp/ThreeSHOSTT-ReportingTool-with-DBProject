CREATE TABLE [dbo].[AccountTransactionDocuments] (
    [Id]                INT             IDENTITY (1795, 1) NOT NULL,
    [Date]              DATETIME        NOT NULL,
    [Name]              NVARCHAR (4000) NULL,
    [Synced]            BIT             CONSTRAINT [DF_AccountTransactionDocuments_Synced] DEFAULT ((0)) NOT NULL,
    [SyncID]            INT             CONSTRAINT [DF_AccountTransactionDocuments_SyncID] DEFAULT ((0)) NOT NULL,
    [ServerUpdateTime]  DATETIME        CONSTRAINT [DF_AccountTransactionDocuments_ServerUpdateTime] DEFAULT (getdate()) NOT NULL,
    [SyncOutletId]      INT             DEFAULT ((0)) NOT NULL,
    [Synced2]           BIT             CONSTRAINT [DF_AccountTransactionDocuments_Synced2] DEFAULT ((0)) NOT NULL,
    [ServerUpdateTime2] DATETIME        CONSTRAINT [DF_AccountTransactionDocuments_ServerUpdateTime2] DEFAULT (getdate()) NOT NULL,
    [UserId]            INT             NULL,
    [WorkPeriodId]      INT             NULL,
    [ShiftId]           INT             NULL,
    [TerminalId]        INT             NULL,
    CONSTRAINT [PK_dbo.AccountTransactionDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

