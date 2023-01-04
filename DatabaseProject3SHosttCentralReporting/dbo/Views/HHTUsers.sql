
Create VIEW [dbo].[HHTUsers] as

									select Users.Id, Users.Name, Users.UserRole_Id, Users.PinCode, Permission.DepartmentIds from Users left join
									(
									SELECT UserRoleID, DepartmentIds = 
											STUFF((SELECT ', ' + REPLACE( Name, 'UseDepartment_', '')
												   FROM Permissions b 
												   WHERE b.UserRoleId = a.UserRoleId AND value = 0 AND b.Name LIKE '%UseDepartment%'
												  FOR XML PATH('')), 1, 2, '')
										FROM Permissions a WHERE a.Name LIKE '%UseDepartment%'
										GROUP BY UserRoleId) Permission on Users.UserRole_Id = Permission.UserRoleId

GO

