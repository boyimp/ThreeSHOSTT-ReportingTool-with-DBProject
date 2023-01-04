
CREATE PROC [dbo].[GetSelectedInventoryGroupNames]
(	
	@TableInventoryGroups TableValueStringParameters readonly	
)
AS

	
	DECLARE 
	@PermittedCount AS INT,
	@SelectedCount AS INT,
	@SelectedInventoryGroups AS NVARCHAR(Max)
	SET @SelectedInventoryGroups = ','
	SET @SelectedCount = 
	(
		SELECT count(*) FROM 
		(
			SELECT DISTINCT GroupCode FROM 
			InventoryItems 
			WHERE InventoryItems.GroupCode IN (SELECT value FROM @TableInventoryGroups) AND GroupCode IS NOT NULL
		)M
	)
	SET @PermittedCount = (SELECT  count(DISTINCT GroupCode) FROM InventoryItems)
						
	IF @SelectedCount =  @PermittedCount
	BEGIN 
		SET @SelectedInventoryGroups = 'All Inventory Groups'
	END 
	ELSE
	BEGIN 
	 	SELECT @SelectedInventoryGroups = COALESCE(@SelectedInventoryGroups+', ' ,'') +  GroupCode
		FROM 
		(	
			SELECT DISTINCT GroupCode 
			FROM 
			InventoryItems 
			WHERE 
			InventoryItems.GroupCode IN (SELECT value FROM @TableInventoryGroups)	
		)
		M		
							
	END 
	--SET @SelectedInventoryGroups =  '' + CAST(@SelectedCount AS NVARCHAR(max)) + ' ' + CAST(@PermittedCount AS NVARCHAR(max))
	SELECT 'Inventory Group(s):'+ replace(@SelectedInventoryGroups, ',,','')  SelectedInventoryGroupNames

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedInventoryGroupNames] TO PUBLIC
    AS [dbo];


GO

