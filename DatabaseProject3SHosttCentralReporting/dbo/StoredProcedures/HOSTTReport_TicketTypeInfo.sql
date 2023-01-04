CREATE PROCEDURE [dbo].[HOSTTReport_TicketTypeInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 
    DECLARE @T  
    TABLE(Header1 varchar(max) --TicketTypeID 
    , Header2 varchar(max) --TycketTypeName 
    , Value1 DECIMAL(18,0) --TicketCount 
    , Value2 DECIMAL(18,4) --AmountExcludingDiscount 
    ,  Value3 DECIMAL(18,4) --Amount 
    ,  Value4 DECIMAL(18,4)--GrossAmount 
    ,  Value5 DECIMAL(18,4) --Tax 
    ,  Value6 DECIMAL(18,4) --Services 
    ,  Value7 DECIMAL(18,4) --PreServices 
    )

    INSERT INTO @T
    SELECT  
    t.tickettypeid, tt.Name
    , count(*) TicketCount
    ,sum(t.Sum - t.TaxTotal + t.PostTaxDiscountServicesTotal 
    + t.PreTaxDiscountServicesTotal 
    - t.PostTaxServicesTotal - t.PreTaxServicesTotal) AmountExcludingDiscount
    , sum(t.Sum - t.TaxTotal - t.PostTaxServicesTotal - t.PreTaxServicesTotal) Amount
    , sum(t.PlainSumForEndOfPeriod) GrossAmount
    , sum(t.TaxTotal) Tax
    , sum(t.PostTaxServicesTotal) Services
    ,sum(t.PreTaxServicesTotal) PreServices
    FROM Tickets t, TicketTypes tt WHERE LastPaymentDate BETWEEN @StartDate AND @EndDate
    AND t.TicketTypeId = tt.Id
    AND t.DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
    GROUP BY t.tickettypeid, tt.Name
    SELECT * FROM @T
End

GO

