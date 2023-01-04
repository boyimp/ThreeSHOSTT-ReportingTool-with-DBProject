CREATE PROCEDURE [dbo].[HOSTTReport_IncomeInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 	
    select pt.Name Header1
    ,CAST(COUNT(AMOUNT) As decimal(8, 2)) Value1--PaymentCount
    ,SUM(Amount) Value2--Amount
    from Tickets t
    , Payments p
    , PaymentTypes pt 
    where p.TicketId = t.Id 
    AND pt.id =p.PaymentTypeId
    AND Amount<>0
    AND t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    and LastPaymentDate between @StartDate and @EndDate
    group by pt.id
    ,pt.Name
order by Value2 desc
End

GO

