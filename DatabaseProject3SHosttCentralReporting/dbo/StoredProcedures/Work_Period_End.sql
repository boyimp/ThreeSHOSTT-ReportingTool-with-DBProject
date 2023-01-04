
CREATE PROCEDURE [dbo].[Work_Period_End]
(
	@WorkPeriodEnd DATETIME
)

--EXEC Work_Period_End '2 Feb 2019 4:00:00 AM'


AS BEGIN	
	BEGIN TRAN 
   	DECLARE		
	@CurrentWorkPeriodId INT,
	@CurrentWorkPeriodStartDate DATETIME,
	@CurrentWorkPeriodEndDate DATETIME,
	@CurrentPeriodicConsumptionID BIGINT,
	@CurrentWarehouseConsmptionID BIGINT,
	@SyncOutletId BIGINT,
	@InventoryBatchPrice decimal(18,4),
	@PreviousWorkPeriodEndDate DATETIME,
	@PreviousWorkPeriodID INT,
	@UnitMultiplier MONEY,
	
	@WarehouseID BIGINT, 
	@WarehouseName NVARCHAR(200),
	
	@InventoryItemId BIGINT,
	@InventoryName NVARCHAR(200),	
	@Source MONEY,
	@Target MONEY,
	@PreviousBalanceOfThisInventory MONEY,
	@TransactionUnit NVARCHAR(200),
	@InventoryItemPrice MONEY,
	@TransactionUnitMultiplier MONEY,
	@OutTran MONEY,
	@InTran MONEY,
	@ProducedValue DECIMAL(18,4),
	@Produced DECIMAL(18,4),	
	@TempProductionOut MONEY,
	@OutTranValue MONEY,
	@InTranValue MONEY,
	@TempProductionOutValue MONEY,
	/**/
	@InStock Money, 
	@Purchase Money, 
	@Consumption Money, 
	@PhysicalInventory Money, 
	@StockIn MONEY , 
	@StockOut MONEY, 
	@Wastage MONEY, 
	@ReturnAmount Money, 
	@StockTake Money, 
	@Remarks VARCHAR(200),
	/**/
	@BatchInfo NVARCHAR(max),
	@BatchInfoWhileWorkPeriodEnd NVARCHAR(max),
	@InStockValue DECIMAL (18, 4),
	@PurchaseValue DECIMAL (18, 4),
	@Production DECIMAL (18, 4),
	@ProductionValue DECIMAL (18, 4),
	@ConsumptionValue DECIMAL (18, 4),
	@PhysicalInventoryValue DECIMAL (18, 4),
	@StockInValue DECIMAL (18, 4),
	@StockOutValue DECIMAL (18, 4),
	@WastageValue DECIMAL (18, 4),
	@ReturnAmountValue DECIMAL (18, 4),
	@StockTakeValue DECIMAL (18, 4),
	@BatchCount int,
	@InTranCount int,
	@CostOfPreviousItem DECIMAL (18, 4),
	@ConsumptionCarryForwardPrevItem DECIMAL (18, 4),
	/**/
	@CurrentInStock MONEY,
	@CurrentInStockValue MONEY,
	@CurrentPurchase MONEY,
	@CurrentPurchaseValue MONEY,
	@CurrentConsumption MONEY,
	@CurrentConsumptionValue MONEY,
	@CurrentPhysicalInventory MONEY,
	@CurrentPhysicalInventoryValue MONEY,
	@CurrentStockIn MONEY,
	@CurrentStockOut MONEY,
	@CurrentWastage MONEY,
	@CurrentReturnAmount MONEY,
	@CurrentPhysicalNegativeImpact MONEY,
	------------------------------------
	@CurrentWastageValue MONEY,
	@CurrentReturnAmountValue MONEY,

	@CurrentStockTake MONEY,
	@CheckPhysicalInventory MONEY,
	/**/
	@opening MONEY, 	
	@sales MONEY, 
	@returntovendor MONEY, 
	@returnfromcustomer MONEY, 
	@adjustmentneg MONEY, 
	@adjustmentpos MONEY,
	/**/
	@InDocName as NVARCHAR(400), 
	@InTranDate as datetime, 
	@InItemPrice as decimal(18,4), 
	@InQuantity as decimal(18,4),
	/**/
	@CarryForwardConsumption AS DECIMAL(18,4),
	@CarryForwardConsumptionValue AS DECIMAL(18,4),
	--
	@StockOutCarryForward                     DECIMAL (18, 4),
	@StockOutCarryForwardValue                DECIMAL (18, 4),
	--
	@WastageCarryForward                      DECIMAL (18, 4),
	@WastageCarryForwardValue                 DECIMAL (18, 4),
	@ReturnAmountCarryForward                 DECIMAL (18, 4),
	@ReturnAmountCarryForwardValue            DECIMAL (18, 4),
	@NegativeStockAdjustmentCarryForward      DECIMAL (18, 4),
	@NegativeStockAdjustmentCarryForwardValue DECIMAL (18, 4),
	@ProductionPurposeCarryForward            DECIMAL (18, 4),
	@ProductionPurposeCarryForwardValue       DECIMAL (18, 4),
	@NegativeStockAdjustment                  DECIMAL (18, 4),
	@NegativeStockAdjustmentValue             DECIMAL (18, 4),
	@PositiveStockAdjustment                  DECIMAL (18, 4),
	@PositiveStockAdjustmentValue             DECIMAL (18, 4),
	--
	@PrevStockOutCarryForward                     DECIMAL (18, 4),	
	@PrevWastageCarryForward                      DECIMAL (18, 4),	
	@PrevReturnAmountCarryForward                 DECIMAL (18, 4),	
	@PrevNegativeStockAdjustmentCarryForward      DECIMAL (18, 4),
	@PrevProductionPurposeCarryForward            DECIMAL (18, 4),
	@ClosingStock	DECIMAL(18, 4),
	@ClosingStockValue	DECIMAL(18, 4),
	@PreviousEODClosingStock	DECIMAL(18, 4),
	@PreviousEODClosingStockValue 	DECIMAL(18, 4)
	/**/
	/*SET @Batchdate ='10 jun 2015',
	SET @UserID = 1
	SET @Remarks = 'lamsam'*/		
	SET @PreviousWorkPeriodID = (SELECT workperiodid FROM PeriodicConsumptions WHERE id = (SELECT max(ID) FROM PeriodicConsumptions))
	SET @PreviousWorkPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE id = @PreviousWorkPeriodID)
	SET @CurrentWorkPeriodId = (SELECT ID FROM WorkPeriods WHERE StartDate = enddate AND id = (SELECT max(id) FROM WorkPeriods))
	SET @CurrentWorkPeriodStartDate = (SELECT startdate FROM WorkPeriods WHERE id = @CurrentWorkPeriodId)	
	
	PRINT @PreviousWorkPeriodID
	PRINT @PreviousWorkPeriodEndDate
	PRINT @CurrentWorkPeriodId
	PRINT @CurrentWorkPeriodStartDate
	create table ##tempBatchInfo
	(
	    BP DECIMAL(18, 4),
	    CD DateTime, 
	    QT DECIMAL(18, 4),
	    SR NVARCHAR(max)
	)
		
	create table ##ConsumptionInfo
	(
		CarryForwardQty DECIMAL(18, 4),
		ConsumptionValue DECIMAL(18, 4)
	)
	
	UPDATE WorkPeriods 
	SET EndDate = @WorkPeriodEnd, EndDescription = 'Work Period End Austomated ' + CAST(@WorkPeriodEnd AS VARCHAR(100)) WHERE Id = @CurrentWorkPeriodId
	SET @CurrentWorkPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE id = @CurrentWorkPeriodId)

	
	INSERT INTO dbo.PeriodicConsumptions (WorkPeriodId, StartDate, EndDate, LastUpdateTime, Name, Synced, SyncID, SyncOutletId)
	VALUES (@CurrentWorkPeriodId, @CurrentWorkPeriodStartDate, @CurrentWorkPeriodEndDate, @WorkPeriodEnd, CAST(@CurrentWorkPeriodStartDate AS VARCHAR(100)) + ' - ' + CAST(@CurrentWorkPeriodEndDate AS VARCHAR(100)), 0, 0, 0)
	
	
	
	SET @CurrentPeriodicConsumptionID = SCOPE_IDENTITY();
		
	DECLARE WarehouseCursor CURSOR LOCAL FOR
	SELECT Id WarehouseId, Name WarehouseName, ow.OutletId
	FROM Warehouses w, SyncOutletWarehouses ow
	WHERE w.Id = ow.WarehouseId
	
	OPEN WarehouseCursor	FETCH NEXT FROM WarehouseCursor INTO	
	@WarehouseID, 
	@WarehouseName,
	@SyncOutletId
	--drop table #tempConsumptionTable
	WHILE @@FETCH_STATUS = 0
	BEGIN
		/*Warehouse Consumptions of this warehouse*/
		SELECT WarehouseId, InventoryItemId, InventoryItemName, sum(Impact)Impact 
		INTO #tempConsumptionTable
		FROM 
		(
			SELECT o.WarehouseId, o.MenuItemName, p.Name PortionName, r.Name, o.Quantity OrderQty, i.ID InventoryItemId,
			i.Name InventoryItemName, (o.Quantity*ri.Quantity)/ i.transactionunitmultiplier Impact
			FROM 
			(
				SELECT * 
				FROM Orders  
				WHERE createddatetime BETWEEN isnull(@PreviousWorkPeriodEndDate, @CurrentWorkPeriodStartDate)  AND @CurrentWorkPeriodEndDate
				AND DecreaseInventory  = 1
				AND WarehouseId = @WarehouseId
			)o, MenuItemPortions p, Recipes r, RecipeItems ri, InventoryItems i
			WHERE o.MenuItemId = p.MenuItemId
			AND o.portionname = p.Name
			AND r.Portion_Id = p.Id
			AND r.id = ri.recipeid
			AND ri.inventoryitem_id = i.id
		) T
		GROUP BY WarehouseId, InventoryItemId, InventoryItemName
		/*Warehouse Consumptions of this warehouse*/
		
		/*Wastage & Stock Take of outlets*/		
		SELECT W.InventoryItemId, Wastage, ReturnAmount, StockTake, WastageValue, ReturnAmountValue, StockTakeValue
		INTO #tempOutletWastageStockTake
		FROM 
		(	
			SELECT i.InventoryItemId, sum(isnull(wastage, 0)) wastage, 0 WastageValue, 0 ReturnAmountValue, 0 StockTakeValue,
			--sum(isnull(wastagevalue, 0)) wastagevalue,
			 sum(isnull(returnamount, 0)) returnamount
			 --, sum(isnull(returnamountvalue, 0)) returnamountvalue
			FROM SyncPeriodicConsumptions c , SyncWarehouseConsumptions w, SyncPeriodicConsumptionItems i
			WHERE c.Id = w.PeriodicConsumptionId AND w.Id = i.PeriodicConsumptionId
			AND c.Id = i.WarehouseConsumptionId
			--AND c.StartDate BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
			AND c.EndDate BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
			AND w.WarehouseId = @WarehouseId
			GROUP BY i.InventoryItemId
		)W,
		(
			select 
			* 
			from 
			(
				select
				i.InventoryItemId, i.PhysicalInventory StockTake,--i.PhysicalInventoryValue StockTakeValue,
				row_number() over(partition by i.InventoryItemId order by c.EndDate desc) as rn	
				FROM SyncPeriodicConsumptions c , SyncWarehouseConsumptions w, SyncPeriodicConsumptionItems i
				WHERE c.Id = w.PeriodicConsumptionId AND w.Id = i.PeriodicConsumptionId
				AND c.Id = i.WarehouseConsumptionId
				--AND c.StartDate BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
				AND c.EndDate BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
				AND w.WarehouseId = @WarehouseId AND c.SyncOutletId = @SyncOutletId
			) t
			where t.rn = 1
		)STake
		WHERE w.InventoryItemId = STake.InventoryItemId
		/*Wastage & Stock Take of outlets*/

	   	
	   	INSERT INTO dbo.WarehouseConsumptions (PeriodicConsumptionId, WarehouseId, IsSyncedToSTAR)
		VALUES (@CurrentPeriodicConsumptionID, @WarehouseID, 0)	
		
		
		SET @CurrentWarehouseConsmptionID = SCOPE_IDENTITY();

		DECLARE WarehouseItemCursor CURSOR LOCAL FOR		
		SELECT Inv.*, isnull(InStock, 0) InStock, isnull(Purchase, 0) Purchase, isnull(Consumption, 0)Consumption, 
		PhysicalInventory, StockIn, StockOut, Wastage, ReturnAmount, StockTake, Remarks,
		BatchInfo, BatchInfoWhileWorkPeriodEnd, InStockValue,PurchaseValue,Production,ProductionValue,
		ConsumptionValue,PhysicalInventoryValue,StockInValue,StockOutValue, WastageValue,  ReturnAmountValue,  StockTakeValue, Cost, 
		ConsumptionCarryForward,
		StockOutCarryForward,
		WastageCarryForward,
		ReturnAmountCarryForward,
		NegativeStockAdjustmentCarryForward,
		ProductionPurposeCarryForward,
		ClosingStock,
		ClosingStockValue
		FROM 
		(
			SELECT Id, Name, TransactionUnit, TransactionUnitPrice, i.TransactionUnitMultiplier, isnull(sum(Source)/i.TransactionUnitMultiplier, 0) Source, isnull(sum(Target)/i.TransactionUnitMultiplier, 0) Target FROM 
			InventoryItemView i LEFT OUTER JOIN
			(			
				SELECT t.InventoryItem_Id, isnull(((t.Quantity*t.Multiplier)), 0) Source, 0 Target   
				FROM InventoryTransactions t, InventoryTransactionDocuments d
				WHERE d.Id = t.InventoryTransactionDocumentId and d.SyncOutletId = @SyncOutletId and
				t.[Date] BETWEEN isnull(@PreviousWorkPeriodEndDate,@CurrentWorkPeriodStartDate) AND @CurrentWorkPeriodEndDate
				AND SourceWarehouseId = @WarehouseID
						
				UNION ALL 
				
				SELECT t.InventoryItem_Id, 0 Source, isnull(((t.Quantity*t.Multiplier)), 0) Target
				FROM InventoryTransactions t
				WHERE 
				t.[Date] BETWEEN isnull(@PreviousWorkPeriodEndDate,@CurrentWorkPeriodStartDate) AND @CurrentWorkPeriodEndDate
				AND TargetWarehouseId = @WarehouseID			
			)
			T
			ON i.Id = t.InventoryItem_Id
			GROUP BY i.Id, i.Name, i.TransactionUnitMultiplier, TransactionUnitPrice, i.TransactionUnit
		) Inv LEFT OUTER JOIN
		(
			SELECT * 
			FROM PeriodicConsumptionItems WHERE WarehouseConsumptionId = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = @PreviousWorkPeriodID)
			AND PeriodicConsumptionId = (
											SELECT max(Id) FROM WarehouseConsumptions 
											WHERE PeriodicConsumptionId = (SELECT max(id) 
											FROM PeriodicConsumptions WHERE WorkPeriodId = @PreviousWorkPeriodID AND Warehouseid = @WarehouseID)
										)
		)PrevConsumptions
		ON PrevConsumptions.InventoryItemId = Inv.Id
		--WHERE inv.id = 465		
		--WHERE inv.name = 'BEEF PATTIES 2 OZ' 
			OPEN WarehouseItemCursor	FETCH NEXT FROM WarehouseItemCursor INTO
			@InventoryItemId ,
			@InventoryName ,
			@TransactionUnit,
			@InventoryItemPrice,
			@TransactionUnitMultiplier,
			@Source,
			@Target,			
			@InStock , 
			@Purchase , 
			@Consumption , 
			@PhysicalInventory , 
			@StockIn  , 
			@StockOut , 
			@Wastage , 
			@ReturnAmount , 
			@StockTake , 
			@Remarks,			
			@BatchInfo,
			@BatchInfoWhileWorkPeriodEnd,
			@InStockValue,
			@PurchaseValue,
			@Production,
			@ProductionValue,
			@ConsumptionValue,
			@PhysicalInventoryValue,
			@StockInValue,
			@StockOutValue,
			@WastageValue,
			@ReturnAmountValue,
			@StockTakeValue,
			@CostOfPreviousItem,
			@ConsumptionCarryForwardPrevItem,
			@PrevStockOutCarryForward,
			@PrevWastageCarryForward,
			@PrevReturnAmountCarryForward,
			@PrevNegativeStockAdjustmentCarryForward,
			@PrevProductionPurposeCarryForward,
			@PreviousEODClosingStock,
			@PreviousEODClosingStockValue
			
			WHILE @@FETCH_STATUS = 0
			BEGIN	
				set @CurrentInStock = null
				set @InTran =  0
				set  @CurrentConsumption = null
				set @TempProductionOut = null 
				set @CurrentPhysicalInventory = null 					
				set @OutTran = null 
				set @CurrentWastage = null
				set @CurrentReturnAmount = null					
				set @CurrentInStockValue = null
				set @InTranValue = null
				set @TempProductionOut = null 
				set @TempProductionOutValue = null
				set @CurrentConsumptionValue = null 
				set @TempProductionOut = null
				set @CurrentPhysicalInventoryValue = null
				set @InTranValue = null
				set @OutTranValue = null
				set @CurrentWastageValue = null
				set @CurrentReturnAmountValue = null
				set @CurrentPhysicalInventoryValue = null
				set @CarryForwardConsumption = null
				set @CarryForwardConsumptionValue = null
				set @StockOutCarryForward = null
				set @StockOutCarryForwardValue = null
				set @WastageCarryForward = null
				set @WastageCarryForwardValue = null
				set @ReturnAmountCarryForward = null
				set @ReturnAmountCarryForwardValue = null
				set @NegativeStockAdjustmentCarryForward = null
				set @NegativeStockAdjustmentCarryForwardValue = null
				set @ProductionPurposeCarryForward = null
				set @ProductionPurposeCarryForwardValue = null
				set @Produced = null
				set @ProducedValue = null
				set @PositiveStockAdjustment = null
				set @PositiveStockAdjustmentValue = null
				set @ClosingStock = null
				set @ClosingStockValue  = null
				set @CurrentPhysicalNegativeImpact  = null
				set @NegativeStockAdjustmentValue  = null
							
				/*BatchInfo Work*/				
				INSERT INTO ##tempBatchInfo
				SELECT 
				cast(JSON_VALUE(CheckBatchInfo.value, '$.BP') as decimal(18,4)) BP
				,cast(JSON_VALUE(CheckBatchInfo.value, '$.CD') as datetime) CD
				,cast(JSON_VALUE(CheckBatchInfo.value, '$.QT') as decimal(18,4))  QT
				,JSON_VALUE(CheckBatchInfo.value, '$.SR') SR
				FROM  OPENJSON(convert(nvarchar(max),@BatchInfo)) CheckBatchInfo				
				
				/*BatchInfo Work*/
				SET @UnitMultiplier = (SELECT TransactionUnitMultiplier FROM InventoryItems WHERE Id = @InventoryItemId)
			
				SET @CurrentInStock = isnull( @PreviousEODClosingStock, 0)
									/*(
										   SELECT CASE  
										   WHEN @PhysicalInventory IS NULL  THEN (@InStock + @Purchase) - @Consumption - isnull(@Wastage, 0) - isnull(@ReturnAmount, 0)
										   ELSE @PhysicalInventory	
										   END 								   
									  )*/
				SET @CurrentInStockValue = isnull( @PreviousEODClosingStockValue, 0)
									 /*(
										   SELECT CASE  
										   WHEN @PhysicalInventory IS NULL  THEN ((@InStock + @Purchase) - @Consumption - isnull(@Wastage, 0) - isnull(@ReturnAmount, 0)) * isnull(@CostOfPreviousItem, 0)
										   ELSE @PhysicalInventory * isnull(@CostOfPreviousItem, 0)
										   END 								   
									  )*/

				/**/
				set @BatchCount = (select count(*) from ##tempBatchInfo)
				if @BatchCount = 0 
				begin
					insert into ##tempBatchInfo
					values(isnull(@CostOfPreviousItem, 0), cast('01 Jan 2019' as datetime), isnull(@InStock, 0), 'Initial Stock')
				end 
				/**/				   
				SET @CurrentPurchase  = (@Target - @Source)
				SET @CurrentConsumption =   (
												SELECT 
												CASE WHEN count(*) > 0 THEN 
												(SELECT max(impact) FROM #tempConsumptionTable WHERE inventoryitemid = @InventoryItemId)
												ELSE 0 
												END  
												FROM #tempConsumptionTable WHERE inventoryitemid = @InventoryItemId 
											)	   				
				/**/
				DECLARE db_cursor CURSOR FOR 
				select DocName, TranDate, ItemPrice, sum(Quantity) Quantity				
				from 
				(
					SELECT InventoryTransactionDocuments.Name DocName, InventoryTransactionDocuments.[Date] TranDate,
					case 
					when InventoryTransactions.Multiplier = 1 then InventoryTransactions.Price * @UnitMultiplier
					else InventoryTransactions.Price
					end ItemPrice,
					(InventoryTransactions.Quantity * InventoryTransactions.Multiplier/@UnitMultiplier) Quantity
					FROM InventoryTransactions , InventoryTransactionDocuments 
					WHERE
					InventoryTransactions.InventoryTransactionDocumentId = InventoryTransactionDocuments.Id
					AND InventoryTransactions.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
					AND InventoryTransactions.InventoryItem_Id = @InventoryItemId
					AND TargetWarehouseId = @WarehouseId
					and InventoryTransactionDocuments.SyncOutletId = @SyncOutletId 
				)InTran
				group by DocName, TranDate, ItemPrice

				OPEN db_cursor  
				FETCH NEXT FROM db_cursor INTO 	
				@InDocName,
				@InTranDate,
				@InItemPrice,
				@InQuantity

				WHILE @@FETCH_STATUS = 0  
				BEGIN
					insert into ##tempBatchInfo
					values(@InItemPrice, @InTranDate, @InQuantity, @InDocName)					  					
				FETCH NEXT FROM db_cursor INTO 
				@InDocName,
				@InTranDate,
				@InItemPrice,
				@InQuantity
				END 

				CLOSE db_cursor  
				DEALLOCATE db_cursor
				/**/
				SET @OutTran  =(isnull((
				 	SELECT sum(t.Quantity*t.Multiplier/@UnitMultiplier)
				 	FROM InventoryTransactionDocuments d, InventoryTransactions t WHERE 
				 	t.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
				 	AND t.InventoryItem_Id = @InventoryItemId
				 	AND SourceWarehouseId = @WarehouseID
				 	AND d.Id = t.InventoryTransactionDocumentId 
				 	AND d.IsProduction <> 1
				 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
				 ), 0))
				 SET @OutTranValue  =(isnull((
				 	SELECT sum(t.Quantity*t.Price)
				 	FROM InventoryTransactionDocuments d, InventoryTransactions t WHERE 
				 	t.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
				 	AND t.InventoryItem_Id = @InventoryItemId
				 	AND SourceWarehouseId = @WarehouseID
				 	AND d.Id = t.InventoryTransactionDocumentId 
				 	AND d.IsProduction <> 1
				 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
				 ), 0))
				SET @InTran =  (isnull((
								 	SELECT sum(InventoryTransactions.Quantity*InventoryTransactions.Multiplier/@UnitMultiplier)
								 	FROM InventoryTransactions , InventoryTransactionDocuments 
								 	WHERE
								 	InventoryTransactions.InventoryTransactionDocumentId = InventoryTransactionDocuments.Id
								 	AND InventoryTransactions.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
								 	AND InventoryTransactions.InventoryItem_Id = @InventoryItemId
								 	AND TargetWarehouseId = @WarehouseId
									and InventoryTransactionDocuments.SyncOutletId = @SyncOutletId				 	
								 	AND InventoryTransactionDocuments.IsProduction <> 1
								 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
								 ), 0))
				SET @InTranValue =  (isnull((
								 	SELECT sum(InventoryTransactions.Quantity*InventoryTransactions.Price)
								 	FROM InventoryTransactions , InventoryTransactionDocuments 
								 	WHERE
								 	InventoryTransactions.InventoryTransactionDocumentId = InventoryTransactionDocuments.Id
								 	AND InventoryTransactions.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
								 	AND InventoryTransactions.InventoryItem_Id = @InventoryItemId
								 	AND TargetWarehouseId = @WarehouseId
									and InventoryTransactionDocuments.SyncOutletId = @SyncOutletId				 	
								 	AND InventoryTransactionDocuments.IsProduction <> 1
								 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
								 ), 0))
				 SET @Produced =  (isnull((
								 	SELECT sum(InventoryTransactions.Quantity*InventoryTransactions.Multiplier/@UnitMultiplier)
								 	FROM InventoryTransactions , InventoryTransactionDocuments 
								 	WHERE
								 	InventoryTransactions.InventoryTransactionDocumentId = InventoryTransactionDocuments.Id
								 	AND InventoryTransactions.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
								 	AND InventoryTransactions.InventoryItem_Id = @InventoryItemId
								 	AND TargetWarehouseId = @WarehouseId
									and InventoryTransactionDocuments.SyncOutletId = @SyncOutletId				 	
								 	AND InventoryTransactionDocuments.IsProduction = 1
								 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
								 ), 0))
				SET @ProducedValue =(isnull((
								 	SELECT sum(InventoryTransactions.Quantity*InventoryTransactions.Price)
								 	FROM InventoryTransactions , InventoryTransactionDocuments 
								 	WHERE
								 	InventoryTransactions.InventoryTransactionDocumentId = InventoryTransactionDocuments.Id
								 	AND InventoryTransactions.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
								 	AND InventoryTransactions.InventoryItem_Id = @InventoryItemId
								 	AND TargetWarehouseId = @WarehouseId
									AND InventoryTransactionDocuments.SyncOutletId = @SyncOutletId				 	
								 	AND InventoryTransactionDocuments.IsProduction = 1
								 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
								 ), 0))
				SET @TempProductionOut =  (isnull((
									 	SELECT sum(t.Quantity*t.Multiplier/@UnitMultiplier)
									 	FROM InventoryTransactionDocuments d, InventoryTransactions t WHERE 
									 	t.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
									 	AND t.InventoryItem_Id = @InventoryItemId
									 	AND SourceWarehouseId = @WarehouseId
									 	AND d.Id = t.InventoryTransactionDocumentId 
									 	AND d.IsProduction = 1
									 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
									 ), 0))	   
				SET @TempProductionOutValue =  (isnull((
									 	SELECT sum(t.Quantity*t.Price)
									 	FROM InventoryTransactionDocuments d, InventoryTransactions t WHERE 
									 	t.[Date] BETWEEN @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
									 	AND t.InventoryItem_Id = @InventoryItemId
									 	AND d.IsProduction = 1
									 	AND SourceWarehouseId = @WarehouseId
									 	AND d.Id = t.InventoryTransactionDocumentId 
									 	--GROUP BY i.InventoryItemId, i.UnitMultiplier
									 ), 0))	   
				/************************************************************************/				
				SET @CurrentWastage =   (
											SELECT 
											CASE WHEN count(*) > 0 THEN 
											(SELECT sum(Wastage) FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId)
											ELSE 0 
											END  
											FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId 
										)
				SET @CurrentReturnAmount =  (
												SELECT 
												CASE WHEN count(*) > 0 THEN 
												(SELECT sum(ReturnAmount) FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId)
												ELSE 0 
												END  
												FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId 
											)
				SET @CurrentWastageValue =   (
											SELECT 
											CASE WHEN count(*) > 0 THEN 
											(SELECT sum(WastageValue) FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId)
											ELSE 0 
											END  
											FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId 
										)
				SET @CurrentReturnAmountValue =  (
												SELECT 
												CASE WHEN count(*) > 0 THEN 
												(SELECT sum(ReturnAmountValue) FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId)
												ELSE 0 
												END  
												FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId 
											)
				SET @CheckPhysicalInventory = (@CurrentInStock + @CurrentPurchase - @CurrentConsumption - @CurrentWastage - @CurrentReturnAmount)
				
				SET @CurrentStockTake = (
											SELECT 
											CASE WHEN count(*) > 0 THEN 
											(SELECT max(StockTake) FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId)
											ELSE @CheckPhysicalInventory
											END  
											FROM #tempOutletWastageStockTake WHERE inventoryitemid = @InventoryItemId 
										)
				
				
				SET @CurrentPhysicalInventory = (SELECT 
												CASE WHEN @CurrentStockTake <> @CheckPhysicalInventory 
												THEN @CurrentStockTake ELSE NULL END)
				SET @CurrentStockIn = @Target
				SET @CurrentStockOut = @Source 
				
				declare @TotalStockToDeduct as decimal(18, 4),
				 @TempBatchPrice as decimal(18, 4),
				 @TempCreationDate as datetime,
				 @TempStock as decimal(18, 4),
				 @TempBatchInfo as nvarchar(max),
				 @NegativeStock as decimal(18, 4)
				 
				set @NegativeStock = (select -1*isnull(sum(QT), 0) from ##tempBatchInfo where QT < 0)
				update ##tempBatchInfo set QT = 0 where QT < 0
				--Here to introduce physical plus stock
				declare @CurrentBatchQty decimal(18,4)				

				SET @CurrentPhysicalNegativeImpact = 0
				set @CurrentBatchQty =(select isnull(sum(QT), 0) from ##tempBatchInfo WHERE QT > 0)
				set @TotalStockToDeduct = (isnull(@NegativeStock, 0) + isnull(@CurrentConsumption, 0) + isnull(@OutTran, 0) + isnull(@TempProductionOut, 0) + isnull(@CurrentWastage, 0) + isnull(@CurrentReturnAmount, 0) /*+ isnull(@NegativeStock, 0)*/)
				if @CurrentPhysicalInventory is not null
				begin
					IF (@CurrentPhysicalInventory > @CurrentBatchQty - @TotalStockToDeduct)
					BEGIN
						update ##tempBatchInfo set QT = QT + (@CurrentPhysicalInventory - (@CurrentBatchQty - @TotalStockToDeduct)) where CD = 
						(select max(CD) from ##tempBatchInfo)
						SET @PositiveStockAdjustment = (@CurrentPhysicalInventory - (@CurrentBatchQty - @TotalStockToDeduct))
						SET @PositiveStockAdjustmentValue = 
															(
																SELECT max(BP) *(@CurrentPhysicalInventory - (@CurrentBatchQty - @TotalStockToDeduct))  
																FROM ##tempBatchInfo where CD = (select max(CD) from ##tempBatchInfo)
															)
					END
					ELSE
					BEGIN 
						SET @CurrentPhysicalNegativeImpact = (@CurrentBatchQty - @TotalStockToDeduct) - @CurrentPhysicalInventory
					END 
				end					
				-----------------------------------------------------
				-----
				-----Deduction Function Consumption
				-----								
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @CurrentConsumption, 0
				SET @CurrentConsumptionValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)
				SET @CarryForwardConsumption = (SELECT max(CarryForwardQty) + isnull(@ConsumptionCarryForwardPrevItem, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Consumption
				-----				
				-----
				-----Deduction Function Carry Forward Consumption
				-----					
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @CarryForwardConsumption, 1
				SET @CarryForwardConsumptionValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)
				SET @CarryForwardConsumption = (SELECT max(CarryForwardQty) + isnull(@CarryForwardConsumption, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Carry Forward Consumption
				----- 
				-----------------------------------------------------
				-----------------------------------------------------
				-----
				-----Deduction Function Production
				
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @TempProductionOut, 0				
				SET @TempProductionOutValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @ProductionPurposeCarryForward  = (SELECT max(CarryForwardQty) + isnull(@PrevProductionPurposeCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Production 
				-----				
				-----
				-----Deduction Function Carry Forward Production
				-----					
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @ProductionPurposeCarryForward, 1				
				SET @ProductionPurposeCarryForwardValue  = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @ProductionPurposeCarryForward = (SELECT max(CarryForwardQty) + isnull(@ProductionPurposeCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Carry Forward Production
				-----
				-----------------------------------------------------
				-----------------------------------------------------
				-----
				-----Deduction Function Stock Out
				-----												
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @OutTran, 0
				SET @OutTranValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)
				SET @StockOutCarryForward = (SELECT max(CarryForwardQty) + isnull(@PrevStockOutCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Stock Out
				-----				
				-----
				-----Deduction Function Carry Forward Stock Out
				-----					
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @StockOutCarryForward, 1
				SET @StockOutCarryForwardValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)
				SET @StockOutCarryForward = (SELECT max(CarryForwardQty) + isnull(@StockOutCarryForward, 0) FROM ##ConsumptionInfo)
				-----
				-----Deduction Function Carry Forward Stock Out
				-----
				-----------------------------------------------------
				-----------------------------------------------------
				-----
				-----Deduction Function Wastage				
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @CurrentWastage, 0				
				SET @CurrentWastageValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @WastageCarryForward  = (SELECT max(CarryForwardQty) + isnull(@PrevWastageCarryForward, 0) FROM ##ConsumptionInfo)
				-----
				-----Deduction Function Wastage 
				-----				
				-----
				-----Deduction Function Carry Forward Wastage
				-----					
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @WastageCarryForward, 1				
				SET @WastageCarryForwardValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @WastageCarryForward = (SELECT max(CarryForwardQty) + isnull(@WastageCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Carry Forward Wastage
				-----
				-----------------------------------------------------
				-----------------------------------------------------
				-----
				-----Deduction Function Return Amount
				-----																
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @CurrentReturnAmount, 0				
				SET @CurrentReturnAmountValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @ReturnAmountCarryForward  = (SELECT max(CarryForwardQty) + isnull(@PrevReturnAmountCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Return Amount 
				-----				
				-----
				-----Deduction Function Carry Forward Return Amount
				-----					
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @ReturnAmountCarryForward, 1				
				SET @ReturnAmountCarryForwardValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @ReturnAmountCarryForward = (SELECT max(CarryForwardQty) + isnull(@ReturnAmountCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Carry Forward Return Amount
				-----
				-----------------------------------------------------
				-----------------------------------------------------
				-----
				-----Deduction Function Negative Impact
				-----
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @CurrentPhysicalNegativeImpact, 0				
				SET @NegativeStockAdjustmentValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @NegativeStockAdjustmentCarryForward  = (SELECT max(CarryForwardQty) + isnull(@PrevNegativeStockAdjustmentCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Negative Impact 
				-----				
				-----
				-----Deduction Function Carry Forward Negative Impact
				-----					
				TRUNCATE TABLE ##ConsumptionInfo
				EXEC BatchDeduction @NegativeStockAdjustmentCarryForward, 1				
				SET @NegativeStockAdjustmentCarryForwardValue = (SELECT max(ConsumptionValue) FROM ##ConsumptionInfo)				
				SET @NegativeStockAdjustmentCarryForward = (SELECT max(CarryForwardQty) + isnull(@NegativeStockAdjustmentCarryForward, 0) FROM ##ConsumptionInfo)				
				-----
				-----Deduction Function Carry Forward Negative Impact
				-----
				-----------------------------------------------------				
							
				set @CurrentBatchQty =(select isnull(sum(QT), 0) from ##tempBatchInfo)

				set @CurrentPhysicalInventoryValue = null
				if @CurrentPhysicalInventory is not null
				begin					
					set @CurrentPhysicalInventoryValue =  (select isnull(SUM(BP*QT), 0) from ##tempBatchInfo WHERE QT > 0)
				end 
						
				Declare  
				@ZeroBatchPrice DATETIME,
				@ZeroBatchCount int
				
				set @BatchCount = (select count(*) from ##tempBatchInfo)
				set @ZeroBatchCount = (select count(*) from ##tempBatchInfo WHERE QT = 0)
				SET @ZeroBatchPrice = (SELECT TOP 1 CD FROM ##tempBatchInfo WHERE QT = 0 order by cast(CD as datetime) desc)
				IF @BatchCount = @ZeroBatchCount
				BEGIN 					
					delete from ##tempBatchInfo where QT = 0 and CD <> @ZeroBatchPrice
				END 
				ELSE
				BEGIN 
					delete from ##tempBatchInfo where QT = 0
				END 
				
				/**/
				set @BatchCount = (select count(*) from ##tempBatchInfo)
				if @BatchCount = 0 
				begin
					insert into ##tempBatchInfo
					values(isnull(@CostOfPreviousItem, 0), cast('01 Jan 2019' as datetime), 0, 'Initial Stock')
				end 
				/**/
				set @TempBatchInfo = (select * from 
				##tempBatchInfo order by cast(CD as datetime)
				for json path)
				set @BatchCount = (select count(*) from ##tempBatchInfo where QT <> 0)
				set @InventoryBatchPrice = 0
				if @BatchCount > 0
				begin
					set @InventoryBatchPrice = (select isnull(SUM(BP*QT)/SUM(QT), 0) from ##tempBatchInfo)
				end 
				
				SET @ClosingStock = (select isnull(SUM(QT), 0) from ##tempBatchInfo)
				SET @ClosingStockValue = (select isnull(SUM(BP*QT), 0) from ##tempBatchInfo WHERE QT > 0)
				IF @ClosingStock > 0 
				begin
					SET @InventoryBatchPrice = (select isnull(SUM(BP*QT), 0)/ @ClosingStock from ##tempBatchInfo WHERE QT > 0)
				END	
				ELSE 
				BEGIN 
					SET @InventoryBatchPrice = 0
				END 
				INSERT INTO dbo.PeriodicConsumptionItems 
				(
					WarehouseConsumptionId, PeriodicConsumptionId, 
					InventoryItemId, InventoryItemName, UnitName, UnitMultiplier, InStock, Purchase, Consumption, 
					PhysicalInventory, Cost, StockIn, StockOut, Wastage, ReturnAmount, StockTake, Remarks, BatchInfo, 
					InStockValue,
					PurchaseValue,
					Production,
					ProductionValue,
					ConsumptionValue,
					PhysicalInventoryValue,
					StockInValue,
					StockOutValue,
					WastageValue,
					ReturnAmountValue,
					StockTakeValue,
					ConsumptionCarryForward, 
					ConsumptionCarryForwardValue,
					StockOutCarryForward, 
					StockOutCarryForwardValue,
					WastageCarryForward,
					WastageCarryForwardValue,
					ReturnAmountCarryForward, 
					ReturnAmountCarryForwardValue,
					NegativeStockAdjustmentCarryForward, 
					NegativeStockAdjustmentCarryForwardValue,
					ProductionPurposeCarryForward,
					ProductionPurposeCarryForwardValue,
					Produced,
					ProducedValue,
					PositiveStockAdjustment,
					PositiveStockAdjustmentValue,
					ClosingStock,
					ClosingStockValue,
					NegativeStockAdjustment,
					NegativeStockAdjustmentValue,
					LatestPrice 
				)
				VALUES 
				(
					@CurrentPeriodicConsumptionID, @CurrentWarehouseConsmptionID, @InventoryItemId, @InventoryName, 
					@TransactionUnit, @TransactionUnitMultiplier, @CurrentInStock, isnull(@InTran, 0), @CurrentConsumption + @TempProductionOut, @CurrentPhysicalInventory, @InventoryBatchPrice, 
					isnull(@InTran, 0), isnull(@OutTran, 0), @CurrentWastage, @CurrentReturnAmount, null, null, @TempBatchInfo, @CurrentInStockValue,  isnull(@InTranValue, 0), 
					@TempProductionOut, @TempProductionOutValue, @CurrentConsumptionValue + @TempProductionOut, @CurrentPhysicalInventoryValue, isnull(@InTranValue, 0), isnull(@OutTranValue, 0), 
					@CurrentWastageValue, @CurrentReturnAmountValue, @CurrentPhysicalInventoryValue,
					@CarryForwardConsumption, @CarryForwardConsumptionValue, @StockOutCarryForward, @StockOutCarryForwardValue,
					@WastageCarryForward, @WastageCarryForwardValue, @ReturnAmountCarryForward, @ReturnAmountCarryForwardValue,
					@NegativeStockAdjustmentCarryForward, 
					@NegativeStockAdjustmentCarryForwardValue,
					@ProductionPurposeCarryForward,
					@ProductionPurposeCarryForwardValue,
					@Produced,
					@ProducedValue,
					isnull(@PositiveStockAdjustment, 0),
					isnull(@PositiveStockAdjustmentValue, 0),
					@ClosingStock,
					@ClosingStockValue,
					@CurrentPhysicalNegativeImpact,
					@NegativeStockAdjustmentValue,
					@InventoryItemPrice
				)
				
				truncate table ##tempBatchInfo				
				
			FETCH NEXT FROM WarehouseItemCursor INTO 
			@InventoryItemId ,
			@InventoryName ,
			@TransactionUnit,
			@InventoryItemPrice,
			@TransactionUnitMultiplier,
			@Source,
			@Target,			
			@InStock , 
			@Purchase , 
			@Consumption , 
			@PhysicalInventory , 
			@StockIn  , 
			@StockOut , 
			@Wastage , 
			@ReturnAmount , 
			@StockTake , 
			@Remarks,			
			@BatchInfo,
			@BatchInfoWhileWorkPeriodEnd,
			@InStockValue,
			@PurchaseValue,
			@Production,
			@ProductionValue,
			@ConsumptionValue,
			@PhysicalInventoryValue,
			@StockInValue,
			@StockOutValue,
			@WastageValue,
			@ReturnAmountValue,
			@StockTakeValue,
			@CostOfPreviousItem,
			@ConsumptionCarryForwardPrevItem,
			@PrevStockOutCarryForward,
			@PrevWastageCarryForward,
			@PrevReturnAmountCarryForward,
			@PrevNegativeStockAdjustmentCarryForward,
			@PrevProductionPurposeCarryForward,
			@PreviousEODClosingStock,
			@PreviousEODClosingStockValue		
			
			END
			CLOSE WarehouseItemCursor
			DEALLOCATE WarehouseItemCursor
			
			DROP TABLE #tempConsumptionTable 
			DROP TABLE #tempOutletWastageStockTake
	  
	FETCH NEXT FROM WarehouseCursor INTO    
	@WarehouseID , 
	@WarehouseName,
	@SyncOutletId
	
	END
	CLOSE WarehouseCursor
	DEALLOCATE WarehouseCursor
	-----------------------------------
	----Update Tickets Work Period-----
	-----------------------------------
	Update Tickets set WorkPeriodStartDate = @CurrentWorkPeriodStartDate
	where tickets.[Date] between @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate

	Update Tickets set WorkPeriodId = @CurrentWorkPeriodId
	where tickets.[Date] between @CurrentWorkPeriodStartDate and @CurrentWorkPeriodEndDate
	-----------------------------------
	----Update Tickets Work Period-----
	-----------------------------------
	INSERT INTO dbo.WorkPeriods (StartDate, EndDate, StartDescription, EndDescription, Name, Synced, IsSyncedToSTAR, SyncID, SyncOutletID)
	VALUES (DATEADD(mi,1,@WorkPeriodEnd), DATEADD(mi,1,@WorkPeriodEnd), 'Automated Started by Scheduled System', null, null, 0, null, 0, 0)
				
	drop table ##tempBatchInfo	
	drop table ##ConsumptionInfo
	COMMIT TRAN 
END 
GRANT EXECUTE ON dbo.Work_Period_End TO PUBLIC

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Work_Period_End] TO PUBLIC
    AS [dbo];


GO

