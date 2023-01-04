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

public partial class UI_TicketsEntityWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("Default.aspx");

        if (!IsPostBack)
        {
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            BindReportViewerData();
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

            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

            DataSet ds = TicketManager.GetTicketsEntityWise(dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), string.Empty, 0);
            List<Ticket> oTickets = TicketManager.GetTicketsFaster(0, string.Empty, dStartDate, dEndDate);
            TicketManager.GetTicketsWithIncludingVAT(oTickets, ds.Tables[0], 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //decimal SUMPrice = Convert.ToDecimal(ds.Tables[0].Compute("SUM(Price)", ""));
                //decimal SUMQuantity = Convert.ToDecimal(ds.Tables[0].Compute("SUM(Quantity)", ""));
                //decimal SUMGross = Convert.ToDecimal(ds.Tables[0].Compute("SUM(Gross)", ""));
                //decimal SUMGift = Convert.ToDecimal(ds.Tables[0].Compute("SUM(Gift)", ""));
                decimal SUMTotalAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["SUMTotalAmount"]);

                List<ReportParameter> paras = ReportManager.GetReportParams();

                //ReportParameter rptSUMPrice = new ReportParameter("rptSUMPrice", SUMPrice.ToString());
                //paras.Add(rptSUMPrice);
                //ReportParameter rptSUMQuantity = new ReportParameter("rptSUMQuantity", SUMQuantity.ToString());
                //paras.Add(rptSUMQuantity);
                //ReportParameter rptSUMGross = new ReportParameter("rptSUMGross", SUMGross.ToString());
                //paras.Add(rptSUMGross);
                //ReportParameter rptSUMGift = new ReportParameter("rptSUMGift", SUMGift.ToString());
                //paras.Add(rptSUMGift);

                ReportParameter rptSUMTotalAmount = new ReportParameter("rptSUMTotalAmount", SUMTotalAmount.ToString("N2"));
                paras.Add(rptSUMTotalAmount);

                string DateRange = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyyy hh:mm tt") + " to " +
                                                   Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");

                ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                paras.Add(rptDateRange);




                rptViewer1.LocalReport.DataSources.Clear();
                rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("TicketsEntityWise", ds.Tables[0]));
                rptViewer1.LocalReport.ReportPath = "./Reports/rptTicketsEntityWise.rdlc";
                rptViewer1.LocalReport.EnableExternalImages = true;
                rptViewer1.LocalReport.SetParameters(paras);
                rptViewer1.LocalReport.Refresh();
            }
        }
        catch (Exception e)
        {

            throw;
        }
        //lblWorkPeriod.Text = "Work Period Considered From " + dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString();
    }
}
