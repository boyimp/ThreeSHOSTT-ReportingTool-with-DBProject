
CREATE FUNCTION [dbo].[fnSplitter]
(
    @IDs VARCHAR(MAX)
)
RETURNS @Tbl_IDs TABLE
(
    ID NVARCHAR(max) NOT NULL
)
AS

BEGIN
    -- Append comma
    SET @IDs = @IDs + ',';
    -- Indexes to keep the position of searching
    DECLARE @Pos1 BIGINT;
    DECLARE @pos2 BIGINT;

    -- Start from first character 
    SET @Pos1 = 1;
    SET @pos2 = 1;

    WHILE @Pos1 < LEN(@IDs)
    BEGIN
        SET @Pos1 = CHARINDEX(',', @IDs, @Pos1);
            INSERT @Tbl_IDs
            SELECT SUBSTRING(@IDs, @pos2, @Pos1 - @pos2);
        -- Go to next non comma character
        SET @pos2 = @Pos1 + 1;
        -- Search from the next charcater
        SET @Pos1 = @Pos1 + 1;
    END;
    RETURN;
END;

GO

