CREATE FUNCTION [SQL#].[String_SplitInts4k]
(@StringValue NVARCHAR (4000) NULL, @Separator NVARCHAR (4000) NULL, @SplitOption INT NULL, @ReturnNullRowOnNullInput BIT NULL)
RETURNS 
     TABLE (
        [SplitNum] INT    NULL,
        [SplitVal] BIGINT NULL)
AS
 EXTERNAL NAME [SQL#].[STRING].[SplitInts]


GO

