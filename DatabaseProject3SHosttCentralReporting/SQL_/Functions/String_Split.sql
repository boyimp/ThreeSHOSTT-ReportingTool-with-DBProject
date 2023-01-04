CREATE FUNCTION [SQL#].[String_Split]
(@StringValue NVARCHAR (MAX) NULL, @Separator NVARCHAR (4000) NULL, @SplitOption INT NULL)
RETURNS 
     TABLE (
        [SplitNum] INT            NULL,
        [SplitVal] NVARCHAR (MAX) NULL)
AS
 EXTERNAL NAME [SQL#].[STRING].[Split]


GO

