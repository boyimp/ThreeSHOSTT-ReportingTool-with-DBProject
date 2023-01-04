CREATE TABLE [dbo].[OutletWiseTargets] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (4000) NOT NULL,
    [Year]            INT             NOT NULL,
    [Month]           INT             NOT NULL,
    [PerformanceDays] INT             NOT NULL,
    [StartDate]       DATETIME        NOT NULL,
    [EndDate]         DATETIME        NOT NULL,
    CONSTRAINT [PK_OutletWiseTarget] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO

GRANT DELETE
    ON OBJECT::[dbo].[OutletWiseTargets] TO PUBLIC
    AS [dbo];


GO

GRANT SELECT
    ON OBJECT::[dbo].[OutletWiseTargets] TO PUBLIC
    AS [dbo];


GO

GRANT REFERENCES
    ON OBJECT::[dbo].[OutletWiseTargets] TO PUBLIC
    AS [dbo];


GO

GRANT UPDATE
    ON OBJECT::[dbo].[OutletWiseTargets] TO PUBLIC
    AS [dbo];


GO

GRANT INSERT
    ON OBJECT::[dbo].[OutletWiseTargets] TO PUBLIC
    AS [dbo];


GO

