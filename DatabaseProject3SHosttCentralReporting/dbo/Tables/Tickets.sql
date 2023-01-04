CREATE TABLE [dbo].[Tickets] (
    [Id]                           INT              IDENTITY (1737, 1) NOT NULL,
    [LastUpdateTime]               DATETIME         NOT NULL,
    [TicketNumber]                 NVARCHAR (4000)  NULL,
    [Date]                         DATETIME         NOT NULL,
    [LastOrderDate]                DATETIME         NOT NULL,
    [LastPaymentDate]              DATETIME         NOT NULL,
    [IsClosed]                     BIT              NOT NULL,
    [IsLocked]                     BIT              NOT NULL,
    [RemainingAmount]              DECIMAL (16, 4)  NULL,
    [TotalAmount]                  DECIMAL (16, 4)  NULL,
    [DepartmentId]                 INT              NOT NULL,
    [TicketTypeId]                 INT              NOT NULL,
    [Note]                         NTEXT            NULL,
    [TicketTags]                   NTEXT            NULL,
    [TicketStates]                 NTEXT            NULL,
    [ExchangeRate]                 NUMERIC (18, 2)  NOT NULL,
    [TaxIncluded]                  BIT              NOT NULL,
    [Name]                         NVARCHAR (4000)  NULL,
    [TransactionDocument_Id]       INT              NULL,
    [DeliveryDate]                 DATETIME         DEFAULT ('09 Oct 1985') NOT NULL,
    [RETURNAMOUNT]                 MONEY            DEFAULT ((0.00)) NOT NULL,
    [NoOfGuests]                   INT              CONSTRAINT [DF_Tickets_NoOfGuests] DEFAULT ((0)) NOT NULL,
    [Sum]                          NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_Sum] DEFAULT ((0)) NOT NULL,
    [PreTaxServicesTotal]          NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_PreTaxServicesTotal] DEFAULT ((0)) NOT NULL,
    [PostTaxServicesTotal]         NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_PostTaxServicesTotal] DEFAULT ((0)) NOT NULL,
    [PlainSum]                     NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_PlainSum] DEFAULT ((0)) NOT NULL,
    [PlainSumForEndOfPeriod]       NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_PlainSumForEndOfPeriod] DEFAULT ((0)) NOT NULL,
    [TaxTotal]                     NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_TaxTotal] DEFAULT ((0)) NOT NULL,
    [ServiceChargeTotal]           NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_ServiceChargeTotal] DEFAULT ((0)) NOT NULL,
    [ActiveTimerAmount]            NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_ActiveTimerAmount] DEFAULT ((0)) NOT NULL,
    [ActiveTimerClosedAmount]      NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_ActiveTimerClosedAmount] DEFAULT ((0)) NOT NULL,
    [Synced]                       BIT              CONSTRAINT [DF_Tickets_Synced] DEFAULT ((0)) NOT NULL,
    [SyncID]                       INT              CONSTRAINT [DF_Tickets_SyncID] DEFAULT ((0)) NOT NULL,
    [TokenNumber]                  NVARCHAR (50)    CONSTRAINT [DF_Tickets_TokenNumber] DEFAULT ((0)) NOT NULL,
    [HHTTicketId]                  UNIQUEIDENTIFIER NULL,
    [SettledBy]                    NVARCHAR (4000)  NULL,
    [ServerUpdateTime]             DATETIME         CONSTRAINT [DF_Tickets_ServerUpdateTime] DEFAULT (getdate()) NOT NULL,
    [SyncNote]                     NTEXT            NULL,
    [SettledCopyPrintMessage]      NVARCHAR (4000)  NULL,
    [ZoneId]                       INT              CONSTRAINT [DF_Tickets_ZoneId] DEFAULT ((0)) NOT NULL,
    [ChangeAmount]                 NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_ChangeAmount] DEFAULT ((0)) NOT NULL,
    [DispatchDate]                 DATETIME         NULL,
    [IsDispatched]                 BIT              CONSTRAINT [DF_Tickets_IsDispatched] DEFAULT ((0)) NOT NULL,
    [SyncOutletId]                 INT              DEFAULT ((0)) NOT NULL,
    [SmartStoreOrderId]            INT              NULL,
    [PreTaxDiscountServicesTotal]  NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_PreTaxDiscountServicesTotal] DEFAULT ((0)) NOT NULL,
    [PostTaxDiscountServicesTotal] NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_PostTaxDiscountServicesTotal] DEFAULT ((0)) NOT NULL,
    [OnlineCommunicationInfo]      NTEXT            NULL,
    [Synced2]                      BIT              CONSTRAINT [DF_Tickets_Synced2] DEFAULT ((0)) NOT NULL,
    [ServerUpdateTime2]            DATETIME         CONSTRAINT [DF_Tickets_ServerUpdateTime2] DEFAULT (getdate()) NOT NULL,
    [SplitCount]                   INT              CONSTRAINT [DF_Tickets_SplitCount] DEFAULT ((1)) NOT NULL,
    [TaxableAmount]                NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_TaxableAmount] DEFAULT ((0)) NOT NULL,
    [NonTaxableAmount]             NUMERIC (16, 4)  CONSTRAINT [DF_Tickets_NonTaxableAmount] DEFAULT ((0)) NOT NULL,
    [FiscalYear]                   NVARCHAR (4000)  CONSTRAINT [DF_Tickets_FiscalYear] DEFAULT (N'-') NOT NULL,
    [TerminalId]                   INT              CONSTRAINT [DF_Tickets_TerminalId] DEFAULT ((1)) NOT NULL,
    [TerminalName]                 NVARCHAR (4000)  CONSTRAINT [DF_Tickets_TerminalName] DEFAULT (N'Server') NOT NULL,
    [UserId]                       INT              NULL,
    [WorkPeriodId]                 INT              NULL,
    [ShiftId]                      INT              NULL,
    [TaxNumber]                    NVARCHAR (4000)  NULL,
    [MergedToTicketId]             INT              NULL,
    [CheckNumber]                  NVARCHAR (4000)  NULL,
    [WorkPeriodStartDate]          DATETIME         NULL,
    [ShiftStartDate]               DATETIME         NULL,
    [SDCReply]                     NVARCHAR (MAX)   NULL,
    [SentToSDC]                    BIT              CONSTRAINT [DF_Tickets_SentToSDC] DEFAULT ((0)) NOT NULL,
    [SDCUrl]                       NVARCHAR (MAX)   NULL,
    [SDCInvoiceNumber]             NVARCHAR (MAX)   NULL,
    [CreditNoteSDCReply]           NVARCHAR (MAX)   NULL,
    [RestrictTicketToBeSubmitted]  BIT              CONSTRAINT [DF_Tickets_RestrictTicketToBeSubmitted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Tickets] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Tickets_dbo.AccountTransactionDocuments_TransactionDocument_Id] FOREIGN KEY ([TransactionDocument_Id]) REFERENCES [dbo].[AccountTransactionDocuments] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_Tickets_LastPaymentDate]
    ON [dbo].[Tickets]([LastPaymentDate] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_TransactionDocument_Id]
    ON [dbo].[Tickets]([TransactionDocument_Id] ASC);


GO

