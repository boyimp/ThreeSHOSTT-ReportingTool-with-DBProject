CREATE PROCEDURE [SQL#].[SQLsharp_GrantPermissions]
@GrantTo NVARCHAR (4000) NULL, @PrintSqlInsteadOfExecute BIT NULL=0
AS EXTERNAL NAME [SQL#].[SQLsharp].[GrantPermissions]


GO

