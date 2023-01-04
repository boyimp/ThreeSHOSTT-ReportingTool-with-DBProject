CREATE TABLE [dbo].[TicketTypes] (
    [Id]                     INT             IDENTITY (3, 1) NOT NULL,
    [ScreenMenuId]           INT             NOT NULL,
    [TaxIncluded]            BIT             NOT NULL,
    [SortOrder]              INT             NOT NULL,
    [Name]                   NVARCHAR (4000) NULL,
    [TicketNumerator_Id]     INT             NULL,
    [OrderNumerator_Id]      INT             NULL,
    [SaleTransactionType_Id] INT             NULL,
    [TokenNumerator_Id]      INT             NULL,
    [RestrictSalesIfNoStock] BIT             CONSTRAINT [DF_TicketTypes_RestrictSalesIfNoStock] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.TicketTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TicketTypes_dbo.AccountTransactionTypes_SaleTransactionType_Id] FOREIGN KEY ([SaleTransactionType_Id]) REFERENCES [dbo].[AccountTransactionTypes] ([Id]),
    CONSTRAINT [FK_dbo.TicketTypes_dbo.Numerators_OrderNumerator_Id] FOREIGN KEY ([OrderNumerator_Id]) REFERENCES [dbo].[Numerators] ([Id]),
    CONSTRAINT [FK_dbo.TicketTypes_dbo.Numerators_TicketNumerator_Id] FOREIGN KEY ([TicketNumerator_Id]) REFERENCES [dbo].[Numerators] ([Id]),
    CONSTRAINT [FK_TicketTypes_Numerators] FOREIGN KEY ([TokenNumerator_Id]) REFERENCES [dbo].[Numerators] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_OrderNumerator_Id]
    ON [dbo].[TicketTypes]([OrderNumerator_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_TicketNumerator_Id]
    ON [dbo].[TicketTypes]([TicketNumerator_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_SaleTransactionType_Id]
    ON [dbo].[TicketTypes]([SaleTransactionType_Id] ASC);


GO



CREATE TRIGGER [dbo].[TriggerTicketTypes]
   ON  [dbo].[TicketTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
	delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);
END

GO


CREATE TRIGGER [dbo].[HOTicketTypes]
   ON  [dbo].[TicketTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'TicketType'
    
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

