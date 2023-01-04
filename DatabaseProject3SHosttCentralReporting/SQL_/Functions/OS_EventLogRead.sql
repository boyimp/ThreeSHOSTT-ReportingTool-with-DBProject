CREATE FUNCTION [SQL#].[OS_EventLogRead]
(@LogName NVARCHAR (4000) NULL, @MachineName NVARCHAR (4000) NULL, @Source NVARCHAR (4000) NULL, @EntryType NVARCHAR (4000) NULL, @InstanceID NVARCHAR (4000) NULL, @Category NVARCHAR (4000) NULL, @UserName NVARCHAR (4000) NULL, @Message NVARCHAR (4000) NULL, @TimeGeneratedBegin DATETIME NULL, @TimeGeneratedEnd DATETIME NULL, @IndexBegin INT NULL, @IndexEnd INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS 
     TABLE (
        [Index]         INT             NULL,
        [Category]      NVARCHAR (500)  NULL,
        [EntryType]     NVARCHAR (50)   NULL,
        [InstanceId]    BIGINT          NULL,
        [Source]        NVARCHAR (500)  NULL,
        [TimeGenerated] DATETIME        NULL,
        [TimeWritten]   DATETIME        NULL,
        [UserName]      NVARCHAR (100)  NULL,
        [Message]       NVARCHAR (MAX)  NULL,
        [Data]          VARBINARY (MAX) NULL)
AS
 EXTERNAL NAME [SQL#.OS].[OS].[EventLogRead]


GO

