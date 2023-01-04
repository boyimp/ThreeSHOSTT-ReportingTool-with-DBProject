CREATE TABLE [dbo].[PrinterMaps] (
    [Id]                INT             IDENTITY (3, 1) NOT NULL,
    [PrintJobId]        INT             NOT NULL,
    [MenuItemGroupCode] NVARCHAR (4000) NULL,
    [MenuItemId]        INT             NOT NULL,
    [PrinterId]         INT             NOT NULL,
    [PrinterTemplateId] INT             NOT NULL,
    CONSTRAINT [PK_dbo.PrinterMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PrinterMaps_dbo.PrintJobs_PrintJobId] FOREIGN KEY ([PrintJobId]) REFERENCES [dbo].[PrintJobs] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_PrintJobId]
    ON [dbo].[PrinterMaps]([PrintJobId] ASC);


GO


CREATE TRIGGER [dbo].[HOPrinterMaps]
   ON  [dbo].[PrinterMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'PrinterMap'
    
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

