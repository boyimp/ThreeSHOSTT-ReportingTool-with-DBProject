CREATE PROCEDURE [SQL#].[SQLsharp_SaveManualToDisk]
@FilePath NVARCHAR (2000) NULL, @CreatePathIfNotExists BIT NULL=0
AS EXTERNAL NAME [SQL#_2].[SQLsharp].[SaveManualToDisk]


GO

