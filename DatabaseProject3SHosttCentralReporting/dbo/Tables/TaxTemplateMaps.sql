CREATE TABLE [dbo].[TaxTemplateMaps] (
    [Id]                INT             IDENTITY (3, 1) NOT NULL,
    [TaxTemplateId]     INT             NOT NULL,
    [MenuItemGroupCode] NVARCHAR (4000) NULL,
    [MenuItemId]        INT             NOT NULL,
    [TerminalId]        INT             NOT NULL,
    [DepartmentId]      INT             NOT NULL,
    [UserRoleId]        INT             NOT NULL,
    [TicketTypeId]      INT             NOT NULL,
    [ZoneId]            INT             CONSTRAINT [DF_TaxTemplateMaps_ZoneId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.TaxTemplateMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TaxTemplateMaps_dbo.TaxTemplates_TaxTemplateId] FOREIGN KEY ([TaxTemplateId]) REFERENCES [dbo].[TaxTemplates] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_TaxTemplateId]
    ON [dbo].[TaxTemplateMaps]([TaxTemplateId] ASC);


GO


CREATE TRIGGER [dbo].[HOTaxTemplateMaps]
   ON  [dbo].[TaxTemplateMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'TaxTemplateMap'
    
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


CREATE TRIGGER [dbo].[TriggerTT]
   ON  [dbo].[TaxTemplateMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO

