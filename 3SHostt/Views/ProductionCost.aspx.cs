using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObjects.InventoryManager;
using Telerik.Web.UI;
using System.Globalization;
using System.Xml;
using Microsoft.Reporting.WebForms;
using BusinessObjects.ReportManager;

public partial class UI_ProductionCost : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("Default.aspx");

        if (!IsPostBack)
        {
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            RadGrid1.ClientSettings.Scrolling.AllowScroll = true;
            RadGrid1.ClientSettings.Scrolling.UseStaticHeaders = true;

        }

    }

    private void BindData()
    {
        DataSet ds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(dtpFromDate.SelectedDate.ToString(), dtpToDate.SelectedDate.ToString(), string.Empty, string.Empty);
        int OpeningStockValue = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["TotalValueOfStock"]);
        int PurchaseStockValue = string.IsNullOrEmpty(ds.Tables[1].Rows[0]["TotalPrice"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["TotalPrice"]);
        int ClosingStockValue = string.IsNullOrEmpty(ds.Tables[2].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[2].Rows[0]["TotalValueOfStock"]);
        int CostOfProducton = (OpeningStockValue + PurchaseStockValue) - ClosingStockValue;
        DataTable table = null;

        table = new DataTable();
        table.Columns.AddRange(new DataColumn[] { new DataColumn("ID"), new DataColumn("Description"), new DataColumn("Value") });
        table.Rows.Add(new object[] { 0, "Opening Stock on " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy"), OpeningStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
        table.Rows.Add(new object[] { 1, "Purchase from " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy") + " To " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"), PurchaseStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
        table.Rows.Add(new object[] { 2, "Closing Stock on " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"), ClosingStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
        table.Rows.Add(new object[] { 3, "Cost of Production ", CostOfProducton.ToString("#,#", CultureInfo.InvariantCulture) });

        RadGrid1.DataSource = table;
        RadGrid1.DataBind();
        RadGrid1.GroupingSettings.CaseSensitive = false;
    }

    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        DataSet ds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(dtpFromDate.SelectedDate.ToString(), dtpToDate.SelectedDate.ToString(), string.Empty, string.Empty);

        int OpeningStockValue = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["TotalValueOfStock"]);
        int PurchaseStockValue = string.IsNullOrEmpty(ds.Tables[1].Rows[0]["TotalPrice"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["TotalPrice"]);
        int ClosingStockValue = string.IsNullOrEmpty(ds.Tables[2].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[2].Rows[0]["TotalValueOfStock"]);
        int CostOfProducton = (OpeningStockValue + PurchaseStockValue) - ClosingStockValue;
        DataTable table = null;

        table = new DataTable();
        table.Columns.AddRange(new DataColumn[] { new DataColumn("ID"), new DataColumn("Description"), new DataColumn("Value") });
        table.Rows.Add(new object[] { 0, "Opening Stock on " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy"), OpeningStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
        table.Rows.Add(new object[] { 1, "Purchase from " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy") + " To " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"), PurchaseStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
        table.Rows.Add(new object[] { 2, "Closing Stock on " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"), ClosingStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
        table.Rows.Add(new object[] { 3, "Cost of Production ", CostOfProducton.ToString("#,#", CultureInfo.InvariantCulture) });
        RadGrid1.DataSource = table;

        RadGrid1.GroupingSettings.CaseSensitive = false;
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    #region ExportRegion
    //protected void ImgbtnExcel_Click(object sender, EventArgs e)
    //{
    //    ConfigureExport();
    //    RadGrid1.MasterTableView.ExportToExcel();
    //}

    //protected void ImgbtnWord_Click(object sender, EventArgs e)
    //{
    //    ConfigureExport();
    //    RadGrid1.MasterTableView.ExportToWord();
    //}

    //protected void ImgbtnPDF_Click(object sender, EventArgs e)
    //{
    //    ConfigureExport();
    //    RadGrid1.MasterTableView.ExportToPdf();
    //}

    //public void ConfigureExport()
    //{
    //    RadGrid1.ExportSettings.ExportOnlyData = true;
    //    RadGrid1.ExportSettings.IgnorePaging = true;
    //    RadGrid1.ExportSettings.OpenInNewWindow = false;
    //    RadGrid1.ExportSettings.FileName = "WorkPeriodWiseInvntoryRegister";
    //}

    #endregion

    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            TableCell cell = item["Detail"];
            Telerik.Web.UI.GridDataItem item1 = (Telerik.Web.UI.GridDataItem)e.Item;

            if (Convert.ToInt32(item1.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]) == 3)
            {
                cell.Text = "";
            }
        }
    }
    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {




        if (e.Column.UniqueName == "ID")
        {
            e.Column.Visible = false;
        }
        if (e.Column.UniqueName == "Detail" || e.Column.UniqueName == "Description")
        {
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
        }
    }

    private void GetData(int ID)
    {
        try
        {
            DataSet dSet = new DataSet();
            

            string CompanyLogo = new Uri(Server.MapPath("~/Images/CompanyLogo.png")).AbsoluteUri;
            string ThreeSLogo = new Uri(Server.MapPath("~/Images/logo.jpg")).AbsoluteUri;

            string OpeningStockReportTitle = "Opening Stock Detail on " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy");
            string PurchaseReportTitle = "Purchase Detail From " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy") + " to " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy");
            string ClosingStockReportTitle = "Closing Stock Detail on " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy");

            List<ReportParameter> paras = ReportManager.GetReportParams();

            switch (ID)
            {
                case 0:
                    dSet = CurrentStockManager.GetOpeningStock((Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy"));
                    //dSet.Tables[0].TableName = "OpeningStockTable";
                    //Session["oReportTitle"] = "Opening Stock Detail on " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy");
                    ReportParameter OReportTitle = new ReportParameter("ReportTitle", OpeningStockReportTitle);
                    paras.Add(OReportTitle);
                    Session["DataSetName"] = "OpeningClosingStock";
                    Session["oDataSet"] = dSet;
                    Session["oReportTitle"] = "Opening Stock Detail";
                    Session["ReportParams"] = paras;
                    Session["oReportName"] = Server.MapPath("~/Reports/rptOpeningClosingStock.rdlc");
                    break;
                case 1:
                    dSet = CurrentStockManager.GetPurchaseStock((Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy"), (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"));
                    //Session["oReportTitle"] = "Purchase Detail From " + (Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("dd MMM yyyy") + " to " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy");
                    //dSet.Tables[0].TableName = "PurchaseStockTable";
                    ReportParameter PReportTitle = new ReportParameter("ReportTitle", PurchaseReportTitle);
                    paras.Add(PReportTitle);
                    Session["DataSetName"] = "PurchaseStock";
                    Session["oDataSet"] = dSet;
                    Session["oReportTitle"] = "Opening Stock Detail";
                    Session["ReportParams"] = paras;
                    Session["oReportName"] = Server.MapPath("~/Reports/rptPurchaseStock.rdlc");
                    
                    break;
                case 2:
                    dSet = CurrentStockManager.GetClosingStock((Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy"));
                    //Session["oReportTitle"] = "Closing Stock Detail on " + (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("dd MMM yyyy");
                    //dSet.Tables[0].TableName = "ClosingStockTable";
                    ReportParameter CReportTitle = new ReportParameter("ReportTitle", ClosingStockReportTitle);
                    paras.Add(CReportTitle);
                    Session["DataSetName"] = "OpeningClosingStock";
                    Session["oDataSet"] = dSet;
                    Session["oReportTitle"] = "Opening Stock Detail";
                    Session["ReportParams"] = paras;
                    Session["oReportName"] = Server.MapPath("~/Reports/rptOpeningClosingStock.rdlc");
                    
                    break;
                case 3:
                    return;
            }


            //Session["oDataSet"] = dSet;
           
            Page.ClientScript.RegisterStartupScript(
                                     this.GetType(), "OpenWindow",
                                     "window.open('./ReportViewerAlter.aspx','_newtab');", true);
            //ResponseHelper.Redirect("~/UI/ReportViewer.aspx", "_blank", "menubar=0,width=900,height=900");
        }
        catch (Exception ex)
        { throw new Exception(ex.Message); }
    }
    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        try
        {
            if (e.Item != null && (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))
            {

                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int ID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);

                switch (e.CommandName)
                {
                    case "Preview":
                        GetData(ID);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}
