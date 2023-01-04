CREATE VIEW [dbo].[HHTTableStates] 
as
select tableStates.*, ISNULL(ticketCount.noOfOpenTicket,0) NoOfOpenTicket from (select e.Id, e.Name, s.EntityStates, st.Color FROM Entities e, EntityStateValues s, States st
WHERE e.Id = s.EntityId
AND s.EntityStates LIKE '%'+st.Name+'%'
And e.EntityTypeId = 2) tableStates
left join (select entityid as tableid, count(Ticket_Id) noOfOpenTicket from TicketEntities where ticket_id in (select id from hhttickets where IsLocked = 0 
AND IsClosed = 0) and entitytypeid = 2
group by entityid) ticketCount on tableStates.Id = ticketCount.tableid

GO

