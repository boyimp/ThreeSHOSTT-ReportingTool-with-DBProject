CREATE VIEW [dbo].[HHTDepartments] as

select d.Id DepartmentId,d.PriceTag, d.Name DepartmentName, t.Id TicketTypeId,
t.Name TicketTypeName, m.id as MenuId, m.Name MenuName
from
Departments d, TicketTypes t, ScreenMenus m
where d.TicketTypeId = t.Id
and t.ScreenMenuId = m.Id

GO

