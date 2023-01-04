CREATE TABLE [dbo].[Printers] (
    [Id]           INT             IDENTITY (4, 1) NOT NULL,
    [ShareName]    NVARCHAR (4000) NULL,
    [PrinterType]  INT             NOT NULL,
    [CodePage]     INT             NOT NULL,
    [CharsPerLine] INT             NOT NULL,
    [PageHeight]   INT             NOT NULL,
    [Name]         NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.Printers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOPrinters]
   ON  [dbo].[Printers]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Printer'
    
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

