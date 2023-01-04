CREATE TABLE [dbo].[MenuItems] (
    [Id]            INT             IDENTITY (357, 1) NOT NULL,
    [GroupCode]     NVARCHAR (4000) NULL,
    [Barcode]       NVARCHAR (4000) NULL,
    [Tag]           NVARCHAR (4000) NULL,
    [Name]          NVARCHAR (4000) NULL,
    [StarID]        INT             NULL,
    [PositionValue] INT             CONSTRAINT [DF_MenuItems_PositionValue] DEFAULT ((0)) NOT NULL,
    [MetaTagIds]    VARCHAR (MAX)   NULL,
    [MetaTagsJSON]  NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.MenuItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOMenuItems]
   ON  [dbo].[MenuItems]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'MenuItem'
    
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

