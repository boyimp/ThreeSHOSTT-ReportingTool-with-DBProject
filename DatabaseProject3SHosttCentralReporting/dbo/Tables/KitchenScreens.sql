CREATE TABLE [dbo].[KitchenScreens] (
    [Id]                  INT             IDENTITY (7, 1) NOT NULL,
    [TicketTypeId]        INT             NOT NULL,
    [EntityTypeId]        INT             NOT NULL,
    [SortOrder]           INT             NOT NULL,
    [DisplayMode]         INT             NOT NULL,
    [BackgroundColor]     NVARCHAR (4000) NULL,
    [BackgroundImage]     NVARCHAR (4000) NULL,
    [FontSize]            INT             NOT NULL,
    [PageCount]           INT             NOT NULL,
    [RowCount]            INT             NOT NULL,
    [ColumnCount]         INT             NOT NULL,
    [ButtonHeight]        INT             NOT NULL,
    [Name]                NVARCHAR (4000) NULL,
    [ShowDeliveredOrders] BIT             CONSTRAINT [DF_KitchenScreens_ShowDeliveredOrders] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo_KitchenScreens] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

GRANT INSERT
    ON OBJECT::[dbo].[KitchenScreens] TO PUBLIC
    AS [dbo];


GO

GRANT SELECT
    ON OBJECT::[dbo].[KitchenScreens] TO PUBLIC
    AS [dbo];


GO

GRANT DELETE
    ON OBJECT::[dbo].[KitchenScreens] TO PUBLIC
    AS [dbo];


GO

GRANT REFERENCES
    ON OBJECT::[dbo].[KitchenScreens] TO PUBLIC
    AS [dbo];


GO

GRANT UPDATE
    ON OBJECT::[dbo].[KitchenScreens] TO PUBLIC
    AS [dbo];


GO

