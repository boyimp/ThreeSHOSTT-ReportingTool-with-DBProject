CREATE TABLE [dbo].[EntityScreenItems] (
    [Id]             INT             IDENTITY (138, 1) NOT NULL,
    [EntityScreenId] INT             NOT NULL,
    [Name]           NVARCHAR (4000) NULL,
    [EntityId]       INT             NOT NULL,
    [EntityState]    NVARCHAR (4000) NULL,
    [SortOrder]      INT             NOT NULL,
    [LastUpdateTime] DATETIME        NOT NULL,
    CONSTRAINT [PK_dbo.EntityScreenItems] PRIMARY KEY CLUSTERED ([Id] ASC, [EntityScreenId] ASC),
    CONSTRAINT [FK_dbo.EntityScreenItems_dbo.EntityScreens_EntityScreenId] FOREIGN KEY ([EntityScreenId]) REFERENCES [dbo].[EntityScreens] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_EntityScreenId]
    ON [dbo].[EntityScreenItems]([EntityScreenId] ASC);


GO

CREATE TRIGGER [dbo].[TriggerEntityScreenItems]
   ON  [dbo].[EntityScreenItems]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);

END
ALTER TABLE [dbo].[EntityScreenItems] ENABLE TRIGGER [TriggerEntityScreenItems];

GO


CREATE TRIGGER [dbo].[HOEntityScreenItems]
   ON  [dbo].[EntityScreenItems]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'EntityScreenItem'
    
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

