
CREATE PROC dbo.GetStartEndDate (
@FromDate DATETIME,
@ToDate DATETIME)
AS
BEGIN
    DECLARE @StartDate           VARCHAR(100),
            @EndDate             VARCHAR(100),
            --@LastPeriodStartDate DATETIME,
            --@LastPeriodEndDate   DATETIME,
            @LastWorkPeriodID    INT,
            @FirstWorkPeriod     INT;
--Exec GetStartEndDate '01 Mar 2020', '24 Mar 2020'

    SET @StartDate = dbo.ufsFormat((SELECT MIN(w.StartDate)
                                         FROM dbo.WorkPeriods w
                                        WHERE w.StartDate <> w.EndDate
                                          AND w.StartDate >= CONVERT(NVARCHAR, @FromDate, 106)),
                                   'yyyy-mm-dd hh:mm:ss');
    SET @LastWorkPeriodID = (SELECT TOP (1) ISNULL(Id, 0)
                                  FROM dbo.WorkPeriods
                                 WHERE StartDate = (   SELECT MAX(w.StartDate)
                                                         FROM dbo.WorkPeriods w
                                                        WHERE w.StartDate <> w.EndDate
                                                          AND w.StartDate < DATEADD(
                                                                                 DAY,
                                                                                 1,
                                                                                 CONVERT(NVARCHAR, @ToDate, 106)))
                                 ORDER BY Id);
    SET @FirstWorkPeriod = (   SELECT TOP (1) ISNULL(Id, 0)
                                 FROM dbo.WorkPeriods
                                WHERE StartDate <> EndDate
                                  AND StartDate = (   SELECT MIN(w.StartDate)
                                                        FROM dbo.WorkPeriods w
                                                       WHERE w.StartDate > CONVERT(NVARCHAR, @FromDate, 106))
                          ORDER BY Id);
    --SET @LastPeriodStartDate = (   SELECT TOP (1) StartDate
    --                                 FROM dbo.WorkPeriods
    --                                WHERE Id = @LastWorkPeriodID
    --                                ORDER BY Id);
    --SET @LastPeriodEndDate = (   SELECT TOP (1) EndDate
    --                               FROM dbo.WorkPeriods
    --                              WHERE Id = @LastWorkPeriodID
    --                              ORDER BY Id);
    SET @EndDate = dbo.ufsFormat((   SELECT TOP (1) EndDate
                                       FROM dbo.WorkPeriods
                                      WHERE Id = @LastWorkPeriodID
                                      ORDER BY Id),
                                 'yyyy-mm-dd hh:mm:ss');

    SELECT ISNULL(@StartDate, '18 sep 3030') StartDate,
           ISNULL(@EndDate, '18 sep 3030') EndDate,
           ISNULL(@FirstWorkPeriod, 0) FirstWorkPeriodID,
           ISNULL(@LastWorkPeriodID, 0) LastWorkPeriodID;
END;

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetStartEndDate] TO PUBLIC
    AS [dbo];


GO

