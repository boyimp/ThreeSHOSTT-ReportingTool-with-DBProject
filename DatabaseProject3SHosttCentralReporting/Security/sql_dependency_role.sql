CREATE ROLE [sql_dependency_role]
    AUTHORIZATION [dbo];


GO

ALTER ROLE [sql_dependency_role] ADD MEMBER [DECSync];


GO

