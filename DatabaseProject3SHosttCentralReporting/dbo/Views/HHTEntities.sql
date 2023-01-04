
CREATE VIEW [dbo].[HHTEntities]
AS
SELECT Id = m.EntityId,
       Name = m.EntityName,
       m.EntityTypeId,
       CustomData,
       ZoneId,
       UserRoleIdCsv = STUFF(
                       (
                           SELECT ', ' + CAST(map.UserRoleIdCsv AS NVARCHAR(11))
                           FROM dbo.HHTEntityPermission map
                           WHERE map.EntityId = m.EntityId
                           FOR XML PATH('')
                       ),
                       1,
                       2,
                       ''
                            ),
       DepartmentIdCsv = STUFF(
                         (
                             SELECT ', ' + CAST(map.DepartmentIdCsv AS NVARCHAR(11))
                             FROM dbo.HHTEntityPermission map
                             WHERE map.EntityId = m.EntityId
                             FOR XML PATH('')
                         ),
                         1,
                         2,
                         ''
                              )
FROM dbo.HHTEntityPermission m
GROUP BY m.EntityId,
         m.EntityName,
         m.EntityTypeId,
         CustomData,
         ZoneId;

GO

