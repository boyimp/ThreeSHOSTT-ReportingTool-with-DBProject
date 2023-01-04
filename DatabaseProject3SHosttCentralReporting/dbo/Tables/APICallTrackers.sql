CREATE TABLE [dbo].[APICallTrackers] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (4000) NOT NULL,
    [Response]           NVARCHAR (4000) NOT NULL,
    [ContentType]        NVARCHAR (4000) NOT NULL,
    [POSTMethodType]     NVARCHAR (4000) NOT NULL,
    [Keys]               NVARCHAR (4000) NOT NULL,
    [APIValues]          NVARCHAR (4000) NOT NULL,
    [CreationDateTime]   DATETIME        NOT NULL,
    [API]                NVARCHAR (4000) NOT NULL,
    [LastUpdateTime]     DATETIME        NOT NULL,
    [Status]             BIT             NOT NULL,
    [IsRealTime]         BIT             CONSTRAINT [DF_APICallTrackers_IsRealTime] DEFAULT ((0)) NOT NULL,
    [EntityID]           INT             NULL,
    [Method]             NVARCHAR (400)  DEFAULT ('POST') NOT NULL,
    [APIName]            NVARCHAR (400)  DEFAULT ('Generic') NOT NULL,
    [AuthorizationToken] NVARCHAR (MAX)  NULL
);


GO

GRANT SELECT
    ON OBJECT::[dbo].[APICallTrackers] TO PUBLIC
    AS [dbo];


GO

GRANT REFERENCES
    ON OBJECT::[dbo].[APICallTrackers] TO PUBLIC
    AS [dbo];


GO

GRANT DELETE
    ON OBJECT::[dbo].[APICallTrackers] TO PUBLIC
    AS [dbo];


GO

GRANT UPDATE
    ON OBJECT::[dbo].[APICallTrackers] TO PUBLIC
    AS [dbo];


GO

GRANT INSERT
    ON OBJECT::[dbo].[APICallTrackers] TO PUBLIC
    AS [dbo];


GO

