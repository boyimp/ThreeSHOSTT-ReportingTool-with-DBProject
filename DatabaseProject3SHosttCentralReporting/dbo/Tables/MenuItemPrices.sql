CREATE TABLE [dbo].[MenuItemPrices] (
    [Id]                INT             IDENTITY (391, 1) NOT NULL,
    [MenuItemPortionId] INT             NOT NULL,
    [PriceTag]          NVARCHAR (10)   NULL,
    [Price]             DECIMAL (16, 4) NULL,
    CONSTRAINT [PK_dbo.MenuItemPrices] PRIMARY KEY CLUSTERED ([Id] ASC, [MenuItemPortionId] ASC),
    CONSTRAINT [FK_dbo.MenuItemPrices_dbo.MenuItemPortions_MenuItemPortionId] FOREIGN KEY ([MenuItemPortionId]) REFERENCES [dbo].[MenuItemPortions] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_MenuItemPortionId]
    ON [dbo].[MenuItemPrices]([MenuItemPortionId] ASC);


GO


CREATE TRIGGER [dbo].[HOMenuItemPrices]
   ON  [dbo].[MenuItemPrices]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'MenuItemPrice'
    
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


CREATE TRIGGER [dbo].[TriggerMIPr]
   ON  [dbo].[MenuItemPrices]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	
	
	SET NOCOUNT ON;

	delete from dbo.HHTSync;
	insert into dbo.HHTSync Values(1);
END

GO

