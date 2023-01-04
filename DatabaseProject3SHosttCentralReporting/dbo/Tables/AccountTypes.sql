CREATE TABLE [dbo].[AccountTypes] (
    [Id]                INT             IDENTITY (15, 1) NOT NULL,
    [DefaultFilterType] INT             NOT NULL,
    [WorkingRule]       INT             NOT NULL,
    [SortOrder]         INT             NOT NULL,
    [Tags]              NVARCHAR (4000) NULL,
    [Name]              NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.AccountTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOAccountTypes]
   ON  [dbo].[AccountTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max),
 	@BasicDataName2 varchar(max)
 	
SET @BasicDataName = 'Account'
SET @BasicDataName2 ='AccountType'
    
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
          
         	
         IF EXISTS(SELECT * FROM SyncLogBasicData WHERE OutletId = @OutletId AND BasicDataName = @BasicDataName2)
         	BEGIN
         		UPDATE SyncLogBasicData SET Synced = 0 , Synced2 = 0 
         		WHERE OutletId=@OutletId AND BasicDataName = @BasicDataName2 
         	END
 
 
         FETCH NEXT FROM OutletsCursor INTO 
         	@OutletId
    
          
        END
        CLOSE OutletsCursor
        DEALLOCATE OutletsCursor
 END

GO

