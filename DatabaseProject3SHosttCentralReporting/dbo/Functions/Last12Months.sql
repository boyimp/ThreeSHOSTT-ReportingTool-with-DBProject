CREATE FUNCTION [dbo].[Last12Months]
(
    @Date datetime
)   RETURNS @tbl TABLE (Start datetime, EndDate datetime)
AS   
BEGIN
    WITH T AS(
    SELECT 
        DATEADD(month, DATEDIFF(month, 0, @Date), 0) AS Start,
        DATEADD(d, -DAY(DATEADD(m,1,@date)),DATEADD(m,1,@date)) AS EndDate,
        12 Cnt
    UNION ALL
    SELECT 
        DATEADD(month, -1, Start),
        DATEADD(d, -DAY(DATEADD(m,1,Start-1)),DATEADD(m,1,Start-1)),
        Cnt-1
    FROM
        T
    WHERE
        Cnt-1>0
    )
    INSERT INTO @tbl 
        (Start, EndDate)
    SELECT 
        Start, EndDate
    FROM T

    RETURN 
END

GO

