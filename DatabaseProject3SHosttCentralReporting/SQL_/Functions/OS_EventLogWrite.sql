CREATE FUNCTION [SQL#].[OS_EventLogWrite]
(@LogName NVARCHAR (4000) NULL, @MachineName NVARCHAR (4000) NULL, @Source NVARCHAR (4000) NULL, @EntryType NVARCHAR (4000) NULL, @InstanceID INT NULL, @Category SMALLINT NULL, @Message NVARCHAR (MAX) NULL, @BinaryData VARBINARY (8000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#.OS].[OS].[EventLogWrite]


GO

