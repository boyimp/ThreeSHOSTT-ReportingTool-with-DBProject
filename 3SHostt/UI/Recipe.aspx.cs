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
using Telerik.Web.UI;
using System.Xml;
using BusinessObjects.ReportManager;

public partial class UI_Recipe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            BindGroupItem();
            BindMenuItem(ddlGroupItem.SelectedValue);
            BindData();
        }
    }

    private void BindGroupItem()
    {
        DataSet ds = MenuItemManager.GetGroupItem();
        DataRow drow = ds.Tables[0].NewRow();
        drow["GroupCode"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlGroupItem.DataSource = ds;
        ddlGroupItem.DataBind();
        ddlGroupItem.SelectedValue = "All";
    }

    private void BindMenuItem(string groupItem)
    {
        DataSet ds = MenuItemManager.GetMenuItem(groupItem == "All" ? string.Empty : groupItem);
        DataRow drow = ds.Tables[0].NewRow();
        drow["Name"] = "All";
        drow["id"] = "0";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlMenuItem.DataSource = ds;
        ddlMenuItem.DataBind();
        ddlMenuItem.SelectedValue = "0";
    }
    
    protected void ddlGroupItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
    }

    protected void ddlGroupItem_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindMenuItem(e.Text);
    }

    protected void ddlMenuItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        try
        {
            DataSet ds = null;
            ds = MenuItemManager.GetRecipes(Convert.ToInt32(ddlMenuItem.SelectedValue), ddlGroupItem.SelectedValue);//(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue), chkOpen.Checked);      
            List<ReportParameter> paras = ReportManager.GetReportParams();
            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Recipe", ds.Tables[0]));
            rptViewer1.LocalReport.ReportPath = "./Reports/rptRecipe.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
        }
        catch (Exception)
        {

            throw;
        }
    }
}