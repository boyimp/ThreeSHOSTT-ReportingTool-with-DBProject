CREATE TABLE [dbo].[ProductTimers] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [PriceType]     INT             NOT NULL,
    [PriceDuration] NUMERIC (16, 2) NOT NULL,
    [MinTime]       NUMERIC (16, 2) NOT NULL,
    [TimeRounding]  NUMERIC (16, 2) NOT NULL,
    [Name]          NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.ProductTimers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOProductTimers]
   ON  [dbo].[ProductTimers]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'ProductTimer'
    
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

