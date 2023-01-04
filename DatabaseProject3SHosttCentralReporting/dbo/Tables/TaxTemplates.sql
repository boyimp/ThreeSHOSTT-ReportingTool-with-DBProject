CREATE TABLE [dbo].[TaxTemplates] (
    [Id]                        INT             IDENTITY (3, 1) NOT NULL,
    [SortOrder]                 INT             NOT NULL,
    [Rate]                      NUMERIC (16, 2) NOT NULL,
    [Name]                      NVARCHAR (4000) NULL,
    [AccountTransactionType_Id] INT             NULL,
    CONSTRAINT [PK_dbo.TaxTemplates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TaxTemplates_dbo.AccountTransactionTypes_AccountTransactionType_Id] FOREIGN KEY ([AccountTransactionType_Id]) REFERENCES [dbo].[AccountTransactionTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionType_Id]
    ON [dbo].[TaxTemplates]([AccountTransactionType_Id] ASC);


GO


CREATE TRIGGER [dbo].[TriggerTTM]
   ON  [dbo].[TaxTemplates]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO


CREATE TRIGGER [dbo].[HOTaxTemplates]
   ON  [dbo].[TaxTemplates]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'TaxTemplate'
    
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

