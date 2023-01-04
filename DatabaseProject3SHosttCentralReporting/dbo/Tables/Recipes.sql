CREATE TABLE [dbo].[Recipes] (
    [Id]                           INT             IDENTITY (27, 1) NOT NULL,
    [FixedCost]                    NUMERIC (16, 2) NOT NULL,
    [Name]                         NVARCHAR (4000) NULL,
    [Portion_Id]                   INT             NULL,
    [IsIntermediate]               BIT             CONSTRAINT [DF_Recipes_IsIntermediate] DEFAULT ((0)) NOT NULL,
    [IntermediateInventoryItem_Id] INT             NULL,
    CONSTRAINT [PK_dbo.Recipes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Recipes_dbo.MenuItemPortions_Portion_Id] FOREIGN KEY ([Portion_Id]) REFERENCES [dbo].[MenuItemPortions] ([Id]),
    CONSTRAINT [FK_Recipes_InventoryItems] FOREIGN KEY ([IntermediateInventoryItem_Id]) REFERENCES [dbo].[InventoryItems] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_Portion_Id]
    ON [dbo].[Recipes]([Portion_Id] ASC);


GO


CREATE TRIGGER [dbo].[HORecipes]
   ON  [dbo].[Recipes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Recipe'
    
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

