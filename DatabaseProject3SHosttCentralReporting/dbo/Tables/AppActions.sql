CREATE TABLE [dbo].[AppActions] (
    [Id]         INT             IDENTITY (12, 1) NOT NULL,
    [ActionType] NVARCHAR (4000) NULL,
    [Parameter]  NTEXT           NULL,
    [SortOrder]  INT             NOT NULL,
    [Name]       NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.AppActions] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOAppActions]
   ON  [dbo].[AppActions]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'AppAction'
    
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

CREATE TRIGGER [dbo].[TriggerAppActions]
   ON  [dbo].[AppActions]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END
ALTER TABLE [dbo].[AppActions] ENABLE TRIGGER [TriggerAppActions];

GO

