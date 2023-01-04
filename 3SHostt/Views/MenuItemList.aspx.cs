using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ThreeS.Domain.Models.Tickets;
using BusinessObjects.TicketsManager;
using BusinessObjects.MenusManager;
using ThreeS.Modules.BasicReports.Reports;
using Microsoft.Reporting.WebForms;
using System.Xml;
using BusinessObjects.ReportManager;
using BusinessObjects;

public partial class UI_MenuItemList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("Default.aspx");
        string companyLogoPath = DBUtility.GetFilePathFromAppConfig("CompanyLogo.png");
        string CompanyLogo = new Uri(HttpContext.Current.Server.MapPath("~" + companyLogoPath)).AbsoluteUri;
        lblPath.Text = companyLogoPath + "--" + CompanyLogo;
        if (!IsPostBack)
        {
            BindDepartment();
            BindReportViewerData();
        }
    }

    private void BindDepartment()
    {
        DataSet ds = TicketManager.GetDepartments();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Id"] = "0";
        drow["Name"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlTicketType.DataSource = ds;
        ddlTicketType.DataBind();
        ddlTicketType.SelectedValue = "0";
    }

    protected void ddlTicketType_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindReportViewerData();
    }
    protected void BindReportViewerData()
    {

        DataSet ds = MenuItemManager.GetMenuItemList(ddlTicketType.SelectedValue.Equals("0") ? string.Empty : "and d.Id ="+ddlTicketType.SelectedValue.ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
   

            List<ReportParameter> paras = ReportManager.GetReportParams();

            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MenuItemList", ds.Tables[0]));
            rptViewer1.LocalReport.ReportPath = "./Reports/rptMenuItemList.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
        }
        //lblWorkPeriod.Text = "Work Period Considered From " + dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString();
    }

  
    
   
}

