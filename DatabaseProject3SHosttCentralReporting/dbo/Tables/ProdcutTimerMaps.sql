CREATE TABLE [dbo].[ProdcutTimerMaps] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [ProductTimerId]    INT             NOT NULL,
    [MenuItemGroupCode] NVARCHAR (4000) NULL,
    [MenuItemId]        INT             NOT NULL,
    [TerminalId]        INT             NOT NULL,
    [DepartmentId]      INT             NOT NULL,
    [UserRoleId]        INT             NOT NULL,
    [TicketTypeId]      INT             NOT NULL,
    CONSTRAINT [PK_dbo.ProdcutTimerMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProdcutTimerMaps_dbo.ProductTimers_ProductTimerId] FOREIGN KEY ([ProductTimerId]) REFERENCES [dbo].[ProductTimers] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_ProductTimerId]
    ON [dbo].[ProdcutTimerMaps]([ProductTimerId] ASC);


GO


CREATE TRIGGER [dbo].[HOProdcutTimerMaps]
   ON  [dbo].[ProdcutTimerMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'ProdcutTimerMap'
    
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

