using BusinessObjects.ReportManager;
using BusinessObjects.TicketsManager;
using BusinessObjects.AccountsManager;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Telerik.Web.UI;

public partial class UI_ProfitAndLosss : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
        {
            Response.Redirect("Default.aspx");
        }

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

    //protected void ddlTicketType_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    //{
    //    e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
    //    e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    //}

    private void BindData()
    {
        try
        {
            int OutletId = Convert.ToInt32(ddlOutlets.SelectedValue);

            DataSet ds = null;
            DateTime dStartDate;
            DateTime dEndDate;

            if (!cbExactTime.Checked)
            {
                DataSet dsStartEndDate = AccountManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
                dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
                dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            }
            else
            {
                dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            }


            if (AccountManager.GetDataFetchFrom() == "FromStoredProcedure")
                ds = AccountManager.GetProfitAndLossReportSP(dStartDate, dEndDate, OutletId);

            List<ReportParameter> paras = ReportManager.GetReportParams(OutletId);

            string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
                                               dEndDate.ToString("dd MMM yyyy hh:mm tt");

            ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
            paras.Add(rptDateRange);


            rptViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptWorkPeriodReport2.rdlc");
            rptViewer1.LocalReport.DataSources.Clear();
            foreach (DataTable dt in ds.Tables)
            {
                rptViewer1.LocalReport.DataSources.Add(new ReportDataSource(dt.TableName, dt));
            }


            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
            //UpdatePanel1.Update();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
