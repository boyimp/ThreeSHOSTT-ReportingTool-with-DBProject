CREATE FUNCTION [SQL#].[String_SplitInts]
(@StringValue NVARCHAR (MAX) NULL, @Separator NVARCHAR (4000) NULL, @SplitOption INT NULL, @ReturnNullRowOnNullInput BIT NULL)
RETURNS 
     TABLE (
        [SplitNum] INT    NULL,
        [SplitVal] BIGINT NULL)
AS
 EXTERNAL NAME [SQL#].[STRING].[SplitInts]


GO

