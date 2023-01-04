
CREATE PROC [dbo].[GetSelectedOutletNames]
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
		SELECT count(*) FROM syncoutlets WHERE Id IN(SELECT value FROM @TableOutlets)
	)

	SET @PermittedCount = (SELECT count(*) from
						(							
								SELECT 
								DISTINCT Id, Name, PermissionName FROM
								(
									SELECT 
									d.Id, d.Name, p.Name PermissionName
									FROM UserRoles r, Users u, Permissions p, SyncOutlets d
									WHERE r.Id = u.UserRole_Id
									AND r.Id = p.UserRoleId 
									AND r.Id = (SELECT UserRole_Id FROM Users WHERE Id = @UserId)
									AND p.Name LIKE '%UseOulet_%'
									AND p.Value = 0									
								)P
								WHERE CAST(Replace(p.PermissionName, 'UseOulet_', '') AS INT) = P.Id
						)T)
	IF @SelectedCount =  @PermittedCount
	BEGIN 
		SET @SelectedOutlet = 'All Permitted Outlet(s)'
	END 
	ELSE
	BEGIN 

		SELECT @SelectedOutlet = COALESCE(@SelectedOutlet+', ' ,'') + Name
		FROM SyncOutlets
		WHERE 
		Id IN (SELECT value FROM @TableOutlets)
							
	END 
	
	SELECT 'Outlet(s):'+ replace(@SelectedOutlet, ',,','') SelectedOutletNames

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedOutletNames] TO PUBLIC
    AS [dbo];


GO

