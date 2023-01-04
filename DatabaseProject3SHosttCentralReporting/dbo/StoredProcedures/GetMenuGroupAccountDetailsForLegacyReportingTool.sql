
CREATE PROC [dbo].[GetMenuGroupAccountDetailsForLegacyReportingTool]
(	
	@IsExactTime BIT, 	
	@StartDate DATETIME,
	@EndDate DATETIME,
	@Outlets NVARCHAR(max),
	@Departments NVARCHAR(max)	
)
AS
----
IF @Outlets = '0'
BEGIN 
SET @Outlets = (
					SELECT TOP 1 Ids FROM (
					SELECT Ids = STUFF( (SELECT (','+CAST(Id AS VARCHAR(10)))
					                      FROM syncoutlets                       
					                      FOR XML PATH('')
					                     ), 1, 1, ''
					                   ) 
					FROM syncoutlets t
					)Q
				)
END 

IF @Departments = '0'
BEGIN 
SET @Departments = (
					SELECT TOP 1 Ids FROM (
					SELECT Ids = STUFF( (SELECT (','+CAST(Id AS VARCHAR(10)))
					                      FROM Departments                       
					                      FOR XML PATH('')
					                     ), 1, 1, ''
					                   ) 
					FROM Departments t
					)Q
				)
End
--

DECLARE	
@DateRange TABLE([StartDate] DATETIME, [EndDate] DATETIME, FirstWorkPeriodId INT , LastWorkPeriodID INT)
Declare 
@FDate AS DATETIME,
@TDate AS DATETIME
	

IF @IsExactTime = 0
BEGIN
	INSERT INTO @DateRange 
	EXEC GetStartEndDate @StartDate, @EndDate
	SET @FDate = (SELECT StartDate FROM @DateRange )
	SET @TDate = (SELECT EndDate FROM @DateRange )
END;
ELSE
BEGIN
	SET @FDate = @StartDate
	SET @TDate = @EndDate
END

DECLARE 
@FirstWorkPeriodId AS INT,
@LastWorkPeriodId AS INT 

SET @StartDate = (SELECT max(StartDate) FROM @DateRange)
SET @EndDate = (SELECT max(EndDate) FROM @DateRange)
SET @FirstWorkPeriodId = (SELECT max(FirstWorkPeriodId) FROM @DateRange)
SET @LastWorkPeriodId = (SELECT max(LastWorkPeriodId) FROM @DateRange);
	
WITH OrdersUptoTax AS 
(
	SELECT TicketId, sum(UptoAll)UptoAll
	FROM Orders WHERE TicketId
	IN
	(	
		SELECT id FROM Tickets WHERE [Date] BETWEEN @FDate AND @TDate
		AND Tickets.SyncOutletId IN
		(
			SELECT * FROM string_split(@Outlets, ',')
		)
		AND Tickets.departmentId IN
		(
			SELECT * FROM string_split(@Departments, ',')
		)
		AND Tickets.TotalAmount > 0
	)
	AND orders.CalculatePrice = 1
	GROUP BY TicketId
)
------------------------------------OrderVsCalculationRelation---------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
SELECT MenuGroupName GroupCode, 0.00 Gross, 0.00 Quantity, 0.00 Gift, [KEY] SortOrder, Perspective UltimateAccount, Value DistinctAmount , 0.00 TotalCollecton
FROM 
(
	SELECT 
	--TickId, MenuItemName
	'Key' = (SELECT 3 + max(SortOrder) FROM CalculationTypes WHERE Name = CalculationName),
	MenuGroupName, CalculationName Perspective, Sum(CalculationAmount) Value		
	FROM 
	(
		SELECT o.Id AS OrderId, TicketId TickId, MenuItemId, o.MenuGroupName, MenuItemName, o.Total,
		o.Total Price, o.Quantity
		,JSON_VALUE(States.value, '$.CalculationName') CalculationName
		,Convert(DECIMAL(38,20), JSON_VALUE(States.value, '$.CalculationAmount')) CalculationAmount, JSON_VALUE(States.value, '$.CalculationAmount') Ch
		,JSON_VALUE(States.value, '$.CalculationTypeID') CalculationTypeID
		,CASE JSON_VALUE(States.value, '$.IsTax') 
		WHEN 'true' THEN 1
		ELSE 0 end
		IsTax
		,CASE JSON_VALUE(States.value, '$.IsDiscount') 
		WHEN 'true' THEN 1
		ELSE 0 END 
		IsDiscount
		, CASE JSON_VALUE(States.value, '$.IsSD') 
		WHEN 'true' THEN 1
		ELSE 0 END IsSD
		,	CASE JSON_VALUE(States.value, '$.IsSD') 
		WHEN 'true' THEN 'SD'
		ELSE JSON_VALUE(States.value, '$.CalculationName')
		END CalcName
		FROM dbo.orders o
		 CROSS APPLY OPENJSON(convert(nvarchar(max),o.OrderCalculationTypes)) States
		WHERE o.CalculatePrice = 1
		AND TicketId 
		IN
		(	
			SELECT id FROM Tickets 
			WHERE [Date] BETWEEN @FDate AND @TDate
			AND Tickets.SyncOutletId IN
			(
				SELECT * FROM string_split(@Outlets, ',')
			)
			AND Tickets.departmentId IN
			(
				SELECT * FROM string_split(@Departments, ',')
			)
			AND Tickets.TotalAmount > 0
		)
	)
	T
	GROUP BY 
	--TickId, OrderId, MenuItemName
	MenuGroupName, CalculationName
	
	----------------------------------SalesValue--------------------------------------------------------------
	----------------------------------------------------------------------------------------------------------
	UNION ALL
	
	
	SELECT 2 'Key', MenuGroupName, 'Sales' Perspective, Sum(Total) Value 
	FROM Orders WHERE TicketId
	IN
	(	
		SELECT id FROM Tickets WHERE [Date] BETWEEN @FDate AND @TDate
		AND Tickets.SyncOutletId IN
		(
				SELECT * FROM string_split(@Outlets, ',')
		)
		AND Tickets.departmentId IN
		(
			SELECT * FROM string_split(@Departments, ',')
		)
		AND Tickets.TotalAmount > 0
	)
	AND orders.CalculatePrice = 1
	GROUP BY MenuGroupName
	
	----------------------------------SalesQuantity------------------------------------------------------------
	-----------------------------------------------------------------------------------------------------------
	UNION ALL
	
	
	SELECT 1 'Key', MenuGroupName, 'Quantity' Perspective, Sum(orders.Quantity)Value 
	FROM Orders WHERE TicketId
	IN
	(	
		SELECT id FROM Tickets WHERE [date] BETWEEN @FDate AND @TDate
		AND Tickets.SyncOutletId IN
		(
			SELECT * FROM string_split(@Outlets, ',')
		)
		AND Tickets.departmentId IN
		(
			SELECT * FROM string_split(@Departments, ',')
		)
		AND Tickets.TotalAmount > 0
	)
	AND orders.DecreaseInventory = 1
	GROUP BY MenuGroupName
	
	---------------------------------OrderPaymentRelation-------------------------------------------------------
	------------------------------------------------------------------------------------------------------------
	UNION ALL
	
	SELECT
	[Key]
	, 
	o.MenuGroupName, p.Name 	Perspective, 
	sum((o.UptoAll/t.TotalAmount)*Amount) Amount
	FROM 
	Tickets t 
	LEFT OUTER JOIN Orders o
	ON o.TicketId = t.Id
	LEFT OUTER JOIN OrdersUptoTax ot ON
	t.Id= ot.ticketid 
	LEFT OUTER JOIN Payments p	
	on t.Id = p.TicketId
	LEFT OUTER JOIN 
	(
		SELECT 
		paymenttypes.SortOrder + (SELECT max(sortorder) FROM CalculationTypes)+ 4  [Key], * FROM 
		paymenttypes 
	)pt
	ON p.PaymentTypeId = pt.Id
	WHERE o.CalculatePrice = 1
	AND t.[Date] BETWEEN @FDate AND @TDate
	AND t.SyncOutletId IN
	(
		SELECT * FROM string_split(@Outlets, ',')
	)
	AND t.departmentId IN
	(
		SELECT * FROM string_split(@Departments, ',')
	)
	AND t.TotalAmount > 0
	GROUP BY o.MenuGroupName, p.Name, [Key]	
	
	--------------------------------TotalPaymentRelation---------------------------------------------------------
	UNION ALL
	SELECT
	[Key] = (SELECT 1 + max(paymenttypes.SortOrder) + (SELECT max(sortorder) FROM CalculationTypes)+ 4  [Key] FROM paymenttypes), 
	o.MenuGroupName, 'Total' Perspective, 
	sum((o.UptoAll/t.TotalAmount)*Amount) Amount
	FROM 
	Tickets t 
	LEFT OUTER JOIN Orders o
	ON o.TicketId = t.Id
	LEFT OUTER JOIN OrdersUptoTax ot ON
	t.Id= ot.ticketid 
	LEFT OUTER JOIN Payments p	
	on t.Id = p.TicketId	
	WHERE o.CalculatePrice = 1
	AND t.[Date] BETWEEN @FDate AND @TDate
	AND t.SyncOutletId IN
	(
		SELECT * FROM string_split(@Outlets, ',')
	)
	AND t.departmentId IN
	(
		SELECT * FROM string_split(@Departments, ',')
	)
	AND t.TotalAmount > 0
	GROUP BY o.MenuGroupName
	--------------------------------TotalPaymentRelation----------------------------------------------------------
	----------------------------------Void Value------------------------------------------------------------------	
	UNION ALL
	
	
	SELECT [Key] = (SELECT 2 + max(paymenttypes.SortOrder) + (SELECT max(sortorder) FROM CalculationTypes)+ 4  [Key] FROM paymenttypes), 
	MenuGroupName, 'Void' Perspective, Sum(PlainTotal) Value 
	FROM Orders WHERE TicketId
	IN
	(	
		SELECT id FROM Tickets WHERE [Date] BETWEEN @FDate AND @TDate
		AND Tickets.SyncOutletId IN
		(
			SELECT * FROM string_split(@Outlets, ',')
		)
		AND Tickets.departmentId IN
		(
			SELECT * FROM string_split(@Departments, ',')
		)
		--AND Tickets.TotalAmount > 0
	)
	AND orders.CalculatePrice = 0 AND orders.DecreaseInventory = 0 
	--AND 	OrderStates LIKE '%Void%'
	GROUP BY MenuGroupName
	----------------------------------Void Value------------------------------------------------------------------	
	----------------------------------Cancel Value----------------------------------------------------------------
	UNION ALL
	
	
	SELECT [Key] = (SELECT 2 + max(paymenttypes.SortOrder) + (SELECT max(sortorder) FROM CalculationTypes)+ 4  [Key] FROM paymenttypes), 
	MenuGroupName, 'Void' Perspective, Sum(PlainTotal) Value 
	FROM Orders WHERE TicketId
	IN
	(	
		SELECT id FROM Tickets WHERE [Date] BETWEEN @FDate AND @TDate
		AND Tickets.SyncOutletId IN
		(
				SELECT * FROM string_split(@Outlets, ',')
		)
		AND Tickets.departmentId IN
		(
			SELECT * FROM string_split(@Departments, ',')
		)
		AND Tickets.TotalAmount > 0
	)
	AND orders.CalculatePrice = 0 AND orders.DecreaseInventory = 0 AND OrderStates LIKE '%Cancel%'
	GROUP BY MenuGroupName
	----------------------------------Cancel Value-----------------------------------------------------------------
	
)Q
ORDER BY [Key]

GO

