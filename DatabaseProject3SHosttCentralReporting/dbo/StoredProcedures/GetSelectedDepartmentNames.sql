
CREATE PROC [dbo].[GetSelectedDepartmentNames]
(
	@UserId INT,
	@TableDepartments TableValueIntParameters readonly	
)
AS

	
	DECLARE 
	@PermittedCount AS INT,
	@SelectedCount AS INT,
	@SelectedOutlet AS NVARCHAR(Max)
	SET @SelectedOutlet = ','
	SET @SelectedCount = 
	(
		SELECT count(*) FROM departments WHERE Id IN(SELECT value FROM @TableDepartments)
	)
	SET @PermittedCount = (SELECT count(*) from
						(							
								SELECT 
								DISTINCT Id, Name, PermissionName FROM
								(
									SELECT 
									d.Id, d.Name, p.Name PermissionName
									FROM UserRoles r, Users u, Permissions p, Departments d
									WHERE r.Id = u.UserRole_Id
									AND r.Id = p.UserRoleId 
									AND r.Id = (SELECT UserRole_Id FROM Users WHERE Id = @UserId)
									AND p.Name LIKE '%UseDepartment%'
									AND p.Value = 0									
								)P
								WHERE CAST(Replace(p.PermissionName, 'UseDepartment_', '') AS INT) = P.Id
						)T)
	IF @SelectedCount =  @PermittedCount
	BEGIN 
		SET @SelectedOutlet = 'All Permitted Department(s)'
	END 
	ELSE
	BEGIN 

		SELECT @SelectedOutlet = COALESCE(@SelectedOutlet+', ' ,'') + Name
		FROM Departments
		WHERE 
		Id IN (SELECT value FROM @TableDepartments)
							
	END 
	
	SELECT 'Department(s):'+ replace(@SelectedOutlet, ',,','') SelectedDepartmentNames

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetSelectedDepartmentNames] TO PUBLIC
    AS [dbo];


GO

