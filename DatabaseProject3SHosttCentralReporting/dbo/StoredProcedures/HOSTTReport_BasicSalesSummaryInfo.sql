CREATE PROCEDURE [dbo].[HOSTTReport_BasicSalesSummaryInfo]
	(
		@StartDate NVARCHAR(MAX)
		, @EndDate NVARCHAR(MAX)
		, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
	)
	AS 
BEGIN 
    BEGIN TRANSACTION
    CREATE TABLE #T(
	    OrderId INT, 
	    TickId INT, 
	    MenuItemId INT, 
	    MenuItemName NVARCHAR(max), 
	    CalculationName NVARCHAR(max),
	    IsTax smallint, 
	    IsDiscount SMALLINT, 
	    IsSD SMALLINT, 
	    CalculationAmount DECIMAL(18,4), 
	    Price DECIMAL(18,4), 
	    UptoDiscount DECIMAL(18,4), 
	    uptoTAX DECIMAL(18,4)
    )
    BEGIN
	   INSERT INTO #T
	   SELECT OrderId, TickId, MenuItemId, MenuItemName, CalcName CalculationName, IsTax, IsDiscount, IsSD,
	   sum(CAST(CalculationAmount AS DECIMAL(18,4))) CalculationAmount, max(Price)Price, max(UptoDiscount)UptoDiscount, max(uptoTAX) uptoTAX	 	 
	   FROM 
	   (
		   SELECT o.Id AS OrderId, TicketId TickId, MenuItemId, MenuItemName, 
		   o.GetPriceValue Price, o.Quantity, UptoDiscount, uptoTAX
		   ,JSON_VALUE(States.value, '$.CalculationName') CalculationName
		   ,JSON_VALUE(States.value, '$.CalculationAmount') CalculationAmount
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
	   ) o,
	   (
		   SELECT Id
		   FROM Tickets
		   WHERE LastPaymentDate BETWEEN @StartDate AND @EndDate
		   and DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
	   ) Tick 
	   WHERE Tick.Id = o.TickId
	   GROUP BY OrderId, TickId, MenuItemId, MenuItemName, CalcName, IsTax, IsDiscount, IsSD
    END 
    BEGIN
	   DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);
	   SET @cols = 
	   STUFF((
		  SELECT DISTINCT
			 ','+QUOTENAME(c.[CalculationName])
		  FROM #T c FOR XML PATH(''), TYPE
	   ).value('.', 'nvarchar(max)'), 1, 1, '');
    END
    SET @query = 
	    'select OrderId, TickId, MenuItemId, MenuGroupName = (SELECT GROUPCODE FROM MENUITEMS where Id=MenuItemId),MenuItemName,Quantity, '+@cols+', UptoDiscount, uptoTAX
	    from 
	    (
		    SELECT 
		    *
		    FROM 
		    (
			    SELECT OrderId, TickId, MenuItemId, MenuItemName,SUM(Quantity) as Quantity, CalcName CalculationName, 
			    sum(CAST(CalculationAmount AS DECIMAL(18,4))) CalculationAmount, max(Price)Price, max(UptoDiscount)UptoDiscount, max(uptoTAX) uptoTAX	 	 
			    FROM 
			    (
				    SELECT o.Id AS OrderId, TicketId TickId, MenuItemId, MenuItemName,
				    o.GetPriceValue Price, o.Quantity, UptoDiscount, uptoTAX, uptoall
				    ,JSON_VALUE(States.value, ''$.CalculationName'') CalculationName
				    ,JSON_VALUE(States.value, ''$.CalculationAmount'') CalculationAmount
				    ,JSON_VALUE(States.value, ''$.CalculationTypeID'') CalculationTypeID
				    ,CASE JSON_VALUE(States.value, ''$.IsTax'') 
					WHEN ''true'' THEN 1
					ELSE 0 end
				    IsTax
				    ,CASE JSON_VALUE(States.value, ''$.IsDiscount'') 
				    WHEN ''true'' THEN 1
				    ELSE 0 END 
				    IsDiscount
				    , CASE JSON_VALUE(States.value, ''$.IsSD'') 
				    WHEN ''true'' THEN 1
				    ELSE 0 END IsSD
				    ,	CASE JSON_VALUE(States.value, ''$.IsSD'') 
				    WHEN ''true'' THEN ''SD''
				    ELSE JSON_VALUE(States.value, ''$.CalculationName'')
				    END CalcName
				    FROM dbo.orders o
				    CROSS APPLY OPENJSON(convert(nvarchar(max),o.OrderCalculationTypes)) States
				    WHERE o.CalculatePrice = 1
			    ) o,
			    (
				    SELECT Id
				    FROM Tickets
				    WHERE LastPaymentDate BETWEEN '''+@StartDate+''' AND '''+@EndDate+'''
				    and DepartmentId in (Select Id from fnSplitter('''+@UserPermittedDepartmentCommaSeparatedIds+'''))
			    ) Tick 
			    WHERE Tick.Id = o.TickId
			    GROUP BY OrderId, TickId, MenuItemId, MenuItemName, CalcName, IsTax, IsDiscount, IsSD
		    )Q
	   )x pivot (max(CalculationAmount) for CalculationName in ('+@cols+')) p
	   ';
    BEGIN
    EXECUTE (@query);
    END
    COMMIT TRANSACTION
End
GRANT EXECUTE ON dbo.[HOSTTReport_BasicSalesSummaryInfo] TO PUBLIC

GO

