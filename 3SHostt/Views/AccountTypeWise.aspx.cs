using BusinessObjects.AccountsManager;
using BusinessObjects.InventoryManager;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Xml;
using BusinessObjects.ReportManager;

public partial class UI_AccountTypeWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("Default.aspx");

        if (!IsPostBack)
        {
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            BindGroupItem();

        }

    }

    private void BindReportViewer1Data(string AccountTypeFilter, bool CheckOpeningBalance)
    {
        try
        {
            DataSet dSet = new DataSet();
            DataTable DT = null;

            dSet = AccountManager.GetAccoutTypeWiseBalance((AccountTypeFilter == "All" ? string.Empty : AccountTypeFilter),
                (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy"), (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"), CheckOpeningBalance);

            List<ReportParameter> paras = ReportManager.GetReportParams();

            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ACTypeWiseBalanceDataSet", dSet.Tables[0]));
            rptViewer1.LocalReport.ReportPath = "./Reports/rptACTypeWiseReport.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();

        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);

        }
    }

    private void BindGroupItem()
    {
        DataSet ds = AccountManager.GetGroupItem();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Name"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlAccountType.DataSource = ds;
        ddlAccountType.DataBind();
        ddlAccountType.SelectedValue = "All";
    }

    protected void ddlAccountType_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Name"].ToString();
    }

    protected void ddlAccountType_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindReportViewer1Data(ddlAccountType.SelectedValue.ToString(),chkOpeningBalance.Checked);
    }

}
