CREATE TABLE [dbo].[OrderTagMaps] (
    [Id]                INT             IDENTITY (4, 1) NOT NULL,
    [OrderTagGroupId]   INT             NOT NULL,
    [MenuItemGroupCode] NVARCHAR (4000) NULL,
    [MenuItemId]        INT             NOT NULL,
    [TerminalId]        INT             NOT NULL,
    [DepartmentId]      INT             NOT NULL,
    [UserRoleId]        INT             NOT NULL,
    [TicketTypeId]      INT             NOT NULL,
    CONSTRAINT [PK_dbo.OrderTagMaps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.OrderTagMaps_dbo.OrderTagGroups_OrderTagGroupId] FOREIGN KEY ([OrderTagGroupId]) REFERENCES [dbo].[OrderTagGroups] ([Id]) ON DELETE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [IX_OrderTagGroupId]
    ON [dbo].[OrderTagMaps]([OrderTagGroupId] ASC);


GO


CREATE TRIGGER [dbo].[TriggerOTM]
   ON  [dbo].[OrderTagMaps]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;
	
delete from dbo.HHTSync;
insert into dbo.HHTSync Values(1);

END

GO

