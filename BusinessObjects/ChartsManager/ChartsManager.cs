using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessObjects.ChartsManager
{
    public class ChartsManager
    {

        public static DataSet GetDailySalesForDashBoard(int OutletId)
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
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
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
                    sql = string.Format(@"
                                            SELECT StartDate, sum(TicketTotalAmount)TicketTotalAmount, SUM(sales) Sales FROM 
                                            (
                                                Select workperiodid, convert(NVARCHAR,StartDate, 106) StartDate, TicketTotalAmount,Sales from
                                                (
                                                Select * from(SELECT TOP 
                                                30 workperiodid,
                                                'StartDate' = (SELECT StartDate FROM WorkPeriods WHERE Id = WorkPeriodID),
                                                    sum(totalamount) TicketTotalAmount, sum(credit) Sales FROM 
                                                (
                                                    SELECT 
                                                    WorkPeriodID = (SELECT max(ID) FROM
                                                                    (	
                                                                        SELECT ID, StartDate, 
                                                                        CASE EndDate
                                                                            WHEN StartDate THEN (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)				         
                                                                            ELSE EndDate
                                                                        END EndDate 
                                                                        FROM 
                                                                        WorkPeriods 
                                                                    ) W
                                                                    WHERE StartDate < t.Date AND enddate >= t.Date),
                                                    * FROM 
                                                    (
                                                        SELECT ti.*, credit from
                                                        Tickets ti, 
                                                        (
                                                            SELECT d.Id tranid, sum(Credit)Credit
                                                            FROM AccountTransactionDocuments d, AccountTransactionValues V, Accounts A, AccountTypes T 
                                                            WHERE V.AccountTypeId = T.Id 
                                                            AND d.Id = v.AccountTransactionDocumentId
                                                            AND A.Id = V.AccountId 
                                                            AND v.AccountId = 1
                                                            GROUP BY d.id
                                                        )a WHERE A.tranid = ti.TransactionDocument_Id
                                                    )t 
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
                    sql = string.Format(@" SELECT StartDate, sum(TicketTotalAmount)TicketTotalAmount, SUM(sales) Sales, SyncOutletId FROM 
                                            (
                                                Select workperiodid, convert(NVARCHAR,StartDate, 106) StartDate, TicketTotalAmount,Sales, SyncOutletId from
                                                (
                                                Select * from(SELECT TOP 
                                                30 workperiodid,
                                                'StartDate' = (SELECT StartDate FROM WorkPeriods WHERE Id = WorkPeriodID),
                                                    sum(totalamount) TicketTotalAmount, sum(credit) Sales, SyncOutletId FROM 
                                                (
                                                    SELECT 
                                                    WorkPeriodID = (SELECT max(ID) FROM
                                                                    (	
                                                                        SELECT ID, StartDate, 
                                                                        CASE EndDate
                                                                            WHEN StartDate THEN (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)				         
                                                                            ELSE EndDate
                                                                        END EndDate 
                                                                        FROM 
                                                                        WorkPeriods 
                                                                    ) W
                                                                    WHERE StartDate < t.Date AND enddate >= t.Date),
                                                    * FROM 
                                                    (
                                                        SELECT ti.*, credit from
                                                        Tickets ti, 
                                                        (
                                                            SELECT d.Id tranid, sum(Credit)Credit
                                                            FROM AccountTransactionDocuments d, AccountTransactionValues V, Accounts A, AccountTypes T 
                                                            WHERE V.AccountTypeId = T.Id 
                                                            AND d.Id = v.AccountTransactionDocumentId
                                                            AND A.Id = V.AccountId 
                                                            AND v.AccountId = 1
                                                            GROUP BY d.id
                                                        )a WHERE A.tranid = ti.TransactionDocument_Id and ti.SyncOutletId = {0}
                                                    )t 
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
                da.Fill(ds, "DailyNetSalesForDashBoard");
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
                    sql = string.Format(@"select MonthYear, sum(amount) TotalAmount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            group by row,MonthYear
                                            Order by row ");

                }
                else
                {
                    sql = string.Format(@"select MonthYear, sum(amount) TotalAmount, SyncOutletID
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is not null
                                            and SyncOutletID = {0}
                                            group by row,MonthYear, SyncOutletID
                                            Order by row ", OutletId);

                }
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "MonthlySalesForDashBoard");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
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
                    sql = string.Format(@"select MonthYear, UltimateAccount,SUM(Amount)Amount 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount <>'Sales'
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row  ");
                }
                else
                {
                    sql = string.Format(@" select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID 
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and UltimateAccount <>'Sales'
                                            and SyncOutletID = {0}
                                            Group by MonthYear,UltimateAccount,row, SyncOutletID
                                            Order by row ", OutletId);
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
                    sql = string.Format(@" select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID 
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
        public static DataSet GetLast12MonthSales(int OutletId)
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
                                            where PaymentAccountType is null
                                            and UltimateAccount ='Sales'
                                            Group by MonthYear,UltimateAccount,row
                                            Order by row  ");
                }
                else
                {
                    sql = string.Format(@"select MonthYear, UltimateAccount,SUM(Amount)Amount, SyncOutletID
                                            from LastTwelveMonthTran
                                            where PaymentAccountType is null
                                            and SyncOutletID = {0}
                                            and UltimateAccount ='Sales'
                                            Group by MonthYear,UltimateAccount,row, SyncOutletID
                                            Order by row  ", OutletId);
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "Last12MonthSales");
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
                    sqlClause = string.Format(@"and DepartmentId = {0}",departmentId);
                }
                
                if (outletId == 0 || departmentId !=0)
                {
                    sql = string.Format(@" WITH mycte as
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
                                                  WHERE [DATE] > '{0}' and [DATE]< '{1}'
                                                  {2}
                                                  GROUP BY {{ fn HOUR(Tickets.[Date]) }}
                                                 )h
                                                ON h.MyHour = mycte.MyHour
                                             )Q
                                             Where NumberOfTickets>0 
                                            Order by HOURS24", FromDate, ToDate,sqlClause);
                }
                else
                {
                    sql = string.Format(@" WITH mycte as
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
                                                  WHERE [DATE] > '{0}' and [DATE]< '{1}'  
                                                  and SyncOutletId ={2}
                                                  GROUP BY SyncOutletId, {{ fn HOUR(Tickets.[Date]) }}
                                                 )h
                                                ON h.MyHour = mycte.MyHour
                                             )Q
                                             Where NumberOfTickets>0 
                                             Order by HOURS24", FromDate, ToDate, outletId);

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
                    sqlClause = string.Format(@" and T.DepartmentId = {0}", departmentId);
                }

                if (outletId > 0)
                    sqlClause = sqlClause + string.Format(@" and T.SyncOutletid = {0}", outletId);

                if (menuItemId != "0")
                {
                    sqlClause += " AND o.MenuItemId in(" + Convert.ToInt32(menuItemId) + ")";
                }
                if (!string.IsNullOrEmpty(groupCodes))
                {
                    sqlClause += string.Format(" AND o.MenuGroupName in ({0})", groupCodes);
                }
                if (true)
                {
                    sql = string.Format(@" SELECT DatePart(WEEKDAY, TicketDate) WEEKDate, DateName(WEEKDAY, TicketDate) WEEKDAY, DatePart(HOUR, TicketDate)HOUR, sum(OrderCount)OrderCount, sum(TotalAmount) TicketTotalAmount, Count(TicketId) TicketCount, sum(SalesTotal) TotalAmount
                                            FROM 
                                            (
	                                            SELECT t.Id TicketId, t.[Date] TicketDate, t.TotalAmount, sum(o.Quantity) OrderCount, sum(o.PlainTotal) SalesTotal
	                                            FROM Orders o, Tickets t
	                                            WHERE o.TicketId = t.Id
	                                            and t.[Date] BETWEEN '{0}' AND '{1}'  
                                                {2}
	                                            AND o.DecreaseInventory = 1
	                                            GROUP BY t.Id , t.[Date], t.TotalAmount                                           
                                            )
                                            Q
                                            GROUP BY DatePart(WEEKDAY, TicketDate) , DateName(WEEKDAY, TicketDate), DatePart(HOUR, TicketDate)
                                            ORDER BY WEEKDate, [Hour]", FromDate, ToDate, sqlClause);
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
                    sqlClause = sqlClause + string.Format(@" and Tickets.SyncOutletid = {0}", OutletId);

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
    }
}
