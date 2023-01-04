CREATE TABLE [dbo].[HOSTTEndUserReports] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Code]               NVARCHAR (MAX) NULL,
    [Name]               NVARCHAR (MAX) NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [EndUserReportOrder] INT            NULL,
    [IsUserDefined]      BIT            NULL,
    CONSTRAINT [PK_HOSTTEndUserReport] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

