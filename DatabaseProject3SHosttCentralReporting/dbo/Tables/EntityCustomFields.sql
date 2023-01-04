CREATE TABLE [dbo].[EntityCustomFields] (
    [Id]                INT             IDENTITY (4, 1) NOT NULL,
    [FieldType]         INT             NOT NULL,
    [EditingFormat]     NVARCHAR (4000) NULL,
    [ValueSource]       NVARCHAR (4000) NULL,
    [Hidden]            BIT             NOT NULL,
    [Name]              NVARCHAR (4000) NULL,
    [EntityType_Id]     INT             NULL,
    [MasterTableColumn] VARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.EntityCustomFields] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EntityCustomFields_dbo.EntityTypes_EntityType_Id] FOREIGN KEY ([EntityType_Id]) REFERENCES [dbo].[EntityTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_EntityType_Id]
    ON [dbo].[EntityCustomFields]([EntityType_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOEntityCustomFields]
   ON  [dbo].[EntityCustomFields]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'EntityCustomField'
    
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

