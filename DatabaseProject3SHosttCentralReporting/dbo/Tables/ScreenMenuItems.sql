CREATE TABLE [dbo].[ScreenMenuItems] (
    [Id]                   INT             IDENTITY (402, 1) NOT NULL,
    [Name]                 NVARCHAR (4000) NULL,
    [ScreenMenuCategoryId] INT             NOT NULL,
    [MenuItemId]           INT             NOT NULL,
    [SortOrder]            INT             NOT NULL,
    [AutoSelect]           BIT             NOT NULL,
    [ButtonColor]          NVARCHAR (4000) NULL,
    [Quantity]             INT             NOT NULL,
    [ImagePath]            NVARCHAR (4000) NULL,
    [FontSize]             FLOAT (53)      NOT NULL,
    [SubMenuTag]           NVARCHAR (4000) NULL,
    [ItemPortion]          NVARCHAR (4000) NULL,
    [OrderTagTemplate_Id]  INT             NULL,
    [Description]          NVARCHAR (4000) NULL,
    [ImageUrl]             NVARCHAR (4000) NULL,
    [ImageUrlSmall]        NVARCHAR (MAX)  NULL,
    [IsBusy]               BIT             CONSTRAINT [DF_ScreenMenuItems_IsBusy] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.ScreenMenuItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ScreenMenuItems_dbo.OrderTagTemplates_OrderTagTemplate_Id] FOREIGN KEY ([OrderTagTemplate_Id]) REFERENCES [dbo].[OrderTagTemplates] ([Id]),
    CONSTRAINT [FK_dbo.ScreenMenuItems_dbo.ScreenMenuCategories_ScreenMenuCategoryId] FOREIGN KEY ([ScreenMenuCategoryId]) REFERENCES [dbo].[ScreenMenuCategories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ScreenMenuItems_MenuItems] FOREIGN KEY ([MenuItemId]) REFERENCES [dbo].[MenuItems] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_ScreenMenuCategoryId]
    ON [dbo].[ScreenMenuItems]([ScreenMenuCategoryId] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_OrderTagTemplate_Id]
    ON [dbo].[ScreenMenuItems]([OrderTagTemplate_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOScreenMenuItems]
   ON  [dbo].[ScreenMenuItems]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'ScreenMenuItem'
    
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

