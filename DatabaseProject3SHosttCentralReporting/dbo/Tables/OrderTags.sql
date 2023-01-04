CREATE TABLE [dbo].[OrderTags] (
    [Id]              INT             IDENTITY (5, 1) NOT NULL,
    [Name]            NVARCHAR (4000) NULL,
    [SortOrder]       INT             NOT NULL,
    [OrderTagGroupId] INT             NOT NULL,
    [Price]           DECIMAL (16, 4) NULL,
    [MenuItemId]      INT             NOT NULL,
    [MaxQuantity]     INT             NOT NULL,
    CONSTRAINT [PK_dbo.OrderTags] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.OrderTags_dbo.OrderTagGroups_OrderTagGroupId] FOREIGN KEY ([OrderTagGroupId]) REFERENCES [dbo].[OrderTagGroups] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_OrderTagGroupId]
    ON [dbo].[OrderTags]([OrderTagGroupId] ASC);


GO


CREATE TRIGGER [dbo].[HOOrderTags]
   ON  [dbo].[OrderTags]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'OrderTag'
    
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


CREATE TRIGGER [dbo].[TriggerOT]
   ON  [dbo].[OrderTags]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	
	
	SET NOCOUNT ON;

	delete from dbo.HHTSync;
	insert into dbo.HHTSync Values(1);
END

GO

