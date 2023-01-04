using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
//using System.Net.Mail;
//using System.IO;
//using BusinessObjects;
//using System.Xml;

public partial class UI_Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}


//public partial class UI_Default : System.Web.UI.Page
//{
//	protected void Page_Load(object sender, EventArgs e)
//	{
//		if (!IsPostBack)
//		{
//            //string appPath = HttpContext.Current.Request.ApplicationPath.ToString();
//            //string hosttSettings = appPath == "/" ? appPath.Replace('/','\\') + "HOSTTSettings.txt" : appPath.Replace('/', '\\') + "_HOSTTSettings.txt";
//            //lblMessage0.Text = appPath + "--" + hosttSettings+"--"+ HttpContext.Current.Server.MapPath("~")
//            //    +"--"+ HttpContext.Current.Server.MapPath("~")+hosttSettings;
//            txtUsername.Focus();
//		}
//		lblMessage.Visible = false;
//	}
//	protected void btnLogin_Click(object sender, EventArgs e)
//	{
//		string username = txtUsername.Text.Trim();
//		string password = txtPassword.Text.Trim();
//        lblMessage.Text = string.Empty;
//        lblMessage.Visible = false;
//        try
//        {
//            if (UserManager.Exist(username, password))
//            {
//                Session.Contents["CurrentUser"] = username;

//                Response.Redirect("~/UI/Home.aspx", false);
//                Context.ApplicationInstance.CompleteRequest();
//            }
//            else
//            {
//                XmlDocument xml = new XmlDocument();
//                string hosttSettingsPath = DBUtility.GetFilePathFromAppConfig("HOSTTSettings.txt");
//                xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);

//                string reportUserName = string.Empty;
//                string reportUserPassword = string.Empty;
//                string reportLogin = string.Empty;

//                reportLogin = xml.SelectSingleNode("/SettingsObject/ReportLogin") == null ? string.Empty :
//                    xml.SelectSingleNode("/SettingsObject/ReportLogin").InnerText.Trim();

//                string[] values = reportLogin.Split(';').Select(sValue => sValue.Trim()).ToArray();
//                if (values.Count() > 1)
//                {
//                    reportUserName = values[0];
//                    reportUserPassword = values[1];
//                }

//                if (username == reportUserName && password == reportUserPassword)
//                {
//                    Session.Contents["CurrentUser"] = username;
//                    Response.Redirect("~/UI/Home.aspx");
//                }
//                else
//                {
//                    lblMessage.Text = "Username or Password is incorrect. Please try again.";
//                    lblMessage.Visible = true;
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            lblMessage.Visible = true;
//            lblMessage.Text = ex.Message;
//        }
//	}
//}
