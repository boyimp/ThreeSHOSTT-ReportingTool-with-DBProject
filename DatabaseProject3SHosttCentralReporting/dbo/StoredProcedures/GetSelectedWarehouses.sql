
CREATE PROC [dbo].[GetSelectedWarehouses]
(
	@UserId INT,
	@TableOutlets TableValueIntParameters readonly	
)
AS

	
	DECLARE 
	@PermittedCount AS INT,
	@SelectedCount AS INT,
	@SelectedOutlet AS NVARCHAR(Max)
	SET @SelectedOutlet = ','
	SET @SelectedCount = 
	(
		SELECT count(*) FROM Warehouses WHERE Id IN(SELECT value FROM @TableOutlets)
	)						
	 SET @PermittedCount = (SELECT count(*) from
						(							
								SELECT 
								DISTINCT Id, Name, PermissionName FROM
								(
									SELECT 
									d.Id, d.Name, p.Name PermissionName
									FROM UserRoles r, Users u, Permissions p, Warehouses d
									WHERE r.Id = u.UserRole_Id
									AND r.Id = p.UserRoleId 
									AND r.Id = (SELECT UserRole_Id FROM Users WHERE Id = @UserId)
									AND p.Name LIKE '%UseWarehouse_%'
									AND p.Value = 0								
								)P
								WHERE CAST(Replace(p.PermissionName, 'UseWarehouse_', '') AS INT) = P.Id
						)T)
	IF @SelectedCount =  @PermittedCount
	BEGIN 
		SET @SelectedOutlet = 'All Permitted Warehouse(s)'
	END 
	ELSE
	BEGIN 

		SELECT @SelectedOutlet = COALESCE(@SelectedOutlet+', ' ,'') + Name
		FROM Warehouses
		WHERE 
		Id IN (SELECT value FROM @TableOutlets)
							
	END 
	
	SELECT 'Warehouse(s):'+ replace(@SelectedOutlet, ',,','') SelectedWarehouseNames

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedWarehouses] TO PUBLIC
    AS [dbo];


GO

