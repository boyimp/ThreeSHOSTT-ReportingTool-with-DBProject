CREATE TABLE [dbo].[Widgets] (
    [Id]                  INT             IDENTITY (18, 1) NOT NULL,
    [EntityScreenId]      INT             NOT NULL,
    [XLocation]           INT             NOT NULL,
    [YLocation]           INT             NOT NULL,
    [Height]              INT             NOT NULL,
    [Width]               INT             NOT NULL,
    [CornerRadius]        INT             NOT NULL,
    [Angle]               FLOAT (53)      NOT NULL,
    [Scale]               FLOAT (53)      NOT NULL,
    [Properties]          NVARCHAR (4000) NULL,
    [CreatorName]         NVARCHAR (4000) NULL,
    [AutoRefresh]         BIT             NOT NULL,
    [AutoRefreshInterval] INT             NOT NULL,
    CONSTRAINT [PK_dbo.Widgets] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Widgets_dbo.EntityScreens_EntityScreenId] FOREIGN KEY ([EntityScreenId]) REFERENCES [dbo].[EntityScreens] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_EntityScreenId]
    ON [dbo].[Widgets]([EntityScreenId] ASC);


GO


CREATE TRIGGER [dbo].[HOWidgets]
   ON  [dbo].[Widgets]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Widget'
    
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

