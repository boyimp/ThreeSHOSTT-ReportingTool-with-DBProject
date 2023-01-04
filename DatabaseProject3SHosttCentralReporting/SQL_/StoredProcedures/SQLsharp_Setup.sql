CREATE PROCEDURE [SQL#].[SQLsharp_Setup]
@SQLsharpSchema [sysname] NULL='SQL#', @SQLsharpAssembly [sysname] NULL=NULL, @JustPrintSQL BIT NULL=0
AS EXTERNAL NAME [SQL#].[SQLsharp].[Setup]


GO

