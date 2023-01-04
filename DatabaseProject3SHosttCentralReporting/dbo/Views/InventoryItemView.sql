
create view [dbo].[InventoryItemView] AS
SELECT i.Id, i.GroupCode, i.BaseUnit, i.TransactionUnit, i.TransactionUnitMultiplier, 
i.AlarmThreshold, i.Name, i.StarID, i.StarCode, i.InventoryTakeType, ISNULL(ItemPrice.UnitPrice, 0) * i.TransactionUnitMultiplier AS TransactionUnitPrice
FROM            
dbo.InventoryItems AS i LEFT OUTER JOIN
(
	SELECT l.InventoryItem_Id, t.Price AS UnitPrice
	FROM            
	(
		SELECT        InventoryItem_Id, MAX(Id) AS id
		FROM            dbo.InventoryTransactions
		GROUP BY InventoryItem_Id
	) AS l INNER JOIN
	(
		SELECT        Id, InventoryTransactionDocumentId, InventoryTransactionTypeId, SourceWarehouseId, 
		TargetWarehouseId, Date, Unit, Multiplier, Quantity, Price/ Multiplier Price, InventoryItem_Id
		FROM            dbo.InventoryTransactions
	) AS t ON l.id = t.Id
) AS ItemPrice ON ItemPrice.InventoryItem_Id = i.Id

GO

