CREATE TABLE [dbo].[AccountTransactionTypes] (
    [Id]                      INT             IDENTITY (25, 1) NOT NULL,
    [SortOrder]               INT             NOT NULL,
    [SourceAccountTypeId]     INT             NOT NULL,
    [TargetAccountTypeId]     INT             NOT NULL,
    [DefaultSourceAccountId]  INT             NOT NULL,
    [DefaultTargetAccountId]  INT             NOT NULL,
    [Name]                    NVARCHAR (4000) NULL,
    [IsForCashboxInteraction] BIT             NULL,
    [DefaultSourceTerminalId] INT             NULL,
    [DefaultTargetTerminalId] INT             NULL,
    CONSTRAINT [PK_dbo.AccountTransactionTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOAccountTransactionTypes]
   ON  [dbo].[AccountTransactionTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'AccountTransactionType'
    
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

