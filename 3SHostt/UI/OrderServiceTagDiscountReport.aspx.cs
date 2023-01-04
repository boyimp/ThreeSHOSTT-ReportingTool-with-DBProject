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
using BusinessObjects.TicketsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using ThreeS.Domain.Models.Tickets;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using BusinessObjects.ReportManager;
using Microsoft.Reporting.WebForms;

public partial class UI_OrderServiceTagDiscountReport : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {          

            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            LoadOutletsCbo();
            BindDepartment();
            foreach (RadComboBoxItem itm in ddlTicketType.Items)
            {
                itm.Checked = true;
            }
            BindData();
            //DataSet ds = TicketManager.GetStartAndEndDateOfLastWorkPeriod();
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    //lblWorkPeriod.Text = "Last Closed Working Period " + ds.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + ds.Tables[0].Rows[0]["EndDate"].ToString();
            //    dtpFromDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"].ToString());
            //    dtpToDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"].ToString());
            //    BindData();
            //}
            //else
            //    lblWorkPeriod.Text = string.Empty;
        }
	}
    protected void ChckedChanged(object sender, EventArgs e)
    {
        if (CheckCurrentWorkPeriod.Checked)
        {
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            BindData();
        }
        else
        {
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
            BindData();
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
    protected void ddlOutlets_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
    private void BindDepartment()
    {
        DataSet ds = TicketManager.GetDepartments();
        //ddlTicketType.DataSource = ds;
        //ddlTicketType.DataBind();
        //ddlTicketType.SelectedValue = "0";
        RadComboBox comboBox = (RadComboBox)ddlTicketType;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["Name"].ToString();
            item.Value = row["Id"].ToString();
            item.Attributes.Add("Name", row["Name"].ToString());

            comboBox.Items.Add(item);

            item.DataBind();
        }
    }
    private void BindData()
    {
        try
        {
            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);
            if (cbExactTime.Checked)
            {
                dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            }
            string Ids = string.Empty;
            var checkeditems = ddlTicketType.CheckedItems;
            foreach (var item in checkeditems)
            {
                Ids = string.IsNullOrEmpty(Ids) ? item.Value.ToString() : Ids + ',' + item.Value.ToString();
            }

            DataSet dsOrderTags = new DataSet();
            DataTable dtOrderTags = new DataTable("OrderTags");
            dtOrderTags.Columns.AddRange(new DataColumn[] { new DataColumn("GroupCode"), new DataColumn("MenuItemName"), new DataColumn("Quantity", typeof(decimal)), new DataColumn("Total", typeof(decimal)) });
            List<Ticket> oTickets = TicketManager.GetTicketsFaster(outletId, Ids, dStartDate, dEndDate);
            List<Ticket> oValidTickets = oTickets.Where(x => x.TotalAmount >= 0).ToList();
            //var orderTags = TicketManager.GetOrderTagService(oValidTickets);
            var orderTags = oValidTickets
                   .SelectMany(x => x.Orders.Where(y => !string.IsNullOrEmpty(y.OrderTags)))
                   .SelectMany(x => x.GetOrderTagValues(y => y.MenuItemId == 0).Select(y => new { TagName = y.TagValue, x.MenuItemName, x.Quantity, x.Value }))
                   .GroupBy(x => new { x.TagName, x.MenuItemName })
                   .Select(x => new { x.Key.TagName, x.Key.MenuItemName, Quantity = x.Sum(y => y.Quantity), Value = x.Sum(y => y.Value) }).ToList();

            foreach (var item in orderTags)
            {
                dtOrderTags.Rows.Add(new object[] { item.TagName.ToString(), item.MenuItemName, item.Quantity.ToString("N2"), item.Value.ToString("N2") });
            }

            dsOrderTags.Tables.Add(dtOrderTags.Copy());
            List<ReportParameter> paras = ReportManager.GetReportParams(outletId);

            string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
                                               dEndDate.ToString("dd MMM yyyy hh:mm tt");

            ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
            paras.Add(rptDateRange);


            rptViewer1.LocalReport.DataSources.Clear();
            foreach (DataTable dt in dsOrderTags.Tables)
            {
                rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(dt.TableName, dt));
            }
            rptViewer1.LocalReport.ReportPath = "./Reports/rptOrderTagsReport.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
        }
        catch (Exception)
        {
            throw;
        }
    }
	protected void btnSearch_Click(object sender, EventArgs e)
	{
        BindData();
	}	
}
