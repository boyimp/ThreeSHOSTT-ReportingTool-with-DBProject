CREATE TABLE [dbo].[HOSTTCredentials] (
    [Id]                                        INT             IDENTITY (1, 1) NOT NULL,
    [MaintainActivityLog]                       BIT             NULL,
    [Name]                                      NVARCHAR (MAX)  NULL,
    [LastUpdateTime]                            DATETIME        NULL,
    [PreferredInventoryUnitType]                INT             NULL,
    [CountryCode]                               VARCHAR (500)   NULL,
    [StartEntitySearchWithCountryCodeByDefault] BIT             NULL,
    [EnableExtendedERPSystemIntegration]        BIT             NULL,
    [TopFrequentOrdersCount]                    INT             NULL,
    [EnforceShiftManangementForPOS]             BIT             NULL,
    [EnforceTerminalLoginForPOS]                BIT             NULL,
    [BaId]                                      INT             NULL,
    [WebApiBaseUrl]                             NVARCHAR (MAX)  NULL,
    [FetchEntitiesFromCloud]                    BIT             DEFAULT ((0)) NOT NULL,
    [FetchOrderHistoryFromCloud]                BIT             DEFAULT ((0)) NOT NULL,
    [FetchOrderHistoryForAllOutlets]            BIT             DEFAULT ((0)) NOT NULL,
    [DirectPrintAtPOSThermalPrinter]            BIT             DEFAULT ((0)) NOT NULL,
    [OverrideBaIdByInternalMappingIfExists]     BIT             DEFAULT ((0)) NULL,
    [CompanyInfo]                               NVARCHAR (MAX)  NULL,
    [Logo]                                      VARBINARY (MAX) NULL,
    [RestrictTicketSubmissionIfNotSentToSDC]    BIT             CONSTRAINT [DF_HOSTTCredentials_RestrictTicketSubmissionIfNotSentToSDC] DEFAULT ((0)) NOT NULL,
    [EntitySearchBarcodeScannerOnly]            BIT             CONSTRAINT [DF_HOSTTCredentials_EntitySearchBarcodeScannerOnly] DEFAULT ((0)) NOT NULL,
    [ExactMatchFromCloud]                       BIT             CONSTRAINT [DF_HOSTTCredentials_ExactMatchFromCloud] DEFAULT ((0)) NOT NULL,
    [MinimumEntityBarcodeLength]                INT             NULL,
    CONSTRAINT [PK_HOSTTCredential] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

