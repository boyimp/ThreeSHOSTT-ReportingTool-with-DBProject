CREATE TABLE [dbo].[AccountTransactionDocumentTypeAccountTransactionTypes] (
    [AccountTransactionDocumentType_Id] INT NOT NULL,
    [AccountTransactionType_Id]         INT NOT NULL,
    CONSTRAINT [PK_dbo.AccountTransactionDocumentTypeAccountTransactionTypes] PRIMARY KEY CLUSTERED ([AccountTransactionDocumentType_Id] ASC, [AccountTransactionType_Id] ASC),
    CONSTRAINT [FK_dbo.AccountTransactionDocumentTypeAccountTransactionTypes_dbo.AccountTransactionDocumentTypes_AccountTransactionDocumentType_] FOREIGN KEY ([AccountTransactionDocumentType_Id]) REFERENCES [dbo].[AccountTransactionDocumentTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AccountTransactionDocumentTypeAccountTransactionTypes_dbo.AccountTransactionTypes_AccountTransactionType_Id] FOREIGN KEY ([AccountTransactionType_Id]) REFERENCES [dbo].[AccountTransactionTypes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionDocumentType_Id]
    ON [dbo].[AccountTransactionDocumentTypeAccountTransactionTypes]([AccountTransactionDocumentType_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionType_Id]
    ON [dbo].[AccountTransactionDocumentTypeAccountTransactionTypes]([AccountTransactionType_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOAccountTransactionDocumentTypeAccountTransactionTypes]
   ON  [dbo].[AccountTransactionDocumentTypeAccountTransactionTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'AccountTransactionDocumentType'
    
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

