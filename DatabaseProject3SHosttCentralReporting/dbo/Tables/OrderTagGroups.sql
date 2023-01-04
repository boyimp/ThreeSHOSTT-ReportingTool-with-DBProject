CREATE TABLE [dbo].[OrderTagGroups] (
    [Id]                      INT             IDENTITY (3, 1) NOT NULL,
    [SortOrder]               INT             NOT NULL,
    [ColumnCount]             INT             NOT NULL,
    [ButtonHeight]            INT             NOT NULL,
    [MaxSelectedItems]        INT             NOT NULL,
    [MinSelectedItems]        INT             NOT NULL,
    [AddTagPriceToOrderPrice] BIT             NOT NULL,
    [FreeTagging]             BIT             NOT NULL,
    [SaveFreeTags]            BIT             NOT NULL,
    [GroupTag]                NVARCHAR (4000) NULL,
    [Name]                    NVARCHAR (4000) NULL,
    [AutomationCommandTag]    BIT             CONSTRAINT [DF_OrderTagGroups_AutomationCommandTag] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.OrderTagGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOOrderTagGroups]
   ON  [dbo].[OrderTagGroups]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'OrderTagGroup'
    
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


CREATE TRIGGER [dbo].[TriggerOTG]
   ON  [dbo].[OrderTagGroups]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	
	
	SET NOCOUNT ON;

	delete from dbo.HHTSync;
	insert into dbo.HHTSync Values(1);
END

GO

