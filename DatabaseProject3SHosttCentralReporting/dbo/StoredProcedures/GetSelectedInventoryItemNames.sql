
CREATE PROC [dbo].[GetSelectedInventoryItemNames]
(	
	@TableInventoryItemGroups TableValueStringParameters readonly,
	@TableInventoryItems TableValueIntParameters readonly	
)
AS

	
	DECLARE 
	@PermittedCount AS INT,
	@SelectedCount AS INT,
	@SelectedInventoryItems AS NVARCHAR(Max)
	SET @SelectedInventoryItems = ','
	SET @SelectedCount = 
	(
		SELECT count(*) FROM 
		(
			SELECT DISTINCT InventoryItems.Name 
			FROM 
			InventoryItems 
			WHERE InventoryItems.GroupCode IN (SELECT value FROM @TableInventoryItemGroups) 
			AND InventoryItems.Id IN (SELECT value FROM @TableInventoryItems)
			AND GroupCode IS NOT NULL
		)M
	)
	SET @PermittedCount = 
		(
			SELECT count(*) FROM 
			(
				SELECT DISTINCT InventoryItems.Name 
				FROM 
				InventoryItems 
				WHERE InventoryItems.GroupCode IN (SELECT value FROM @TableInventoryItemGroups) 				
				AND GroupCode IS NOT NULL
			)M
		)
						
	IF @SelectedCount =  @PermittedCount AND @SelectedCount > 0
	BEGIN 
		SET @SelectedInventoryItems = 'All Inventory Items'
	END 
	ELSE
	BEGIN 
	 	SELECT @SelectedInventoryItems = COALESCE(@SelectedInventoryItems+', ' ,'') +  InventoryItemNames
		FROM 
		(	
			SELECT DISTINCT Name InventoryItemNames
			FROM 
			InventoryItems 
			WHERE 
			InventoryItems.Id IN (SELECT value FROM @TableInventoryItems)				
		)
		M		
							
	END 
	--SET @SelectedInventoryItemGroups =  '' + CAST(@SelectedCount AS NVARCHAR(max)) + ' ' + CAST(@PermittedCount AS NVARCHAR(max))
	SELECT 'Inventory Item(s):'+ replace(@SelectedInventoryItems, ',,','')  SelectedInventoryItemNames

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedInventoryItemNames] TO PUBLIC
    AS [dbo];


GO

