CREATE TABLE [dbo].[EntityScreenMaps] (
    [Id]             INT IDENTITY (8, 1) NOT NULL,
    [EntityScreenId] INT NOT NULL,
    [TerminalId]     INT NOT NULL,
    [DepartmentId]   INT NOT NULL,
    [UserRoleId]     INT NOT NULL,
    [TicketTypeId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.EntityScreenMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EntityScreenMaps_dbo.EntityScreens_EntityScreenId] FOREIGN KEY ([EntityScreenId]) REFERENCES [dbo].[EntityScreens] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_EntityScreenId]
    ON [dbo].[EntityScreenMaps]([EntityScreenId] ASC);


GO


CREATE TRIGGER [dbo].[HOEntityScreenMaps]
   ON  [dbo].[EntityScreenMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'EntityScreenMap'
    
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

CREATE TRIGGER [dbo].[TriggerEntityScreenMaps]
   ON  [dbo].[EntityScreenMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END
ALTER TABLE [dbo].[EntityScreenMaps] ENABLE TRIGGER [TriggerEntityScreenMaps];

GO

