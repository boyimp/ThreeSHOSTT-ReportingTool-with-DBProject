CREATE FUNCTION [SQL#].[LookUp_GetStateInfo]
(@SearchCode NVARCHAR (4000) NULL, @CountryCode NVARCHAR (4000) NULL)
RETURNS 
     TABLE (
        [NumericCode]   NCHAR (2)       NULL,
        [TwoLetterCode] NCHAR (2)       NULL,
        [Name]          NVARCHAR (50)   NULL,
        [FlagImage]     VARBINARY (MAX) NULL,
        [CountryCode]   NCHAR (2)       NULL)
AS
 EXTERNAL NAME [SQL#.LookUps].[LookUp].[GetStateInfo]


GO

