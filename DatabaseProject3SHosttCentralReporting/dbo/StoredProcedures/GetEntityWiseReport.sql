
CREATE PROC [dbo].[GetEntityWiseReport]
(	
	@IsExactTime BIT, 	
	@StartDate DATETIME,
	@EndDate DATETIME,
	@TableOutlets TableValueIntParameters readonly,
	@TableDepartments TableValueIntParameters readonly,
	@TableEntities TableValueIntParameters readonly	
)
AS

	DECLARE @DateRange TABLE([StartDate] DATETIME, [EndDate] DATETIME, FirstWorkPeriodId INT , LastWorkPeriodID INT)	
	DECLARE @PropertyOrder TABLE([sortorder] int, [Id] int, Name NVARCHAR(max))
	
	DECLARE	
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
	

	SELECT @FDate FromDate, @TDate ToDate,
	en.EntityTypeId, en.EntityTypeName, en.EntityId, en.EntityName,
	PlainSum, VisitCount, DiscountAmount, Complimentary, PropertyName, PropertyValue
	--Entities and their custom properties-------------------------------------------------------
	FROM 
	(
		SELECT et.Id EntityTypeId, et.Name EntityTypeName, 
		en.Id EntityId, en.Name EntityName,
		isnull(enc.Name, et.Name) PropertyName, isnull(enc.Value, '-') PropertyValue
		FROM Entities en 
		LEFT OUTER JOIN EntityTypes et 
		ON en.EntityTypeId = et.Id
		LEFT OUTER JOIN 
		(
			SELECT e.Id EntityId,  e.Name EntityName
			, JSON_VALUE(States.value, '$.Name') Name
			, JSON_VALUE(States.value, '$.Value') Value
			FROM dbo.Entities e
			CROSS APPLY OPENJSON(convert(nvarchar(max), e.CustomData)) States
		)enc
		ON	en.Id = enc.EntityId
		WHERE en.Id IN 
		(
			SELECT value FROM @TableEntities
		)
	)En 
	--Entities and their custom properties---------------------------------------------------------
	--Fetching PlainSum-->Sales and visit count of selected entities
	LEFT OUTER JOIN 
	(
		SELECT te.EntityId, sum(t.PlainSum) PlainSum, count(*) VisitCount
		FROM 
		TicketEntities te 
		LEFT OUTER JOIN 
		Tickets t
		ON  t.Id = te.Ticket_Id			
		WHERE t.[Date] BETWEEN @FDate AND @TDate
		AND t.SyncOutletId IN
		(
			SELECT value FROM @TableOutlets
		)
		AND t.departmentId IN
		(
			SELECT value FROM @TableDepartments
		)
		AND te.entityId IN 
		(
			SELECT value FROM @TableEntities
		)
		GROUP BY te.EntityId
	)Tick 
	ON EN.EntityId = Tick.EntityId
	--Fetching PlainSum-->Sales and visit count of selected entities
	--Fetching Discounts given to selected entities
	LEFT OUTER JOIN 
	(
	
		SELECT te.EntityId, sum(calculationamount)DiscountAmount 
		FROM Calculations c, Tickets t, TicketEntities te
		WHERE c.TicketId = t.Id
		AND t.Id = te.Ticket_Id
		AND c.DecreaseAmount = 1
		AND t.[Date] BETWEEN @FDate AND @TDate
		AND t.SyncOutletId IN
		(
			SELECT value FROM @TableOutlets
		)
		AND t.departmentId IN
		(
			SELECT value FROM @TableDepartments
		)
		AND te.entityId IN 
		(
			SELECT value FROM @TableEntities
		)
		GROUP BY te.EntityId
	)Dis
	ON En.EntityId = Dis.EntityId
	--Fetching Discounts given to selected entities
	--Fetching Complimentary given to selected entities
	LEFT OUTER JOIN 
	(
		SELECT 
		te.EntityId, sum(isnull(o.UptoAll, 0)) Complimentary
		FROM 
		tickets t, Orders o, TicketEntities te
		WHERE o.TicketId = t.Id
		AND te.Ticket_Id = t.Id
		AND o.DecreaseInventory = 1
		AND o.CalculatePrice = 0
		AND t.[Date] BETWEEN @FDate AND @TDate
		AND t.SyncOutletId IN
		(
			SELECT value FROM @TableOutlets
		)
		AND t.departmentId IN
		(
			SELECT value FROM @TableDepartments
		)
		AND te.entityId IN 
		(
			SELECT value FROM @TableEntities
		)
		GROUP BY te.EntityId
	) Compl
	ON En.EntityId = Compl.EntityId
	--Fetching Complimentary given to selected entities

GRANT EXECUTE ON dbo.GetEntityWiseReport TO PUBLIC

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetEntityWiseReport] TO PUBLIC
    AS [dbo];


GO

