
CREATE PROCEDURE [dbo].[BatchInventoryPotential]
(
	@StartDateParam DATETIME,
	@EndDateParam DATETIME,
	@firstworkperiodid INT,
	@lastworkperiodid INT,
	@warehouseidparam INT,
	@sqlClause VARCHAR(1000)
)

--EXEC BatchInventoryPotential '10/11/2018 11:41:27 AM', '11/14/2018 2:44:06 PM', 624, 1655, 25, ''
AS
BEGIN

DECLARE 
@StartDate  VARCHAR(100),
@EndDate  VARCHAR(100),
@count AS INT,
@SKUID AS INT,
@TranSKUID AS INT,
@Quantity AS MONEY, 
@TempStock AS MONEY,
@TempBatch AS INT,
@TempPrice AS MONEY,
@CustomerOrderID AS INT, 
@OrderDate AS DATETIME, 
@WarehouseID AS INT, 
@OrderedQty AS MONEY,  
@Price AS MONEY,
@TransferAmount AS MONEY, 
@CustomerOrderType AS INT,
@TempBatchQuantity AS MONEY 

SET @StartDate = (SELECT [dbo].[ufsFormat] (@StartDateParam, 'mmm dd yyyy hh:mm:ss:ms AM/PM'))
SET @EndDate = (SELECT [dbo].[ufsFormat] (@EndDateParam, 'mmm dd yyyy hh:mm:ss:ms AM/PM'))

--************************************************************************************************************************************
create table #ReturnTable
(
	  Id INT
	 ,IsIntermediate int 
	, StartDate VARCHAR (100)
	, EndDate VARCHAR (100)
	, GroupCode NVARCHAR (4000)
	, Inventory NVARCHAR (4000)
	, WarehouseID INT
	, Warehouse NVARCHAR (4000)
	, InventoryID INT 
	, Unit NVARCHAR (4000)
	, Last_Unit_Price NUMERIC (16, 2)
	, Opening_Stock NUMERIC (16, 3)
	, Opening_StockValue NUMERIC (33, 5)
	, PurchaseORTransfer NUMERIC (38, 3)
	, PurchaseORTransferValue NUMERIC (38, 5)
	, StockOut NUMERIC (38, 3)
	, StockOutValue NUMERIC (38, 5)
	, Consumption NUMERIC (38, 3)
	, ConsumptionValue NUMERIC (38, 5)
	, Wastage NUMERIC (38, 3)
	, WastageValue NUMERIC (38, 5)
	, ReturnAmount NUMERIC (38, 3)
	, ReturnValue NUMERIC (38, 5)
	, ExpectedQty NUMERIC (38, 3)
	, ExpectedValue NUMERIC (38, 5)
	, PhysicalInventory NUMERIC (16, 3)
	, PhysicalInventoryValue NUMERIC (33, 5)
	, ClosingStock NUMERIC (20, 3)
	, ClosingStockValue NUMERIC (37, 5)
	, ActualConsumption NUMERIC (38, 3)
	, ActualConsumptionValue NUMERIC (38, 5)
	, CountVarianceQty NUMERIC (38, 3)
	, CountVarianceValue NUMERIC (38, 5)
	
)

create table #TranTemp
(
    Rank int, 
    SKUID int, 
    Price Money, 
    Quantity Money,     
    StockTranItemID int   
)

create table #SKUBatch
(
    SKUID int, 
    Price Money, 
    BatchNo INT,
    Quantity Money,
    TransferQty Money 
)

INSERT  INTO #TranTemp
SELECT 0, 0, 0, 0, 0
union


SELECT rank() OVER (ORDER BY InventoryItem_Id, L.Id) AS 'Rank', InventoryItem_Id, Price, 
Purchase, Id
from 
(
	select i.Id InventoryItem_Id, isnull(q.Price, 0) Price,  isnull(Purchase, 0) Purchase, isnull(q.Id, 0) Id,
	StockIn =isnull((
							SELECT max(instock)
							FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
							WHERE 
							pcs.Id = wcs.PeriodicConsumptionId
							AND wcs.Id = ps.PeriodicConsumptionId 
							AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @firstworkperiodid)
							AND ps.InventoryItemId = i.Id
							AND wcs.warehouseid = @warehouseidparam
					), 0)
	from 
	inventoryItems i left outer join
	(
		select
		t.InventoryItem_Id, t.Price,  Quantity Purchase, t.Id
		FROM InventoryTransactionDocuments d, InventoryTransactions t
		WHERE d.Id = t.InventoryTransactionDocumentId
		AND t.TargetWarehouseId = @warehouseidparam
		AND d.[Date] < @StartDate
	) Q on i.id = InventoryItem_Id
) L

INSERT INTO #SKUBatch
SELECT  InventoryItems.Id SKUID, isnull(P.Price, 0) Price, isnull(BatchNo, 1), sum(isnull(Quantity, 0))Quantity, 0  TransferQty 
FROM InventoryItems Left outer join 
(
	SELECT SKUID1 SKUID, Price1 Price,
	sum(Batch) over (partition by SKUID1 order by StockTranItemId) BatchNo, Quantity
	FROM
	(
		
		select t1.StockTranItemId, t1.Rank Rank1,t2.Rank Rank2,t1.SKUID SKUID1, t2.SKUID SKUID2, t1.Price Price1, t2.Price Price2, t1.Quantity, 
		CASE WHEN t1.SKUID = t2.SKUID  AND t1.Price = t2.Price THEN 0
		ELSE 1
		END  Batch
		from #TranTemp t1, #TranTemp t2 
		where t1.Rank = t2.Rank + 1 				
	)
	Q
) P on InventoryItems.Id = P.SKUID
GROUP BY InventoryItems.Id, Price, BatchNo
ORDER BY InventoryItems.Id, BatchNo


SET @TempBatchQuantity = 0


DECLARE OutTransactions CURSOR LOCAL FOR
SELECT q.Id , 
Purchase - StockIn
Consumptions
FROM 
(
	SELECT I.*, isnull(Purchase, 0)Purchase,
	StockIn =isnull((
						SELECT max(instock)
		                FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
		                WHERE 
		                pcs.Id = wcs.PeriodicConsumptionId
		                AND wcs.Id = ps.PeriodicConsumptionId 
		                AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @firstworkperiodid)
		                AND ps.InventoryItemId = i.Id
		                AND wcs.warehouseid = @warehouseidparam
	            ), 0)
	FROM InventoryItems i LEFT OUTER JOIN 
	(
		SELECT t.InventoryItem_Id, sum(Quantity) Purchase FROM InventoryTransactionDocuments d, InventoryTransactions t
		WHERE d.Id = t.InventoryTransactionDocumentId
		AND t.TargetWarehouseId = 1
		AND d.[Date] < @StartDate
		GROUP BY t.InventoryItem_Id
	) Purchases
	ON i.Id = Purchases.InventoryItem_Id
)Q 


OPEN OutTransactions
FETCH NEXT FROM OutTransactions INTO 
   @SKUID, 
   @Quantity
WHILE @@FETCH_STATUS = 0   
BEGIN
   if  @Quantity < 0
   begin 
		SET @TempBatch = (SELECT TOP 1 BatchNo FROM #skubatch WHERE SKUID = @SKUID ORDER BY BatchNo desc)
		SET @TempStock = (SELECT TOP 1 Quantity FROM #skubatch WHERE SKUID = @SKUID ORDER BY BatchNo desc)
		UPDATE #skubatch SET Quantity = Quantity - @Quantity WHERE SKUID = @SKUID AND BatchNo = @TempBatch
   end      
   WHILE  @Quantity > 0
   BEGIN
   		SET @TempBatchQuantity = (SELECT sum(Quantity) FROM #skubatch WHERE SKUID = @SKUID)   		
   		IF @TempBatchQuantity = 0
   		BEGIN
   			SET @TempBatch = (SELECT TOP 1 BatchNo FROM #skubatch WHERE SKUID = @SKUID ORDER BY BatchNo desc)
	   		SET @TempStock = (SELECT TOP 1 Quantity FROM #skubatch WHERE SKUID = @SKUID ORDER BY BatchNo desc)
   		END 
   		ELSE
   		BEGIN
   			SET @TempBatch = (SELECT TOP 1 BatchNo FROM #skubatch WHERE SKUID = @SKUID AND Quantity <> 0 ORDER BY BatchNo)
	   		SET @TempStock = (SELECT TOP 1 Quantity FROM #skubatch WHERE SKUID = @SKUID AND Quantity <> 0 ORDER BY BatchNo)
   		END   		
		
   		IF @Quantity >= @TempStock and  @TempStock <> 0
   		BEGIN
	   		SET @Quantity = @Quantity - @TempStock
	   		UPDATE #skubatch SET Quantity = 0 WHERE SKUID = @SKUID AND BatchNo = @TempBatch
	   	END
	   	ELSE 
	   	BEGIN	   		
	   		UPDATE #skubatch SET Quantity = @TempStock - @Quantity WHERE SKUID = @SKUID AND BatchNo = @TempBatch
	   		SET @Quantity = 0
	   	END 
   END
FETCH NEXT FROM OutTransactions INTO 
   @SKUID,
   @Quantity
END
CLOSE OutTransactions
DEALLOCATE OutTransactions



	INSERT INTO #ReturnTable
	select *, (Opening_Stock+PurchaseORTransfer-PhysicalInventory) ActualConsumption,(Opening_Stock+PurchaseORTransfer-PhysicalInventory)*Last_Unit_Price ActualConsumptionValue,
	PhysicalInventory-ExpectedQty CountVarianceQty ,(PhysicalInventoryValue-ExpectedValue) CountVarianceValue  from(
		Select
		Id,IsIntermediate,StartDate,	EndDate, 	GroupCode,	Inventory, WarehouseID,	
		Warehouse,	InventoryID, Unit,
		UnitPrice Last_Unit_Price, InStock Opening_Stock,InStock*Unitprice Opening_StockValue, Purchase PurchaseORTransfer, 
		Purchase*UnitPrice PurchaseORTransferValue ,
		 isnull(StockOut,0) StockOut, isnull(StockOut,0)*UnitPrice StockOutValue,Consumption ,Consumption*UnitPrice ConsumptionValue , Wastage , Wastage*UnitPrice WastageValue , 
		ReturnAmount ,ReturnAmount*UnitPrice ReturnValue ,(InStock+Purchase-StockOut-Consumption-Wastage-ReturnAmount) ExpectedQty ,
		(InStock+Purchase-Consumption-StockOut-Wastage-ReturnAmount)*UnitPrice ExpectedValue , isnull(PhysicalInventory, ClosingStock) PhysicalInventory , (UnitPrice*(isnull(PhysicalInventory, ClosingStock))) PhysicalInventoryValue ,
		isnull(ClosingStock ,InStock + Purchase-StockOut -Wastage - ReturnAmount- Consumption) ClosingStock ,isnull(ClosingStock ,InStock + Purchase-StockOut -Wastage - ReturnAmount- Consumption)*UnitPrice ClosingStockValue  
		FROM
		  (
	    SELECT @StartDate StartDate,
	    @EndDate EndDate,
	    i.Id,i.IsIntermediate, i.GroupCode, i.Name Inventory, w.Id WarehouseID, w.Name Warehouse, 
	    i.Id InventoryID, i.Name, i.transactionunit Unit,
	    StockIn =(
	                SELECT max(StockIn)
	                FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
	                WHERE 
	                pcs.Id = wcs.PeriodicConsumptionId
	                AND wcs.Id = ps.PeriodicConsumptionId 
	                AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @firstworkperiodid)
	                AND ps.InventoryItemId = i.Id
	                AND wcs.warehouseid = w.Id
	            ),
	    StockOut =(
	                SELECT max(StockOut)
	                FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
	                WHERE 
	                pcs.Id = wcs.PeriodicConsumptionId
	                AND wcs.Id = ps.PeriodicConsumptionId 
	                AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @firstworkperiodid)
	                AND ps.InventoryItemId = i.Id
	                AND wcs.warehouseid = w.Id
	            ),
	    InStock =(
	                SELECT max(instock)
	                FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
	                WHERE 
	                pcs.Id = wcs.PeriodicConsumptionId
	                AND wcs.Id = ps.PeriodicConsumptionId 
	                AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @firstworkperiodid)
	                AND ps.InventoryItemId = i.Id
	                AND wcs.warehouseid = w.Id
	            ),
	    sum(isnull(p.Wastage, 0)) Wastage ,
	    sum(isnull(p.ReturnAmount,0)) ReturnAmount,
	    PhysicalInventory  =(
	                SELECT max(PhysicalInventory)
	                FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
	                WHERE 
	                pcs.Id = wcs.PeriodicConsumptionId
	                AND wcs.Id = ps.PeriodicConsumptionId 
	                AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @lastworkperiodid )
	                AND ps.InventoryItemId = i.Id
	                AND wcs.warehouseid = w.Id
	            ),
	
	    sum(p.Purchase) 'Purchase', sum(p.Consumption) Consumption, 
	    ClosingStock =(
	                    SELECT (instock + purchase -isnull(StockOut,0)- isnull(Wastage, 0) - isnull(ReturnAmount, 0) - consumption)
	                    FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
	                    WHERE 
	                    pcs.Id = wcs.PeriodicConsumptionId
	                    AND wcs.Id = ps.PeriodicConsumptionId 
	                    AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @lastworkperiodid )
	                    AND ps.InventoryItemId = i.Id
	                    AND wcs.warehouseid = w.Id
	                ),
	    UnitPrice = isnull((
	                 SELECT Price 
	                 FROM InventoryTransactions 
	                 WHERE 
	                 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
	                 AND  id = (
	                             SELECT max(id) FROM InventoryTransactions 
	                             WHERE InventoryItem_Id =  i.id
	                             AND  InventoryTransactions.Unit = i.TransactionUnit
	                           )				                                             
	             ),0)	
	    FROM 
	    PeriodicConsumptions pc,WarehouseConsumptions wc,
	    PeriodicConsumptionitems p, inventoryitems i, warehouses w
	    WHERE  pc.Id = wc.PeriodicConsumptionId
	    AND wc.Id = p.PeriodicConsumptionId
	    AND p.InventoryItemId = i.Id
	    AND wc.WarehouseId = w.Id                                
	    AND pc.Id >= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @firstworkperiodid )
	    AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @lastworkperiodid ) 
	    AND w.Id =  @warehouseidparam
	    GROUP BY i.GroupCode, i.Name, w.Id, w.Name, 
		i.Id,i.IsIntermediate, i.Name, i.transactionunit 
	
	 )Q
	)TQ
	ORDER BY  GroupCode, Inventory;
    
                                          
	SELECT  s.SKUID Id, IsIntermediate, StartDate, EndDate, GroupCode, Inventory, WarehouseID, InventoryID, Unit, 
	S.LastUnitPrice Last_Unit_Price, Opening_Stock, Opening_Stock* s.OpeningAvgPrice Opening_StockValue,
	isnull(PurchaseORTransfer, 0)PurchaseORTransfer, isnull(Purchase, 0)*isnull(S.PurchasePrice, 0) PurchaseORTransferValue, StockOut,  StockOutValue,
	Consumption, Consumption*LastUnitPrice ConsumptionValue, Wastage, Wastage*LastUnitPrice WastageValue,
	ReturnAmount, ReturnAmount*LastUnitPrice ReturnValue, ExpectedQty, ExpectedQty*LastUnitPrice ExpectedValue,
	PhysicalInventory, PhysicalInventory*LastUnitPrice PhysicalInventoryValue, ClosingStock, ClosingStock*LastUnitPrice ClosingStockValue,
	ActualConsumption, ActualConsumption*LastUnitPrice ActualConsumptionValue, CountVarianceQty, CountVarianceQty*LastUnitPrice CountVarianceValue 
	FROM
	( 
		SELECT skuid, Quantity, InventoryItem_Id, PurchasePrice, Purchase, A.OpeningAvgPrice,
		CASE 
		WHEN (A.Quantity+isnull(B.Purchase, 0)) > 0 THEN 
		((A.OpeningAvgPrice * A.Quantity) + (isnull(B.Purchase, 0)*isnull(B.PurchasePrice, 0)))/ (A.Quantity+isnull(B.Purchase, 0)) 
		ELSE 0 
		END LastUnitPrice
		FROM 
		(
			SELECT skuid, sum(Quantity)Quantity,
			case when sum(Quantity) > 0 then
			sum(Quantity*Price)/sum(Quantity)
			else 
			0
			end OpeningAvgPrice
			FROM 
			#skubatch
			GROUP BY skuid
		)A LEFT OUTER JOIN 
		(
			SELECT
			t.InventoryItem_Id, sum(t.Price*Quantity)/sum(Quantity) PurchasePrice,  sum(Quantity) Purchase
			FROM InventoryTransactionDocuments d, InventoryTransactions t
			WHERE d.Id = t.InventoryTransactionDocumentId
			AND t.TargetWarehouseId = @warehouseidparam
			AND d.[Date] BETWEEN @StartDate AND @EndDate
			GROUP BY InventoryItem_Id
		)B
		ON A.SKUID = B.InventoryItem_Id
	)
	S LEFT OUTER join 
	#ReturnTable R
	ON S.SKUID = r.InventoryID
	
	If(OBJECT_ID('tempdb..#TranTemp') Is Not Null)
	Begin
	    Drop Table #TranTemp
	END
	If(OBJECT_ID('tempdb..#skubatch') Is Not Null)
	Begin
	    Drop Table #skubatch
	END
	
	If(OBJECT_ID('tempdb..#ReturnTable') Is Not Null)
	Begin
	    Drop Table #ReturnTable
	END
END

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[BatchInventoryPotential] TO PUBLIC
    AS [dbo];


GO

