CREATE TABLE [dbo].[TicketTagGroups] (
    [Id]                           INT             IDENTITY (2, 1) NOT NULL,
    [SortOrder]                    INT             NOT NULL,
    [FreeTagging]                  BIT             NOT NULL,
    [SaveFreeTags]                 BIT             NOT NULL,
    [ButtonColorWhenTagSelected]   NVARCHAR (4000) NULL,
    [ButtonColorWhenNoTagSelected] NVARCHAR (4000) NULL,
    [ForceValue]                   BIT             NOT NULL,
    [AskBeforeCreatingTicket]      BIT             NOT NULL,
    [DataType]                     INT             NOT NULL,
    [Name]                         NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.TicketTagGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

