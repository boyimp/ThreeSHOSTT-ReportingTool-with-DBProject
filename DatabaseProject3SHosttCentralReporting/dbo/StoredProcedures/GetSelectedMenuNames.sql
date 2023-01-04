
CREATE PROC [dbo].[GetSelectedMenuNames]
(	
	@TableMenuGroups TableValueStringParameters readonly,
	@TableMenuItems TableValueIntParameters readonly	
)
AS

	
	DECLARE 
	@PermittedCount AS INT,
	@SelectedCount AS INT,
	@SelectedMenuItems AS NVARCHAR(Max)
	SET @SelectedMenuItems = ','
	SET @SelectedCount = 
	(
		SELECT count(*) FROM 
		(
			SELECT DISTINCT MenuItems.Name 
			FROM 
			MenuItems 
			WHERE MenuItems.GroupCode IN (SELECT value FROM @TableMenuGroups) 
			AND menuitems.Id IN (SELECT value FROM @TableMenuItems)
			AND GroupCode IS NOT NULL
		)M
	)
	SET @PermittedCount = 
		(
			SELECT count(*) FROM 
			(
				SELECT DISTINCT MenuItems.Name 
				FROM 
				MenuItems 
				WHERE MenuItems.GroupCode IN (SELECT value FROM @TableMenuGroups) 				
				AND GroupCode IS NOT NULL
			)M
		)
						
	IF @SelectedCount =  @PermittedCount AND @SelectedCount > 0
	BEGIN 
		SET @SelectedMenuItems = 'All Menu Items'
	END 
	ELSE
	BEGIN 
	 	SELECT @SelectedMenuItems = COALESCE(@SelectedMenuItems+', ' ,'') +  MenuItemNames
		FROM 
		(	
			SELECT DISTINCT Name MenuItemNames
			FROM 
			MenuItems 
			WHERE 
			MenuItems.Id IN (SELECT value FROM @TableMenuItems)				
		)
		M		
							
	END 
	--SET @SelectedMenuGroups =  '' + CAST(@SelectedCount AS NVARCHAR(max)) + ' ' + CAST(@PermittedCount AS NVARCHAR(max))
	SELECT 'Menu Item(s):'+ replace(@SelectedMenuItems, ',,','')  SelectedMenuNames

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedMenuNames] TO PUBLIC
    AS [dbo];


GO

