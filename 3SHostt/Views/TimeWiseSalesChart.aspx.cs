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

public partial class UI_TimeWiseSalesChart : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("Default.aspx");

        if (!IsPostBack)
        {
        //    CheckCurrentWorkPeriod.Checked = true;
        //    dtpFromDate.SelectedDate = DateTime.Today;
        //    dtpToDate.SelectedDate = DateTime.Today;
        //    dtpFromDate.Enabled = false;
        //    dtpToDate.Enabled = false;



        //    DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), true);
        //    DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        //    DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        //    if (cbExactTime.Checked)
        //    {
        //        dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
        //        dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        //    }
        //    if (dsStartEndDate != null && dsStartEndDate.Tables != null && dsStartEndDate.Tables.Count > 0 && dsStartEndDate.Tables[0].Rows.Count > 0)
        //    {
        //        dtpFromDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString());
        //        dtpToDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString());
        //        BindReportViewerData(Convert.ToDateTime(dStartDate), Convert.ToDateTime(dEndDate));
        //    }
        //    else
        //        lblWorkPeriod.Text = string.Empty;
        }
    }
    
    protected void ddlOutlets_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
   

    protected void ddlDepartment_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        //DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), true);
        //DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        //DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        //if (cbExactTime.Checked)
        //{
        //    dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
        //    dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        //}
        //if (dsStartEndDate != null && dsStartEndDate.Tables != null && dsStartEndDate.Tables.Count > 0 && dsStartEndDate.Tables[0].Rows.Count > 0)
        //{
        //    BindReportViewerData(Convert.ToDateTime(dStartDate), Convert.ToDateTime(dEndDate));
        //}
        //else
        //    lblWorkPeriod.Text = string.Empty;

    }

    protected void BindReportViewerData(DateTime FromDate, DateTime ToDate)
    {
    //    int outletId = Convert.ToInt32(0);
    //    DataSet dSTimeWiseSales = new DataSet();
    //    DataSet dSMonthlySales = new DataSet();
    //    dSTimeWiseSales = ChartsManager.GetTimeWiseSalesForChart(FromDate.ToString("M/d/yyyy hh:mm tt"), ToDate.ToString("M/d/yyyy hh:mm tt"), outletId, Convert.ToInt32(0));

    //    List<ReportParameter> paras = new List<ReportParameter>();

    //    ReportParameter rptDate = new ReportParameter("rptDate", "Work Period Considered from " + FromDate.ToString("dd MMM yyyy hh:mm tt") + " to " + ToDate.ToString("dd MMM yyyy hh:mm tt"));
    //    paras.Add(rptDate);

        //rptViewer1.LocalReport.DataSources.Clear();
        //rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("TimeWiseSalesDataset", dSTimeWiseSales.Tables[0]));
        //rptViewer1.LocalReport.ReportPath = "./Reports/rptTimeWiseSales.rdlc";
        //rptViewer1.LocalReport.SetParameters(paras);
        //rptViewer1.LocalReport.Refresh();

    }

    protected void ChckedChanged(object sender, EventArgs e)
    {
    //if (CheckCurrentWorkPeriod.Checked)
    //{
    //    dtpFromDate.Enabled = false;
    //    dtpToDate.Enabled = false;

    //    DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), true);
    //    DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
    //    DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
    //    if (dsStartEndDate != null && dsStartEndDate.Tables != null && dsStartEndDate.Tables.Count > 0 && dsStartEndDate.Tables[0].Rows.Count > 0)
    //    {
    //        dtpFromDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString());
    //        dtpToDate.SelectedDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString());
    //        BindReportViewerData(Convert.ToDateTime(dStartDate), Convert.ToDateTime(dEndDate));
    //    }
    //    else
    //        lblWorkPeriod.Text = string.Empty;
    //}
    //else
    //{
    //    dtpFromDate.Enabled = true;
    //    dtpToDate.Enabled = true;
    }

    

    //protected void ddlDepartment_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    //if (ddlDepartment.SelectedIndex > 0)
    //    //    ddlOutlets.SelectedIndex = 0;
    //}
}
