CREATE PROCEDURE [dbo].[HOSTTReport_UserWiseOrderCollectionInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 	
    select CreatingUserName Header1 --UserName
    , SUM(o.Total)  Value1		 --Amount
    from tickets t
    , orders o 
    where t.id = o.TicketId 
    and o.IncreaseInventory <> 1
    AND t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    and LastPaymentDate between @StartDate and @EndDate
    group by CreatingUserName
End

GO

