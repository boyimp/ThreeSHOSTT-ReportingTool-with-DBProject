CREATE PROCEDURE [dbo].[HOSTTReport_ItemSalesGroupAmountInfo](
    @StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
)
AS 
BEGIN 
    select MenuGroupName Header1, (Amount/Total)*100 Value2, Amount Value3 from (
	   select MenuGroupName, 
	   'Total' = 
	   (
		  select Sum(Total)
		  from orders
		  where CreatedDateTime between @StartDate AND @EndDate and DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
	   ),
	   Sum(Total) Amount
	   from orders
	   where CreatedDateTime between @StartDate AND @EndDate and DepartmentId in (Select Id from fnSplitter(@UserPermittedDepartmentCommaSeparatedIds))
	   group by MenuGroupName
    )T
    Order by Amount Desc
END

GO

