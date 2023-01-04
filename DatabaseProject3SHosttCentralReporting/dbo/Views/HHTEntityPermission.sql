
CREATE VIEW [dbo].[HHTEntityPermission]
AS
SELECT DISTINCT
       e.Id EntityId,
       e.Name EntityName,
       e.EntityTypeId,
       e.CustomData,
       e.ZoneId,
       UserRoleIdCsv = STUFF(
                       (
                           SELECT ', ' + CAST(map.UserRoleId AS NVARCHAR(11))
                           FROM dbo.EntityScreenMaps map
                           WHERE map.EntityScreenId = m.EntityScreenId
                           FOR XML PATH('')
                       ),
                       1,
                       2,
                       ''
                            ),
       DepartmentIdCsv = STUFF(
                         (
                             SELECT ', ' + CAST(map.DepartmentId AS NVARCHAR(11))
                             FROM dbo.EntityScreenMaps map
                             WHERE map.EntityScreenId = m.EntityScreenId
                             FOR XML PATH('')
                         ),
                         1,
                         2,
                         ''
                              )
FROM dbo.EntityScreens s
    JOIN dbo.EntityScreenItems i
        ON i.EntityScreenId = s.Id
    JOIN dbo.EntityScreenMaps m
        ON m.EntityScreenId = s.Id
    JOIN dbo.Entities e
        ON e.Id = i.EntityId;
--WHERE s.Id = i.EntityScreenId
--      AND s.Id = m.EntityScreenId
--      --AND s.EntityTypeId = 2
--      AND i.EntityId = e.Id;

GO

