CREATE TABLE [dbo].[ActionContainers] (
    [Id]               INT             IDENTITY (34, 1) NOT NULL,
    [AppActionId]      INT             NOT NULL,
    [AppRuleId]        INT             NOT NULL,
    [Name]             NVARCHAR (4000) NULL,
    [ParameterValues]  NTEXT           NULL,
    [CustomConstraint] NVARCHAR (4000) NULL,
    [SortOrder]        INT             NOT NULL,
    CONSTRAINT [PK_dbo.ActionContainers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ActionContainers_dbo.AppRules_AppRuleId] FOREIGN KEY ([AppRuleId]) REFERENCES [dbo].[AppRules] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_AppRuleId]
    ON [dbo].[ActionContainers]([AppRuleId] ASC);


GO



CREATE TRIGGER [dbo].[TriggerActionContainers]
   ON  [dbo].[ActionContainers]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO


CREATE TRIGGER [dbo].[HOActionContainers]
   ON  [dbo].[ActionContainers]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'AppRule'
    
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

