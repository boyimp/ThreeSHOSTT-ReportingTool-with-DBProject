using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using BusinessObjects;

/// <summary>
/// Summary description for UserManager
/// </summary>
public class UserManager
{
    public static User Get(int userID)
    {
        string dbConnString = DBUtility.GetConnectionString();
        SqlConnection dbConn = new SqlConnection(dbConnString);
        dbConn.Open();
        string sqlString = "SELECT * FROM Users WHERE PinCode = @UserID";
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
        dbCommand.Parameters.AddWithValue("@UserID", userID);
        SqlDataReader reader = dbCommand.ExecuteReader();

        User user = null;
        while (reader.Read())
        {
            user.Username = (string)reader["Name"];
            user.Password = (string)reader["PinCode"];
        }
        reader.Close();

        dbConn.Close();

        return user;
    }
    public static bool Exist(string username, string password)
    {
        bool exist = false;
        string dbConnString = DBUtility.GetConnectionString();
        var sscsb = new SqlConnectionStringBuilder(dbConnString);
        sscsb.ConnectTimeout = 20;
        SqlConnection dbConn = new SqlConnection(sscsb.ConnectionString);
        dbConn.Open();
        string sql = "SELECT * FROM Users WHERE Name = '" + username + "' AND PinCode = '" + password + "'";
        SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
        DataSet ds = new DataSet();
        da.Fill(ds, "Users");
        if (ds.Tables["Users"].Rows.Count > 0)
        {
            exist = true;
        }

        dbConn.Close();
        return exist;
    }
}