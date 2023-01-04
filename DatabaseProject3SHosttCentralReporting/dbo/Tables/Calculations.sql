CREATE TABLE [dbo].[Calculations] (
    [Id]                                          INT             IDENTITY (296, 1) NOT NULL,
    [TicketId]                                    INT             NOT NULL,
    [Name]                                        NVARCHAR (4000) NULL,
    [Order]                                       INT             NOT NULL,
    [CalculationTypeId]                           INT             NOT NULL,
    [AccountTransactionTypeId]                    INT             NOT NULL,
    [CalculationType]                             INT             NOT NULL,
    [IncludeTax]                                  BIT             NOT NULL,
    [DecreaseAmount]                              BIT             NOT NULL,
    [UsePlainSum]                                 BIT             NOT NULL,
    [Amount]                                      DECIMAL (16, 4) NULL,
    [CalculationAmount]                           DECIMAL (16, 4) NULL,
    [CalculationTypeMap]                          VARCHAR (MAX)   NULL,
    [Dynamic]                                     BIT             CONSTRAINT [DF_Calculations_Dynamic] DEFAULT ((0)) NOT NULL,
    [IncludeOtherCalculations]                    BIT             CONSTRAINT [DF_Calculations_IncludeOtherCalculations] DEFAULT ((0)) NOT NULL,
    [UserName]                                    NVARCHAR (4000) NULL,
    [IsTax]                                       BIT             CONSTRAINT [DF_Calculations_IsTax] DEFAULT ((0)) NOT NULL,
    [IsDiscount]                                  BIT             CONSTRAINT [DF_Calculations_IsDiscount] DEFAULT ((0)) NOT NULL,
    [CalculationAmountWithoutOtherCalculations]   DECIMAL (16, 4) NOT NULL,
    [IncludedCalculations]                        NTEXT           NULL,
    [CalculationSumValue]                         NUMERIC (16, 4) CONSTRAINT [DF_Calculations_CalculationSumValue] DEFAULT ((0)) NOT NULL,
    [CalculationSumValueWithoutOtherCalculations] NUMERIC (16, 4) CONSTRAINT [DF_Calculations_CalculationSumValueWithoutOtherCalculations] DEFAULT ((0)) NOT NULL,
    [IsAuto]                                      BIT             CONSTRAINT [DF_Calculations_IsAuto] DEFAULT ((0)) NOT NULL,
    [IsSD]                                        BIT             CONSTRAINT [DF_Calculations_IsSD] DEFAULT ((0)) NOT NULL,
    [IsServiceCharge]                             BIT             CONSTRAINT [DF_Calculations_IsServiceCharge] DEFAULT ((0)) NOT NULL,
    [SplitCount]                                  INT             CONSTRAINT [DF_Calculations_SplitCount] DEFAULT ((1)) NOT NULL,
    [UserId]                                      INT             NULL,
    [WorkPeriodId]                                INT             NULL,
    [ShiftId]                                     INT             NULL,
    [TerminalId]                                  INT             NULL,
    [GovCode]                                     NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Calculations] PRIMARY KEY CLUSTERED ([Id] ASC, [TicketId] ASC),
    CONSTRAINT [FK_dbo.Calculations_dbo.Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [dbo].[Tickets] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_TicketId]
    ON [dbo].[Calculations]([TicketId] ASC);


GO

