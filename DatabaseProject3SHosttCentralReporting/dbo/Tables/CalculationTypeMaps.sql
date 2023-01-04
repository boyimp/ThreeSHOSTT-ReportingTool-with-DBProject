CREATE TABLE [dbo].[CalculationTypeMaps] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [CalculationTypeId] INT             NOT NULL,
    [MenuItemGroupCode] NVARCHAR (4000) NULL,
    [MenuItemId]        INT             NOT NULL,
    [TerminalId]        INT             NOT NULL,
    [DepartmentId]      INT             NOT NULL,
    [UserRoleId]        INT             NOT NULL,
    [TicketTypeId]      INT             NOT NULL,
    [ZoneId]            INT             CONSTRAINT [DF_CalculationTypeMaps_ZoneId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo_CalculationTypeMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo_CalculationTypeMaps_dbo_CalculationTypes_CalculationTypeId] FOREIGN KEY ([CalculationTypeId]) REFERENCES [dbo].[CalculationTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_CalculationTypeId]
    ON [dbo].[CalculationTypeMaps]([CalculationTypeId] ASC);


GO


CREATE TRIGGER [dbo].[HOCalculationTypeMaps]
   ON  [dbo].[CalculationTypeMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'CalculationType'
    
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

