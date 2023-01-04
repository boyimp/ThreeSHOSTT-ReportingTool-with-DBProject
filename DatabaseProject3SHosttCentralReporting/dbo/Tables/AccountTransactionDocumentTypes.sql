CREATE TABLE [dbo].[AccountTransactionDocumentTypes] (
    [Id]                      INT             IDENTITY (13, 1) NOT NULL,
    [ButtonHeader]            NVARCHAR (4000) NULL,
    [ButtonColor]             NVARCHAR (4000) NULL,
    [MasterAccountTypeId]     INT             NOT NULL,
    [DefaultAmount]           NVARCHAR (4000) NULL,
    [DescriptionTemplate]     NVARCHAR (4000) NULL,
    [ExchangeTemplate]        NVARCHAR (4000) NULL,
    [BatchCreateDocuments]    BIT             NOT NULL,
    [Filter]                  INT             NOT NULL,
    [SortOrder]               INT             NOT NULL,
    [Name]                    NVARCHAR (4000) NULL,
    [IsForCashboxInteraction] BIT             NULL,
    CONSTRAINT [PK_dbo.AccountTransactionDocumentTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOAccountTransactionDocumentTypes]
   ON  [dbo].[AccountTransactionDocumentTypes]
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

