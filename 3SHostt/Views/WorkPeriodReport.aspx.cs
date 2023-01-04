using BusinessObjects.ReportManager;
using BusinessObjects.TicketsManager;

using Microsoft.Reporting.WebForms;

using System;
using System.Collections.Generic;
using System.Data;

public partial class UI_WorkPeriodReport : System.Web.UI.Page
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
            LoadOutletsCbo();
            BindDepartment();
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
    private void BindData()
    {
        try
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Binding Work Periods Report");
            int OutletId = Convert.ToInt32(ddlOutlets.SelectedValue);
            bool IsShort = false;
            DataSet ds = null;
            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            if (cbExactTime.Checked)
            {
                dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            }
            if (Request.QueryString["ReportType"] != null && Request.QueryString["ReportType"].Equals("Short"))
                IsShort = true;

            ds = TicketManager.GetReport(dStartDate, dEndDate, ddlTicketType.SelectedValue.ToString(), OutletId, IsShort);//(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue), chkOpen.Checked);      

            List<ReportParameter> paras = ReportManager.GetReportParams(OutletId);

            string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
                                               dEndDate.ToString("dd MMM yyyy hh:mm tt");

            ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
            paras.Add(rptDateRange);

            ReportParameter rptIsShort = new ReportParameter("rptIsShort", IsShort.ToString());
            paras.Add(rptIsShort);

            rptViewer1.LocalReport.DataSources.Clear();
            foreach (DataTable dt in ds.Tables)
            {
                rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(dt.TableName, dt));
            }
            rptViewer1.LocalReport.ReportPath = "./Reports/rptWorkPeriodReport.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
        }
        catch (Exception ex)
        {

            NLog.LogManager.GetCurrentClassLogger().Error(ex);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
