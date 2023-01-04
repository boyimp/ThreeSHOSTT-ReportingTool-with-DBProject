CREATE VIEW [dbo].[HHTTickets] as
SELECT t.Id, t.HHTTicketId, t.DepartmentId, t.TicketTypeId, t.Note as TicketNote, 
t.Name as CreatedBy, t.NoOfGuests, t.[PlainSum], t.[TaxTotal], t.[TotalAmount],
 t.[Date] as CreateDate, t.LastUpdateTime, t.IsLocked, t.IsClosed,t.[Name]  FROM Tickets t WHERE [Date] >=
(SELECT max(startdate) 
FROM WorkPeriods 
WHERE StartDate = enddate);

GO

