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
using BusinessObjects.ReportManager;

public partial class UI_CurrentStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            RadGrid1.ClientSettings.Scrolling.AllowScroll = true;
            RadGrid1.ClientSettings.Scrolling.UseStaticHeaders = true;
            BindGroupItem();
            BindInventoryItem(string.Empty);
            BindWarehouse();
            BindBrand();
            BindVendor();
        }
        BindData();
    }

    private void BindData()
    {
        RadGrid1.DataSource = CurrentStockManager.GetCurrentStocks(ddlGroupItem.SelectedValue, ddlInventoryItem.SelectedValue, ddlWarehouse.SelectedValue, ddlBrand.SelectedValue, ddlVendor.SelectedValue);
        RadGrid1.DataBind();
        RadGrid1.GroupingSettings.CaseSensitive = false;

    }
    private void BindGroupItem()
    {
        DataSet ds = CurrentStockManager.GetGroupItem();
        DataRow drow = ds.Tables[0].NewRow();
        drow["GroupCode"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlGroupItem.DataSource = ds;
        ddlGroupItem.DataBind();
        ddlGroupItem.SelectedValue = "All";
    }

    private void BindInventoryItem(string groupItem)
    {
        DataSet ds = CurrentStockManager.GetInventoryItem(groupItem == "All" ? string.Empty : groupItem);
        DataRow drow = ds.Tables[0].NewRow();
        drow["Name"] = "All";
        drow["id"] = "0";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlInventoryItem.DataSource = ds;
        ddlInventoryItem.DataBind();
        ddlInventoryItem.SelectedValue = "0";
    }

    private void BindWarehouse()
    {
        DataSet ds = WarehouseManager.GetWarehouse();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Name"] = "All";
        drow["id"] = "0";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlWarehouse.DataSource = ds;
        ddlWarehouse.DataBind();
        ddlWarehouse.SelectedValue = "0";
    }
    private void BindBrand()
    {
        DataSet ds = CurrentStockManager.GetBrand();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Brand"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlBrand.DataSource = ds;
        ddlBrand.DataBind();
        ddlBrand.SelectedValue = "All";
    }
    private void BindVendor()
    {
        DataSet ds = CurrentStockManager.GetVendor();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Vendor"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlVendor.DataSource = ds;
        ddlVendor.DataBind();
        ddlVendor.SelectedValue = "All";
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
    protected void ddlBrand_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Brand"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Brand"].ToString();
    }
    protected void ddlVendor_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Vendor"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Vendor"].ToString();
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
        RadGrid1.MasterTableView.AllowFilteringByColumn = false;
        RadGrid1.MasterTableView.ShowFooter = true;
        //210 × 297
        RadGrid1.MasterTableView.Caption = "Stock Report";
        RadGrid1.ExportSettings.Pdf.Subject = "My Data";
        RadGrid1.ExportSettings.Pdf.PageTitle = "This is a page title";

        //RadGrid1.ExportSettings.Pdf.Page = "Ticket Reports:" + Environment.NewLine + lblWorkPeriod.Text.ToString();
        RadGrid1.ExportSettings.Pdf.PageLeftMargin = Unit.Parse("5mm");
        RadGrid1.ExportSettings.Pdf.PageTopMargin = Unit.Parse("10mm");
        RadGrid1.ExportSettings.Pdf.PageRightMargin = Unit.Parse("5mm");
        RadGrid1.ExportSettings.Pdf.PageBottomMargin = Unit.Parse("5mm");

        RadGrid1.ExportSettings.Pdf.PageHeight = Unit.Parse("210mm");
        RadGrid1.ExportSettings.Pdf.PageWidth = Unit.Parse("297mm");

        RadGrid1.MasterTableView.BorderStyle = BorderStyle.None;
        //RadGrid1.MasterTableView.BorderWidth = Unit.Parse("1mm");

        RadGrid1.ExportSettings.ExportOnlyData = true;
        RadGrid1.ExportSettings.IgnorePaging = true;
        RadGrid1.ExportSettings.OpenInNewWindow = true;
        RadGrid1.ExportSettings.FileName = "StockReport";
    }

    protected void RadGrid1_PdfExporting(object sender, GridPdfExportingArgs e)
    {

        e.RawHTML = ReportManager.GetRawHTML(e, "Stock Report:").RawHTML;
        
    }
}