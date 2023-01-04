CREATE TABLE [dbo].[InventoryTransactionDocumentTypes] (
    [Id]                          INT             IDENTITY (2, 1) NOT NULL,
    [SourceEntityTypeId]          INT             NOT NULL,
    [TargetEntityTypeId]          INT             NOT NULL,
    [DefaultSourceEntityId]       INT             NOT NULL,
    [DefaultTargetEntityId]       INT             NOT NULL,
    [SortOrder]                   INT             NOT NULL,
    [Name]                        NVARCHAR (4000) NULL,
    [AccountTransactionType_Id]   INT             NULL,
    [InventoryTransactionType_Id] INT             NULL,
    CONSTRAINT [PK_dbo.InventoryTransactionDocumentTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.InventoryTransactionDocumentTypes_dbo.AccountTransactionTypes_AccountTransactionType_Id] FOREIGN KEY ([AccountTransactionType_Id]) REFERENCES [dbo].[AccountTransactionTypes] ([Id]),
    CONSTRAINT [FK_dbo.InventoryTransactionDocumentTypes_dbo.InventoryTransactionTypes_InventoryTransactionType_Id] FOREIGN KEY ([InventoryTransactionType_Id]) REFERENCES [dbo].[InventoryTransactionTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_InventoryTransactionType_Id]
    ON [dbo].[InventoryTransactionDocumentTypes]([InventoryTransactionType_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionType_Id]
    ON [dbo].[InventoryTransactionDocumentTypes]([AccountTransactionType_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOInventoryTransactionDocumentTypes]
   ON  [dbo].[InventoryTransactionDocumentTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'InventoryTransactionDocumentType'
    
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

