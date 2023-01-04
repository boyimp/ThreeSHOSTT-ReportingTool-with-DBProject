CREATE TABLE [dbo].[PrinterTemplates] (
    [Id]                              INT             IDENTITY (3, 1) NOT NULL,
    [Template]                        NTEXT           NULL,
    [MergeLines]                      BIT             NOT NULL,
    [Name]                            NVARCHAR (4000) NULL,
    [XtraTemplate]                    NVARCHAR (MAX)  NULL,
    [MapToTicketsModulesPrintCommand] BIT             NULL,
    CONSTRAINT [PK_dbo.PrinterTemplates] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOPrinterTemplates]
   ON  [dbo].[PrinterTemplates]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'PrinterTemplate'
    
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

