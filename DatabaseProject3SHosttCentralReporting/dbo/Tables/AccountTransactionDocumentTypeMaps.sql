CREATE TABLE [dbo].[AccountTransactionDocumentTypeMaps] (
    [Id]                               INT IDENTITY (13, 1) NOT NULL,
    [AccountTransactionDocumentTypeId] INT NOT NULL,
    [TerminalId]                       INT NOT NULL,
    [DepartmentId]                     INT NOT NULL,
    [UserRoleId]                       INT NOT NULL,
    [TicketTypeId]                     INT NOT NULL,
    CONSTRAINT [PK_dbo.AccountTransactionDocumentTypeMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountTransactionDocumentTypeMaps_dbo.AccountTransactionDocumentTypes_AccountTransactionDocumentTypeId] FOREIGN KEY ([AccountTransactionDocumentTypeId]) REFERENCES [dbo].[AccountTransactionDocumentTypes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionDocumentTypeId]
    ON [dbo].[AccountTransactionDocumentTypeMaps]([AccountTransactionDocumentTypeId] ASC);


GO


CREATE TRIGGER [dbo].[HOAccountTransactionDocumentTypeMaps]
   ON  [dbo].[AccountTransactionDocumentTypeMaps]
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

