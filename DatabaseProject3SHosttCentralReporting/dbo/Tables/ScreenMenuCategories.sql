CREATE TABLE [dbo].[ScreenMenuCategories] (
    [Id]                    INT             IDENTITY (56, 1) NOT NULL,
    [Name]                  NVARCHAR (4000) NULL,
    [SortOrder]             INT             NOT NULL,
    [ScreenMenuId]          INT             NOT NULL,
    [MostUsedItemsCategory] BIT             NOT NULL,
    [ColumnCount]           INT             NOT NULL,
    [MenuItemButtonHeight]  INT             NOT NULL,
    [MenuItemButtonColor]   NVARCHAR (4000) NULL,
    [MenuItemFontSize]      FLOAT (53)      NOT NULL,
    [WrapText]              BIT             NOT NULL,
    [PageCount]             INT             NOT NULL,
    [MainButtonHeight]      INT             NOT NULL,
    [MainButtonColor]       NVARCHAR (4000) NULL,
    [MainFontSize]          FLOAT (53)      NOT NULL,
    [SubButtonHeight]       INT             NOT NULL,
    [NumeratorType]         INT             NOT NULL,
    [NumeratorValues]       NVARCHAR (4000) NULL,
    [AlphaButtonValues]     NVARCHAR (4000) NULL,
    [ImagePath]             NVARCHAR (4000) NULL,
    [MaxItems]              INT             NOT NULL,
    [Description]           NVARCHAR (4000) NULL,
    [ImageUrl]              NVARCHAR (4000) NULL,
    [ForeGroundColor]       NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.ScreenMenuCategories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ScreenMenuCategories_dbo.ScreenMenus_ScreenMenuId] FOREIGN KEY ([ScreenMenuId]) REFERENCES [dbo].[ScreenMenus] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_ScreenMenuId]
    ON [dbo].[ScreenMenuCategories]([ScreenMenuId] ASC);


GO


CREATE TRIGGER [dbo].[HOScreenMenuCategories]
   ON  [dbo].[ScreenMenuCategories]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'ScreenMenuCategory'
    
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

CREATE TRIGGER [dbo].[TriggerSMC]
   ON  dbo.ScreenMenuCategories 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
SET NOCOUNT ON;
delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END
IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TriggerMI]'))
DROP TRIGGER [dbo].[TriggerMI];

GO

