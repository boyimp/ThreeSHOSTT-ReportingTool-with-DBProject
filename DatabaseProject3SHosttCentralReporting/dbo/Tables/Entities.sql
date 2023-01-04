CREATE TABLE [dbo].[Entities] (
    [Id]             INT             IDENTITY (116, 1) NOT NULL,
    [EntityTypeId]   INT             NOT NULL,
    [LastUpdateTime] DATETIME        NOT NULL,
    [SearchString]   NVARCHAR (4000) NULL,
    [CustomData]     NVARCHAR (MAX)  NULL,
    [AccountId]      INT             NOT NULL,
    [WarehouseId]    INT             NOT NULL,
    [Name]           NVARCHAR (4000) NULL,
    [ZoneId]         INT             CONSTRAINT [DF_Entities_ZoneId] DEFAULT ((0)) NOT NULL,
    [CreationDate]   DATETIME        CONSTRAINT [DF_Entities_CreationDate] DEFAULT (getdate()) NULL,
    [MasterDataID]   INT             NULL,
    CONSTRAINT [PK_dbo.Entities] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOEntities]
   ON  [dbo].[Entities]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Entity'
    
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


CREATE TRIGGER [dbo].[TriggerE]
   ON  [dbo].[Entities]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);

END

GO

