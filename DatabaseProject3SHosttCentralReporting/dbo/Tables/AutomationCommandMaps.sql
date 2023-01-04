CREATE TABLE [dbo].[AutomationCommandMaps] (
    [Id]                  INT             IDENTITY (8, 1) NOT NULL,
    [AutomationCommandId] INT             NOT NULL,
    [DisplayOnTicket]     BIT             NOT NULL,
    [DisplayOnPayment]    BIT             NOT NULL,
    [DisplayOnOrders]     BIT             NOT NULL,
    [EnabledStates]       NVARCHAR (4000) NULL,
    [VisibleStates]       NVARCHAR (4000) NULL,
    [TerminalId]          INT             NOT NULL,
    [DepartmentId]        INT             NOT NULL,
    [UserRoleId]          INT             NOT NULL,
    [TicketTypeId]        INT             NOT NULL,
    [DisplayOnKitchen]    BIT             CONSTRAINT [DF_AutomationCommandMaps_DisplayOnKitchen] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.AutomationCommandMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AutomationCommandMaps_dbo.AutomationCommands_AutomationCommandId] FOREIGN KEY ([AutomationCommandId]) REFERENCES [dbo].[AutomationCommands] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_AutomationCommandId]
    ON [dbo].[AutomationCommandMaps]([AutomationCommandId] ASC);


GO



CREATE TRIGGER [dbo].[TriggerAutomationCommandMaps]
   ON  [dbo].[AutomationCommandMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO


CREATE TRIGGER [dbo].[HOAutomationCommandMaps]
   ON  [dbo].[AutomationCommandMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'AutomationCommand'
    
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

