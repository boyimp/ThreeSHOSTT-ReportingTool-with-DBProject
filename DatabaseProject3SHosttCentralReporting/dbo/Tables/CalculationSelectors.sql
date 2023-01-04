CREATE TABLE [dbo].[CalculationSelectors] (
    [Id]                INT             IDENTITY (5, 1) NOT NULL,
    [ButtonHeader]      NVARCHAR (4000) NULL,
    [ButtonColor]       NVARCHAR (4000) NULL,
    [SortOrder]         INT             NOT NULL,
    [Name]              NVARCHAR (4000) NULL,
    [PasswordProtected] BIT             CONSTRAINT [DF_CalculationSelectors_PasswordProtected] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.CalculationSelectors] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE TRIGGER [dbo].[HOCalculationSelectors]
   ON  [dbo].[CalculationSelectors]
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

