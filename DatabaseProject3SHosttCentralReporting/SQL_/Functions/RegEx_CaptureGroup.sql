CREATE FUNCTION [SQL#].[RegEx_CaptureGroup]
(@ExpressionToValidate NVARCHAR (MAX) NULL, @RegularExpression NVARCHAR (MAX) NULL, @CaptureGroupNumber INT NULL, @NotFoundReplacement NVARCHAR (MAX) NULL, @StartAt INT NULL, @Length INT NULL, @RegExOptionsList NVARCHAR (4000) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SQL#].[REGEX].[CaptureGroup]


GO

