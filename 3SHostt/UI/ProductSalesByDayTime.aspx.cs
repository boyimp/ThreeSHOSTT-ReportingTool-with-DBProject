using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BusinessObjects.ChartsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using ThreeS.Domain.Models.Tickets;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using BusinessObjects.ReportManager;
using Microsoft.Reporting.WebForms;
using BusinessObjects.TicketsManager;

public partial class UI_ProductSalesByDayTime : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            CheckCurrentWorkPeriod.Checked = true;
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            LoadOutletsCbo();
            BindDepartment();
            BindGroupItem();
            foreach (RadComboBoxItem itm in ddlGroupItem.Items)
            {
                itm.Checked = true;
            }
            BindMenuItem();

            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            if (cbExactTime.Checked)
            {
                dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            }
            if (dsStartEndDate != null && dsStartEndDate.Tables != null && dsStartEndDate.Tables.Count > 0 && dsStartEndDate.Tables[0].Rows.Count > 0)
            {
                dtpFromDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString());
                dtpToDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString());
                BindReportViewerData(Convert.ToDateTime(dStartDate), Convert.ToDateTime(dEndDate));
            }
            else
                lblWorkPeriod.Text = string.Empty;
        }
	}
    private void LoadOutletsCbo()
    {
        DataSet ds = TicketManager.GetOutlets();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Id"] = "0";
        drow["Name"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlOutlets.DataSource = ds;
        ddlOutlets.DataBind();
        ddlOutlets.SelectedValue = "0";
    }
    private void BindMenuItem()
    {
        DataSet ds = MenuItemManager.GetMenuItem(string.Empty);
        DataRow drow = ds.Tables[0].NewRow();
        drow["Name"] = "All";
        drow["id"] = "0";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlMenuItem.DataSource = ds;
        ddlMenuItem.DataBind();
        ddlMenuItem.SelectedValue = "0";
    }
    private void BindGroupItem()
    {
        DataSet ds = MenuItemManager.GetGroupItem();
        RadComboBox comboBox = (RadComboBox)ddlGroupItem;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["GroupCode"].ToString();
            item.Value = row["GroupCode"].ToString().Replace("'", "''");
            item.Attributes.Add("GroupCode", row["GroupCode"].ToString());
            comboBox.Items.Add(item);
            item.DataBind();
        }
    }
    protected void ddlOutlets_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
    private void BindDepartment()
    {
        DataSet ds = TicketManager.GetDepartments();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Id"] = "0";
        drow["Name"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlDepartment.DataSource = ds;
        ddlDepartment.DataBind();
        ddlDepartment.SelectedValue = "0";
    }

    protected void ddlDepartment_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
    protected void ddlMenuItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

       

        if (cbExactTime.Checked)
        {
            dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        }
        if (dsStartEndDate != null && dsStartEndDate.Tables != null && dsStartEndDate.Tables.Count > 0 && dsStartEndDate.Tables[0].Rows.Count > 0)
        {
            BindReportViewerData(Convert.ToDateTime(dStartDate), Convert.ToDateTime(dEndDate));
        }
        else
            lblWorkPeriod.Text = string.Empty;

   }

    protected void BindReportViewerData(DateTime FromDate, DateTime ToDate)
	{
        //DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(FromDate, ToDate, CheckCurrentWorkPeriod.Checked);
        //DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        //DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

        //lblWorkPeriod.Text = "Working Period Considered from :" + dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString();
        int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);
        string GroupCodes = string.Empty;
        var checkeditems = ddlGroupItem.CheckedItems;
        foreach (var item in checkeditems)
        {
            GroupCodes = string.IsNullOrEmpty(GroupCodes) ? "'" + item.Value.ToString() + "'" : GroupCodes + ",'" + item.Value.ToString() + "'";
        }
        DataSet dSTimeWiseSales = new DataSet();
        dSTimeWiseSales = ChartsManager.GetProductSalesByDayTime(FromDate.ToString("M/d/yyyy hh:mm tt"), ToDate.ToString("M/d/yyyy hh:mm tt"), outletId, Convert.ToInt32(ddlDepartment.SelectedValue), ddlMenuItem.SelectedValue, GroupCodes);
        double days = (ToDate - FromDate).TotalDays;
        int weeks = (int)Math.Ceiling(days / 7);
        int nDays = Convert.ToInt32(days);

        List<ReportParameter> paras = new List<ReportParameter>();

        ReportParameter rptDate = new ReportParameter("rptDate", "Work Period Considered from " + FromDate.ToString("dd MMM yyyy") + " to " + ToDate.ToString("dd MMM yyyy")+Environment.NewLine
            +"Number of Days :"+ nDays.ToString()+Environment.NewLine+"Number of Week :"+weeks.ToString());
        paras.Add(rptDate);

        rptViewer1.LocalReport.DataSources.Clear();
        rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ProductSalesByDayTime", dSTimeWiseSales.Tables[0]));
        rptViewer1.LocalReport.ReportPath = "./Reports/rptProductSalesByDayTime.rdlc";
        rptViewer1.LocalReport.SetParameters(paras);
        rptViewer1.LocalReport.Refresh();

	}

    protected void ChckedChanged(object sender, EventArgs e)
    {
        if (CheckCurrentWorkPeriod.Checked)
        {
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;

            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            if (dsStartEndDate != null && dsStartEndDate.Tables != null && dsStartEndDate.Tables.Count > 0 && dsStartEndDate.Tables[0].Rows.Count > 0)
            {
                dtpFromDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString());
                dtpToDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString());
                BindReportViewerData(Convert.ToDateTime(dStartDate), Convert.ToDateTime(dEndDate));
            }
            else
                lblWorkPeriod.Text = string.Empty;
        }
        else
        {
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
        }
    }
    protected void ddlOutlets_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    protected void ddlDepartment_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlDepartment.SelectedIndex > 0)
            ddlOutlets.SelectedIndex = 0;
    }
}
