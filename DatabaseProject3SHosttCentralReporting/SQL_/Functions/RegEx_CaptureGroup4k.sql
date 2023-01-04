CREATE FUNCTION [SQL#].[RegEx_CaptureGroup4k]
(@ExpressionToValidate NVARCHAR (4000) NULL, @RegularExpression NVARCHAR (4000) NULL, @CaptureGroupNumber INT NULL, @NotFoundReplacement NVARCHAR (4000) NULL, @StartAt INT NULL, @Length INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SQL#].[REGEX].[CaptureGroup]


GO

