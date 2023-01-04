using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessObjects.WebPushManager
{
    public class WebPushManager
    {
        public static void Insert(WebPushClientModel webPushClient)
        {
            SqlTransaction tran = null;
            using (var Conn = new SqlConnection(DBUtility.GetConnectionString()))
            {
                try
                {

                    string query = string.Format(@" UPDATE  [WebPushClients] Set UserName = @UserName,
			                                                PushEndpoint=@PushEndpoint,
			                                                P256DH = @P256DH, 
			                                                Auth = @Auth
                                                            WHERE UserName = @UserName and PushEndpoint=@PushEndpoint and
			                                                P256DH = @P256DH and Auth = @Auth

                                                    IF @@ROWCOUNT = 0
                                                         Insert Into WebPushClients
                                                            (
	                                                             [Username]
                                                                ,[PushEndPoint]
                                                                ,[P256DH]
                                                                ,[Auth]
                                                            )
                                                            Values
                                                            (
	                                                            @UserName,@PushEndpoint,@P256DH,@Auth
                                                            )                                                         
                                                    ");
                    Conn.Open();
                    tran = Conn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand(query, Conn, tran);
                    cmd.Parameters.AddWithValue("@UserName", webPushClient.UserName);
                    cmd.Parameters.AddWithValue("@PushEndpoint", webPushClient.PushEndpoint);
                    cmd.Parameters.AddWithValue("@P256DH", webPushClient.P256DH);
                    cmd.Parameters.AddWithValue("@Auth", webPushClient.Auth);

                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    Conn.Close();



                    if (Conn.State == System.Data.ConnectionState.Open)
                        Conn.Close();

                }
                catch (Exception)
                {
                    if (tran != null) tran.Rollback();
                    throw;
                }
            }

        }
        public static void Delete(WebPushClientModel webPushClient)
        {
            SqlTransaction tran = null;
            using (var Conn = new SqlConnection(DBUtility.GetConnectionString()))
            {
                try
                {
                    //Have to figure out a way to Delete from UI only Specific UserName and related touples
                    string query = string.Format(@" Delete from  [WebPushClients] 
                                                    WHERE  PushEndpoint=@PushEndpoint and
			                                        P256DH = @P256DH and Auth = @auth
                                                    ");
                    Conn.Open();
                    tran = Conn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand(query, Conn, tran);
                    //cmd.Parameters.AddWithValue("@UserName", webPushClient.UserName);
                    cmd.Parameters.AddWithValue("@PushEndpoint", webPushClient.PushEndpoint);
                    cmd.Parameters.AddWithValue("@P256DH", webPushClient.P256DH);
                    cmd.Parameters.AddWithValue("@Auth", webPushClient.Auth);

                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    Conn.Close();



                    if (Conn.State == System.Data.ConnectionState.Open)
                        Conn.Close();

                }
                catch (Exception)
                {
                    if (tran != null) tran.Rollback();
                    throw;
                }
            }

        }
    }
}
