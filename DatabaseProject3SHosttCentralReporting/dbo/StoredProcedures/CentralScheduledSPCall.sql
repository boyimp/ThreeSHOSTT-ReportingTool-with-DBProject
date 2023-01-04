/* CentralStoredProcedure-2*/
CREATE PROCEDURE [dbo].[CentralScheduledSPCall]
(
	@HoursToCheck INT,
	@r NVARCHAR(max) out
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    
    -- The interval between cleanup attempts
    declare 
    @CountOfNotSyncedOutlets SMALLINT,
    @HaveReachedDayEnd SMALLINT,
    @WorkPeriodEndTime DATETIME, 
    @CurrentWorkPeriodId INT, 
    @CurrentWorkPeriodStartDate DATETIME,
    @PreviousWorkPeriodEndDate DATETIME,
	@PreviousWorkPeriodID INT, 
	@NotSyncedOutlets NVARCHAR(max)
    
    SET @CountOfNotSyncedOutlets = 0

	SET @PreviousWorkPeriodID = (SELECT workperiodid FROM PeriodicConsumptions WHERE id = (SELECT max(ID) FROM PeriodicConsumptions))
	SET @PreviousWorkPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE id = @PreviousWorkPeriodID)

	SET @CurrentWorkPeriodId = (SELECT ID FROM WorkPeriods WHERE StartDate = enddate AND id = (SELECT max(id) FROM WorkPeriods))
	SET @CurrentWorkPeriodStartDate = (SELECT startdate FROM WorkPeriods WHERE id = @CurrentWorkPeriodId)	
    SET @WorkPeriodEndTime = (SELECT 
    CASE WHEN @PreviousWorkPeriodEndDate IS NULL then
    Dateadd(hour, @HoursToCheck, @CurrentWorkPeriodStartDate)
    ELSE 
    Dateadd(hour, @HoursToCheck, @PreviousWorkPeriodEndDate)
    END)
    SET @CountOfNotSyncedOutlets = 
    (
    	SELECT count(*) FROM        
        (
        	SELECT * FROM SyncOutlets WHERE LastOutletWorkPeriodStarted < @WorkPeriodEndTime
        	UNION all
        	--SELECT * FROM SyncOutlets WHERE LastOutletWorkPeriodEnded < @WorkPeriodEndTime
        	--UNION ALL 
        	SELECT * FROM SyncOutlets WHERE LastSyncedOutletTime < @WorkPeriodEndTime
        ) Q
    )
    
    SELECT @NotSyncedOutlets = COALESCE(@NotSyncedOutlets + ', ', '') + Name 
	FROM        
    (
    	SELECT Name FROM SyncOutlets WHERE LastOutletWorkPeriodStarted < @WorkPeriodEndTime
    	UNION 
    	--SELECT Name FROM SyncOutlets WHERE LastOutletWorkPeriodEnded < @WorkPeriodEndTime
    	--UNION ALL 
    	SELECT Name FROM SyncOutlets WHERE LastSyncedOutletTime < @WorkPeriodEndTime
    ) Q
	SET @r ='Work Period End Attempt:' + (SELECT CONVERT(varchar,max(@WorkPeriodEndTime),113)) + ':: Not Synced Outlet Count: ' +  CAST(@CountOfNotSyncedOutlets AS VARCHAR(100)) + ':: Not Synced Outlets: '+ @NotSyncedOutlets + '::Last Ended Central Work Period: ' + (SELECT CONVERT(varchar,max(enddate),113) FROM WorkPeriods)
    IF @CountOfNotSyncedOutlets = 0
    BEGIN
    	EXEC Work_Period_End @WorkPeriodEndTime
    END	
    
END
COMMIT

GO

