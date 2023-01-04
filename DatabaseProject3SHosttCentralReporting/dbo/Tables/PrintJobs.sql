CREATE TABLE [dbo].[PrintJobs] (
    [Id]                     INT             IDENTITY (3, 1) NOT NULL,
    [WhatToPrint]            INT             NOT NULL,
    [UseForPaidTickets]      BIT             NOT NULL,
    [ExcludeTax]             BIT             NOT NULL,
    [Name]                   NVARCHAR (4000) NULL,
    [KitchenIndividualLines] BIT             CONSTRAINT [DF_PrintJobs_KitchenIndividualLines] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.PrintJobs] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOPrintJobs]
   ON  [dbo].[PrintJobs]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'PrintJob'
    
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

