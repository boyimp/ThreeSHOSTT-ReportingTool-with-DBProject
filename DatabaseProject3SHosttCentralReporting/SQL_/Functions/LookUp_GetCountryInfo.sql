CREATE FUNCTION [SQL#].[LookUp_GetCountryInfo]
(@SearchCode NVARCHAR (4000) NULL)
RETURNS 
     TABLE (
        [NumericCode]     NCHAR (3)       NULL,
        [TwoLetterCode]   NCHAR (2)       NULL,
        [ThreeLetterCode] NCHAR (3)       NULL,
        [Name]            NVARCHAR (50)   NULL,
        [FlagImage]       VARBINARY (MAX) NULL)
AS
 EXTERNAL NAME [SQL#.LookUps].[LookUp].[GetCountryInfo]


GO

