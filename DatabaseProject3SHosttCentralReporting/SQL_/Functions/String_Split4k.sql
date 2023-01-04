CREATE FUNCTION [SQL#].[String_Split4k]
(@StringValue NVARCHAR (4000) NULL, @Separator NVARCHAR (4000) NULL, @SplitOption INT NULL)
RETURNS 
     TABLE (
        [SplitNum] INT             NULL,
        [SplitVal] NVARCHAR (4000) NULL)
AS
 EXTERNAL NAME [SQL#].[STRING].[Split]


GO

