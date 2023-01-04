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
using Telerik.Web.UI;

public partial class UI_ItemSalesAccountWise : System.Web.UI.Page
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
            BindReportViewerData();
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

    protected void ddlTicketType_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }

    protected void ChckedChanged(object sender, EventArgs e)
    {
        if (CheckCurrentWorkPeriod.Checked)
        {
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            btnSearch_Click(sender, e);
        }
        else
        {
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
            btnSearch_Click(sender, e);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindReportViewerData();
    }
    protected void BindReportViewerData()
    {
        try
        {
            int OutletId = Convert.ToInt32(ddlOutlets.SelectedValue);
            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            string Ids = string.Empty;
            var checkeditems = ddlTicketType.CheckedItems;
            foreach (var item in checkeditems)
            {
                Ids = string.IsNullOrEmpty(Ids) ? item.Value.ToString() : Ids + ',' + item.Value.ToString();
            }
            if (cbExactTime.Checked)
            {
                dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            }
            if (rbGroupView.Checked)
            {
                DataSet ds = null;
                if (TicketManager.GetDataFetchFrom() != "FromStoredProcedure")
                    ds = TicketManager.GetItemSalesAccountWiseGroup(dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), Ids, OutletId);
                else
                    ds = TicketManager.GetMenuGroupAccountDetails(OutletId.ToString(), Ids, dStartDate, dEndDate);

                decimal SUMPrice = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMPrice"]);
                decimal SUMQuantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMQuantity"]);
                decimal SUMGross = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMGross"]);
                decimal SUMGift = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMGift"]);
                decimal SUMTotalCollection = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMTotalCollection"]);

                List<ReportParameter> paras = null;
                paras = ReportManager.GetReportParams(OutletId);

                ReportParameter rptSUMPrice = new ReportParameter("rptSUMPrice", SUMPrice.ToString("N2"));
                paras.Add(rptSUMPrice);
                ReportParameter rptSUMQuantity = new ReportParameter("rptSUMQuantity", SUMQuantity.ToString("N2"));
                paras.Add(rptSUMQuantity);
                ReportParameter rptSUMGross = new ReportParameter("rptSUMGross", SUMGross.ToString("N2"));
                paras.Add(rptSUMGross);
                ReportParameter rptSUMGift = new ReportParameter("rptSUMGift", SUMGift.ToString("N2"));
                paras.Add(rptSUMGift);
                ReportParameter rptSUMTotalCollection = new ReportParameter("rptSUMTotalCollection", SUMTotalCollection.ToString("N2"));
                paras.Add(rptSUMTotalCollection);

                string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
                                               dEndDate.ToString("dd MMM yyyy hh:mm tt");

                ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                paras.Add(rptDateRange);

                rptViewer1.LocalReport.DataSources.Clear();
                rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemSalesAccountWise", ds.Tables[1]));
                rptViewer1.LocalReport.ReportPath = "./Reports/rptItemSalesAccountWiseGroup.rdlc";
                rptViewer1.LocalReport.EnableExternalImages = true;
                rptViewer1.LocalReport.SetParameters(paras);
                rptViewer1.LocalReport.Refresh();
            }
            else
            {
                DataSet ds = null;
                if (TicketManager.GetDataFetchFrom() != "FromStoredProcedure")
                    ds = TicketManager.GetItemSalesAccountWise(dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), Ids, OutletId);
                else
                    ds = TicketManager.GetMenuItemsAccountDetails(OutletId.ToString(), Ids, dStartDate, dEndDate);

                decimal SUMPrice = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMPrice"]);
                decimal SUMQuantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMQuantity"]);
                decimal SUMGross = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMGross"]);
                decimal SUMGift = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMGift"]);
                //decimal SUMTotalCollection = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUMTotalCollection"]);

                List<ReportParameter> paras = null;
                paras = ReportManager.GetReportParams(OutletId);

                ReportParameter rptSUMPrice = new ReportParameter("rptSUMPrice", SUMPrice.ToString("N2"));
                paras.Add(rptSUMPrice);
                ReportParameter rptSUMQuantity = new ReportParameter("rptSUMQuantity", SUMQuantity.ToString("N2"));
                paras.Add(rptSUMQuantity);
                ReportParameter rptSUMGross = new ReportParameter("rptSUMGross", SUMGross.ToString("N2"));
                paras.Add(rptSUMGross);
                ReportParameter rptSUMGift = new ReportParameter("rptSUMGift", SUMGift.ToString("N2"));
                paras.Add(rptSUMGift);

                string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
                                               dEndDate.ToString("dd MMM yyyy hh:mm tt");

                ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                paras.Add(rptDateRange);

                rptViewer1.LocalReport.DataSources.Clear();
                //rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemSalesAccountWise", ds.Tables[0]));
                rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet_ItemSalesAccountWise", ds.Tables[1]));
                rptViewer1.LocalReport.ReportPath = "./Reports/rptItemSalesAccountWise.rdlc";
                rptViewer1.LocalReport.EnableExternalImages = true;
                rptViewer1.LocalReport.SetParameters(paras);
                rptViewer1.LocalReport.Refresh();
            }
        }
        catch // (Exception e)
        {

            throw;
        }
        //lblWorkPeriod.Text = "Work Period Considered From " + dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString();
    }

}
