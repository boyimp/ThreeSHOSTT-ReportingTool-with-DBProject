
CREATE PROC [dbo].[GetTickets]
(		
	@StartDate DATETIME,
	@EndDate DATETIME,
	@TableOutlets TableValueIntParameters readonly,
	@TableDepartments TableValueIntParameters readonly	
)
AS

	SELECT t.Id TicketId,
	ticketnumber, o.Name OutletName, d.Name DepartmentName, t.SettledBy,
	'Customer'= (Isnull(
				(
					SELECT isnull(Name, '') Name 
					FROM Entities e, TicketEntities te 
					WHERE e.Id = te.EntityId
					AND te.EntityTypeId = 1--Customer = 1 
					AND te.Ticket_Id = t.Id
				), '')),
	'Table'= (Isnull(
	(
		SELECT isnull(Name, '') Name 
		FROM Entities e, TicketEntities te 
		WHERE e.Id = te.EntityId
		AND te.EntityTypeId = 2--Customer = 1 
		AND te.Ticket_Id = t.Id
	), '')),
	'Waiter'= (Isnull(
	(
		SELECT isnull(Name, '') Name 
		FROM Entities e, TicketEntities te 
		WHERE e.Id = te.EntityId
		AND te.EntityTypeId = 3--Customer = 1 
		AND te.Ticket_Id = t.Id
	), '')), 
	[Date] CreatedDate, t.LastUpdateTime, datediff(minute, [Date], t.LastUpdateTime)TimeSpent, 
	CASE isclosed
	WHEN 1 THEN 'Closed'
	ELSE 'Open'
	END isClosed
	, isnull(t.Note, '-') Note, t.NoOfGuests, t.TotalAmount
	FROM tickets t, Departments d, SyncOutlets o
	WHERE t.WorkPeriodStartDate BETWEEN @StartDate AND @EndDate
	AND o.Id IN
	(
		SELECT value FROM @TableOutlets
	)
	AND d.Id IN
	(
		SELECT value FROM @TableDepartments
	)
	AND t.DepartmentId = d.Id
	AND t.SyncOutletId = o.Id

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[GetTickets] TO PUBLIC
    AS [dbo];


GO

