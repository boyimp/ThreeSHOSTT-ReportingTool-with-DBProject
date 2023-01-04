using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObjects.InventoryManager;
using Telerik.Web.UI;
using System.Xml;
using Microsoft.Reporting.WebForms;
using BusinessObjects.ReportManager;

public partial class UI_WorkPeriodWiseInvntoryRegister : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            RadGrid1.ClientSettings.Scrolling.AllowScroll = true;
            RadGrid1.ClientSettings.Scrolling.UseStaticHeaders = true;
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;            
            BindGroupItem();
            BindInventoryItem(ddlGroupItem.SelectedValue);
            BindWarehouse();
            DataSet ds = CurrentStockManager.GetStartAndEndDateOfLastWorkPeriod();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lblWorkPeriod.Text = "Last Closed Working Period " + ds.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + ds.Tables[0].Rows[0]["EndDate"].ToString();
                dtpFromDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"].ToString());
                dtpToDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"].ToString());
            }
            else
                lblWorkPeriod.Text = string.Empty;            
        }
    }

    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet dsStartEndDate = CurrentStockManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        int nFisrtWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["FirstWorkPeriodID"]);
        int nLastWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["LastWorkPeriodID"]);

        DataSet ds = CurrentStockManager.GetWorkPeriodWiseInvntoryRegister(ddlGroupItem.SelectedValue, ddlInventoryItem.SelectedValue, ddlWarehouse.SelectedValue, dStartDate, dEndDate, nFisrtWorkPeriodID, nLastWorkPeriodID);
        RadGrid1.DataSource = ds;        
        RadGrid1.GroupingSettings.CaseSensitive = false;

        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            lblWorkPeriod.Text = "Work Period Considered From " + Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyyy hh:mm tt") + " TO " + Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");
        else
            lblWorkPeriod.Text = String.Empty;
    }

    private void BindData()
    {
        DataSet dsStartEndDate = CurrentStockManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        int nFisrtWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["FirstWorkPeriodID"]);
        int nLastWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["LastWorkPeriodID"]);
        
        DataSet ds = CurrentStockManager.GetWorkPeriodWiseInvntoryRegister(ddlGroupItem.SelectedValue, ddlInventoryItem.SelectedValue, ddlWarehouse.SelectedValue, dStartDate, dEndDate, nFisrtWorkPeriodID, nLastWorkPeriodID);
        RadGrid1.DataSource = ds;
        RadGrid1.DataBind();
        RadGrid1.GroupingSettings.CaseSensitive = false;

        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            lblWorkPeriod.Text = "Work Period Considered From " + Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyyy hh:mm tt") + " TO " + Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");
        else
            lblWorkPeriod.Text = String.Empty;
    }

    private void BindGroupItem()
    {
        DataSet ds = CurrentStockManager.GetGroupItem();        
        ddlGroupItem.DataSource = ds;
        ddlGroupItem.DataBind();
        ddlGroupItem.SelectedIndex = 1;
    }

    private void BindInventoryItem(string groupItem)
    {
        DataSet ds = CurrentStockManager.GetInventoryItem(groupItem == "All" ? string.Empty : groupItem);        
        ddlInventoryItem.DataSource = ds;
        ddlInventoryItem.DataBind();        
    }

    private void BindWarehouse()
    {
        DataSet ds = WarehouseManager.GetWarehouse();        
        ddlWarehouse.DataSource = ds;
        ddlWarehouse.DataBind();
        ddlWarehouse.SelectedValue = "0";
    }

    protected void ddlGroupItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
    }

    protected void ddlGroupItem_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindInventoryItem(e.Text);
    }

    protected void ddlInventoryItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }

    protected void ddlWarehouse_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void ImgbtnExcel_Click(object sender, EventArgs e)
    {
        ConfigureExport();
        RadGrid1.MasterTableView.ExportToExcel();
    }

    protected void ImgbtnWord_Click(object sender, EventArgs e)
    {
        ConfigureExport();
        RadGrid1.MasterTableView.ExportToWord();
    }

    protected void ImgbtnPDF_Click(object sender, EventArgs e)
    {
        ConfigureExport();
        RadGrid1.MasterTableView.ExportToPdf();
    }

    public void ConfigureExport()
    {
        RadGrid1.MasterTableView.GridLines = GridLines.Both;

        RadGrid1.MasterTableView.Font.Bold = true;
        RadGrid1.MasterTableView.Font.Italic = false;
        RadGrid1.MasterTableView.Font.Name = "verdana";
        RadGrid1.MasterTableView.Font.Overline = false;
        RadGrid1.MasterTableView.Font.Size = 8;
        RadGrid1.MasterTableView.Font.Strikeout = false;
        RadGrid1.MasterTableView.Font.Underline = true;
        RadGrid1.MasterTableView.ShowFooter = false;
        RadGrid1.MasterTableView.AllowFilteringByColumn = false;

        RadGrid1.MasterTableView.GetColumn("More").Visible = false;
        //210 × 297
        RadGrid1.MasterTableView.Caption = "Work Period wise Inventory Register:" + Environment.NewLine + lblWorkPeriod.Text.ToString();
        RadGrid1.ExportSettings.Pdf.Subject = "My Data";
        RadGrid1.ExportSettings.Pdf.PageTitle = "This is a page title";

        //RadGrid1.ExportSettings.Pdf.Page = "Ticket Reports:" + Environment.NewLine + lblWorkPeriod.Text.ToString();
        RadGrid1.ExportSettings.Pdf.PageLeftMargin = Unit.Parse("5mm");
        RadGrid1.ExportSettings.Pdf.PageTopMargin = Unit.Parse("10mm");
        RadGrid1.ExportSettings.Pdf.PageRightMargin = Unit.Parse("5mm");
        RadGrid1.ExportSettings.Pdf.PageBottomMargin = Unit.Parse("5mm");


        RadGrid1.ExportSettings.Pdf.PageHeight = Unit.Parse("630mm");
        RadGrid1.ExportSettings.Pdf.PageWidth = Unit.Parse("1010mm");

        RadGrid1.ExportSettings.ExportOnlyData = true;
        RadGrid1.ExportSettings.IgnorePaging = true;
        RadGrid1.ExportSettings.OpenInNewWindow = true;
        RadGrid1.ExportSettings.FileName = "Work Period wise Inventory Register";
    }

    protected void RadGrid1_PdfExporting(object sender, GridPdfExportingArgs e)
    {
        e.RawHTML = ReportManager.GetRawHTML(e, "Work Period wise Inventory Register:" + lblWorkPeriod.Text.ToString()).RawHTML;
    }

    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        e.Column.FilterControlWidth = Unit.Pixel(40);
        //if (e.Column.DataType == typeof(decimal))
        //{            
        //    ((Telerik.Web.UI.GridBoundColumn)e.Column).FooterText = "Total: ";
        //    ((Telerik.Web.UI.GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;            
        //}        
        if (e.Column.UniqueName == "InventoryID" || e.Column.UniqueName == "WarehouseID")
        {
            e.Column.Visible = false;
        }
        else
        {
            e.Column.FilterControlWidth = 100;
            e.Column.HeaderText.Replace('_', ' ');
            e.Column.HeaderStyle.Wrap = true;
            e.Column.HeaderStyle.Width = 150;
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
            e.Column.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
        }
    }

    private void GetData(int InventoryID, int WarehouseID, string StartDate, string EndDate, string GroupCode, string InventoryName, string WarehouseName)
    {
        try
        {
            DataSet dSet = new DataSet();
            DataTable TransferredTo = null;
            DataTable TransferredFrom = null;
            DataTable Consumption = null;

            TransferredTo = CurrentStockManager.GetWorkPeriodWiseInvntoryLedger(InventoryID, WarehouseID, StartDate, EndDate);
            TransferredFrom = CurrentStockManager.GetWorkPeriodWiseInvntoryTransferToAnotherWarehouseLedger(InventoryID, WarehouseID, StartDate, EndDate);
            Consumption = CurrentStockManager.GetWorkPeriodConsumption(InventoryID, WarehouseID, StartDate, EndDate);

            dSet.Tables.Add(TransferredTo);
            dSet.Tables.Add(TransferredFrom);
            dSet.Tables.Add(Consumption);

            List<ReportParameter> paras = ReportManager.GetReportParams();

            ReportParameter rptStartDate = new ReportParameter("StartDate", StartDate);
            paras.Add(rptStartDate);
            ReportParameter rptEndDate = new ReportParameter("EndDate", EndDate);
            paras.Add(rptEndDate);
            ReportParameter rptGroupCode = new ReportParameter("GroupCode", GroupCode);
            paras.Add(rptGroupCode);
            ReportParameter rptInventoryName = new ReportParameter("InventoryName", InventoryName);
            paras.Add(rptInventoryName);
            ReportParameter rptWarehouseName = new ReportParameter("WarehouseName", WarehouseName);
            paras.Add(rptWarehouseName);



            Session["DataSetName"] = "rptDataset";
            Session["oReportName"] = Server.MapPath("~/Reports/rptPurchaseTransfer.rdlc");
            Session["oDataSet"] = dSet;
            Session["oReportTitle"] = "PurchaseAndTranser";
            Session["ReportParams"] = paras;


            //Session["StartDate"] = StartDate;
            //Session["EndDate"] = EndDate;
            //Session["GroupCode"] = GroupCode;
            //Session["InventoryName"] = InventoryName;
            //Session["WarehouseName"] = WarehouseName;
            //Session["oReportName"] = Server.MapPath("~/Reports/rptPurchaseTransfer.rdlc");
            //Session["oDataSet"] = dSet;

            ResponseHelper.Redirect("~/UI/ReportViewerAlter.aspx", "_blank", "menubar=0,width=700,height=1000");
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
                //inventoryid, w.Id warehouseid
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int InventoryID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InventoryID"]);
                int WarehouseID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["WarehouseID"]);
                string sStartDate = Convert.ToString(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StartDate"]);
                string sEndDate = Convert.ToString(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EndDate"]);
                //string sStartDate = Convert.ToDateTime(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StartDate"]).ToString("dd MMM yyyy hh:mm tt");
                //string sEndDate = Convert.ToDateTime(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");
                string GroupCode = Convert.ToString(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["GroupCode"]);
                string Inventory = Convert.ToString(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Inventory"]);
                string Warehouse = Convert.ToString(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Warehouse"]);

                switch (e.CommandName)
                {
                    case "Preview":
                        GetData(InventoryID, WarehouseID, sStartDate, sEndDate,GroupCode, Inventory, Warehouse);
                        break;
                    default:
                        break;
                }
            }
        }
        catch// (Exception ex)
        {
            //ShowMessage("Error has occurred." + ex.Message, 0);
            //lblMessage.Text = ex.StackTrace;
            //lblMessage.Visible = true;
        }
    }
}
