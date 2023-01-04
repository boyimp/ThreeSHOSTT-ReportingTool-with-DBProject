CREATE TABLE [dbo].[PaymentTypeMaps] (
    [Id]                     INT IDENTITY (6, 1) NOT NULL,
    [PaymentTypeId]          INT NOT NULL,
    [DisplayAtPaymentScreen] BIT NOT NULL,
    [DisplayUnderTicket]     BIT NOT NULL,
    [TerminalId]             INT NOT NULL,
    [DepartmentId]           INT NOT NULL,
    [UserRoleId]             INT NOT NULL,
    [TicketTypeId]           INT NOT NULL,
    CONSTRAINT [PK_dbo.PaymentTypeMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PaymentTypeMaps_dbo.PaymentTypes_PaymentTypeId] FOREIGN KEY ([PaymentTypeId]) REFERENCES [dbo].[PaymentTypes] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_PaymentTypeId]
    ON [dbo].[PaymentTypeMaps]([PaymentTypeId] ASC);


GO


CREATE TRIGGER [dbo].[HOPaymentTypeMaps]
   ON  [dbo].[PaymentTypeMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'PaymentTypeMap'
    
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

