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
using Telerik.Web.UI;
using System.Xml;
using BusinessObjects.ReportManager;

public partial class UI_ItemSalesProfitLossFixed : System.Web.UI.Page
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
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            BindDepartment();
            BindGroupItem();
            BindMenuItem(ddlGroupItem.SelectedValue);
        }
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
    private void BindGroupItem()
    {
        DataSet ds = MenuItemManager.GetGroupItem();
        DataRow drow = ds.Tables[0].NewRow();
        drow["GroupCode"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlGroupItem.DataSource = ds;
        ddlGroupItem.DataBind();
        ddlGroupItem.SelectedValue = "All";
    }

    private void BindMenuItem(string groupItem)
    {
        DataSet ds = MenuItemManager.GetMenuItem(groupItem == "All" ? string.Empty : groupItem);
        DataRow drow = ds.Tables[0].NewRow();
        drow["Name"] = "All";
        drow["id"] = "0";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlMenuItem.DataSource = ds;
        ddlMenuItem.DataBind();
        ddlMenuItem.SelectedValue = "0";
    }
    protected void ddlGroupItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString().Replace("'", "''");
    }

    protected void ddlGroupItem_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindMenuItem(e.Text.Replace("'", "''"));
    }

    protected void ddlMenuItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
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
    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

        RadGrid1.DataSource = GetData(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue));//AccountManager.GetCurrentBalanceOfAChead(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
        RadGrid1.GroupingSettings.CaseSensitive = false;
        lblWorkPeriod.Text = "Work Period Considered From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

        RadGrid1.DataSource = GetData(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue));//AccountManager.GetCurrentBalanceOfAChead(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
        RadGrid1.DataBind();
        lblWorkPeriod.Text = "Work Period Considered From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
    }

    protected void btnReprocess_Click(object sender, EventArgs e)
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        SetData(dStartDate, dEndDate);//AccountManager.GetCurrentBalanceOfAChead(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
        
        lblWorkPeriod.Text = "Ticket Reprocessed.....";
    }

    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        e.Column.FilterControlWidth = Unit.Pixel(40);
        if (e.Column.UniqueName == "Quantity" || e.Column.UniqueName == "Gross" || e.Column.UniqueName == "NetAmount" || e.Column.UniqueName == "ProductionCostFixed"
            || e.Column.UniqueName == "TotalProductionCostFixed" || e.Column.UniqueName == "ProductionProfitFixed" || e.Column.UniqueName == "ProductionCostRecipeWise"
            || e.Column.UniqueName == "TotalProductionCostRecipeWise" || e.Column.UniqueName == "ProductionProfitRecipeWise" || e.Column.UniqueName == "Deviation")
        {
            ((Telerik.Web.UI.GridBoundColumn)e.Column).FooterText = "Total: ";
            ((Telerik.Web.UI.GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;
        }
        if (e.Column.UniqueName == "ItemId")
        {
            e.Column.Visible = false;
        }
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
        RadGrid1.MasterTableView.GetColumn("More").Visible = false;
        RadGrid1.MasterTableView.ShowFooter = false;
        //210 × 297
        RadGrid1.MasterTableView.Caption = "Item Sales Report:" + Environment.NewLine + lblWorkPeriod.Text.ToString();
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
        RadGrid1.ExportSettings.FileName = "ItemSalesReport";
    }

    protected void RadGrid1_PdfExporting(object sender, GridPdfExportingArgs e)
    {

        e.RawHTML = ReportManager.GetRawHTML(e, "Item Sales Report:" + lblWorkPeriod.Text.ToString()).RawHTML;
    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.Item != null && (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int MenuItemId = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ItemId"]);
                string PortionName = Convert.ToString(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PortionName"]);

                switch (e.CommandName)
                {
                    case "Preview":
                        GetDrillThrough(MenuItemId, PortionName);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void GetDrillThrough(int MenuItemId, string PortionName)
    {
        DataSet ds = new DataSet();
        ds = MenuItemManager.ProductionCostDrill(MenuItemId, PortionName);


        if (ds.Tables[0].Rows.Count > 0)
        {

            try
            {

                List<ReportParameter> paras = ReportManager.GetReportParams();
                Session["DataSetName"] = "ProductionCostDrill";
                Session["oReportName"] = Server.MapPath("~/Reports/rptProductionCostDrill.rdlc");
                Session["oDataSet"] = ds;
                Session["oReportTitle"] = "ProductionCostDrill";
                Session["ReportParams"] = paras;
                //ResponseHelper.Redirect("~/UI/ReportViewer.aspx", "_blank", "menubar=0,width=1500,height=900");
                Page.ClientScript.RegisterStartupScript(
                                     this.GetType(), "OpenWindow",
                                     "window.open('./ReportViewerAlter.aspx','_newtab');", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('" + "No Data found" + "');", true);
        }
    }

    private IList<ItemProfitLossFixedInfo> GetData(DateTime dtStart, DateTime dtEnd, int departmentid)
    {
        //List<Ticket> tickets = TicketManager.GetTickets(dtStart, dtEnd, 0, outletId);
        List<Ticket> tickets = TicketManager.GetTicketsFaster(0, departmentid == 0 ? string.Empty : departmentid.ToString(), dtStart, dtEnd);
        List<ThreeS.Domain.Models.Menus.MenuItem> menuItemss = MenuItemManager.GetMenuItemsFaster(Convert.ToInt32(ddlMenuItem.SelectedValue), ddlGroupItem.SelectedValue);

        var menuItems = departmentid == 0 ? ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss).OrderBy(x => x.ID) : ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, departmentid).OrderBy(x => x.ID);

        IList<ItemProfitLossFixedInfo> Products = new List<ItemProfitLossFixedInfo>();
        foreach (var menuItemInfo in menuItems)
        {           
            var s = menuItemss.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });
            DataTable dtProductionCostFixed = MenuItemManager.ProductionCostFixed(menuItemInfo.ID, menuItemInfo.Portion);
            decimal PcostF = dtProductionCostFixed.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
            decimal TPcostF = PcostF * menuItemInfo.Quantity;
            decimal PprofitF = menuItemInfo.AmountWithOrderTag - TPcostF;
            
            DataSet ds = TicketManager.GetDepartments();
            DataRow drow = ds.Tables[0].NewRow();
            drow["Id"] = "0";
            drow["Name"] = "All";
            ds.Tables[0].Rows.InsertAt(drow, 0);
            DataRow[] rows = ds.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));

            ItemProfitLossFixedInfo q = new ItemProfitLossFixedInfo
            {
                ItemId = menuItemInfo.ID,
                DepartmentName = rows[0]["Name"].ToString(),
                GroupName = s.First().GroupCode,
                ItemName = s.First().Name,
                PortionName = menuItemInfo.Portion,
                Price = Math.Round(menuItemInfo.Price, 2),
                Quantity = Math.Round(menuItemInfo.Quantity, 2),
                NetAmount = Math.Round(menuItemInfo.AmountWithOrderTag, 2),
                Gross = Math.Round(menuItemInfo.Price * menuItemInfo.Quantity,2),
                ProductionCostFixed = Math.Round(PcostF,2),
                TotalProductionCostFixed =Math.Round(TPcostF,2),
                ProductionProfitFixed = Math.Round(PprofitF,2),
            };
            Products.Add(q);
        }
        return Products;
    }

    private void SetData(DateTime dtStart, DateTime dtEnd)
    {
        List<Ticket> tickets = TicketManager.GetTickets(0, ddlTicketType.SelectedValue.ToString(), dtStart, dtEnd);
        List<ThreeS.Domain.Models.Menus.MenuItem> menuItemss = MenuItemManager.GetMenuItems();
        int i = 0;
        foreach(Ticket tick in tickets)
        {
            i++;
            tick.Recalculate();
            TicketManager.UpdateTicket(tick);
            lblWorkPeriod.Text = i.ToString() +" Out of "+ tickets.Count+ " is processed......Ticket No:"+ tick.Id.ToString()+ " Processed......";
        }
    }
}