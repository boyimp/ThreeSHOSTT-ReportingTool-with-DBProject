CREATE TABLE [dbo].[AccountScreenValues] (
    [Id]               INT             IDENTITY (45, 1) NOT NULL,
    [AccountTypeId]    INT             NOT NULL,
    [AccountTypeName]  NVARCHAR (4000) NULL,
    [DisplayDetails]   BIT             NOT NULL,
    [SortOrder]        INT             NOT NULL,
    [AccountScreen_Id] INT             NULL,
    CONSTRAINT [PK_dbo.AccountScreenValues] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountScreenValues_dbo.AccountScreens_AccountScreen_Id] FOREIGN KEY ([AccountScreen_Id]) REFERENCES [dbo].[AccountScreens] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountScreen_Id]
    ON [dbo].[AccountScreenValues]([AccountScreen_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOAccountScreenValues]
   ON  [dbo].[AccountScreenValues]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'AccountScreen'
    
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

