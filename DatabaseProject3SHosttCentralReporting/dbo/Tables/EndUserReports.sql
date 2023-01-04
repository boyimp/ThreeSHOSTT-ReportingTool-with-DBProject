CREATE TABLE [dbo].[EndUserReports] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Code]               NVARCHAR (MAX) NULL,
    [Name]               NVARCHAR (MAX) NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [EndUserReportOrder] INT            NULL,
    CONSTRAINT [PK_EndUserReport] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

