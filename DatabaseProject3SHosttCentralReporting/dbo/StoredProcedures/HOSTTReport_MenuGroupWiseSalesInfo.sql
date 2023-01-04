CREATE PROCEDURE [dbo].[HOSTTReport_MenuGroupWiseSalesInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 
    select m.GroupCode Header1 --GroupName
    ,Sum(o.Quantity) Value1 -- Quantity
    ,Sum(o.Total) Value2 -- Amount
    ,SUM(o.Quantity*Price) Value3 -- GrossAmount
    ,SUM(o.UptoTAX) Value4 --UptoTAX
    from tickets t
    , Orders o
    , MenuItems m 
    where t.id = o.TicketId 
    and o.MenuItemId = m.id 
    and o.DecreaseInventory =1 
    and o.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    and t.LastPaymentDate between @StartDate and @EndDate
    group by m.groupCode
Order By Value2 desc
End

GO

