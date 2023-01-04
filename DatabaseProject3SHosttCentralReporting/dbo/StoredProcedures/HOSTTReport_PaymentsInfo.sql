CREATE PROCEDURE [dbo].[HOSTTReport_PaymentsInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 	
	select PaymentName Header1
	, PaymentTypeId Value1
	,(Amount/Total)*100 Value2
	, Amount Value3  
	from (
		SELECT pt.Name PaymentName
		, Cast(p.PaymentTypeId as decimal) PaymentTypeId
		, sum(p.Amount) Amount
		, 'Total' = (	select sum(p.Amount) 
					from tickets t, Payments p
					where t.Id = p.TicketId 
					And t.Date BETWEEN @StartDate AND @EndDate 
					And t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
				)
		from tickets t
		, Payments p
		, PaymentTypes pt
		where t.Id = p.TicketId 
		And p.PaymentTypeId = pt.Id
		And t.Date BETWEEN @StartDate AND @EndDate 
		And t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
		group by pt.Name,p.PaymentTypeId
	)T
	Order by Value3 desc
End

GO

