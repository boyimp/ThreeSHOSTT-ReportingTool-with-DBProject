CREATE PROCEDURE [dbo].[HOSTTReport_UserTotalCollectionInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN	
    select [Name] header1
    , SUM(TotalAmount) Value1
    from Tickets t 
    where t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    and LastPaymentDate between @StartDate and @EndDate
    group by [Name]
    order by Value1 desc
End

GO

