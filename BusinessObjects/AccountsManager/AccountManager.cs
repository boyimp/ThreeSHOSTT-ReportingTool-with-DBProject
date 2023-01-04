using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();
				//(SELECT enddate FROM WorkPeriods WHERE StartDate = (SELECT max(w.startdate) FROM WorkPeriods w WHERE w.StartDate <= Dateadd(Day,1,'03 Aug 2014')))

				if (outletId > 0)
				{
					sql = string.Format(@"SELECT
							StartDate = [dbo].[ufsFormat] ('{0}', 'yyyy-mm-dd hh:mm:ss'),
							EndDate = [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'),
							T.Name AccountType,A.id AccountId, A.Name AccountName,
							sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
							FROM AccountTransactionValues V, Accounts A, AccountTypes T, AccountTransactionDocuments AD WHERE
							V.AccountTypeId = T.Id AND A.Id = V.AccountId AND v.AccountTransactionDocumentId = ad.Id and 
							V.Date  BETWEEN '{0}' and '{1}' and SyncOutletId = {2}
							GROUP BY V.accounttypeid, T.Name, V.accountid,A.id, A.Name", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"), outletId);
				}
				else
				{
					sql = string.Format(@"SELECT
							StartDate = [dbo].[ufsFormat] ('{0}', 'yyyy-mm-dd hh:mm:ss'),
							EndDate = [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'),
							T.Name AccountType,A.id AccountId, A.Name AccountName,
							sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
							FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
							V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
							V.Date  BETWEEN '{0}' and '{1}'
							GROUP BY V.accounttypeid, T.Name, V.accountid,A.id, A.Name", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"));
				}
				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
				da.Fill(ds);
				dbConn.Close();
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}

		public static DataSet GetGroupItem()
		{
			DataSet ds = new DataSet();
			try
			{
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();
				string sql = "select Name from AccountTypes";
				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
				da.Fill(ds);
				dbConn.Close();
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		public static DataSet GetAccounts(string search)
		{
			DataSet ds = new DataSet();
			try
			{

				string sql = string.Empty;
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();
				if (string.IsNullOrEmpty(search))
					sql = "SELECT ID, AccountTypeID, Name FROM Accounts";
				else
				{
					sql = string.Format("SELECT ID, AccountTypeID, Name FROM Accounts where AccountTypeID =(Select Id FROM AccountTypes where Name = '{0}')", search);
				}
				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
				da.Fill(ds);
				dbConn.Close();
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}

		public static DataSet GetAccoutTypeWiseBalance(string AccountTypeFilter, string startDate, string endDate, bool OpeningBalance)
		{

			DataSet ds = new DataSet();

			try
			{
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();
				string sqlClause = string.Empty;
				if (string.IsNullOrEmpty(AccountTypeFilter))
				{
					sqlClause += "";
				}
				else
				{
					sqlClause += "Where AccountTypeName ='" + AccountTypeFilter + "'";
				}

				if (OpeningBalance)
				{
					string sql = string.Format(@"SELECT * FROM 
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
												  V.Date  < '{0}'
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
												  V.Date  BETWEEN '{0}' and '{1}'
											 )Q
											 GROUP BY TranDate, AccountsTypeID, AccountTypeName
 
											)Trans
											 {2}
											 ORDER BY trandate desc", startDate, endDate, sqlClause);

					SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
					da.Fill(ds, "ACTypeWiseBalanceDataSet");
					dbConn.Close();
				}
				else
				{ 
						
					string sql = string.Format(@"SELECT ROW_NUMBER() OVER(ORDER BY TranDate asc) row_number , convert(varchar, trandate, 106) TranDate, AccountsTypeID, AccountTypeName,
											 sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											 FROM 
											 (
												  SELECT convert(date, V.Date) TranDate, V.Date,
												  T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												  (Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												  FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												  V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												  V.Date  BETWEEN '{0}' and '{1}'
											 )Q
											 {2}
											 GROUP BY TranDate, AccountsTypeID, AccountTypeName",startDate,endDate,sqlClause);

					SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
					da.Fill(ds, "ACTypeWiseBalanceDataSet");
					dbConn.Close();
				
				}

				
				


			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		public static DataSet GetAccoutWiseBalance(string AccountTypeFilter, string AccountFilter, string startDate, string endDate, bool OpeningBalance)
		{

			DataSet ds = new DataSet();

			try
			{
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();
				string sqlClause = string.Empty;
				if (string.IsNullOrEmpty(AccountFilter) && string.IsNullOrEmpty(AccountTypeFilter))
				{
					sqlClause += "";
				}
				else if (string.IsNullOrEmpty(AccountFilter) && !string.IsNullOrEmpty(AccountTypeFilter))
				{

					sqlClause += " Where AccountTypeName ='" + AccountTypeFilter + "'";


				}
				else
				{
					sqlClause += " Where AccountName ='" + AccountFilter + "'";
				}

				if (OpeningBalance)
				{
					string sql = string.Format(@"SELECT 0 row_number, 'Opening_Balance' TrandDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName, 
											sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											FROM 
											(
												SELECT convert(date, V.Date) TranDate, 
												T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												(Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												V.Date  < '{0}'
											)Q
											{2}
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
												V.Date  BETWEEN '{0}' and '{1}'
											)Q
											{2}
											GROUP BY TranDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName", startDate, endDate, sqlClause);



					SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
					da.Fill(ds, "ACWiseBalanceDataSet");
					dbConn.Close();
				}
				else
				{

					string sql = string.Format(@"
											SELECT ROW_NUMBER() OVER(ORDER BY TranDate asc) row_number, convert(varchar, trandate, 106) TrandDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName, 
											sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
											FROM 
											(
												SELECT convert(date, V.Date) TranDate, 
												T.Id AccountsTypeID, T.Name AccountTypeName, A.Id AccountsID,  A.Name AccountName, 
												(Debit) Debit, (Credit)Credit, (Debit)-(Credit) Balance
												FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
												V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
												V.Date  BETWEEN '{0}' and '{1}'
											)Q
											{2}
											GROUP BY TranDate, AccountsTypeID, AccountTypeName, AccountsID, AccountName", startDate, endDate, sqlClause);



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
		public static DataSet GetAccoutWiseBalance(string AccountTypeFilter, string AccountFilter, string startDate, string endDate, int outletId)
		{

			DataSet ds = new DataSet();

			try
			{
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();
				string sqlClause = string.Empty;
				if (!string.IsNullOrEmpty(AccountFilter))
				{
					sqlClause = sqlClause + string.Format(@" and A.Name = '{0}'", AccountFilter);
				}
				if (!string.IsNullOrEmpty(AccountTypeFilter))
				{
					sqlClause = sqlClause + string.Format(@" and T.Name like '%{0}%'", AccountTypeFilter);
				}
				if (outletId > 0)
					sqlClause = sqlClause + string.Format(@" and ad.SyncOutletId = {0}", outletId);
				string sql = string.Format(@"
										SELECT
										T.Name AccountType,A.id AccountId, A.Name AccountName,
										sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
										FROM AccountTransactionValues V, Accounts A, AccountTypes T, AccountTransactionDocuments AD WHERE
										V.AccountTypeId = T.Id AND A.Id = V.AccountId AND v.AccountTransactionDocumentId = ad.Id  
										and V.Date  BETWEEN '{0}' and '{1}' 
										{2}
										GROUP BY V.accounttypeid, T.Name, V.accountid,A.id, A.Name
										order by AccountName", startDate, endDate, sqlClause);



				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
				da.Fill(ds, "ACWiseBalanceDataSet");
				dbConn.Close();

			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		public static DataSet GetAccountDrillThrough(int accountId, DateTime fromDate, DateTime toDate, int outletId)
		{

			DataSet ds = new DataSet();
			try
			{
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
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
							WHERE v.[Date] < '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}'  
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
							WHERE v.[Date] BETWEEN '{fromDate.ToString("dd MMM yyyy hh: mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}' 
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
					sql1 = string.Format(@"SELECT isnull(sum(debit),0) Debit,isnull(sum(credit),0) Credit,isnull(sum(debit)- sum(credit),0)Balance
											FROM AccountTransactionValues 
											WHERE [Date] < '{0}'  AND accountid = {1}", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), accountId);

					sql2 = $@"SELECT v.[Date]
									,at.Name + ':'+ ad.Name Description
									,v.Debit
									,v.Credit
									, v.debit-v.credit Balance
							FROM AccountTransactionValues v
								, AccountTransactions at
								, AccountTransactionDocuments ad
							WHERE v.[Date] BETWEEN '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}' 
								AND v.accountid = {accountId}
								AND v.AccountTransactionId = at.Id 
								AND at.AccountTransactionDocumentId = ad.id
							ORDER BY v.[Date]";

					sql3 = string.Format(@"SELECT isnull(sum(debit),0) Debit,isnull(sum(credit),0) Credit,isnull(sum(debit)- sum(credit),0)Balance
											FROM AccountTransactionValues 
											WHERE [Date] <= '{0}'  AND accountid = {1}", toDate.ToString("dd MMM yyyy hh:mm:ss tt"), accountId);
				}
				string sql = sql1 + ";" + sql2 + ";"+ sql3;
				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
				da.Fill(ds);

				ds.Tables[0].TableName = "OpeningBalance";
				ds.Tables[1].TableName = "ACDrill";
				ds.Tables[2].TableName = "ClosingBalance";
				dbConn.Close();
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return ds;
		}
		public static DataTable GetAccountType(int AccountTypeId)
		{
			DataTable dt = new DataTable();
			try
			{

				string sql = string.Empty;
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();

				sql = string.Format(@"Select * from AccountTypes where Id = {0}", AccountTypeId);
			  
				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
				da.Fill(dt);
				dbConn.Close();
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}

			return dt;
		}
		public static DataTable GetAccountType()
		{
			DataTable dt = new DataTable();
			try
			{

				string sql = string.Empty;
				string dbConnString = DBUtility.GetConnectionString();
				SqlConnection dbConn = new SqlConnection(dbConnString);
				dbConn.Open();
				sql = string.Format(@"Select * from AccountTypes");
				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
				da.Fill(dt);
				dbConn.Close();
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}
			return dt;
		}
	}
}