CREATE PROC [dbo].[GiftStateWise_SP]
(   
    @IsExactTime BIT,   
    @StartDate DATETIME,
    @EndDate DATETIME,
    @TableOutlets TableValueIntParameters readonly,
    @TableDepartments TableValueIntParameters readonly,
    @ActionType TableValueStringParameters readonly
  
)
AS
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
SET @StartDate = (SELECT max(StartDate) FROM @DateRange);
SET @EndDate = (SELECT max(EndDate) FROM @DateRange);
SET @FirstWorkPeriodId = (SELECT max(FirstWorkPeriodId) FROM @DateRange);
SET @LastWorkPeriodId = (SELECT max(LastWorkPeriodId) FROM @DateRange);         
With TicketState as 
(
    select
    tkt.SyncOutletId,
    tkt.Id,
    tkt.TicketNumber,
    tkt.Date,
    tkt.TotalAmount,
    CAST(tkt.Note AS NVARCHAR(100)) Note,
    tkt.IsClosed,
    tkt.LastUpdateTime,
    JSON_VALUE(TicketStates.value, '$.S') TicketStates
    from  Tickets as tkt
    LEFT OUTER join TicketTypes as tktType on tktType.Id = tkt.TicketTypeId 
    cross APPLY OPENJSON(convert(nvarchar(max),tkt.TicketStates)) TicketStates
    WHERE tkt.Date BETWEEN @FDate AND @TDate
    and tkt.SyncOutletId IN (SELECT value FROM @TableOutlets)
    AND tkt.departmentId IN(SELECT value FROM @TableDepartments)
)
SELECT Tick.*,
                    OrderId,
                    Entry_By,
                    Order_Time,
                    Action_Type,
                    Approved_By,
                    Login_By,
                    Action_Time,
                    Reason,
                    Menu_Item_Name Menu_Item,
                    Quantity, 
                    Price, 
                    Product_Price_Total Total
                FROM 
            (
                select
                    t.OutletName,
                    t.Id TicketId,
                    t.TicketNumber,
                    'Customers'= (Isnull(
                                    (
                                        SELECT isnull(Name, '') Name 
                                        FROM Entities e, TicketEntities te 
                                        WHERE e.Id = te.EntityId
                                        AND te.EntityTypeId = 1--Customer = 1 
                                        AND te.Ticket_Id = t.Id
                                    ), '')),
                    'Tables'= (Isnull    (   
                                    (
                                        SELECT isnull(Name, '') Name 
                                        FROM Entities e, TicketEntities te 
                                        WHERE e.Id = te.EntityId
                                        AND te.EntityTypeId in (2,7)--Customer = 1  
                                        AND te.Ticket_Id = t.Id
                                    ), '')
                                ),
                    'Waiters'= (Isnull(
                                    (
                                        SELECT isnull(Name, '') Name 
                                        FROM Entities e, TicketEntities te 
                                        WHERE e.Id = te.EntityId
                                        AND te.EntityTypeId in (3,6)
                                        AND te.Ticket_Id = t.Id
                                    ), '')),
                    Format(cast(t.Date as datetime),'dd-MMM-yyyy hh:mm:ss tt','en-us') Date,
                    LTRIM(RIGHT(CONVERT(VARCHAR(20), t.Date, 100), 7)) as Opening,
                    LTRIM(RIGHT(CONVERT(VARCHAR(20), t.LastUpdateTime, 100), 7)) as Closing,
                    t.Note,
                    t.StateName TicketStates
                from 
                (
                    SELECT O.Name OutletName, a.Id, TicketNumber,   [Date], TotalAmount,    Note,   IsClosed, LastUpdateTime,
                    StateName = 
                        STUFF((SELECT ', ' + TicketStates
                               FROM TicketState b 
                               WHERE b.Id = a.Id 
                              FOR XML PATH('')), 1, 2, '')
                    FROM TicketState a, SyncOutlets O
                    where a.LastUpdateTime between @FDate and @TDate
                    and o.Id=a.SyncOutletId
                    
                    GROUP BY O.Name, a.Id,  TicketNumber,   [Date], TotalAmount,    Note,   IsClosed,   LastUpdateTime                  
                )t 
                group by    t.OutletName, t.id, t.TicketNumber, t.Date, t.LastUpdateTime, t.TotalAmount, t.Note, t.StateName, t.Isclosed
                            
            )Tick INNER JOIN 
            (
                SELECT
                    
                    Orders.Id OrderId,
                    TicketId,
                    Tickets.TicketNumber Ticket_Number, 
                    tickets.LastOrderDate Ticket_Date_Time,
                    Format(cast(orders.CreatedDateTime as datetime),'dd-MMM-yyyy hh:mm:ss tt','en-us') Order_Time,
                    orders.CreatingUserName Entry_By,
                    JSON_VALUE(OrderStates.value, '$.S') Action_Type,
                    JSON_VALUE(OrderStates.value, '$.UN') Login_By,
                    JSON_VALUE(OrderStates.value, '$.AN') Approved_By,
                    Format(cast(JSON_VALUE(OrderStates.value, '$.SD') as datetime),'dd-MMM-yyyy hh:mm:ss tt','en-us') Action_Time,
                    isnull(JSON_VALUE(OrderStates.value, '$.SV'),'') Reason,
                    Orders.MenuItemName Menu_Item_Name,
                    Price, 
                    cast(Quantity AS int) Quantity,
                    cast((Quantity*Price) AS DECIMAL(18,2)) Product_Price_Total
                FROM Tickets , orders cross APPLY OPENJSON(convert(nvarchar(max),orders.OrderStates)) OrderStates
                WHERE   tickets.Id = orders.TicketId
                        AND JSON_VALUE(OrderStates.value, '$.S') in ( SELECT value FROM @ActionType )  --'%Void%'
                        --AND orders.CreatedDateTime between @FDate and @TDate  
                        AND tickets.id IN
                                (   
                                    SELECT Tickets.Id
                                    FROM Tickets 
                                    WHERE Tickets.Date BETWEEN @FDate AND @TDate
                                    and Tickets.SyncOutletId IN(SELECT value FROM @TableOutlets)
                                    AND Tickets.departmentId IN(SELECT value FROM @TableDepartments)
                                    
                                )
                                 
            )TickOrder
            on tick.TicketId = tickorder.ticketid
            ORDER BY Order_Time desc;

GO

