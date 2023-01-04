CREATE TABLE [dbo].[RecipeItems] (
    [Id]               INT             IDENTITY (87, 1) NOT NULL,
    [RecipeId]         INT             NOT NULL,
    [Quantity]         NUMERIC (16, 3) NOT NULL,
    [InventoryItem_Id] INT             NULL,
    CONSTRAINT [PK_dbo.RecipeItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.RecipeItems_dbo.InventoryItems_InventoryItem_Id] FOREIGN KEY ([InventoryItem_Id]) REFERENCES [dbo].[InventoryItems] ([Id]),
    CONSTRAINT [FK_dbo.RecipeItems_dbo.Recipes_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [dbo].[Recipes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_RecipeId]
    ON [dbo].[RecipeItems]([RecipeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_InventoryItem_Id]
    ON [dbo].[RecipeItems]([InventoryItem_Id] ASC);


GO


CREATE TRIGGER [dbo].[HORecipeItems]
   ON  [dbo].[RecipeItems]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'RecipeItem'
    
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

