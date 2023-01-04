CREATE PROCEDURE [dbo].[HOSTTReport_DepartmentOrdersInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN	
    select D.Name Header1
    , Sum(o.Total) Value1 
    from tickets t
    , orders o
    , Departments d
    where t.Id = o.TicketId 
    and t.DepartmentId = d.Id
    and t.Date BETWEEN @StartDate AND @EndDate 
    and t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
group by o.DepartmentId, d.Name
End

GO

