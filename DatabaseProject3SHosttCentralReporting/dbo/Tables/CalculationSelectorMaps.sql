CREATE TABLE [dbo].[CalculationSelectorMaps] (
    [Id]                    INT IDENTITY (6, 1) NOT NULL,
    [CalculationSelectorId] INT NOT NULL,
    [TerminalId]            INT NOT NULL,
    [DepartmentId]          INT NOT NULL,
    [UserRoleId]            INT NOT NULL,
    [TicketTypeId]          INT NOT NULL,
    CONSTRAINT [PK_dbo.CalculationSelectorMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CalculationSelectorMaps_dbo.CalculationSelectors_CalculationSelectorId] FOREIGN KEY ([CalculationSelectorId]) REFERENCES [dbo].[CalculationSelectors] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_CalculationSelectorId]
    ON [dbo].[CalculationSelectorMaps]([CalculationSelectorId] ASC);


GO


CREATE TRIGGER [dbo].[HOCalculationSelectorMaps]
   ON  [dbo].[CalculationSelectorMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'CalculationSelector'
    
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

