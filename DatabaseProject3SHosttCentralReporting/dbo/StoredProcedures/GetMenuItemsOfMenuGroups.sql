
CREATE PROC [dbo].[GetMenuItemsOfMenuGroups]
(	
	@TableMenuGroup TableValueStringParameters readonly	
)
AS

	SELECT Id MenuItemId, MenuItems.Name MenuItemName 
	FROM MenuItems WHERE GroupCode IN(SELECT value FROM @TableMenuGroup)

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetMenuItemsOfMenuGroups] TO PUBLIC
    AS [dbo];


GO

