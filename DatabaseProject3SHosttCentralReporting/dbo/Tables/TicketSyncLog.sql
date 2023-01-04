CREATE TABLE [dbo].[TicketSyncLog] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [TicketID]     INT            NOT NULL,
    [TicketNumber] NVARCHAR (100) NOT NULL,
    [Operation]    CHAR (1)       NULL,
    [IsClosed]     SMALLINT       NOT NULL,
    [UpdateDate]   DATETIME       NOT NULL,
    [Sync]         SMALLINT       NOT NULL
);


GO

