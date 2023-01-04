CREATE TABLE [dbo].[Orders] (
    [Id]                             INT             IDENTITY (5778, 1) NOT NULL,
    [TicketId]                       INT             NOT NULL,
    [WarehouseId]                    INT             NOT NULL,
    [DepartmentId]                   INT             NOT NULL,
    [MenuItemId]                     INT             NOT NULL,
    [MenuItemName]                   NVARCHAR (4000) NULL,
    [PortionName]                    NVARCHAR (4000) NULL,
    [Price]                          DECIMAL (16, 4) NULL,
    [Quantity]                       NUMERIC (16, 3) NOT NULL,
    [PortionCount]                   INT             NOT NULL,
    [Locked]                         BIT             NOT NULL,
    [CalculatePrice]                 BIT             NOT NULL,
    [DecreaseInventory]              BIT             NOT NULL,
    [IncreaseInventory]              BIT             NOT NULL,
    [OrderNumber]                    INT             NOT NULL,
    [CreatingUserName]               NVARCHAR (4000) NULL,
    [CreatedDateTime]                DATETIME        NOT NULL,
    [AccountTransactionTypeId]       INT             NOT NULL,
    [ProductTimerValueId]            INT             NULL,
    [PriceTag]                       NVARCHAR (4000) NULL,
    [Tag]                            NVARCHAR (4000) NULL,
    [Taxes]                          NTEXT           NULL,
    [OrderTags]                      NTEXT           NULL,
    [OrderStates]                    NTEXT           NULL,
    [MenuGroupName]                  NVARCHAR (4000) NULL,
    [ActiveTimerAmount]              NUMERIC (16, 4) CONSTRAINT [DF_Orders_ActiveTimerAmount] DEFAULT ((0)) NOT NULL,
    [PlainTotal]                     NUMERIC (16, 4) CONSTRAINT [DF_Orders_PlainTotal] DEFAULT ((0)) NOT NULL,
    [Total]                          NUMERIC (16, 4) CONSTRAINT [DF_Orders_Total] DEFAULT ((0)) NOT NULL,
    [Value]                          NUMERIC (16, 4) CONSTRAINT [DF_Orders_Value] DEFAULT ((0)) NOT NULL,
    [GetPriceValue]                  NUMERIC (16, 4) CONSTRAINT [DF_Orders_GetPrice] DEFAULT ((0)) NOT NULL,
    [GrossUnitPrice]                 NUMERIC (16, 4) CONSTRAINT [DF_Orders_GrossUnitPrice] DEFAULT ((0)) NOT NULL,
    [GrossPrice]                     NUMERIC (16, 4) CONSTRAINT [DF_Orders_GrossPrice] DEFAULT ((0)) NOT NULL,
    [OrderTagPrice]                  NUMERIC (16, 4) CONSTRAINT [DF_Orders_OrderTagPrice] DEFAULT ((0)) NOT NULL,
    [Delivered]                      BIT             CONSTRAINT [DF_Orders_Delivered] DEFAULT ((0)) NOT NULL,
    [DeliveryDateTime]               DATETIME        NULL,
    [TaggedOrder]                    BIT             CONSTRAINT [DF_Orders_TaggedOrder] DEFAULT ((0)) NOT NULL,
    [AutomationUserName]             NVARCHAR (4000) CONSTRAINT [DF_Orders_AutomationUserName] DEFAULT (N'_') NOT NULL,
    [AutomationDateTime]             DATETIME        NULL,
    [CalculationTypes]               NTEXT           NULL,
    [NetTicketUnitPrice]             NUMERIC (16, 4) CONSTRAINT [DF_Orders_GrossTicketUnitPrice] DEFAULT ((0)) NOT NULL,
    [OrderCalculationTaxAmount]      NUMERIC (16, 4) CONSTRAINT [DF_Orders_OrderCalculationTaxAmount] DEFAULT ((0)) NOT NULL,
    [OrderCalculationDiscountAmount] NUMERIC (16, 4) CONSTRAINT [DF_Orders_OrderCalculationDiscountAmount] DEFAULT ((0)) NOT NULL,
    [TaxIncluded]                    BIT             CONSTRAINT [DF_Orders_TaxIncluded] DEFAULT ((0)) NOT NULL,
    [Delay]                          NUMERIC (16, 4) CONSTRAINT [DF_Orders_Delay] DEFAULT ((0)) NOT NULL,
    [UptoSD]                         NUMERIC (16, 4) CONSTRAINT [DF_Orders_UptoSD] DEFAULT ((0)) NOT NULL,
    [UptoTAX]                        NUMERIC (16, 4) CONSTRAINT [DF_Orders_UptoTAX] DEFAULT ((0)) NOT NULL,
    [UptoDiscount]                   NUMERIC (16, 4) CONSTRAINT [DF_Orders_UptoDiscount] DEFAULT ((0)) NOT NULL,
    [UptoAll]                        NUMERIC (16, 4) CONSTRAINT [DF_Orders_UptoAll] DEFAULT ((0)) NOT NULL,
    [OrderCalculationTypes]          NTEXT           NULL,
    [SplitCount]                     INT             CONSTRAINT [DF_Orders_SplitCount] DEFAULT ((1)) NOT NULL,
    [TaxableAmount]                  NUMERIC (16, 4) CONSTRAINT [DF_Orders_TaxableAmount] DEFAULT ((0)) NOT NULL,
    [NonTaxableAmount]               NUMERIC (16, 4) CONSTRAINT [DF_Orders_NonTaxableAmount] DEFAULT ((0)) NOT NULL,
    [TerminalId]                     INT             CONSTRAINT [DF_Orders_TerminalId] DEFAULT ((1)) NOT NULL,
    [TerminalName]                   NVARCHAR (4000) CONSTRAINT [DF_Orders_TerminalName] DEFAULT (N'Server') NOT NULL,
    [UserId]                         INT             NULL,
    [ShiftId]                        INT             NULL,
    [WorkPeriodId]                   INT             NULL,
    [MergeTicketId]                  INT             NULL,
    [IsSentToSDC]                    BIT             CONSTRAINT [DF_Orders_IsSentToSDC] DEFAULT ((0)) NOT NULL,
    [IsCreditNoteSentToSDC]          BIT             CONSTRAINT [DF_Orders_IsCreditNoteSentToSDC] DEFAULT ((0)) NOT NULL,
    [SDCMenuItemCode]                NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Orders] PRIMARY KEY CLUSTERED ([Id] ASC, [TicketId] ASC),
    CONSTRAINT [FK_dbo.Orders_dbo.ProductTimerValues_ProductTimerValueId] FOREIGN KEY ([ProductTimerValueId]) REFERENCES [dbo].[ProductTimerValues] ([Id]),
    CONSTRAINT [FK_dbo.Orders_dbo.Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [dbo].[Tickets] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_ProductTimerValueId]
    ON [dbo].[Orders]([ProductTimerValueId] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_TicketId]
    ON [dbo].[Orders]([TicketId] ASC);


GO

