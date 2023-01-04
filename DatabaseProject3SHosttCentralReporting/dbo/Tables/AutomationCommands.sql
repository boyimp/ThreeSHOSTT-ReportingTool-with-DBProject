CREATE TABLE [dbo].[AutomationCommands] (
    [Id]                INT             IDENTITY (8, 1) NOT NULL,
    [ButtonHeader]      NVARCHAR (4000) NULL,
    [Color]             NVARCHAR (4000) NULL,
    [Values]            NVARCHAR (4000) NULL,
    [ToggleValues]      BIT             NOT NULL,
    [SortOrder]         INT             NOT NULL,
    [Name]              NVARCHAR (4000) NULL,
    [PasswordProtected] BIT             CONSTRAINT [DF_AutomationCommands_PasswordProtected] DEFAULT ((0)) NOT NULL,
    [NeedToTagReason]   BIT             CONSTRAINT [DF_AutomationCommands_NeedToTagReason] DEFAULT ((0)) NOT NULL,
    [ExecuteOnce]       BIT             CONSTRAINT [DF_AutomationCommands_ExecuteOnce] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.AutomationCommands] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOAutomationCommands]
   ON  [dbo].[AutomationCommands]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'AutomationCommand'
    
DECLARE OutletsCursor CURSOR LOCAL FOR 
    
        SELECT
        Id from SyncOutlets
        
        OPEN OutletsCursor        
        FETCH NEXT FROM OutletsCursor INTO 
          @OutletId
          
          
        WHILE @@FETCH_STATUS = 0
        BEGIN          
         
         IF EXISTS(SELECT * FROM SyncLogBasicData WHERE OutletId = @OutletId AND BasicDataName = @BasicDataName)
         	BEGIN
         		UPDATE SyncLogBasicData SET Synced = 0 , Synced2 = 0 
         		WHERE OutletId=@OutletId AND BasicDataName = @BasicDataName 
         	END
       

         FETCH NEXT FROM OutletsCursor INTO 
         	@OutletId
    
          
        END
        CLOSE OutletsCursor
        DEALLOCATE OutletsCursor
 END

GO



CREATE TRIGGER [dbo].[TriggerAutomationCommands]
   ON  [dbo].[AutomationCommands]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO

