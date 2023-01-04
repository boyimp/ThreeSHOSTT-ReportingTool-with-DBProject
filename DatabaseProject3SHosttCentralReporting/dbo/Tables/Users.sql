CREATE TABLE [dbo].[Users] (
    [Id]                INT             IDENTITY (6, 1) NOT NULL,
    [PinCode]           NVARCHAR (4000) NULL,
    [Name]              NVARCHAR (4000) NULL,
    [UserRole_Id]       INT             NULL,
    [SmartStoreUserId]  NVARCHAR (4000) NULL,
    [SmartStorePinCode] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Users_dbo.UserRoles_UserRole_Id] FOREIGN KEY ([UserRole_Id]) REFERENCES [dbo].[UserRoles] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_UserRole_Id]
    ON [dbo].[Users]([UserRole_Id] ASC);


GO



CREATE TRIGGER [dbo].[TriggerUsers]
   ON  [dbo].[Users]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO


CREATE TRIGGER [dbo].[HOUsers]
   ON  [dbo].[Users]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'User'
    
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

