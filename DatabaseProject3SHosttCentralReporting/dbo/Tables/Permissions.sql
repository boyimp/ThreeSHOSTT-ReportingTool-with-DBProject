CREATE TABLE [dbo].[Permissions] (
    [Id]         INT             IDENTITY (52, 1) NOT NULL,
    [Value]      INT             NOT NULL,
    [UserRoleId] INT             NOT NULL,
    [Name]       NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.Permissions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Permissions_dbo.UserRoles_UserRoleId] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRoles] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_UserRoleId]
    ON [dbo].[Permissions]([UserRoleId] ASC);


GO


CREATE TRIGGER [dbo].[HOPermissions]
   ON  [dbo].[Permissions]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Permission'
    
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



CREATE TRIGGER [dbo].[TriggerPermissions]
   ON  [dbo].[Permissions]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO

