CREATE VIEW  [hostt_DosaExpressCentral].HHTEntityPermission AS  
SELECT distinct e.Id EntityId, e.Name EntityName, e.EntityTypeId , UserRoleIdCsv = STUFF((SELECT ', ' + CAST(map.UserRoleId AS NVARCHAR(10))
            FROM EntityScreenMaps map 
            WHERE map.EntityScreenId = m.EntityScreenId            
           FOR XML PATH('')), 1, 2, ''),

           DepartmentIdCsv = STUFF((SELECT ', ' + CAST(map.DepartmentId AS NVARCHAR(10))
            FROM EntityScreenMaps map 
            WHERE map.EntityScreenId = m.EntityScreenId            
           FOR XML PATH('')), 1, 2, '')
FROM EntityScreens s, EntityScreenItems i, EntityScreenMaps m, Entities e
WHERE s.Id = i.EntityScreenId AND s.Id = m.EntityScreenId
--AND s.EntityTypeId = 2
AND i.EntityId = e.Id

GO

