
CREATE PROC [dbo].[GetSelectedMenuGroupNames]
(	
	@TableMenuGroups TableValueStringParameters readonly	
)
AS

	
	DECLARE 
	@PermittedCount AS INT,
	@SelectedCount AS INT,
	@SelectedMenuGroups AS NVARCHAR(Max)
	SET @SelectedMenuGroups = ','
	SET @SelectedCount = 
	(
		SELECT count(*) FROM 
		(
			SELECT DISTINCT GroupCode FROM 
			MenuItems 
			WHERE MenuItems.GroupCode IN (SELECT value FROM @TableMenuGroups) AND GroupCode IS NOT NULL
		)M
	)
	SET @PermittedCount = (SELECT  count(DISTINCT GroupCode) FROM MenuItems)
						
	IF @SelectedCount =  @PermittedCount
	BEGIN 
		SET @SelectedMenuGroups = 'All Menu Groups'
	END 
	ELSE
	BEGIN 
	 	SELECT @SelectedMenuGroups = COALESCE(@SelectedMenuGroups+', ' ,'') +  GroupCode
		FROM 
		(	
			SELECT DISTINCT GroupCode 
			FROM 
			MenuItems 
			WHERE 
			MenuItems.GroupCode IN (SELECT value FROM @TableMenuGroups)	
		)
		M		
							
	END 
	--SET @SelectedMenuGroups =  '' + CAST(@SelectedCount AS NVARCHAR(max)) + ' ' + CAST(@PermittedCount AS NVARCHAR(max))
	SELECT 'Menu Group(s):'+ replace(@SelectedMenuGroups, ',,','')  SelectedMenuGroupNames

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedMenuGroupNames] TO PUBLIC
    AS [dbo];


GO

