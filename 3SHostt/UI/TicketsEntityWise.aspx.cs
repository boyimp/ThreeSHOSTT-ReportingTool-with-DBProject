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

public partial class UI_TicketsEntityWise : System.Web.UI.Page
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
            BindDepartment();
            LoadOutletsCbo();
            //BindReportViewerData();
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
            //btnSearch_Click(sender, e);
        }
        else
        {
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
            //btnSearch_Click(sender, e);
        }
    }
    protected void ddlOutlets_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
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
        try
        {            
            DateTime dStartDate;
            DateTime dEndDate;
            if (!cbExactTime.Checked)
            {
                DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
                dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
                dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            }
            else
            {
                dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            }
            DataTable table = new DataTable("TicketsEntityWise");
            DataTable table2 = new DataTable();
            int OutletId = Convert.ToInt32(ddlOutlets.SelectedValue);
            string Ids = string.Empty;
            var checkeditems = ddlTicketType.CheckedItems;
            Ids = string.Join(",", checkeditems.Select(i => i.Value));
            DataSet ds = TicketManager.GetTicketsEntityWise(dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), Ids, OutletId);
            DataSet ds2 = new DataSet();
            double dVATDenominator = TicketManager.GetVATIncludingDenominator(OutletId);
            List<Ticket> oTickets = new List<Ticket>();
            if (dVATDenominator > 0)
                oTickets = TicketManager.GetTicketsFaster(OutletId, Ids, dStartDate, dEndDate);

            table2 = TicketManager.GetTicketsWithIncludingVAT(oTickets, ds.Tables[0], 0);

            table.Columns.AddRange(new DataColumn[] {  new DataColumn("TicketId"),
                        new DataColumn("TicketDate", typeof(DateTime)), new DataColumn("UltimateAccount"), new DataColumn("Amount", typeof(decimal)), new DataColumn("TotalAmount",typeof(decimal)),
            new DataColumn("SortOrder", typeof(int)), new DataColumn("EntityTypeName"), new DataColumn("EntityName"), new DataColumn("NoOfGuests", typeof(int)), new DataColumn("SettledBy"),
                new DataColumn("Note")});

            foreach (DataRow row in table2.Rows)
            {
                table.Rows.Add(new object[] { row["TicketId"].ToString(),
                    Convert.ToDateTime(row["TicketDate"]).ToString(), row["UltimateAccount"].ToString(), Convert.ToDecimal(row["Amount"]), Convert.ToDecimal(row["TotalAmount"]),
                    Convert.ToInt32(row["SortOrder"]), row["EntityTypeName"].ToString(), row["EntityName"].ToString(), Convert.ToInt32(row["NoOfGuests"]),row["SettledBy"] as string,
                    row["Note"].ToString() });
            }
            ds2.Tables.Add(table);

            ds2.Tables.Add(ds.Tables[1].Copy());
            if (ds2.Tables[0].Rows.Count > 0)
            {            
                decimal SUMTotalAmount = Convert.ToDecimal(ds2.Tables[1].Rows[0]["SUMTotalAmount"]);

                List<ReportParameter> paras = ReportManager.GetReportParams();

                ReportParameter rptSUMTotalAmount = new ReportParameter("rptSUMTotalAmount", SUMTotalAmount.ToString("N2"));
                paras.Add(rptSUMTotalAmount);

                string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
                                                    dEndDate.ToString("dd MMM yyyy hh:mm tt");

                ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                paras.Add(rptDateRange);

                rptViewer1.LocalReport.DataSources.Clear();
                rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("TicketsEntityWise", ds2.Tables[0]));
                rptViewer1.LocalReport.ReportPath = "./Reports/rptTicketsEntityWise.rdlc";
                rptViewer1.LocalReport.EnableExternalImages = true;
                rptViewer1.LocalReport.SetParameters(paras);
                rptViewer1.LocalReport.Refresh();
            }
        }
        catch// (Exception e)
        {
            
            throw;
        }
    }
}
