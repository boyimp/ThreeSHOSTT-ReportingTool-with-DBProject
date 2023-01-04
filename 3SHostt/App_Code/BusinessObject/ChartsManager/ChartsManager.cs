using BusinessObjects.TicketsManager;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ThreeS.Report.v2.Controllers;

namespace BusinessObjects.ChartsManager
{
    public class ChartsManager
    {
        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetSyncOutletsWithDeletedSyncOutlets()
        {
            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"     SELECT 
                                    [SyncOutlets].[Id], 
                                    [SyncOutlets].[Name]
                                FROM [SyncOutlets]
                            UNION
                                SELECT DISTINCT 
                                    [Tickets].[SyncOutletId], 
                                    IIF( [Tickets].[SyncOutletId] = 0 ,'0 - DEFAULT' ,CONCAT([Tickets].[SyncOutletId],' - DELETED'))
                                FROM [Tickets]
                                    WHERE [Tickets].[SyncOutletId] NOT IN (
                                                                            SELECT 
                                                                                [SyncOutlets].[Id]
                                                                            FROM [SyncOutlets]
                                                                           )";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetDepartmentsWithDeletedDepartments()
        {
            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"     SELECT 
                                    [Departments].[Id], 
                                    [Departments].[Name]
                                FROM [Departments]
                            UNION
                                SELECT DISTINCT 
                                    [Tickets].[DepartmentId], 
                                    IIF( [Tickets].[DepartmentId] = 0 ,'0 - DEFAULT' ,CONCAT([Tickets].[DepartmentId],' - DELETED'))
                                FROM [Tickets]
                                    WHERE [Tickets].[DepartmentId] NOT IN (
                                                                            SELECT 
                                                                                [Departments].[Id]
                                                                            FROM [Departments]
                                                                           )";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetMenuItemsWithDeletedMenuItems()
        {
            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"     SELECT 
                                    [MenuItems].[Id], 
                                    [MenuItems].[Name]
                                FROM [MenuItems]
                            UNION
                                SELECT DISTINCT 
                                    [Orders].[MenuItemId], 
                                    IIF( [Orders].[MenuItemId] = 0 ,'0 - DEFAULT' ,CONCAT([Orders].[MenuItemId],' - DELETED'))
                                FROM [Orders]
                                    WHERE [Orders].[MenuItemId] NOT IN (
                                                                        SELECT 
                                                                            [MenuItems].[Id]
                                                                        FROM [MenuItems]
                                                                        )";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetMenuItemGroupsWithDeletedMenuItemGroups()
        {
            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"     SELECT DISTINCT 
                                    [MenuItems].[GroupCode]
                                FROM [MenuItems]
                            UNION
                                SELECT DISTINCT 
                                    CONCAT([Orders].[MenuGroupName],' - DELETED')
                                FROM [Orders]
                                    WHERE [Orders].[MenuGroupName] NOT IN (
                                                                            SELECT DISTINCT 
                                                                                [MenuItems].[GroupCode]
                                                                            FROM [MenuItems]
                                                                           )";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetDailySalesParameterized(
            DateTime from, DateTime to,
            List<int> outlets, List<int> departments,
            List<int> menuItems, List<string> menuItemCategoris)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach
            string menuItemAsParameter = "0";
            foreach (int menuItem in menuItems)
            {
                menuItemAsParameter += "," + menuItem.ToString();
            }//foreach

            int count = 0;
            string menuItemCategoriesAsParameter = "";
            foreach (string menuItemCategory in menuItemCategoris)
            {
                if (count == 0)
                {
                    menuItemCategoriesAsParameter = "'" + menuItemCategory + "'";
                }
                else
                {
                    menuItemCategoriesAsParameter += "," + "'" + menuItemCategory + "'";
                }//else
                count++;
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT
                                [StartDate] ,
                                FLOOR(SUM([TicketTotalAmount])) AS [TicketTotalAmount] ,
                                FLOOR(SUM(sales)) AS [Sales]
                                FROM ( SELECT [WorkPeriodId] , CONVERT(NVARCHAR , [StartDate] , 106) AS [StartDate] , [TicketTotalAmount] , [Sales]
                        FROM ( SELECT *
                        FROM ( SELECT
                                    TOP(9223372036854775807)
                                    [WorkPeriodId] ,
                                    'StartDate' = ( SELECT [StartDate]
                                                    FROM [WorkPeriods]
                                                    WHERE [Id] = [WorkPeriodID] ),
                        SUM([TotalAmount]) AS [TicketTotalAmount] , SUM([Credit]) AS [Sales]
                        FROM ( SELECT WorkPeriodID = ( SELECT MAX(ID)
                        FROM ( SELECT [ID] , [StartDate] ,
                            CASE [EndDate]
                                WHEN [StartDate]
                                    THEN ( SELECT [dbo].[ufsFormat] ( DATEADD(second , 1 , MAX(Date)) , 'yyyy-mm-dd hh:mm:ss')
                            FROM [AccountTransacTionValues] )
                                    ELSE [EndDate]
                            END AS [EndDate]
                        FROM [WorkPeriods] ) AS W
                            WHERE [StartDate] < t.[Date]
                            AND [EndDate] >= t.[Date]
                                            ) , [TotalAmount], [Credit]
                        FROM ( SELECT [Tickets].[TotalAmount], [Tickets].[Date] , [Credit]
                        FROM [Tickets], Orders o , ( SELECT [AccountTransacTionDocuments].[Id] AS [TranId] , SUM([AccountTransacTionValues].[Credit]) AS Credit
                        FROM [AccountTransacTionDocuments] , [AccountTransacTionValues], [Accounts] , [AccountTypes]
                            WHERE [AccountTransacTionValues].[AccountTypeId] = [AccountTypes].[Id]
                                AND [AccountTransacTionDocuments].Id = [AccountTransacTionValues].[AccountTransacTionDocumenTId]
                                AND [Accounts].[Id] = [AccountTransacTionValues].[AccounTId]
                                AND [AccountTransacTionValues].[AccounTId] = 1
                        GROUP BY [AccountTransacTionDocuments].[Id] ) AS A
                            WHERE A.[TranId] = [Tickets].[TransactionDocument_Id]
                            and [Tickets].Id = o.TicketId
                            AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                            AND [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                            AND o.MenuItemId in ({menuItemAsParameter})
                            AND o.MenuGroupName in ({menuItemCategoriesAsParameter})
                            AND [Tickets].[Date]
                        BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                            ) AS t ) AS W
                        GROUP BY [WorkPeriodId]
                        ORDER BY [WorkPeriodId] DESC
                                           ) AS Q
                                    ) AS T
                             ) AS U
                GROUP BY [StartDate]
                ORDER BY CAST([StartDate] AS DATE) DESC";


            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetTimeWiseSalesMultiParameterized(
            DateTime from, DateTime to, List<int> outlets, List<int> departments, 
            List<int> menuItems, List<string> menuItemCategoris)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string menuItemAsParameter = "0";
            foreach (int menuItem in menuItems)
            {
                menuItemAsParameter += "," + menuItem.ToString();
            }//foreach

            int count = 0;
            string menuItemCategoriesAsParameter = "";
            foreach (string menuItemCategory in menuItemCategoris)
            {
                if (count == 0)
                {
                    menuItemCategoriesAsParameter = "'" + menuItemCategory + "'";
                }
                else
                {
                    menuItemCategoriesAsParameter += "," + "'" + menuItemCategory + "'";
                }//else
                count++;

            }//foreach


            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);

            //Before GrandTotal coming from Ticket.TotalAmount and Sales from Ticket.PlainSum
            //NoOfTickets is not match with legacy report because empty tickets is excluded in Order Table.
            //NoOfGaust provides wrong value.(Need to work on it)
            var query = $@"
                            WITH
                                [TEMPORARY]
                                    AS
                                        (
                                            SELECT 0 AS [HourFromTicketDate], 1 AS [HourForTimeRange]
                                        UNION ALL
                                            SELECT [HourFromTicketDate] + 1, [HourForTimeRange] + 1
                                            FROM [TEMPORARY]
                                                WHERE [HourFromTicketDate] + 1 < 24
                                        )
                            SELECT *
                            FROM
                                (SELECT
                                        CONVERT(TIME, DATEADD([HOUR], [TEMPORARY].[HourFromTicketDate], 0)) AS HOURS24,
                                        CAST(CONVERT(TIME(0), DATEADD([HOUR], [TEMPORARY].[HourFromTicketDate], 0)) AS VARCHAR) +'-' +CAST(CONVERT(TIME(0), DATEADD([HOUR], [TEMPORARY].[HourForTimeRange], 0)) AS VARCHAR) AS [TimeRange],
                                        COALESCE([NumberOfTickets],0) AS [NumberOfTickets],
                                        COALESCE(GrandTotal,0) AS [GrandTotal],
                                        CONVERT(Decimal(20,2),
                                        COALESCE([Sales],0)) AS [Sales],
                                        [NumberOfGuests]
                                FROM [TEMPORARY]
                            LEFT JOIN
                                ( SELECT
                                        SUM([Orders].[UptoAll]) AS [GrandTotal],
                                        Sum([Orders].[Total]) AS [Sales],
                                        COUNT(DISTINCT([Orders].[TicketId])) AS [NumberOfTickets],
                                        SUM([Tickets].[NoOfGuests]) AS [NumberOfGuests],
                                        DATEPART([HOUR],[Tickets].[Date]) AS [HourFromTicketDate]
                                  FROM [Tickets] JOIN [Orders]
                                    ON [Orders].[TicketId] = [Tickets].[Id]
                                        AND [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                                        AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                        AND [Orders].[MenuItemId] IN ({menuItemAsParameter})
                                        AND [Orders].[MenuGroupName] IN ({menuItemCategoriesAsParameter})
                                        AND [Orders].[DecreaseInventory] = 1
                                        AND [Tickets].[Date] BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                            GROUP BY  DATEPART([HOUR],[Tickets].[Date])) AS [SUB_QUERY_1]
                                ON [SUB_QUERY_1].[HourFromTicketDate] = [TEMPORARY].[HourFromTicketDate] ) AS [SUB_QUERY_2]
                                    WHERE [NumberOfTickets] > 0
                            ORDER BY [HOURS24]";


            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetDayTimeWiseSalesMultiParameterized(DateTime from, DateTime to,
            List<int> outlets, List<int> departments, List<int> menuItems, List<string> menuItemCategoris)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string menuItemAsParameter = "0";
            foreach (int menuItem in menuItems)
            {
                menuItemAsParameter += "," + menuItem.ToString();
            }//foreach

            int count = 0;
            string menuItemCategoriesAsParameter = "";
            foreach (string menuItemCategory in menuItemCategoris)
            {
                if (count == 0)
                {
                    menuItemCategoriesAsParameter = "'" + menuItemCategory + "'";
                }
                else
                {
                    menuItemCategoriesAsParameter += "," + "'" + menuItemCategory + "'";
                }//else
                count++;

            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT
                                DATENAME(WEEKDAY, [TEMPORARY].[TicketDate]) AS [WeekDay],
                                DATEPART(HOUR, [TEMPORARY].[TicketDate]) AS [Hour],
                                COUNT([TEMPORARY].[TicketId]) AS [TicketQuantity],
                                SUM([TEMPORARY].[OrderCount]) AS [OrderQuantity],
                                SUM([TEMPORARY].[SalesTotal]) AS [Price]
                            FROM
                                (
                                    SELECT
                                        [Tickets].[Id] AS [TicketId],
                                        [Tickets].[Date] AS [TicketDate],
                                        CAST(SUM([Orders].[Quantity]) AS INT) AS [OrderCount],
                                        CAST(SUM([Orders].[PlainTotal]) AS DECIMAL(30,10) ) AS [SalesTotal]
                                    FROM [Orders] , [Tickets]
                                        WHERE [Orders].[TicketId] = [Tickets].[Id]
                                            AND [Tickets].[Date] 
                                        BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                            AND [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                                            AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                            AND [Orders].[MenuItemId] IN ({menuItemAsParameter})
                                            AND [Orders].[MenuGroupName] IN ({menuItemCategoriesAsParameter})
                                            AND [Orders].[DecreaseInventory] = 1
                                    GROUP BY [Tickets].[Id] , [Tickets].[Date], [Tickets].[TotalAmount]
                                ) AS [TEMPORARY]
                            GROUP BY DATEPART(WEEKDAY, [TicketDate]) , DATENAME(WEEKDAY, [TicketDate]), DATEPART(HOUR, [TicketDate])";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetSalesYearly(
            List<int> outlets, List<int> departments, string years)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT
    YEAR([Tickets].[Date]) AS [YEAR],
    DATENAME( MONTH, DATEADD( MONTH, 0, [Tickets].[Date])) AS [Month],
    Count([TicketTypes].[Name]) AS [Quantity],
    CAST(SUM([Tickets].[PlainSum]) AS DECIMAL(30,2)) AS [NetAmount],
    CAST(SUM([Tickets].[TotalAmount]) AS DECIMAL(30,2)) AS [GrossAmount]
FROM [Tickets]
INNER JOIN [TicketTypes]
    On [Tickets].[TicketTypeId] = [TicketTypes].[Id]
        AND [Tickets].[DepartmentId] IN (select Id from Departments where Id in ({departmentsAsParameter}))
        AND [Tickets].[SyncOutletId] IN (select Id from SyncOutlets where Id in ({outletsAsParameter}))
        AND  YEAR([Tickets].[Date]) >= YEAR({years})
GROUP BY YEAR([Tickets].[Date]),MONTH([Tickets].[Date]),DATENAME( MONTH, DATEADD( MONTH,0, [Tickets].[Date]))
ORDER BY [YEAR] DESC ,Month([Tickets].[Date])";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jahin Hasan
        public async static Task<IEnumerable<dynamic>> GetFastAndLastDateOfTickets()
        {

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var FastAndLastDate = $@"DECLARE
                    @StartDate as DATE, @EndDate as DATE
                    set @StartDate = (select min(date) from Tickets)
                    set @EndDate = (select max(date) from Tickets)
                    exec GetStartEndDate @StartDate, @EndDate";
            db.Open();
            var FastAndLast = await db.QueryAsync(FastAndLastDate);
            db.Close();
            return FastAndLast;
        }//func

        //Author Jahin Hasan
        public async static Task<IEnumerable<dynamic>> GetlastSelectedYearDate(string endDate)
        {

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var FastAndLastDate = $@"DECLARE
@StartDate as DATE, @EndDate as DATE
set @StartDate = ({endDate})
set @EndDate = GETDATE();
exec GetStartEndDate @StartDate, @EndDate";
            db.Open();
            var FastAndLast = await db.QueryAsync(FastAndLastDate);
            db.Close();
            return FastAndLast;
        }//func

        //Author Jahin Hasan
        public async static Task<IEnumerable<dynamic>> GetOutletWiseSalesInfo(
            DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"WITH
                            [TEMPORARY]
                                        AS
                                            (
                                                SELECT
                                                    [SyncOutlets].[Name] [OutletName],
                                                    SUM([Tickets].[TotalAmount]) [GrossAmount],
                                                    SUM([Tickets].[PlainSum]) [NetAmount]
                                                    FROM [Tickets], [SyncOutlets]
                                                    WHERE	[Tickets].[SyncOutletId] = [SyncOutlets].Id
                                                    AND [Date] BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                    AND [Tickets].[SyncOutletId] in ({outletsAsParameter})
                                                    AND [Tickets].[DepartmentId] in ({departmentsAsParameter})
                                                    GROUP BY [SyncOutlets].[Name]
                                            )--TEMPORARY
                            SELECT
                                [TEMPORARY].[OutletName] ,
                                [TEMPORARY].[NetAmount],
                                CAST(100 / ((SUM([TEMPORARY].[NetAmount]) OVER ()+(0.0)) / [TEMPORARY].[NetAmount]) AS DECIMAL(30,2)) AS [Net(%)],
                                [TEMPORARY].[GrossAmount],
                                CAST(100 / ((SUM([TEMPORARY].[GrossAmount]) OVER ()+(0.0)) / [TEMPORARY].[GrossAmount]) AS DECIMAL(30,2)) AS [Gross(%)]
                                FROM [TEMPORARY]
                                ORDER BY [TEMPORARY].[NetAmount] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetOutletWiseAndPaymentMethodWiseSalesInfo(
            DateTime from, DateTime to, List<int> outlets,
            List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"WITH
        [TEMPORARY]
                    AS
                        (
                            SELECT
                                CAST(SUM([Payments].[Amount]) AS DECIMAL(30,2)) AS [Amount],
                                [Payments].[Name] AS [PaymentMethod],
                                [Tickets].[SyncOutletId]
                            FROM [Payments] INNER JOIN [Tickets]
                                ON [Payments].[TicketId] = [Tickets].[Id]
                            WHERE [Tickets].[IsClosed] = 1
                                AND [Tickets].[Date] >= '{fromTimeStamp}'
                                AND [Tickets].[Date] <= '{toTimeStamp}'
                                AND [Tickets].SyncOutletId in ({outletsAsParameter})
                                AND [Tickets].DepartmentId in ({departmentsAsParameter})
                            GROUP BY [Tickets].[SyncOutletId],[Payments].[Name]
                        )
        SELECT
            [SyncOutlets].[Name],
            [TEMPORARY].[PaymentMethod],
            [TEMPORARY].[Amount]
        FROM [TEMPORARY] INNER JOIN [SyncOutlets]
            ON [TEMPORARY].[SyncOutletId] = [SyncOutlets].[Id]";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        public static DataSet GetDailyNetSalesForDashBoard(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Empty;
                if (OutletId == 0)
                {
                    sql = string.Format(@"SELECT StartDate , SUM(TicketTotalAmount) AS TicketTotalAmount , SUM(sales) AS Sales
                        FROM ( SELECT workperiodid , CONVERT(NVARCHAR , StartDate , 106) AS StartDate , TicketTotalAmount , Sales
                               FROM ( SELECT *
                                      FROM ( SELECT TOP 30 workperiodid , 'StartDate' = ( SELECT StartDate
                                                                                          FROM WorkPeriods
                                                                                          WHERE Id = WorkPeriodID
                                                                                        ) , SUM(totalamount) AS TicketTotalAmount , SUM(credit) AS Sales
                                             FROM ( SELECT WorkPeriodID = ( SELECT MAX(ID)
                                                                            FROM ( SELECT ID , StartDate ,
                                                                                               CASE EndDate
                                                                                                   WHEN StartDate
                                                                                                   THEN ( SELECT [dbo].[ufsFormat] ( DATEADD(second , 1 , MAX(Date)) , 'yyyy-mm-dd hh:mm:ss'
                                                                                                                                   )
                                                                                                          FROM AccountTransactionValues
                                                                                                        )
                                                                                                   ELSE EndDate
                                                                                               END AS EndDate
                                                                                   FROM WorkPeriods
                                                                                 ) AS W
                                                                            WHERE StartDate < t.Date
                                                                                  AND 
                                                                                  enddate >= t.Date
                                                                          ) , *
                                                    FROM ( SELECT ti.Id , ti.LastUpdateTime , ti.TicketNumber , ti.[Date] , ti.LastOrderDate , ti.LastPaymentDate , ti.IsClosed , ti.IsLocked , ti.RemainingAmount , ti.TotalAmount , ti.DepartmentId , ti.TicketTypeId , ti.Note , ti.TicketTags , ti.TicketStates , ti.ExchangeRate , ti.TaxIncluded , ti.Name , ti.TransactionDocument_Id , ti.DeliveryDate , ti.RETURNAMOUNT , ti.NoOfGuests , ti.Sum , ti.PreTaxServicesTotal , ti.PostTaxServicesTotal , ti.PlainSum , ti.PlainSumForEndOfPeriod , ti.TaxTotal , ti.ServiceChargeTotal , ti.ActiveTimerAmount , ti.ActiveTimerClosedAmount , ti.Synced , ti.SyncID , ti.SyncOutletId , ti.TokenNumber , ti.HHTTicketId , ti.SettledBy , ti.ServerUpdateTime , ti.SyncNote , ti.SettledCopyPrintMessage , ti.ZoneId , ti.ChangeAmount , ti.DispatchDate , ti.IsDispatched , ti.SmartStoreOrderId , ti.PreTaxDiscountServicesTotal , ti.PostTaxDiscountServicesTotal , ti.OnlineCommunicationInfo , ti.Synced2 , ti.ServerUpdateTime2 , ti.SplitCount , ti.TaxableAmount , ti.NonTaxableAmount , ti.FiscalYear , ti.TerminalId , ti.TerminalName , ti.UserId , ti.ShiftId , ti.TaxNumber , ti.MergedToTicketId , credit
                                                           FROM Tickets AS ti , ( SELECT d.Id AS tranid , SUM(Credit) AS Credit
                                                                                  FROM AccountTransactionDocuments AS d , AccountTransactionValues AS V , Accounts AS A , AccountTypes AS T
                                                                                  WHERE V.AccountTypeId = T.Id
                                                                                        AND 
                                                                                        d.Id = v.AccountTransactionDocumentId
                                                                                        AND 
                                                                                        A.Id = V.AccountId
                                                                                        AND 
                                                                                        v.AccountId = 1
                                                                                  GROUP BY d.id
                                                                                ) AS a
                                                           WHERE A.tranid = ti.TransactionDocument_Id
                                                         ) AS t
                                                  ) AS W
                                             GROUP BY workperiodid
                                             ORDER BY workperiodid DESC
                                           ) AS Q
                                    --Order by StartDate
                                    ) AS T
                             ) AS U
                        GROUP BY StartDate
                        ORDER BY CAST(StartDate AS DATE);");
                }
                else
                {
                    sql = $@"SELECT StartDate , SUM(TicketTotalAmount) AS TicketTotalAmount , SUM(sales) AS Sales
                                FROM ( SELECT workperiodid , CONVERT(NVARCHAR , StartDate , 106) AS StartDate , TicketTotalAmount , Sales
                                       FROM ( SELECT *
                                              FROM ( SELECT TOP 30 workperiodid , 'StartDate' = ( SELECT StartDate
                                                                                                  FROM WorkPeriods
                                                                                                  WHERE Id = WorkPeriodID
                                                                                                ) , SUM(totalamount) AS TicketTotalAmount , SUM(credit) AS Sales
                                                     FROM ( SELECT WorkPeriodID = ( SELECT MAX(ID)
                                                                                    FROM ( SELECT ID , StartDate ,
                                                                                                       CASE EndDate
                                                                                                           WHEN StartDate
                                                                                                           THEN ( SELECT [dbo].[ufsFormat] ( DATEADD(second , 1 , MAX(Date)) , 'yyyy-mm-dd hh:mm:ss'
                                                                                                                                           )
                                                                                                                  FROM AccountTransactionValues
                                                                                                                )
                                                                                                           ELSE EndDate
                                                                                                       END AS EndDate
                                                                                           FROM WorkPeriods
                                                                                         ) AS W
                                                                                    WHERE StartDate < t.Date
                                                                                          AND 
                                                                                          enddate >= t.Date
                                                                                  ) , *
                                                            FROM ( SELECT ti.Id , ti.LastUpdateTime , ti.TicketNumber , ti.[Date] , ti.LastOrderDate , ti.LastPaymentDate , ti.IsClosed , ti.IsLocked , ti.RemainingAmount , ti.TotalAmount , ti.DepartmentId , ti.TicketTypeId , ti.Note , ti.TicketTags , ti.TicketStates , ti.ExchangeRate , ti.TaxIncluded , ti.Name , ti.TransactionDocument_Id , ti.DeliveryDate , ti.RETURNAMOUNT , ti.NoOfGuests , ti.Sum , ti.PreTaxServicesTotal , ti.PostTaxServicesTotal , ti.PlainSum , ti.PlainSumForEndOfPeriod , ti.TaxTotal , ti.ServiceChargeTotal , ti.ActiveTimerAmount , ti.ActiveTimerClosedAmount , ti.Synced , ti.SyncID , ti.SyncOutletId , ti.TokenNumber , ti.HHTTicketId , ti.SettledBy , ti.ServerUpdateTime , ti.SyncNote , ti.SettledCopyPrintMessage , ti.ZoneId , ti.ChangeAmount , ti.DispatchDate , ti.IsDispatched , ti.SmartStoreOrderId , ti.PreTaxDiscountServicesTotal , ti.PostTaxDiscountServicesTotal , ti.OnlineCommunicationInfo , ti.Synced2 , ti.ServerUpdateTime2 , ti.SplitCount , ti.TaxableAmount , ti.NonTaxableAmount , ti.FiscalYear , ti.TerminalId , ti.TerminalName , ti.UserId , ti.ShiftId , ti.TaxNumber , ti.MergedToTicketId , credit
                                                                   FROM Tickets AS ti , ( SELECT d.Id AS tranid , SUM(Credit) AS Credit
                                                                                          FROM AccountTransactionDocuments AS d , AccountTransactionValues AS V , Accounts AS A , AccountTypes AS T
                                                                                          WHERE V.AccountTypeId = T.Id
                                                                                                AND 
                                                                                                d.Id = v.AccountTransactionDocumentId
                                                                                                AND 
                                                                                                A.Id = V.AccountId
                                                                                                AND 
                                                                                                v.AccountId = 1
                                                                                          GROUP BY d.id
                                                                                        ) AS a
                                                                   WHERE A.tranid = ti.TransactionDocument_Id and ti.SyncOutletId = {OutletId}
                                                                 ) AS t
                                                          ) AS W
                                                     GROUP BY workperiodid
                                                     ORDER BY workperiodid DESC
                                                   ) AS Q
                                            --Order by StartDate
                                            ) AS T
                                     ) AS U
                                GROUP BY StartDate
                                ORDER BY CAST(StartDate AS DATE);";

                }
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "DailyNetSalesForDashBoard");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetDailySalesForDashBoard(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection dbConn = new SqlConnection(DBUtility.GetConnectionString()))
                {
                    string sql = string.Empty;
                    dbConn.Open();
                    if (OutletId == 0)
                    {
                        sql = string.Format(@"SELECT StartDate, sum(TicketTotalAmount)TicketTotalAmount FROM 
                                            (
	                                            Select workperiodid, convert(NVARCHAR,StartDate, 106) StartDate, TicketTotalAmount from
	                                            (
	                                                Select 
	                                                --TOP 30 
	                                                * from
	                                                (
		                                                SELECT TOP 
		                                                30 workperiodid,
		                                                'StartDate' = (SELECT StartDate FROM WorkPeriods WHERE Id = WorkPeriodID),
			                                            sum(totalamount) TicketTotalAmount FROM 
		                                                (
			                                                SELECT 
			                                                WorkPeriodTID = (
								                                                SELECT max(ID) FROM
								                                                (    
									                                                SELECT ID, StartDate, 
									                                                CASE EndDate
										                                                WHEN StartDate THEN (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)                                                      
										                                                ELSE EndDate
									                                                END EndDate 
									                                                FROM 
									                                                WorkPeriods 
								                                                ) W
								                                                WHERE StartDate < t.Date AND enddate >= t.Date
							                                                ),
			                                                * FROM Tickets t 
		                                                ) W
		                                                GROUP BY workperiodid
		                                                ORDER BY workperiodid desc
	                                                )Q
	                                                --Order by StartDate
	                                            )T
                                            )U
                                            GROUP BY StartDate
                                            ORDER BY cast(StartDate as date)");


                    }
                    else
                    {
                        sql = string.Format(@"SELECT StartDate, sum(TicketTotalAmount)TicketTotalAmount, SyncOutletId
                                                FROM 
                                                (
                                                    Select workperiodid,SyncOutletId, convert(NVARCHAR,StartDate, 106) StartDate, TicketTotalAmount from
                                                    (
                                                        Select 
                                                        --TOP 30 
                                                        * from
                                                        (
                                                            SELECT TOP 
                                                            30 workperiodid,SyncOutletId,
                                                            'StartDate' = (SELECT StartDate FROM WorkPeriods WHERE Id = WorkPeriodID),
                                                            sum(totalamount) TicketTotalAmount FROM 
                                                            (
                                                                SELECT 
                                                                WorkPeriodID = (
                                                                                    SELECT max(ID) FROM
                                                                                    (    
                                                                                        SELECT ID, StartDate, 
                                                                                        CASE EndDate
                                                                                            WHEN StartDate THEN (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)                                                      
                                                                                            ELSE EndDate
                                                                                        END EndDate 
                                                                                        FROM 
                                                                                        WorkPeriods 
                                                                                    ) W
                                                                                    WHERE StartDate < t.Date AND enddate >= t.Date
                                                                                ),
                                                                * FROM Tickets t 
                                                                where t.SyncOutletId = {0}
                                                            ) W
                                                            GROUP BY workperiodid, SyncOutletId
                                                            ORDER BY workperiodid desc
                                                        )Q
                                                        --Order by StartDate
                                                    )T
                                                )U
                                                
                                                GROUP BY StartDate, SyncOutletId
                                                ORDER BY cast(StartDate as date)", OutletId);

                    }
                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                    da.Fill(ds, "DailySalesForDashBoard");
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetDailySalesForDashBoardFaster(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sql = string.Empty;
                string sqlClause = string.Empty;
                dbConn.Open();
                if (OutletId > 0)
                {
                    sqlClause = sqlClause + string.Format(@" and Tickets.SyncOutletid = {0}", OutletId);
                }

                if (true)
                {
                    sql = string.Format(@" DECLARE  
                                            @WorkPeriodId int, 
                                            @StartDate datetime, 
                                            @EndDate datetime,
                                            @TotalAmount money,
                                            @SalesTotal money

                                            create table #TempTable
                                            (
                                                StartDate DateTime, 
                                                TicketTotalAmount Money, 
                                                Sales Money,
                                            )

                                            DECLARE Daily_Route_SKU_Cursor CURSOR LOCAL FOR 

                                            Select * from (
				                                            SELECT Top 30 ID, StartDate, 
					                                            CASE EndDate
						                                            WHEN StartDate THEN (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') 
												                                            FROM AccountTransactionValues)				         
						                                            ELSE EndDate
					                                            END EndDate 
					                                            FROM 
					                                            WorkPeriods 
					                                            order by StartDate desc
	                                            )T
	                                            Order by T.StartDate


                                            OPEN Daily_Route_SKU_Cursor FETCH NEXT FROM Daily_Route_SKU_Cursor INTO 
                                            @WorkPeriodId , 
                                            @StartDate , 
                                            @EndDate 

                                            WHILE @@FETCH_STATUS = 0
                                            BEGIN
                                            Set @TotalAmount =0
                                            SET @SalesTotal = 0
                                            SET @TotalAmount = (Select SUM(TotalAmount) from Tickets where Date >= @StartDate and Date <= @EndDate {0})
                                            SET @SalesTotal = (Select SUM(PlainSum) from Tickets where Date >= @StartDate and Date <= @EndDate {0})


                                            INSERT INTO #TempTable
                                            VALUES (@StartDate, @TotalAmount, @SalesTotal)


                                            FETCH NEXT FROM Daily_Route_SKU_Cursor INTO 
                                            @WorkPeriodId , 
                                            @StartDate , 
                                            @EndDate 

                                            END
                                            CLOSE Daily_Route_SKU_Cursor
                                            DEALLOCATE Daily_Route_SKU_Cursor

                                            Select StartDate, SUM(TicketTotalAmount) TicketTotalAmount, SUM(Sales) Sales
                                            from #TempTable
                                            group by StartDate
                                            order by StartDate

                                            Drop Table #TempTable", sqlClause);
                }
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "DailySalesForDashBoard");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static string GetSQLDailySalesForDashBoardFaster(int OutletId)
        {
            try
            {

                var sql = new StringBuilder();
                var sqlClause = "";
                if (OutletId > 0)
                {
                    sqlClause += $@" and t.SyncOutletid = {OutletId}";
                }

                sql.AppendFormat(@"SELECT D.StartDate,
                            TotalAmount=SUM(TotalAmount),
                            SalesTotal=SUM(PlainSum)
                            from Tickets t,
                            (
	                            SELECT Top 30 ID, StartDate, 
	                            CASE EndDate
		                            WHEN StartDate THEN (SELECT DATEADD(second,1,max(Date)) 
								                            FROM AccountTransactionValues)				         
		                            ELSE EndDate
	                            END EndDate 
	                            FROM 
	                            WorkPeriods 
	                            order by StartDate desc
                            )D 
                            where t.Date >= D.StartDate and t.Date <= D.EndDate {0}
                            group by D.StartDate
                            Order by D.StartDate", sqlClause);
                return sql.ToString();

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static DataSet GetDashBoardFaster(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection dbConn = new SqlConnection(DBUtility.GetConnectionString()))
                {
                    string sqlClause = string.Empty;
                    dbConn.Open();
                    if (OutletId > 0)
                    {
                        sqlClause = sqlClause + $@" and t.SyncOutletid = {OutletId}";
                    }
                    ///GetDailySalesForDashBoard
                    string sql = $@"SELECT D.StartDate,
                            TotalAmount=SUM(TotalAmount),
                            SalesTotal=SUM(PlainSum)
                            from Tickets t,
                            (
	                            SELECT Top 1000 ID, StartDate, 
	                            CASE EndDate
		                            WHEN StartDate THEN (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') 
								                            FROM AccountTransactionValues)				         
		                            ELSE EndDate
	                            END EndDate 
	                            FROM 
	                            WorkPeriods 
	                            order by StartDate desc
                            )D 
                            where t.Date >= D.StartDate and t.Date <= D.EndDate {sqlClause}
                            group by D.StartDate
                            Order by D.StartDate;";
                    //GetMonthlySalesForDashBoard
                    if (OutletId == 0)
                    {
                        sql += @"
                                            select MonthYear, sum(amount) TotalAmount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            group by row,MonthYear
                                            Order by row ;";

                    }
                    else
                    {
                        sql += $@"
                                            select MonthYear, sum(amount) TotalAmount, SyncOutletID
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            and SyncOutletID = {OutletId}
                                            group by row,MonthYear, SyncOutletID
                                            Order by row ;";

                    }
                    if (OutletId == 0)
                    {
                        sql = @"select MonthYear, UltimateAccount,SUM(Amount)Amount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount <>'Sales'
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row  ";
                    }
                    else
                    {
                        sql = $@"select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount <>'Sales'
                                            and SyncOutletID = {OutletId}
                                            Group by MonthYear,UltimateAccount,row, SyncOutletID
                                            Order by row ;";
                    }
                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                    da.Fill(ds, "DailySalesForDashBoard");
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static async Task<IEnumerable<dynamic>> GetLast12MonthSales(List<int> outlets)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"select MonthYear, UltimateAccount,SUM(Amount)Amount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            AND UltimateAccount ='Sales'
                                            AND  [SyncOutletId] in ({outletsAsParameter})
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }

        public static DataSet GetLast12MonthSales(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection dbConn = new SqlConnection(DBUtility.GetConnectionString()))
                {
                    dbConn.Open();
                    string sql = string.Empty;
                    if (OutletId == 0)
                    {
                        sql = string.Format(@"select MonthYear, UltimateAccount,SUM(Amount)Amount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount ='Sales'
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row  ");
                    }
                    else
                    {
                        sql = $@"select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and SyncOutletID = {OutletId}
                                            and UltimateAccount ='Sales'
                                            Group by MonthYear,UltimateAccount,row, SyncOutletID
                                            Order by row  ";
                    }

                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                    da.Fill(ds, "Last12MonthSales");
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static string GetSQLLast12MonthSales(int OutletId)
        {
            try
            {
                if (OutletId == 0)
                {
                    return string.Format(@"select MonthYear, UltimateAccount,SUM(Amount)Amount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount ='Sales'
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row  ");
                }
                else
                {
                    return $@"select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and SyncOutletID = {OutletId}
                                            and UltimateAccount ='Sales'
                                            Group by MonthYear,UltimateAccount,row, SyncOutletID
                                            Order by row  ";
                }

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetLast12MonthSalesAccountWise(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Empty;
                if (OutletId == 0)
                {
                    sql = string.Format(@"
                                            select MonthYear, UltimateAccount,SUM(Amount)Amount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount <>'Sales'
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row  ");
                }
                else
                {
                    sql = $@" 
                                            select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount <>'Sales'
                                            and SyncOutletID = {OutletId}
                                            Group by MonthYear,UltimateAccount,row, SyncOutletID
                                            Order by row ";
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "Last12MonthSalesAccountWise");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetLast12MonthSalesPaymentWise(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Empty;
                if (OutletId == 0)
                {
                    sql = string.Format(@"select MonthYear, UltimateAccount,SUM(Amount)Amount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            --and UltimateAccount <>'Sales'
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row ");
                }
                else
                {
                    sql = string.Format(@"select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID 
	                                        from LastTwelveMonthTran
	                                        where PaymentAccountType is not null
	                                        and SyncOutletID = {0}
	                                        Group by MonthYear,UltimateAccount,row, SyncOutletID
	                                        Order by row  ", OutletId);
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "Last12MonthSalesPaymentWise");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetMonthlySalesForDashBoard(int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sql = string.Empty;
                dbConn.Open();
                if (OutletId == 0)
                {
                    sql = string.Format(@" select MonthYear, sum(amount) TotalAmount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            group by row,MonthYear
                                            Order by row ");

                }
                else
                {
                    sql = string.Format(@"  select MonthYear, sum(amount) TotalAmount, SyncOutletID
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            and SyncOutletID = {0}
                                            group by row,MonthYear, SyncOutletID
                                            Order by row ", OutletId);

                }
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = 160000;
                da.Fill(ds, "MonthlySalesForDashBoard");
                dbConn.Close();
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static async Task<IEnumerable<dynamic>> GetMonthlySalesForDashBoard(List<int> outlets)
        {

            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"select MonthYear, sum(amount) TotalAmount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            AND  [SyncOutletId] in ({outletsAsParameter})
                                            group by row,MonthYear
                                            Order by row";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;

        }

        public static DataSet GetProductSalesByDayTime(string FromDate, string ToDate, int outletId, int departmentId, string menuItemId, string groupCodes)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Empty;
                string sqlClause = string.Empty;
                if (departmentId != 0)
                {
                    sqlClause = $@" and T.DepartmentId = {departmentId}";
                }

                if (outletId > 0)
                {
                    sqlClause += $@" and T.SyncOutletid = {outletId}";
                }

                if (menuItemId != "0")
                {
                    sqlClause += " AND o.MenuItemId in(" + Convert.ToInt32(menuItemId) + ")";
                }
                if (!string.IsNullOrEmpty(groupCodes))
                {
                    sqlClause += $" AND o.MenuGroupName in ({groupCodes})";
                }

                sql = $@" SELECT 
                                    DatePart(WEEKDAY, TicketDate) WEEKDate, 
                                    DateName(WEEKDAY, TicketDate) WEEKDAY, 
                                    DatePart(HOUR, TicketDate)HOUR,
                                    sum(OrderCount)OrderCount, 
                                    sum(TotalAmount) TicketTotalAmount, 
                                    Count(TicketId) TicketCount, 
                                    sum(SalesTotal) TotalAmount
                                FROM 
                                      (
	                                    SELECT 
                                            t.Id TicketId, 
                                            t.[Date] TicketDate, 
                                            t.TotalAmount, 
                                            sum(o.Quantity) OrderCount, 
                                            sum(o.PlainTotal) SalesTotal
	                                            FROM Orders o, Tickets t
	                                            WHERE o.TicketId = t.Id
	                                            and t.[Date] BETWEEN '{FromDate}' AND '{ToDate}'  
                                                {sqlClause}
	                                            AND o.DecreaseInventory = 1
	                                            GROUP BY t.Id , t.[Date], t.TotalAmount                                           
                                            )
                                            Q
                                            GROUP BY DatePart(WEEKDAY, TicketDate) , DateName(WEEKDAY, TicketDate), DatePart(HOUR, TicketDate)
                                            ORDER BY WEEKDate, [Hour]";

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetTimeWiseSalesForChart(string FromDate, string ToDate, int outletId, int departmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Empty;
                string sqlClause = string.Empty;
                if (departmentId != 0)
                {
                    sqlClause = $@"and DepartmentId = {departmentId}";
                }

                if (outletId == 0 || departmentId != 0)
                {
                    sql = $@" WITH mycte as
                                            (
                                                SELECT 0 AS MyHour, 1 YourHour
                                                UNION ALL
                                                SELECT MyHour + 1, YourHour+1
                                                FROM mycte 
                                                WHERE MyHour + 1 < 24
                                            )
                                            Select * from
                                            (
                                                SELECT 
                                                CAST(CONVERT(TIME(0), DATEADD(MINUTE, 60*mycte.MyHour, 0)) AS VARCHAR) HOURS24, CAST(CONVERT(TIME(0), DATEADD(MINUTE, 60*mycte.MyHour, 0)) AS VARCHAR) +'-' +CAST(CONVERT(TIME(0), DATEADD(MINUTE, 60*mycte.YourHour, 0)) AS VARCHAR) TimeRange,
                                                COALESCE(NumberOfTickets,0) NumberOfTickets,
                                                COALESCE(GrandTotal,0) GrandTotal, Convert(Decimal(20,2),COALESCE(Sales,0)) Sales, NumberOfGuests
                                                FROM mycte
                                                LEFT JOIN 
                                                (
                                                  SELECT  sum(totalamount)GrandTotal, Sum(PlainSum) Sales, COUNT(Tickets.ID) AS NumberOfTickets,SUM(NoOfGuests)NumberOfGuests,{{ fn HOUR(dbo.Tickets.[Date]) }} AS MyHour
                                                  FROM    Tickets
                                                  WHERE [DATE] > '{FromDate}' and [DATE]< '{ToDate}'
                                                  {sqlClause}
                                                  GROUP BY {{ fn HOUR(Tickets.[Date]) }}
                                                 )h
                                                ON h.MyHour = mycte.MyHour
                                             )Q
                                             Where NumberOfTickets>0 
                                            Order by HOURS24";
                }
                else
                {
                    sql = $@" WITH mycte as
                                            (
                                                SELECT 0 AS MyHour, 1 YourHour
                                                UNION ALL
                                                SELECT MyHour + 1, YourHour+1
                                                FROM mycte 
                                                WHERE MyHour + 1 < 24
                                            )
                                            Select * from
                                            (
                                                SELECT 
                                                CAST(CONVERT(TIME(0), DATEADD(MINUTE, 60*mycte.MyHour, 0)) AS VARCHAR) HOURS24, 
                                                CAST(CONVERT(TIME(0), DATEADD(MINUTE, 60*mycte.MyHour, 0)) AS VARCHAR) +'-' +CAST(CONVERT(TIME(0), DATEADD(MINUTE, 60*mycte.YourHour, 0)) AS VARCHAR) TimeRange,
                                                COALESCE(NumberOfTickets,0) NumberOfTickets,
                                                COALESCE(GrandTotal,0) GrandTotal, 
                                                Convert(Decimal(20,2),COALESCE(Sales,0)) Sales, NumberOfGuests
                                                FROM mycte
                                                LEFT JOIN 
                                                (
                                                  SELECT  sum(totalamount)GrandTotal, Sum(PlainSum) Sales, COUNT(Tickets.ID) AS NumberOfTickets,SUM(NoOfGuests)NumberOfGuests,{{ fn HOUR(dbo.Tickets.[Date]) }} AS MyHour
                                                  FROM    Tickets
                                                  WHERE [DATE] > '{FromDate}' and [DATE]< '{ToDate}'  
                                                  and SyncOutletId ={outletId}
                                                  GROUP BY SyncOutletId, {{ fn HOUR(Tickets.[Date]) }}
                                                 )h
                                                ON h.MyHour = mycte.MyHour
                                             )Q
                                             Where NumberOfTickets>0 
                                             Order by HOURS24";

                }
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
    }
}
