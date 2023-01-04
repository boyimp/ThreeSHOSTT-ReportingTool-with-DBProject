using BusinessObjects.AccountsManager;
using BusinessObjects.InventoryManager;
using System;
using System.Data;
using System.Xml;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using BusinessObjects.ReportManager;

public partial class UI_AccountWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            BindGroupItem();
            BindAccounts(ddlAccountType.SelectedValue.ToString());

        }

    }

    #region BindReportViewerData
    private void BindReportViewerData(string AccountTypeFilter, string AccountFilter, bool OpeningBalance )
    {
        try
        {
            DataSet dSet = new DataSet();
            dSet = AccountManager.GetAccoutWiseBalance((AccountTypeFilter == "All" ? string.Empty : AccountTypeFilter), (AccountFilter == "All" ? string.Empty : AccountFilter),
                (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy"), (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"),OpeningBalance);

            List<ReportParameter> paras = ReportManager.GetReportParams();


            rptViewer2.LocalReport.DataSources.Clear();
            rptViewer2.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ACWiseBalanceDataSet", dSet.Tables[0]));
            rptViewer2.LocalReport.ReportPath = "./Reports/rptACWiseReport.rdlc";
            rptViewer2.LocalReport.EnableExternalImages = true;
            rptViewer2.LocalReport.SetParameters(paras);
            rptViewer2.LocalReport.Refresh();

        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }

    }
    #endregion
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

    private void BindAccounts(string Account)
    {
        DataSet ds = AccountManager.GetAccounts(Account == "All" ? string.Empty : Account);
        DataRow drow = ds.Tables[0].NewRow();
        drow["Name"] = "All";
        drow["id"] = "0";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlAccounts.DataSource = ds;
        ddlAccounts.DataBind();
        ddlAccounts.SelectedValue = "All";
    }

    protected void ddlAccountType_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Name"].ToString();
    }

    protected void ddlAccountType_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindAccounts(e.Text);
    }

    protected void ddlAccounts_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Name"].ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        BindReportViewerData(ddlAccountType.SelectedValue.ToString(), ddlAccounts.SelectedValue.ToString(), chkOpeningBalance.Checked);
    }


}
