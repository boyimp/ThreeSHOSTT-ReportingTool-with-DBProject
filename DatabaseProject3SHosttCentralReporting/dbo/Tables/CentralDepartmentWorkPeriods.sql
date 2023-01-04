CREATE TABLE [dbo].[CentralDepartmentWorkPeriods] (
    [DepartmentID]     INT             NOT NULL,
    [WorkPeriodId]     INT             NOT NULL,
    [StartDate]        DATETIME        NOT NULL,
    [EndDate]          DATETIME        NOT NULL,
    [StartDescription] NVARCHAR (4000) NULL,
    [EndDescription]   NVARCHAR (4000) NULL,
    [Name]             NVARCHAR (4000) NULL
);


GO

