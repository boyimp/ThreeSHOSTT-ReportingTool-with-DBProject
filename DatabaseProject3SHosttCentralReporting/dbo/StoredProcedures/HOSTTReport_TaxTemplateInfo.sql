CREATE PROCEDURE [dbo].[HOSTTReport_TaxTemplateInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 
SELECT TaxName Header1
, sum(TaxAmount) Value1
FROM 
(
    SELECT *, 
    CASE 
    WHEN PlainSum > 0 THEN (Price + Price*PreTaxServicesTotal/PlainSum)*TransactionRate /100*Quantity
    ELSE 0 END TaxAmount
    FROM 
    (
	   SELECT o.Id AS OrderId, TicketId TickId, MenuItemId, MenuItemName, o.GetPriceValue Price, o.Quantity
	   ,JSON_VALUE(States.value, '$.TN') TaxName
	   ,JSON_VALUE(States.value, '$.TR') TransactionRate
	   ,JSON_VALUE(States.value, '$.AT') AccountTransactionTypeId
	   FROM dbo.orders o
	   CROSS APPLY OPENJSON(convert(nvarchar(max),o.Taxes)) States
	   WHERE o.CalculatePrice = 1
    ) o,
    (
	   SELECT Id, PreTaxServicesTotal, PlainSum, Tickets.TaxIncluded, Tickets.TicketNumber
	   FROM Tickets
	   WHERE LastPaymentDate BETWEEN @StartDate AND @EndDate
	   AND DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    ) Tick 
    WHERE Tick.Id = o.TickId
)Q
GROUP BY  TaxName	
End

GO

