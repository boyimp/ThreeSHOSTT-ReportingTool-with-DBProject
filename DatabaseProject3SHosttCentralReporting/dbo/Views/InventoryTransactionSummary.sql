
CREATE VIEW [dbo].[InventoryTransactionSummary] AS 

 

	SELECT p.InventoryItem_Id, 
		   p.Price, p.Unit,
		   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
								 ORDER BY Id DESC) AS rk
	FROM InventoryTransactions p

GO

