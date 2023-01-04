CREATE TABLE [dbo].[ScreenMenus] (
    [Id]                  INT             IDENTITY (3, 1) NOT NULL,
    [Name]                NVARCHAR (4000) NULL,
    [CategoryColumnCount] INT             CONSTRAINT [DF_ScreenMenus_CategoryColumnCount] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbo.ScreenMenus] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO



CREATE TRIGGER [dbo].[TriggerScreenMenus]
   ON  [dbo].[ScreenMenus]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO


CREATE TRIGGER [dbo].[HOScreenMenus]
   ON  [dbo].[ScreenMenus]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'ScreenMenu'
    
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

