
CREATE VIEW [dbo].[HHTWorkPeriod] as

select startdate as WorkPeriodStartTime from workperiods where StartDate = enddate

GO

