
CREATE PROC [dbo].[GetInventoryItemsOfGroups]
(	
	@TableInventoryGroup TableValueStringParameters readonly	
)
AS

	SELECT Id InventoryId, I.Name InventoryItemName 
	FROM InventoryItems i WHERE i.GroupCode IN(SELECT value FROM @TableInventoryGroup)

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryItemsOfGroups] TO PUBLIC
    AS [dbo];


GO

