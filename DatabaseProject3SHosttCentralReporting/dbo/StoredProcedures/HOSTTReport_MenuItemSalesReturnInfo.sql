CREATE PROCEDURE [dbo].[HOSTTReport_MenuItemSalesReturnInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 
    select CAST(m.Id As decimal(8, 2)) as Value1 --MenuItemID
    ,m.Name MenuItemName
    ,m.GroupCode GroupName
    ,SUM(o.Quantity) Value2 --Quantity
    ,MAX(o.Price) Value3 --Price
    ,SUM(o.Total) Amount 
    from tickets t
    , orders o
    , menuitems m 
    where o.ticketid=t.id 
    and o.MenuItemId = m.Id 
    and o.IncreaseInventory=1 
    and t.LastPaymentDate BETWEEN @StartDate AND @EndDate    
    AND t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    group by m.id
    ,m.Name
    ,m.GroupCode
    order by Value2 desc
End

GO

