CREATE TABLE [dbo].[PaymentTypes] (
    [Id]                        INT             IDENTITY (6, 1) NOT NULL,
    [SortOrder]                 INT             NOT NULL,
    [ButtonColor]               NVARCHAR (4000) NULL,
    [Name]                      NVARCHAR (4000) NULL,
    [AccountTransactionType_Id] INT             NULL,
    [Account_Id]                INT             NULL,
    [PaymentMethod]             INT             CONSTRAINT [DF_PaymentTypes_PaymentMethod] DEFAULT ((0)) NOT NULL,
    [PaymentVerificationURL]    NVARCHAR (4000) NULL,
    [Identifier]                NVARCHAR (4000) NULL,
    [Keys]                      NVARCHAR (4000) NULL,
    [HttpMethod]                VARCHAR (400)   NULL,
    [AuthorizationToken]        NVARCHAR (MAX)  NULL,
    [MetaTagIds]                VARCHAR (MAX)   NULL,
    [MetaTagsJSON]              NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.PaymentTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PaymentTypes_dbo.Accounts_Account_Id] FOREIGN KEY ([Account_Id]) REFERENCES [dbo].[Accounts] ([Id]),
    CONSTRAINT [FK_dbo.PaymentTypes_dbo.AccountTransactionTypes_AccountTransactionType_Id] FOREIGN KEY ([AccountTransactionType_Id]) REFERENCES [dbo].[AccountTransactionTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionType_Id]
    ON [dbo].[PaymentTypes]([AccountTransactionType_Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_Account_Id]
    ON [dbo].[PaymentTypes]([Account_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOPaymentTypes]
   ON  [dbo].[PaymentTypes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
DECLARE  
    @OutletId int,
 	@BasicDataName varchar(max)
 	
SET @BasicDataName = 'PaymentType'
    
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

