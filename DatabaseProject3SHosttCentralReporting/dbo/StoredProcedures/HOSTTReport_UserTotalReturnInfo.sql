CREATE PROCEDURE [dbo].[HOSTTReport_UserTotalReturnInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 	
    select CreatingUserName Header1 -- UserName
    ,SUM(o.Total) Value1 --Amount
    from tickets t
    , Orders o 
    where o.IncreaseInventory = 1
    and o.TicketId =t.Id 
    AND t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    and LastPaymentDate between @StartDate and @EndDate
    group by o.CreatingUserName
End

GO

