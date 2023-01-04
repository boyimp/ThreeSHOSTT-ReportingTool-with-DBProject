CREATE TABLE [dbo].[SyncOutlets] (
    [Id]                          INT             IDENTITY (1, 1) NOT NULL,
    [Name]                        NVARCHAR (4000) NULL,
    [LastOutletWorkPeriodStarted] DATETIME        NULL,
    [LastOutletWorkPeriodEnded]   DATETIME        NULL,
    [LastSyncedOutletTime]        DATETIME        NULL,
    [LastSyncedServerTimeUTC]     DATETIME        NULL,
    [STARInterfaceWarehouse]      INT             NULL,
    [OutletInfo]                  VARCHAR (4000)  NULL,
    [LastFinalizedWorkPeriod]     DATETIME        NULL,
    [IsActive]                    SMALLINT        NULL,
    [MetaTagIds]                  VARCHAR (MAX)   NULL,
    [MetaTagsJSON]                NVARCHAR (MAX)  NULL,
    [MasterDataID]                INT             NULL,
    [IsCentral]                   BIT             CONSTRAINT [DF_SyncOutlets_IsCentral] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SyncOutlets] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOSyncOutlets]
   ON  [dbo].[SyncOutlets]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
IF UPDATE([Name]) OR UPDATE([OutletInfo]) OR UPDATE([STARInterfaceWarehouse]) or UPDATE([IsActive])
or NOT EXISTS (select Id from SyncOutlets where Id in (select deleted.Id from deleted))
Begin
	DECLARE  
		@OutletId int,
 		@BasicDataName varchar(max)
 	
	SET @BasicDataName = 'SyncOutlet'
    
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
 End

GO

