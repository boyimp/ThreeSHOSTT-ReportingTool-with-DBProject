CREATE TABLE [dbo].[EntityTypes] (
    [Id]                  INT             IDENTITY (5, 1) NOT NULL,
    [SortOrder]           INT             NOT NULL,
    [EntityName]          NVARCHAR (4000) NULL,
    [AccountTypeId]       INT             NOT NULL,
    [WarehouseTypeId]     INT             NOT NULL,
    [AccountNameTemplate] NVARCHAR (4000) NULL,
    [Name]                NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.EntityTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[TriggerET]
   ON  [dbo].[EntityTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;

	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO


CREATE TRIGGER [dbo].[HOEntityTypes]
   ON  [dbo].[EntityTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'EntityType'
    
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

