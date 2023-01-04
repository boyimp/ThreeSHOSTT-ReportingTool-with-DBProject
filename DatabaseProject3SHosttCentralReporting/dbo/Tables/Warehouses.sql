CREATE TABLE [dbo].[Warehouses] (
    [Id]                  INT             IDENTITY (3, 1) NOT NULL,
    [WarehouseTypeId]     INT             NOT NULL,
    [SortOrder]           INT             NOT NULL,
    [Name]                NVARCHAR (4000) NULL,
    [MasterDataID]        INT             NULL,
    [MetaTagIds]          VARCHAR (MAX)   NULL,
    [InventoryMetaTagIds] VARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.Warehouses] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOWarehouses]
   ON  [dbo].[Warehouses]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Warehouse'
    
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

