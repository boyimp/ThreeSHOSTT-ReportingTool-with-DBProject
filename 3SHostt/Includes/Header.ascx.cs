//In the name of Allah
using System;
using System.Web;
using ThreeS.Report.v2.Utils;

public partial class Includes_Header : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (HttpContext.Current.Session[GlobalData.CURRENT_USER] is null)
        {
            Response.Redirect("Default.aspx");
        }//if
        lblLoggedInPFCompanyUser.Text = HttpContext.Current.Session[GlobalData.CURRENT_USER].ToString();

    }
}
