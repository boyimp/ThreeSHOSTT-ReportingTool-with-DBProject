CREATE TABLE [dbo].[EntityScreens] (
    [Id]              INT             IDENTITY (7, 1) NOT NULL,
    [TicketTypeId]    INT             NOT NULL,
    [EntityTypeId]    INT             NOT NULL,
    [SortOrder]       INT             NOT NULL,
    [DisplayMode]     INT             NOT NULL,
    [BackgroundColor] NVARCHAR (4000) NULL,
    [BackgroundImage] NVARCHAR (4000) NULL,
    [FontSize]        INT             NOT NULL,
    [PageCount]       INT             NOT NULL,
    [RowCount]        INT             NOT NULL,
    [ColumnCount]     INT             NOT NULL,
    [ButtonHeight]    INT             NOT NULL,
    [DisplayState]    NVARCHAR (4000) NULL,
    [StateFilter]     NVARCHAR (4000) NULL,
    [AskTicketType]   BIT             NOT NULL,
    [Name]            NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.EntityScreens] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE TRIGGER [dbo].[TriggerEntityScreens]
   ON  [dbo].[EntityScreens]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END
ALTER TABLE [dbo].[EntityScreens] ENABLE TRIGGER [TriggerEntityScreens];

GO


CREATE TRIGGER [dbo].[HOEntityScreens]
   ON  [dbo].[EntityScreens]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'EntityScreen'
    
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

