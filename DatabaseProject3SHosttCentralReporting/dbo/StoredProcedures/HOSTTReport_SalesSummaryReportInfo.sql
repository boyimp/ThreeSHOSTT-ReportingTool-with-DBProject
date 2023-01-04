CREATE PROCEDURE [dbo].[HOSTTReport_SalesSummaryReportInfo](
    @StartDate nvarchar(max), @EndDate nvarchar(max), @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 
 BEGIN TRANSACTION
    CREATE TABLE #T  
    (
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
    CREATE TABLE #FinalTable    
    (
	    /*ItemName*/ Header1 NVARCHAR(MAX), 
	    /*Quantity*/ Value1 DECIMAL(18,2), 
	    /*NetSales*/ Value2 DECIMAL(18,2), 
	    /*SD*/ Value3 DECIMAL(18,2), 
	    /*VAT*/ Value4 DECIMAL(18,2),
	    /*SalesTotal*/ Value5 DECIMAL(18,2)
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
	   DECLARE @vatSumString nvarchar(max);
	   SET @vatSumString = 
	   STUFF(
	   (
		  SELECT DISTINCT
			 ',0)+ISNULL('+QUOTENAME(c.[CalculationName])
		  FROM #T c where c.[CalculationName] like '%VAT%' FOR XML PATH(''), TYPE
	   ).value('.', 'nvarchar(max)'), 1, 1, 'ISNULL(');
	   SET @vatSumString =  @vatSumString + ',0)' ;
	   SET @vatSumString =REPLACE(@vatSumString,'ISNULL(0)+','');
	   END
    if (@vatSumString is not null or @vatSumString != '')
	 BEGIN
	   DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX),@TempTableCreateQuery NVARCHAR(MAX);
	   SET @cols = 
	   STUFF((
		  SELECT DISTINCT
			 ','+QUOTENAME(c.[CalculationName])
		  FROM #T c FOR XML PATH(''), TYPE
	   ).value('.', 'nvarchar(max)'), 1, 1, '');
   
	   DECLARE @ColumnsandDatatype NVARCHAR(MAX)
	   SET @ColumnsandDatatype = 
	   STUFF((
		  SELECT DISTINCT
			 ' DECIMAL(18,2),'+QUOTENAME(c.[CalculationName])
		  FROM #T c FOR XML PATH(''), TYPE
	   ).value('.', 'nvarchar(max)'), 1, 1, '');
	   SET @ColumnsandDatatype =@ColumnsandDatatype+' DECIMAL(18,2)';
   
    SET @TempTableCreateQuery = N'CREATE TABLE ##TempTable(OrderId NVARCHAR(MAX), TickId NVARCHAR(MAX), MenuItemId NVARCHAR(MAX), MenuGroupName NVARCHAR(MAX),MenuItemName NVARCHAR(MAX),Quantity '+@ColumnsandDatatype+',UptoDiscount  DECIMAL(18,2), uptoTAX  DECIMAL(18,2))';
    EXECUTE (@TempTableCreateQuery);   
    
    DECLARE @insertIntoTempQuery nvarchar(max);
    SET @insertIntoTempQuery = 'INSERT INTO ##TempTable EXEC [dbo].[HOSTTReport_BasicSalesSummaryInfo] N'''+@StartDate+''',N'''+@EndDate+''', N'''+@UserPermittedDepartmentCommaSeparatedIds+''';'
    EXECUTE (@insertIntoTempQuery)
    

SET NOCOUNT ON;
  DECLARE @id NVARCHAR(MAX), @value INT;    
   DECLARE id_cursor CURSOR FOR 
   SELECT DISTINCT c.[CalculationName] FROM #T c WHERE c.CalculationName LIKE '%VAT%';
  OPEN id_cursor;
  FETCH NEXT FROM id_cursor INTO @id;
  WHILE @@FETCH_STATUS = 0
  BEGIN       
    DECLARE @Tickets DECIMAL(18,2),@NetSales DECIMAL(18,2),@SD DECIMAL(18,2), @VAT DECIMAL(18,2),@SalesTotal DECIMAL(18,2);
    DECLARE @Ticketssql nvarchar(max),@NetSalessql nvarchar(max),@SDsql nvarchar(max), @VATsql nvarchar(max),@SalesTotalsql nvarchar(max); 
    SET @Ticketssql = 'SELECT @Tickets=COUNT(*) from ##TempTable WHERE ['+ @id +'] is not null';
    SET @NetSalessql = 'SELECT @NetSales=SUM(UptoDiscount) from ##TempTable WHERE ['+ @id +'] is not null';
    IF COL_LENGTH('##TempTable', 'SD') IS NOT NULL
    BEGIN
	  SET @SDsql = 'SELECT @SD=SUM(ISNULL(SD,0)) from ##TempTable WHERE ['+ @id +'] is not null';
	  EXECUTE sp_executesql @SDsql, N'@SD nvarchar(max) OUTPUT',@SD = @SD OUTPUT
    END
    ELSE SET @SD = 0;
    SET @VATsql = 'SELECT @VAT=SUM(ISNULL(['+ @id +'],0)) from ##TempTable WHERE ['+ @id +'] is not null';    
    SET @SalesTotalsql = 'SELECT @SalesTotal=SUM(uptoTAX) from ##TempTable WHERE ['+ @id +'] is not null';    
    EXECUTE sp_executesql @Ticketssql, N'@Tickets int OUTPUT',@Tickets = @Tickets OUTPUT
    EXECUTE sp_executesql @NetSalessql, N'@NetSales nvarchar(max) OUTPUT', @NetSales =  @NetSales OUTPUT
    
    EXECUTE sp_executesql @VATsql, N'@VAT nvarchar(max) OUTPUT',@VAT = @VAT OUTPUT
    EXECUTE sp_executesql @SalesTotalsql, N'@SalesTotal nvarchar(max) OUTPUT',@SalesTotal = @SalesTotal OUTPUT
    INSERT INTO #FinalTable VALUES(@id,@Tickets, @NetSales,@SD,@VAT,@SalesTotal);
    FETCH NEXT FROM id_cursor INTO @id;
  END
  CLOSE id_cursor;
  DEALLOCATE id_cursor;
	    SELECT * from #FinalTable;
		DROP TABLE IF EXISTS ##TempTable	   
	   DROP TABLE IF EXISTS #FinalTable
	   DROP TABLE IF EXISTS #T	
	END
  ELSE SELECT * from #FinalTable
COMMIT TRANSACTION   
	End

GO

