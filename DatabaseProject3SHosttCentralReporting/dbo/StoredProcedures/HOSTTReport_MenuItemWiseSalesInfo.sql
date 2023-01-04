CREATE PROCEDURE [dbo].[HOSTTReport_MenuItemWiseSalesInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
    BEGIN 
    select CAST(m.ID As decimal(8, 2)) as Value1 -- ID
    , m.Name Header1  -- Name
    , m.GroupCode Header2 --GroupCode
    , SUM(o.Quantity) Value2 -- Quantity
    , o.PortionName Header3--PortionName
    , o.Price Value3-- Price
    , SUM(o.Total) Value4-- Amount
    , SUM(o.UptoTAX) Value5-- UptoTAX
    from tickets t
    , orders o
    , MenuItems m 
    where t.id =o.TicketId 
    and o.DecreaseInventory =1 
    and o.MenuItemId = m.Id 
    and t.LastPaymentDate between @StartDate and @EndDate
    and o.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    group by  m.Id 
    , m.Name
    , m.GroupCode
    , o.PortionName 
    , o.Price
    order by m.GroupCode,Value2
End

GO

