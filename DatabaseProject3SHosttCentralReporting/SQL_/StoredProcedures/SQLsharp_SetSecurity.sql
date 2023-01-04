CREATE PROCEDURE [SQL#].[SQLsharp_SetSecurity]
@PermissionSet INT NULL, @AssemblyName NVARCHAR (4000) NULL=NULL, @SetTrustworthyIfNoUser BIT NULL=0, @DoNotPrintSuccessMessage BIT NULL=0
AS EXTERNAL NAME [SQL#].[SQLsharp].[SetSecurity]


GO

