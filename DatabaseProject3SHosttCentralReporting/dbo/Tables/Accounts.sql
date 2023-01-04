CREATE TABLE [dbo].[Accounts] (
    [Id]                INT             IDENTITY (45, 1) NOT NULL,
    [AccountTypeId]     INT             NOT NULL,
    [ForeignCurrencyId] INT             NOT NULL,
    [Name]              NVARCHAR (4000) NULL,
    [MasterAccountId]   INT             NULL,
    CONSTRAINT [PK_dbo.Accounts] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOAccounts]
   ON  [dbo].[Accounts]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Account'
    
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

