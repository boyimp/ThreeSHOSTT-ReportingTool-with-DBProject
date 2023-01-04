
CREATE PROC [dbo].[GetSelectedEntities]
(	
	@EntityType INT,	
	@TableEntities TableValueIntParameters readonly	
)
AS

	
	DECLARE 
	@PermittedCount AS INT,
	@SelectedCount AS INT,
	@SelectedMenuItems AS NVARCHAR(Max),
	@EntityName AS NVARCHAR(Max)
	SET @SelectedMenuItems = ','
	SET @EntityName = (SELECT name FROM EntityTypes WHERE id =  @EntityType)
	SET @SelectedCount = 
	(
		SELECT count(*) FROM 
		(
			SELECT *
			FROM 
			Entities 
			WHERE Id IN (SELECT value FROM @TableEntities) 			
		)M
	)
	SET @PermittedCount = 
		(
			SELECT count(*) FROM 
			(
				SELECT *
				FROM 
				Entities 
				WHERE  EntityTypeId = @EntityType				
			)M
		)
						
	IF @SelectedCount =  @PermittedCount AND @SelectedCount > 0
	BEGIN 
		SET @SelectedMenuItems = 'All '+ @EntityName
	END 
	ELSE
	BEGIN 
	 	SELECT @SelectedMenuItems = COALESCE(@SelectedMenuItems+', ' ,'') +  EntityName
		FROM 
		(	
			SELECT DISTINCT Name EntityName
			FROM Entities			 
			WHERE 
			Id IN (SELECT value FROM @TableEntities)				
		)
		M		
							
	END 
	--SET @SelectedMenuGroups =  '' + CAST(@SelectedCount AS NVARCHAR(max)) + ' ' + CAST(@PermittedCount AS NVARCHAR(max))
	SELECT 'Entity Type:'+ replace(@SelectedMenuItems, ',,','')  SelectedEntities

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedEntities] TO PUBLIC
    AS [dbo];


GO

