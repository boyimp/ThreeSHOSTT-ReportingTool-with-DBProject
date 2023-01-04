
create view [dbo].[LastTwelveMonthTran]
AS
WITH LastDate (maxdate)
AS 
(
	SELECT max(Date) FROM tickets
) 

SELECT [Row], cast(month AS VARCHAR(10))+ ''''+ cast(Year AS VARCHAR(10)) MonthYear, PaymentAccountType, UltimateAccount, SyncOutletID, --PaymentTypeId, PaymentTypeName, 
sum(Amount) Amount
from
(
    SELECT  
    Month = (SELECT Month FROM
                ( 	
                    SELECT ROW_NUMBER() OVER(ORDER BY start asc) AS Row, datename(month, start)Month, * FROM Last12Months(getdate())
                )M WHERE convert(NVARCHAR,P.StartDate, 106) BETWEEN start AND enddate
            ),
        Row = (SELECT Row FROM
                ( 	
                    SELECT ROW_NUMBER() OVER(ORDER BY start asc) AS Row, datename(month, start)Month, * FROM Last12Months(getdate())
                )M WHERE convert(NVARCHAR,P.StartDate, 106) BETWEEN start AND enddate
            ),
            Year(P.StartDate) Year, P.TicketNumber, p.UltimateAccount, P.PaymentAccountType, p.SyncOutletID, p.Amount Amount
    FROM  
    (
    	SELECT  'WorkPeriodID' = 
		(
			SELECT ID 
			FROM 
			(
				SELECT Id, StartDate, 
				CASE
					WHEN startdate = enddate THEN (SELECT maxdate FROM LastDate)
					ELSE enddate
				END as	EndDate, StartDescription, EndDescription, Name
				FROM WorkPeriods
			)w
			WHERE t.ticketdate BETWEEN startdate AND enddate
		),
		'WorkPeriodDescription' = 
		(
			SELECT convert(varchar(20), startdate, 113)+ '--'+ convert(varchar(20), enddate, 113)
			FROM 
			(
				SELECT Id, StartDate, 
				CASE
					WHEN startdate = enddate THEN (SELECT maxdate FROM LastDate)
					ELSE enddate
				END as	EndDate, StartDescription, EndDescription, Name
				FROM WorkPeriods
			)w
			WHERE t.ticketdate BETWEEN startdate AND enddate
		),
		'StartDate' = 
		(
			SELECT StartDate
			FROM 
			(
				SELECT Id, StartDate, 
				CASE
					WHEN startdate = enddate THEN (SELECT maxdate FROM LastDate)
					ELSE enddate
				END as	EndDate, StartDescription, EndDescription, Name
				FROM WorkPeriods
			)w
			WHERE t.ticketdate BETWEEN startdate AND enddate
		),
		'EndDate' = 
		(
			SELECT EndDate
			FROM 
			(
				SELECT Id, StartDate, 
				CASE
					WHEN startdate = enddate THEN (SELECT maxdate FROM LastDate)
					ELSE enddate
				END as	EndDate, StartDescription, EndDescription, Name
				FROM WorkPeriods
			)w
			WHERE t.ticketdate BETWEEN startdate AND enddate
		),
		t.SyncOutletID, DepartmentName, TicketId, TicketNumber, TicketDate, SortOrder, PaymentAccountType, Payment, UltimateAccount,Amount
		from TicketView t
		WHERE UltimateAccount IS NOT NULL
    )P
	--WHERE P.PaymentAccountType IS NULL
	--WHERE P.PaymentAccountType IS not NULL
)MonthlyTickets
WHERE month IS NOT NULL

GROUP BY [row], month,Year, UltimateAccount, PaymentAccountType, SyncOutletID--, PaymentTypeId, PaymentTypeName
--ORDER BY row

GO

GRANT SELECT
    ON OBJECT::[dbo].[LastTwelveMonthTran] TO PUBLIC
    AS [dbo];


GO

