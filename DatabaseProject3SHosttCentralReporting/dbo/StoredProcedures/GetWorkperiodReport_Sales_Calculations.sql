
CREATE  PROC [dbo].[GetWorkperiodReport_Sales_Calculations]
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

	declare
	@totalsalesvalue as money,
	@totalcalculationvalue as money,
	@totaldiscount as money,
    @totalnetvalue as money

	set @totalsalesvalue= (select sum(t.PlainSum) from Tickets t
    where date BETWEEN @FDate AND @TDate
    and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
	and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ',')))

	set @totalcalculationvalue= (select sum(CalculationAmount) from Calculations c, Tickets t
    where c.TicketId=t.Id
    and t.Date BETWEEN @FDate AND @TDate
    and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
	and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ',')))

	set @totaldiscount = (select sum(CalculationAmount) from Calculations c, Tickets t
    where c.TicketId=t.Id
    and IsDiscount=1
    and t.Date BETWEEN @FDate AND @TDate
    and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
	and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ',')))
    
    set @totalnetvalue = @totalcalculationvalue+@totalsalesvalue


select Perspective Sales, [VALUE] Net, Percentage Gross from 
(
	SELECT 'key'=(1),
	tt.Name Perspective, SUM(t.PlainSum) AS Value, FORMAT(( SUM(t.PlainSum)/@totalsalesvalue),'P') Percentage
	FROM dbo.Tickets AS t INNER JOIN
	dbo.TicketTypes AS tt ON t.TicketTypeId = tt.Id 
	WHERE t.date BETWEEN @FDate AND @TDate
	and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
	and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ','))
	group by tt.Name  

	union

	select 'key'=(1.5), 'Sales Total' as Perspective, @totalsalesvalue AS Value, FORMAT (@totalsalesvalue/@totalsalesvalue,'P')  as Percentage

	union

	SELECT 'key'=(2),
	c.Name Perspective, Sum(c.CalculationAmount) Value, FORMAT((sum(c.CalculationAmount)/@totaldiscount),'P') as Percentage
	FROM tickets t, Calculations c
	WHERE t.Id = c.TicketId
	AND t.[Date] BETWEEN @FDate AND @TDate
	and c.IsDiscount=1
	and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
	and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ','))
	GROUP BY  c.Name  

	union

	select 'key'=(2.5), 'Discount Total' as Perspective, @totaldiscount AS Value, FORMAT (@totaldiscount/@totaldiscount,'P')  as Percentage

	union

	select 'key'=(2.6), 'Net Total' as Perspective, @totalsalesvalue+@totaldiscount AS Value, '-' as Percentage

	union

	SELECT 'key'=(3),
	c.Name Perspective, Sum(c.CalculationAmount) Value, '-' as Percentage
	FROM tickets t, Calculations c
	WHERE t.Id = c.TicketId
	AND t.[Date] BETWEEN @FDate AND @TDate
	and c.IsDiscount<>1  --select * from calculations
	and t.DepartmentId in (SELECT * FROM string_split(@Departments, ','))
	and t.SyncOutletId in (SELECT * FROM string_split(@Outlets, ','))
	GROUP BY (c.Name) 

	union

    select 'key'=(3.5), 'Grand Total' as Perspective, @totalnetvalue AS Value, '-' as Percentage
   
) wp 
order by wp.[key]

GO

