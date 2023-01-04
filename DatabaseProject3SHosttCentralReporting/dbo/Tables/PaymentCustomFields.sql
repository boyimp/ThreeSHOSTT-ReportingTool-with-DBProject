CREATE TABLE [dbo].[PaymentCustomFields] (
    [Id]             INT             IDENTITY (4, 1) NOT NULL,
    [FieldType]      INT             NOT NULL,
    [EditingFormat]  NVARCHAR (4000) NULL,
    [ValueSource]    NVARCHAR (4000) NULL,
    [Hidden]         BIT             NOT NULL,
    [Name]           NVARCHAR (4000) NULL,
    [PaymentType_Id] INT             NULL,
    [EntryType]      INT             CONSTRAINT [DF_PaymentCustomFields_EntryType] DEFAULT ((0)) NOT NULL,
    [KeyMapping]     NVARCHAR (4000) NULL,
    CONSTRAINT [PK_dbo.PaymentCustomFields] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo_PaymentCustomFields_dbo_PaymentTypes_PaymentType_Id] FOREIGN KEY ([PaymentType_Id]) REFERENCES [dbo].[PaymentTypes] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_Payment_Id]
    ON [dbo].[PaymentCustomFields]([PaymentType_Id] ASC);


GO

GRANT DELETE
    ON OBJECT::[dbo].[PaymentCustomFields] TO PUBLIC
    AS [dbo];


GO

GRANT SELECT
    ON OBJECT::[dbo].[PaymentCustomFields] TO PUBLIC
    AS [dbo];


GO

GRANT REFERENCES
    ON OBJECT::[dbo].[PaymentCustomFields] TO PUBLIC
    AS [dbo];


GO

GRANT UPDATE
    ON OBJECT::[dbo].[PaymentCustomFields] TO PUBLIC
    AS [dbo];


GO

GRANT INSERT
    ON OBJECT::[dbo].[PaymentCustomFields] TO PUBLIC
    AS [dbo];


GO

