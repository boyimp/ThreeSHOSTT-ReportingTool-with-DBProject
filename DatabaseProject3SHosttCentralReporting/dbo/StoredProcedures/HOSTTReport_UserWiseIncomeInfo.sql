CREATE PROCEDURE [dbo].[HOSTTReport_UserWiseIncomeInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN	
    select pt.Name Header1				   --PaymentTypeName
    ,SettledBy Header2					   --UserName
    ,CAST(COUNT(AMOUNT) As decimal(8, 2)) Value1--PaymentCount
    ,SUM(Amount) Value2					   -- Amount
    from Tickets t
    , Payments p
    , PaymentTypes pt 
    where p.TicketId = t.Id 
    and pt.id =p.PaymentTypeId
    and Amount<>0
    AND t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    and LastPaymentDate between @StartDate and @EndDate
    group by pt.id
    ,pt.Name
    ,t.SettledBy
    order by Value2 desc
End

GO

