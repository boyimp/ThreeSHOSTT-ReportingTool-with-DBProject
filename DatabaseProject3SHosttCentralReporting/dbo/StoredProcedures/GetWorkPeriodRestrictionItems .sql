
CREATE PROCEDURE [dbo].[GetWorkPeriodRestrictionItems ]

AS BEGIN 

DECLARE 
@PreviousWorkPeriodID AS INT,
@CurrentWorkPeriodId AS INT,
@PreviousWorkPeriodEndDate AS DATETIME,
@CurrentWorkPeriodStartDate AS DATETIME


SET @PreviousWorkPeriodID = (SELECT workperiodid FROM PeriodicConsumptions WHERE id = (SELECT max(ID) FROM PeriodicConsumptions))
SET @PreviousWorkPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE id = @PreviousWorkPeriodID)
SET @CurrentWorkPeriodId = (SELECT ID FROM WorkPeriods WHERE StartDate = enddate AND id = (SELECT max(id) FROM WorkPeriods))
SET @CurrentWorkPeriodStartDate = (SELECT startdate FROM WorkPeriods WHERE id = @CurrentWorkPeriodId)	
				
 

SELECT 
Inv.Id InventoryItemId, Inv.Name InventoryItemName, w.Name WarehouseName,  cast(IsCostZeroButHasStockMovement as bit)IsCostZeroButHasStockMovement, cast(IsInStockNegative as bit)IsInStockNegative, Impact Quantity
FROM
InventoryItems Inv, Warehouses w,
(
	SELECT * FROM 
   (	
	SELECT WarehouseId, InventoryItemId, sum(Impact)Impact, 1 IsCostZeroButHasStockMovement, 0 IsInStockNegative
	FROM 
	(
		SELECT o.WarehouseId, o.MenuItemName, p.Name PortionName, r.Name, o.Quantity OrderQty, i.ID InventoryItemId,
		i.Name InventoryItemName, (o.Quantity*ri.Quantity)/ i.transactionunitmultiplier Impact
		FROM 
		(
			SELECT * 
			FROM Orders  
			WHERE createddatetime 
			BETWEEN isnull(@PreviousWorkPeriodEndDate, @CurrentWorkPeriodStartDate)  AND getdate()			
			AND DecreaseInventory  = 1				
		)o, MenuItemPortions p, Recipes r, RecipeItems ri, InventoryItems i
		WHERE o.MenuItemId = p.MenuItemId
		AND o.portionname = p.Name
		AND r.Portion_Id = p.Id
		AND r.id = ri.recipeid
		AND ri.inventoryitem_id = i.id
	) T
	GROUP BY WarehouseId, InventoryItemId
	UNION		
	SELECT t.SourceWarehouseId WarehouseId, t.InventoryItem_Id InventoryItemId, isnull(((t.Quantity*t.Multiplier)), 0) Impact, 1 IsCostZeroButHasStockMovement, 0 IsInStockNegative
	FROM InventoryTransactions t, InventoryTransactionDocuments d
	WHERE d.Id = t.InventoryTransactionDocumentId 
	AND t.[Date] 
	BETWEEN isnull(@PreviousWorkPeriodEndDate,@CurrentWorkPeriodStartDate) AND getdate()	
	UNION 
	SELECT t.TargetWarehouseId WarehouseId, t.InventoryItem_Id InventoryItemId, isnull(((t.Quantity*t.Multiplier)), 0) Impact, 1 IsCostZeroButHasStockMovement, 0 IsInStockNegative
	FROM InventoryTransactions t, InventoryTransactionDocuments d
	WHERE d.Id = t.InventoryTransactionDocumentId 
	AND t.[Date] 
	BETWEEN isnull(@PreviousWorkPeriodEndDate,@CurrentWorkPeriodStartDate) AND getdate()
	
	)Q
	WHERE InventoryItemId NOT IN
	(
		SELECT InventoryItem_Id 
		FROM InventoryTransactions 
		WHERE TargetWarehouseId = Q.WarehouseId 
		AND MasterDataID IS NOT null
	)
	UNION 
	SELECT * FROM 
	(
		SELECT WC.WarehouseId, pci.InventoryItemId, 
		CASE  
		WHEN PhysicalInventory IS NULL  THEN (isnull(InStock, 0) + Isnull(Purchase, 0) + Isnull(Produced, 0)) - Isnull(Consumption, 0) - isnull(Wastage, 0) - isnull(ReturnAmount, 0)
		ELSE PhysicalInventory	
		END 	
		Impact, 
		0 IsCostZeroButHasStockMovement, 1 IsInStockNegative
		FROM PeriodicConsumptionItems pci,
		(
			SELECT * FROM WarehouseConsumptions 
			WHERE PeriodicConsumptionId = 
			(
				SELECT max(id) 
				FROM PeriodicConsumptions 
				WHERE WorkPeriodId = @PreviousWorkPeriodID				
			)
		)WC
		WHERE  pci.PeriodicConsumptionId = WC.Id
		AND WarehouseConsumptionId = (SELECT max(id) FROM PeriodicConsumptions WHERE 
		WorkPeriodId = @PreviousWorkPeriodID)
	) L
	WHERE L.Impact < 0
)D
WHERE Inv.Id = D.InventoryItemId
AND W.Id = D.WarehouseId

END 
GRANT EXECUTE ON dbo.GetWorkPeriodRestrictionItems  TO PUBLIC

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetWorkPeriodRestrictionItems ] TO PUBLIC
    AS [dbo];


GO

