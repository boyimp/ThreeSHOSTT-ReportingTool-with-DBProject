CREATE TABLE [dbo].[Departments] (
    [Id]                      INT             IDENTITY (3, 1) NOT NULL,
    [SortOrder]               INT             NOT NULL,
    [PriceTag]                NVARCHAR (10)   NULL,
    [WarehouseId]             INT             NOT NULL,
    [TicketTypeId]            INT             NOT NULL,
    [TicketCreationMethod]    INT             NOT NULL,
    [Name]                    NVARCHAR (4000) NULL,
    [LastSyncTimeBasicData]   DATETIME        NULL,
    [LastSyncTimeTransaction] DATETIME        NULL,
    [Tag]                     NVARCHAR (4000) NULL,
    [ButtonColor]             NVARCHAR (4000) NULL,
    [MetaTagIds]              VARCHAR (MAX)   NULL,
    [MetaTagsJSON]            NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Departments] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HODepartments]
   ON  [dbo].[Departments]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Department'
    
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



CREATE TRIGGER [dbo].[TriggerDepartments]
   ON  [dbo].[Departments]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO

