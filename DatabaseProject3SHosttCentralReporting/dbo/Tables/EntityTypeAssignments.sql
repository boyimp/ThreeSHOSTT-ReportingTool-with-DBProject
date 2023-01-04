CREATE TABLE [dbo].[EntityTypeAssignments] (
    [Id]                                INT             IDENTITY (9, 1) NOT NULL,
    [EntityTypeId]                      INT             NOT NULL,
    [EntityTypeName]                    NVARCHAR (4000) NULL,
    [AskBeforeCreatingTicket]           BIT             NOT NULL,
    [State]                             NVARCHAR (4000) NULL,
    [SortOrder]                         INT             NOT NULL,
    [TicketType_Id]                     INT             NULL,
    [MustSelectionBeforeCreatingTicket] BIT             CONSTRAINT [DF_EntityTypeAssignments_MustSelectionBeforeCreatingTicket] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.EntityTypeAssignments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EntityTypeAssignments_dbo.TicketTypes_TicketType_Id] FOREIGN KEY ([TicketType_Id]) REFERENCES [dbo].[TicketTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_TicketType_Id]
    ON [dbo].[EntityTypeAssignments]([TicketType_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOEntityTypeAssignments]
   ON  [dbo].[EntityTypeAssignments]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'EntityTypeAssignment'
    
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

