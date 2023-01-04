CREATE TABLE [dbo].[OutletWiseTargetItems] (
    [ID]                  INT             IDENTITY (1, 1) NOT NULL,
    [OutletWiseTargetId]  INT             NOT NULL,
    [SyncOutlet_Id]       INT             NULL,
    [MenuItemPortion_Id]  INT             NULL,
    [MenuItemName]        NVARCHAR (4000) NOT NULL,
    [MenuItemPortionName] NVARCHAR (4000) NOT NULL,
    [MenuItemPrice]       NUMERIC (16, 2) NOT NULL,
    [TargetInPcs]         NUMERIC (16, 2) NOT NULL,
    [TargetInWeight]      NUMERIC (16, 2) NOT NULL,
    [TargetInValue]       NUMERIC (16, 2) NOT NULL,
    CONSTRAINT [PK_OutletWiseTargetItem] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_OutletWiseTarget_OutletWiseTargetItem] FOREIGN KEY ([OutletWiseTargetId]) REFERENCES [dbo].[OutletWiseTargets] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_OutletWiseTarget_SyncOutlet] FOREIGN KEY ([SyncOutlet_Id]) REFERENCES [dbo].[SyncOutlets] ([Id]),
    CONSTRAINT [FK_Portion_OutletWiseTargetItem] FOREIGN KEY ([MenuItemPortion_Id]) REFERENCES [dbo].[MenuItemPortions] ([Id])
);


GO

GRANT DELETE
    ON OBJECT::[dbo].[OutletWiseTargetItems] TO PUBLIC
    AS [dbo];


GO

GRANT SELECT
    ON OBJECT::[dbo].[OutletWiseTargetItems] TO PUBLIC
    AS [dbo];


GO

GRANT REFERENCES
    ON OBJECT::[dbo].[OutletWiseTargetItems] TO PUBLIC
    AS [dbo];


GO

GRANT UPDATE
    ON OBJECT::[dbo].[OutletWiseTargetItems] TO PUBLIC
    AS [dbo];


GO

GRANT INSERT
    ON OBJECT::[dbo].[OutletWiseTargetItems] TO PUBLIC
    AS [dbo];


GO

