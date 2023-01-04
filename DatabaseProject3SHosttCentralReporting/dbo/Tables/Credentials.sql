CREATE TABLE [dbo].[Credentials] (
    [Id]                         INT            IDENTITY (1, 1) NOT NULL,
    [MaintainActivityLog]        BIT            NULL,
    [Name]                       NVARCHAR (MAX) NULL,
    [LastUpdateTime]             DATETIME       NULL,
    [PreferredInventoryUnitType] INT            NULL,
    CONSTRAINT [PK_Credential] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

