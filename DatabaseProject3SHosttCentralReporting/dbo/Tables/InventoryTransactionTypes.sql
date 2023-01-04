CREATE TABLE [dbo].[InventoryTransactionTypes] (
    [Id]                       INT             IDENTITY (3, 1) NOT NULL,
    [SourceWarehouseTypeId]    INT             NOT NULL,
    [TargetWarehouseTypeId]    INT             NOT NULL,
    [DefaultSourceWarehouseId] INT             NOT NULL,
    [DefaultTargetWarehouseId] INT             NOT NULL,
    [SortOrder]                INT             NOT NULL,
    [Name]                     NVARCHAR (4000) NULL,
    [IsProduction]             BIT             CONSTRAINT [DF_InventoryTransactionTypes_IsProduction] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.InventoryTransactionTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOInventoryTransactionTypes]
   ON  [dbo].[InventoryTransactionTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'InventoryTransactionType'
    
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

