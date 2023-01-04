CREATE TABLE [dbo].[CalculationTypes] (
    [Id]                        INT             IDENTITY (6, 1) NOT NULL,
    [SortOrder]                 INT             NOT NULL,
    [CalculationMethod]         INT             NOT NULL,
    [Amount]                    NUMERIC (16, 2) NOT NULL,
    [MaxAmount]                 NUMERIC (18, 2) NOT NULL,
    [IncludeTax]                BIT             NOT NULL,
    [DecreaseAmount]            BIT             NOT NULL,
    [UsePlainSum]               BIT             NOT NULL,
    [Name]                      NVARCHAR (4000) NULL,
    [AccountTransactionType_Id] INT             NULL,
    [IsDynamic]                 BIT             CONSTRAINT [DF_CalculationTypes_IsDynamic] DEFAULT ((0)) NOT NULL,
    [IncludeOtherCalculations]  BIT             CONSTRAINT [DF_CalculationTypes_IncludeOtherCalculations] DEFAULT ((0)) NOT NULL,
    [IsTax]                     BIT             CONSTRAINT [DF_CalculationTypes_IsTax] DEFAULT ((0)) NOT NULL,
    [IsDiscount]                BIT             CONSTRAINT [DF_CalculationTypes_IsDiscount] DEFAULT ((0)) NOT NULL,
    [IncludedCalculations]      NTEXT           NULL,
    [IsSD]                      BIT             CONSTRAINT [DF_CalculationTypes_IsSD] DEFAULT ((0)) NOT NULL,
    [IsServiceCharge]           BIT             CONSTRAINT [DF_CalculationTypes_IsServiceCharge] DEFAULT ((0)) NOT NULL,
    [GovCode]                   NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.CalculationTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CalculationTypes_dbo.AccountTransactionTypes_AccountTransactionType_Id] FOREIGN KEY ([AccountTransactionType_Id]) REFERENCES [dbo].[AccountTransactionTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_AccountTransactionType_Id]
    ON [dbo].[CalculationTypes]([AccountTransactionType_Id] ASC);


GO


CREATE TRIGGER [dbo].[HOCalculationTypes]
   ON  [dbo].[CalculationTypes]
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

