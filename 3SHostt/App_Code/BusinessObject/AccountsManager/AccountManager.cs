using System;
using System.Data;
using System.Data.SqlClient;

namespace BusinessObjects.AccountsManager
{
	public class AccountManager
	{
		public static DataSet GetCurrentBalanceOfAChead(DateTime fromDate, DateTime toDate, int outletId)
		{
			DataSet ds = new DataSet();
			try
            {
                string sql = string.Empty;


                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    //(SELECT enddate FROM WorkPeriods WHERE StartDate = (SELECT max(w.startdate) FROM WorkPeriods w WHERE w.StartDate <= Dateadd(Day,1,'03 Aug 2014')))

                    if (outletId > 0)
                    {
                        sql = $@"SELECT
							StartDate = [dbo].[ufsFormat] ('{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'),
							EndDate = [dbo].[ufsFormat] ('{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'),
							T.Name AccountType,A.id AccountId, A.Name AccountName,
							sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
							FROM AccountTransactionValues V, Accounts A, AccountTypes T, AccountTransactionDocuments AD WHERE
							V.AccountTypeId = T.Id AND A.Id = V.AccountId AND v.AccountTransactionDocumentId = ad.Id and 
							V.Date  BETWEEN '{fromDate:dd MMM yyyy hh:mm:ss tt}' and '{toDate:dd MMM yyyy hh:mm:ss tt}' and SyncOutletId = {outletId}
							GROUP BY V.accounttypeid, T.Name, V.accountid,A.id, A.Name";
                    }
                    else
                    {
                        sql = $@"SELECT
							StartDate = [dbo].[ufsFormat] ('{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'),
							EndDate = [dbo].[ufsFormat] ('{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'),
							T.Name AccountType,A.id AccountId, A.Name AccountName,
							sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
							FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
							V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
							V.Date  BETWEEN '{fromDate:dd MMM yyyy hh:mm:ss tt}' and '{toDate:dd MMM yyyy hh:mm:ss tt}'
							GROUP BY V.accounttypeid, T.Name, V.accountid,A.id, A.Name";
                    }
                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.Fill(ds);
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		//ok
		public static DataSet GetGroupItem()
		{
			DataSet ds = new DataSet();
			try
            {
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    string sql = "select Name from AccountTypes";
                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.Fill(ds);
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		//ok 
		public static DataSet GetAccounts(string accountName)
		{
			DataSet ds = new DataSet();
			try
            {

                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    if (string.IsNullOrEmpty(accountName))
                    {
                        sql = "SELECT ID, AccountTypeID, Name FROM Accounts";
                    }
                    else
                    {
                        sql = $"SELECT ID, AccountTypeID, Name FROM Accounts where AccountTypeID =(Select Id FROM AccountTypes where Name = '{accountName}')";
                    }
                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.Fill(ds);
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		} 
		//ok
		public static DataSet GetAccoutTypeWiseBalance(string AccountTypeName, string startDate, string endDate, bool isOpeningBalance)
		{

			DataSet ds = new DataSet();

			try
            {
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    string sqlClause = string.Empty;
                    if (string.IsNullOrEmpty(AccountTypeName)) 
                    {
                        sqlClause += "";
                    }
                    else
                    {
                        sqlClause += "Where AccountTypeName ='" + AccountTypeName + "'";
                    }

                    if (isOpeningBalance)
                    {
                        string sql = $@"SELECT * FROM 
											(
											 SELECT 0 AS row_number, 'Opening_Balance' TranDate, AccountsTypeID, AccountTypeName,
											 sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											 FROM 
											 (
												  SELECT  convert(date, V.Date) TranDate, 
												  T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												  (Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												  FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												  V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												  V.Date  < '{startDate}'
											 )Q
											 GROUP BY AccountsTypeID, AccountTypeName
 
											 Union
 
											 SELECT ROW_NUMBER() OVER(ORDER BY TranDate asc) row_number , convert(varchar, trandate, 106) TrandDate, AccountsTypeID, AccountTypeName,
											 sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											 FROM 
											 (
												  SELECT convert(date, V.Date) TranDate, V.Date,
												  T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												  (Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												  FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												  V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												  V.Date  BETWEEN '{startDate}' and '{endDate}'
											 )Q
											 GROUP BY TranDate, AccountsTypeID, AccountTypeName
 
											)Trans
											 {sqlClause}
											 ORDER BY trandate desc";

                        SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                        da.Fill(ds, "ACTypeWiseBalanceDataSet");
                        dbConn.Close();
                    }
                    else
                    {

                        string sql = $@"SELECT ROW_NUMBER() OVER(ORDER BY TranDate asc) row_number , convert(varchar, trandate, 106) TranDate, AccountsTypeID, AccountTypeName,
											 sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											 FROM 
											 (
												  SELECT convert(date, V.Date) TranDate, V.Date,
												  T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												  (Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												  FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												  V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												  V.Date  BETWEEN '{startDate}' and '{endDate}'
											 )Q
											 {sqlClause}
											 GROUP BY TranDate, AccountsTypeID, AccountTypeName";

                        SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                        da.Fill(ds, "ACTypeWiseBalanceDataSet");
                        dbConn.Close();

                    }
                }





            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		//ok
		public static DataSet GetAccoutWiseBalance(string AccountTypeName, string AccountName, string startDate, string endDate, bool isOpeningBalance)
		{

			DataSet ds = new DataSet();

			try
            {
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    string sqlClause = string.Empty;
                    if (string.IsNullOrEmpty(AccountName) && string.IsNullOrEmpty(AccountTypeName))
                    {
                        sqlClause += "";
                    }
                    else if (string.IsNullOrEmpty(AccountName) && !string.IsNullOrEmpty(AccountTypeName))
                    {

                        sqlClause += " Where AccountTypeName ='" + AccountTypeName + "'";


                    }
                    else
                    {
                        sqlClause += " Where AccountName ='" + AccountName + "'";
                    }

                    if (isOpeningBalance)
                    {
                        string sql = $@"SELECT 0 row_number, 'Opening_Balance' TrandDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName, 
											sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											FROM 
											(
												SELECT convert(date, V.Date) TranDate, 
												T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												(Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												V.Date  < '{startDate}'
											)Q
											{sqlClause}
											GROUP BY AccountsTypeID, AccountTypeName, AccountsID, AccountName
											--Where

											Union

											SELECT ROW_NUMBER() OVER(ORDER BY TranDate asc) row_number, convert(varchar, trandate, 106) TrandDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName, 
											sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											FROM 
											(
												SELECT convert(date, V.Date) TranDate, 
												T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												(Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												V.Date  BETWEEN '{startDate}' and '{endDate}'
											)Q
											{sqlClause}
											GROUP BY TranDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName";



                        SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                        da.Fill(ds, "ACWiseBalanceDataSet");
                        dbConn.Close();
                    }
                    else
                    {

                        string sql = $@"
											SELECT ROW_NUMBER() OVER(ORDER BY TranDate asc) row_number, convert(varchar, trandate, 106) TrandDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName, 
											sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											FROM 
											(
												SELECT convert(date, V.Date) TranDate, 
												T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												(Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												V.Date  BETWEEN '{startDate}' and '{endDate}'
											)Q
											{sqlClause}
											GROUP BY TranDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName";



                        SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                        da.Fill(ds, "ACWiseBalanceDataSet");
                        dbConn.Close();

                    }
                }


            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		//ok
		public static DataSet GetAccoutWiseBalance(string AccountTypeName, string AccountName, string startDate, string endDate, int outletId)
		{

			DataSet ds = new DataSet();

			try
            {
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    string sqlClause = string.Empty;
                    if (!string.IsNullOrEmpty(AccountName))
                    {
                        sqlClause += $@" and A.Name = '{AccountName}'";
                    }
                    if (!string.IsNullOrEmpty(AccountTypeName))
                    {
                        sqlClause += $@" and T.Name like '%{AccountTypeName}%'";
                    }
                    if (outletId > 0)
                    {
                        sqlClause += string.Format(@" and ad.SyncOutletId = {0}", outletId);
                    }

                    string sql = $@"
										SELECT
										T.Name AccountType,A.id AccountId, A.Name AccountName,
										sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
										FROM AccountTransactionValues V, Accounts A, AccountTypes T, AccountTransactionDocuments AD WHERE
										V.AccountTypeId = T.Id AND A.Id = V.AccountId AND v.AccountTransactionDocumentId = ad.Id  
										and V.Date  BETWEEN '{startDate}' and '{endDate}' 
										{sqlClause}
										GROUP BY V.accounttypeid, T.Name, V.accountid,A.id, A.Name
										order by AccountName";



                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.Fill(ds, "ACWiseBalanceDataSet");
                    dbConn.Close();
                }

            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		//ok
		public static DataSet GetAccountDrillThrough(int accountId, DateTime fromDate, DateTime toDate, int outletId)
		{

			DataSet ds = new DataSet();
			try
            {
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    string sql1 = string.Empty;
                    string sql2 = string.Empty;
                    string sql3 = string.Empty;

                    dbConn.Open();
                    if (outletId > 0)
                    {
                        sql1 = $@"SELECT isnull(sum(v.debit),0) Debit
									,isnull(sum(v.credit),0) Credit
									,isnull(sum(v.debit)- sum(v.credit),0)Balance
							FROM AccountTransactionValues v
								,  AccountTransactions at
							WHERE v.[Date] < '{fromDate:dd MMM yyyy hh:mm:ss tt}'  
								AND v.AccountId = {accountId} 
								AND v.AccountTransactionId = at.Id  
							and at.SyncOutletId = {outletId};";

                        sql2 = $@"SELECT v.[Date]
								, at.Name + ':'+ ad.Name Description
								, v.Debit
								, v.Credit
								, v.debit-v.credit Balance
							FROM AccountTransactionValues v
								, AccountTransactions at
								, AccountTransactionDocuments ad
							WHERE v.[Date] BETWEEN '{fromDate:dd MMM yyyy hh: mm:ss tt}' and '{toDate:dd MMM yyyy hh:mm:ss tt}' 
								AND v.accountid = {accountId}
								AND v.AccountTransactionId = at.Id 
								AND at.AccountTransactionDocumentId = ad.id
								AND at.SyncOutletId = {outletId}
							ORDER BY v.[Date]";

                        sql3 = $@"SELECT isnull(sum(v.debit),0) Debit
									,isnull(sum(v.credit),0) Credit
									,isnull(sum(v.debit)- sum(v.credit),0)Balance
							FROM  AccountTransactionValues v
								,  AccountTransactions at
							WHERE v.[Date] <= '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}' 
							AND v.accountid = {accountId}
							AND v.AccountTransactionId = at.Id 
							AND at.SyncOutletId = {outletId}";
                    }
                    else
                    {
                        sql1 = $@"SELECT isnull(sum(debit),0) Debit,isnull(sum(credit),0) Credit,isnull(sum(debit)- sum(credit),0)Balance
											FROM AccountTransactionValues 
											WHERE [Date] < '{fromDate:dd MMM yyyy hh:mm:ss tt}'  AND accountid = {accountId}";

                        sql2 = $@"SELECT v.[Date]
									,at.Name + ':'+ ad.Name Description
									,v.Debit
									,v.Credit
									, v.debit-v.credit Balance
							FROM AccountTransactionValues v
								, AccountTransactions at
								, AccountTransactionDocuments ad
							WHERE v.[Date] BETWEEN '{fromDate:dd MMM yyyy hh:mm:ss tt}' and '{toDate:dd MMM yyyy hh:mm:ss tt}' 
								AND v.accountid = {accountId}
								AND v.AccountTransactionId = at.Id 
								AND at.AccountTransactionDocumentId = ad.id
							ORDER BY v.[Date]";

                        sql3 = $@"SELECT isnull(sum(debit),0) Debit,isnull(sum(credit),0) Credit,isnull(sum(debit)- sum(credit),0)Balance
											FROM AccountTransactionValues 
											WHERE [Date] <= '{toDate:dd MMM yyyy hh:mm:ss tt}'  AND accountid = {accountId}";
                    }
                    string sql = sql1 + ";" + sql2 + ";" + sql3;
                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.Fill(ds);

                    ds.Tables[0].TableName = "OpeningBalance";
                    ds.Tables[1].TableName = "ACDrill";
                    ds.Tables[2].TableName = "ClosingBalance";
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		//ok
		public static DataTable GetAccountType(int AccountTypeId)
		{
			DataTable dt = new DataTable();
			try
            {

                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();

                    sql = $@"Select * from AccountTypes where Id = {AccountTypeId}";

                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.Fill(dt);
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return dt;
		}
		//ok
		public static DataTable GetAccountType()
		{
			DataTable dt = new DataTable();
			try
            {

                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    sql = @"Select * from AccountTypes";
                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.Fill(dt);
                    dbConn.Close();
                }
            }
            catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}
			return dt;
		}

        public static DataSet GetProfitAndLossReportSP(DateTime fromDate, DateTime toDate, int OutletId)
        {
            DataSet ds = new DataSet();
            DataTable table = GetProfitAndLossTable(Convert.ToString(OutletId), fromDate, toDate);
            ds.Tables.Add(table);
            return ds;
        }
        public static string GetDataFetchFrom()
        {
            return DBUtility.GetDataFetchFrom();
        }
        public static DataTable GetProfitAndLossTable(string OutletId, DateTime fromDate, DateTime toDate) 
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetProfit_andLossReport]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "ProfitAndLoss";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataSet GetStartAndEndDate(DateTime fromDate, DateTime toDate, bool bIsCurrentWorkPeriod)
        {
            DataSet ds = new DataSet();
            string searchStr = string.Empty;
            try
            {

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Empty;
                if (!bIsCurrentWorkPeriod)
                {
                    sql = string.Format(@"DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100),
											@LastPeriodStartDate DATETIME,
											@LastPeriodEndDate DATETIME,
											@LastWorkPeriodID INT

											SET @StartDate = (SELECT [dbo].[ufsFormat] ((SELECT min(w.startdate) FROM WorkPeriods w WHERE w.startdate >= '{0}'), 'yyyy-mm-dd hh:mm:ss'))
											SET @LastWorkPeriodID = (SELECT WorkPeriods.Id FROM WorkPeriods WHERE StartDate = (SELECT max(w.startdate) FROM WorkPeriods w WHERE w.StartDate >= '{0}' AND  w.StartDate < Dateadd(Day,1,'{1}')))
											SET @LastPeriodStartDate = (SELECT startdate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)
											SET @LastPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)                                            
											IF @LastPeriodEndDate = @LastPeriodStartDate
											BEGIN
												SET @EndDate = (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)
											END
											ELSE
											BEGIN 
												SET @EndDate = (SELECT [dbo].[ufsFormat]((SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID), 'yyyy-mm-dd hh:mm:ss'))
											END 

											SELECT isnull(@StartDate, '18 sep 3030') StartDate, isnull(@EndDate, '18 sep 3030') EndDate
											", fromDate.ToString("dd MMM yyyy"), toDate.ToString("dd MMM yyyy"));
                }
                else
                {
                    sql = string.Format(@"DECLARE 
										@StartDate  DateTime,
										@EndDate  DateTime,
										@MaxEndDate DateTime

										SET @StartDate = (SELECT max(startdate) FROM WorkPeriods)
										SET @EndDate = (SELECT max(EndDate) FROM WorkPeriods)
										SET @MaxEndDate = @EndDate

										IF @StartDate = @EndDate
										BEGIN
											SET @MaxEndDate = (SELECT max(EndDate) FROM WorkPeriods where StartDate<>EndDate)
											SET @EndDate = (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)
										END
										SELECT [dbo].[ufsFormat](@StartDate, 'yyyy-mm-dd hh:mm:ss') StartDate, [dbo].[ufsFormat](@EndDate, 'yyyy-mm-dd hh:mm:ss') EndDate,[dbo].[ufsFormat](@MaxEndDate, 'yyyy-mm-dd hh:mm:ss') MaxEndDate");
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "StartAndEndDate");
                dbConn.Close();

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ds;
        }
        public static DataSet GetOutlets()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@"select id, /*(Name +'-'+Cast(LastSyncedOutletTime as varchar))*/ Name FROM SyncOutlets where isActive<>0");

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