CREATE TABLE [dbo].[ActivityLogs] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (MAX) NULL,
    [DataObject]          NVARCHAR (MAX) NULL,
    [ResponsibleUserName] NVARCHAR (MAX) NULL,
    [TerminalName]        NVARCHAR (MAX) NULL,
    [DepartmentName]      NVARCHAR (MAX) NULL,
    [LastUpdateTime]      DATETIME       NULL,
    CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

