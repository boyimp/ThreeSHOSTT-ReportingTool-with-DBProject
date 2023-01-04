CREATE TABLE [dbo].[Terminals] (
    [Id]                 INT             IDENTITY (2, 1) NOT NULL,
    [IsDefault]          BIT             NOT NULL,
    [AutoLogout]         BIT             NOT NULL,
    [Name]               NVARCHAR (4000) NULL,
    [ReportPrinter_Id]   INT             NULL,
    [OccupancyStatus]    INT             NULL,
    [OccupiedByUserId]   INT             NULL,
    [LastUpdateTime]     DATETIME        NULL,
    [MachineKey]         NVARCHAR (MAX)  NULL,
    [OccupiedByUserName] NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Terminals] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Terminals_dbo.Printers_ReportPrinter_Id] FOREIGN KEY ([ReportPrinter_Id]) REFERENCES [dbo].[Printers] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_ReportPrinter_Id]
    ON [dbo].[Terminals]([ReportPrinter_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOTerminals]
   ON  [dbo].[Terminals]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'Terminal'
    
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

