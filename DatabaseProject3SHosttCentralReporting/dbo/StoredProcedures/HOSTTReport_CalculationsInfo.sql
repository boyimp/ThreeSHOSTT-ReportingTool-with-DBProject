CREATE PROCEDURE [dbo].[HOSTTReport_CalculationsInfo]
	(
		@StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
	)
	AS 
BEGIN 
	SELECT c.Name Header1 
	, Cast(c.IsDiscount as decimal) Value1 
	, Cast(c.IsSD as decimal) Value2 
	, Cast(c.IsTax as decimal) Value3
	, cast(c.IsAuto as decimal) Value3
	, Cast(count(*)as decimal) Value5
	, sum(CalculationAmount) Value6 
    FROM Calculations c, 
    (   SELECT * FROM Tickets t
	       WHERE LastPaymentDate BETWEEN @StartDate AND @EndDate
	       and t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    )t
    WHERE c.TicketId = t.Id
    GROUP BY c.Name, c.IsDiscount, c.IsSD, c.IsTax, c.IsAuto
    order by c.IsDiscount,c.IsSD,c.IsTax desc	
End

GO

