CREATE TABLE [dbo].[AccountScreenMaps] (
    [Id]              INT IDENTITY (7, 1) NOT NULL,
    [AccountScreenId] INT NOT NULL,
    [TerminalId]      INT NOT NULL,
    [DepartmentId]    INT NOT NULL,
    [UserRoleId]      INT NOT NULL,
    [TicketTypeId]    INT NOT NULL,
    CONSTRAINT [PK_dbo.AccountScreenMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountScreenMaps_dbo.AccountScreens_AccountScreenId] FOREIGN KEY ([AccountScreenId]) REFERENCES [dbo].[AccountScreens] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountScreenId]
    ON [dbo].[AccountScreenMaps]([AccountScreenId] ASC);


GO


CREATE TRIGGER [dbo].[HOAccountScreenMaps]
   ON  [dbo].[AccountScreenMaps]
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

