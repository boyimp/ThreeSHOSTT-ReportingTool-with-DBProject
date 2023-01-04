using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;

/// <summary>
/// Summary description for WarehouseManager
/// </summary>
public class WarehouseManager
{
    public static DataSet GetWarehouse()
    {
        DataSet ds = new DataSet();
        try
        {
            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();
            string sql = "select id,Name FROM Warehouses order by Name";
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