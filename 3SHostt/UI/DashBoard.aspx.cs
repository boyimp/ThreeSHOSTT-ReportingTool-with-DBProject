//In the name of Allah
using BusinessObjects.ChartsManager;
using BusinessObjects.TicketsManager;
using System;
using System.Data;
using System.Web;
using ThreeS.Report.v2.Utils;

public partial class UI_DashBoard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (HttpContext.Current.Session[GlobalData.CURRENT_USER] is null)
        {
            Response.Redirect("./");
        }//if

        BusinessObjects.DBUtility.Current = HttpContext.Current;
        if (!IsPostBack)
        {
            LoadOutletsCbo();
            BindAsync();
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
    protected void ddlOutlets_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindAsync();
    }
    private void BindAsync()
    {
        try
        {
            int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);

            //var t1 = Task.Factory.StartNew(() => ChartsManager.GetDailySalesForDashBoardFaster(outletId).Tables[0]);
            //var t2 = Task.Factory.StartNew(() => ChartsManager.GetMonthlySalesForDashBoard(outletId).Tables[0]);
            //var t3 = Task.Factory.StartNew(() => ChartsManager.GetLast12MonthSalesAccountWise(outletId).Tables[0]);
            //var t4 = Task.Factory.StartNew(() => ChartsManager.GetLast12MonthSalesPaymentWise(outletId).Tables[0]);
            //var t5 = Task.Factory.StartNew(() => ChartsManager.GetLast12MonthSales(outletId).Tables[0]);
            //var t6 = Task.Factory.StartNew(() => ChartsManager.GetDailyNetSalesForDashBoard(outletId).Tables[0]);
            //var dts = await Task.WhenAll(t1, t2, t3, t4, t5, t6).ConfigureAwait(false);
            DataTable dt1 = ChartsManager.GetDailySalesForDashBoardFaster(outletId).Tables[0];// dts[0];// 
            DataTable dt2 = dt1;
            DataTable dt3 = ChartsManager.GetMonthlySalesForDashBoard(outletId).Tables[0];// dts[1];//
            DataTable dt4 = ChartsManager.GetLast12MonthSalesAccountWise(outletId).Tables[0];
            DataTable dt5 = ChartsManager.GetLast12MonthSalesPaymentWise(outletId).Tables[0];
            DataTable dt6 = ChartsManager.GetLast12MonthSales(outletId).Tables[0];
            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DashBoardDailyNetSalesDataSet", dt1));
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DashBoardDailySalesDataSet", dt2));
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DashBoardMonthlySalesDataSet", dt3));
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Last12MonthSalesAccountWise", dt4));
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Last12MonthSalesPaymentWise", dt5));
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Last12MonthSales", dt6));
            rptViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDashBoard.rdlc");
            rptViewer1.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
