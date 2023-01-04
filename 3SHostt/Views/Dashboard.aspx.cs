using BusinessObjects.AccountsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.ChartsManager;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BusinessObjects.TicketsManager;
using ThreeS.Report.v2.Utils;

public partial class UI_NewDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session[GlobalData.CURRENT_USER] is null)
        {
            Response.Redirect("Default.aspx");
        }//if


    }
}
