using BusinessObjects;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

public partial class Views_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUsername.Focus();
        }
        lblMessage.Visible = false;
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        if (UserManager.Exist(username, password))
        {
            Session["CurrentUser"] = username;
            Response.Redirect("~/Views/Index.aspx");
        }
        else
        {
            XmlDocument xml = new XmlDocument();
            string hosttSettingsPath = DBUtility.GetFilePathFromAppConfig("HOSTTSettings.txt");
            xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);

            string reportUserName = string.Empty;
            string reportUserPassword = string.Empty;
            string reportLogin = string.Empty;

            reportLogin = xml.SelectSingleNode("/SettingsObject/ReportLogin") == null ? string.Empty :
                xml.SelectSingleNode("/SettingsObject/ReportLogin").InnerText.Trim();

            string[] values = reportLogin.Split(';').Select(sValue => sValue.Trim()).ToArray();
            if (values.Count() > 1)
            {
                reportUserName = values[0];
                reportUserPassword = values[1];
            }

            if (username == reportUserName && password == reportUserPassword)
            {
                Session["CurrentUser"] = username;
                Response.Redirect("~/UI/Home.aspx");
            }
            else
            {
                lblMessage.Text = "Username or Password is incorrect. Please try again.";
                lblMessage.Visible = true;
            }
        }
    }
}
