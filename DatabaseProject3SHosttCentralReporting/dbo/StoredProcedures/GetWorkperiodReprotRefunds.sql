
Create PROC [dbo].[GetWorkperiodReprotRefunds]
(	
	@IsExactTime BIT, 	
	@StartDate DATETIME,
	@EndDate DATETIME,
	@Outlets NVARCHAR(max),
	@Departments NVARCHAR(max)
)
AS
----
IF @Outlets = '0'
BEGIN 
SET @Outlets = (
					SELECT TOP 1 Ids FROM (
					SELECT Ids = STUFF( (SELECT (','+CAST(Id AS VARCHAR(10)))
					                      FROM syncoutlets                       
					                      FOR XML PATH('')
					                     ), 1, 1, ''
					                   ) 
					FROM syncoutlets t
					)Q
				)
END 

IF @Departments = '0'
BEGIN 
SET @Departments = (
					SELECT TOP 1 Ids FROM (
					SELECT Ids = STUFF( (SELECT (','+CAST(Id AS VARCHAR(10)))
					                      FROM Departments                       
					                      FOR XML PATH('')
					                     ), 1, 1, ''
					                   ) 
					FROM Departments t
					)Q
				)
END 
DECLARE	
@DateRange TABLE([StartDate] DATETIME, [EndDate] DATETIME, FirstWorkPeriodId INT , LastWorkPeriodID INT)
Declare 
@FDate AS DATETIME,
@TDate AS DATETIME
	

IF @IsExactTime = 0
BEGIN
	INSERT INTO @DateRange 
	EXEC GetStartEndDate @StartDate, @EndDate
	SET @FDate = (SELECT StartDate FROM @DateRange )
	SET @TDate = (SELECT EndDate FROM @DateRange )
END;
ELSE
BEGIN
	SET @FDate = @StartDate
	SET @TDate = @EndDate
END

DECLARE 
@FirstWorkPeriodId AS INT,
@LastWorkPeriodId AS INT 

SET @StartDate = (SELECT max(StartDate) FROM @DateRange)
SET @EndDate = (SELECT max(EndDate) FROM @DateRange)
SET @FirstWorkPeriodId = (SELECT max(FirstWorkPeriodId) FROM @DateRange)
SET @LastWorkPeriodId = (SELECT max(LastWorkPeriodId) FROM @DateRange);

declare @totalrefund as money
set @totalrefund= (select sum(Amount) from Payments, Tickets t
where Amount like '-%'
and Payments.TicketId=t.Id
and t.[Date] between @FDate and @TDate
and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ',')))

select Perspective, [VALUE]  from 
(
    select 'key'=(1), p.Name as Perspective, sum(p.Amount) as VALUE 
	from Payments p, Tickets t
    where t.date BETWEEN @FDate AND @TDate
    and p.Amount like '-%'
    and p.TicketId=t.Id
    and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
    and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ','))
    group by p.Name

	UNION

	select 'key'=(1.5), 'Total Refund' as Perspective,  @totalrefund AS Value
) wp 
order by wp.[key]

GO

