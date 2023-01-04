CREATE view [dbo].[TicketMasterView] AS

	SELECT  d.Name DepartmentName, tt.Name TicketTypeName, 
	'' TableName, '' CustomerName, '' CustomerRegCode, '' AdministratorName, t.*
	FROM Tickets t, Departments d, TicketTypes tt
	WHERE t.DepartmentId = d.Id AND tt.Id = t.TicketTypeId

GO

