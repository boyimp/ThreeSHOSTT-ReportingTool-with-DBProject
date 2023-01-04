using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessObjects.SyncManager
{
    public class SyncManager
    {

        public static DataSet GetSyncOutletStatus()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sql = string.Empty;
                dbConn.Open();
               
                    sql = string.Format(@"select Id, Name, [dbo].[ufsFormat](LastOutletWorkPeriodStarted, 'mmm dd yyyy hh:mm AM/PM') LastWorkPeriodStarted, [dbo].[ufsFormat](LastOutletWorkPeriodEnded, 'mmm dd yyyy hh:mm AM/PM') LastWorkPeriodEnded, 
                            [dbo].[ufsFormat](LastSyncedOutletTime, 'mmm dd yyyy hh:mm AM/PM') LastSyncedTime, [dbo].[ufsFormat](LastFinalizedWorkPeriod, 'mmm dd yyyy hh:mm AM/PM') LastFinalizedWorkPeriod, Cast(IsActive as bit) IsActive, OutletInfo
                            from SyncOutlets
                            order by Name");

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds, "SyncOutletStatus");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataTable GetSyncOutletStatus(int SyncOutletId)
        {
            DataTable ds = new DataTable("SyncOutletStatus");
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sql = string.Empty;
                dbConn.Open();

                sql = string.Format(@"select Id, Name, [dbo].[ufsFormat](LastOutletWorkPeriodStarted, 'mmm dd yyyy hh:mm AM/PM') LastWorkPeriodStarted, [dbo].[ufsFormat](LastOutletWorkPeriodEnded, 'mmm dd yyyy hh:mm AM/PM') LastWorkPeriodEnded, 
                            [dbo].[ufsFormat](LastSyncedOutletTime, 'mmm dd yyyy hh:mm AM/PM') LastSyncedTime, [dbo].[ufsFormat](LastFinalizedWorkPeriod, 'mmm dd yyyy hh:mm AM/PM') LastFinalizedWorkPeriod, Cast(IsActive as bit) IsActive, OutletInfo
                            from SyncOutlets
                            Where Id = {0}", SyncOutletId);

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
    }
}
