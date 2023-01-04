CREATE VIEW [dbo].[HHTScreenMenuItems] as
select * from ScreenMenuItems where MenuItemId in (select Id from MenuItems)

GO

