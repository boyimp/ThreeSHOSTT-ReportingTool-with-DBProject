CREATE TABLE [dbo].[InventoryItems] (
    [Id]                        INT             IDENTITY (33, 1) NOT NULL,
    [GroupCode]                 NVARCHAR (4000) NULL,
    [BaseUnit]                  NVARCHAR (4000) NULL,
    [TransactionUnit]           NVARCHAR (4000) NULL,
    [TransactionUnitMultiplier] INT             NOT NULL,
    [AlarmThreshold]            INT             NOT NULL,
    [Name]                      NVARCHAR (4000) NULL,
    [StarID]                    INT             CONSTRAINT [DF_InventoryItems_StarID] DEFAULT ((0)) NOT NULL,
    [StarCode]                  NVARCHAR (4000) NULL,
    [InventoryTakeType]         NVARCHAR (4000) NULL,
    [STARBrand]                 NVARCHAR (4000) NULL,
    [STARVendor]                NVARCHAR (4000) NULL,
    [Barcode]                   NVARCHAR (4000) NULL,
    [IsIntermediate]            BIT             CONSTRAINT [DF_InventoryItems_IsIntermediate] DEFAULT ((0)) NOT NULL,
    [MetaTagIds]                VARCHAR (MAX)   NULL,
    [MetaTagsJSON]              NVARCHAR (MAX)  NULL,
    [isActive]                  BIT             CONSTRAINT [DF_InventoryItems_isActive] DEFAULT ((1)) NOT NULL,
    [PositionValue]             INT             CONSTRAINT [DF_InventoryItems_PositionValue] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.InventoryItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOInventoryItems]
   ON  [dbo].[InventoryItems]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'InventoryItem'
    
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

