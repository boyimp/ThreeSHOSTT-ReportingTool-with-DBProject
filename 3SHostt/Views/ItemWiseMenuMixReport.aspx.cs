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
using System.Xml;
using BusinessObjects.ReportManager;
using BusinessObjects;

public partial class UI_ItemWiseMenuMixReport : System.Web.UI.Page
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
            BindDepartment();
            BindGroupItem();
            BindMenuItem(ddlGroupItem.SelectedValue);
            BindReportViewerData();
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
        e.Item.Value = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
    }

    protected void ddlGroupItem_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindMenuItem(e.Text);
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindReportViewerData();
    }
    protected void BindReportViewerData()
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

        DataSet ds = DatasetConverter.ToDataSet<MenuGroupItemInfo>(GetData(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue)));

        if (ds.Tables[0].Rows.Count > 0)
        {
            decimal TotalQuantity = Convert.ToDecimal(ds.Tables[0].Compute("SUM(Quantity)", ""));
            decimal TotalNetAmount = Convert.ToDecimal(ds.Tables[0].Compute("SUM(NetAmount)", ""));

            string DateRange = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyyy hh:mm tt") + " to " + 
                                               Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");
            List<ReportParameter> paras = ReportManager.GetReportParams();

            ReportParameter rptTotalQuantity = new ReportParameter("rptTotalQuantity", TotalQuantity.ToString());
            paras.Add(rptTotalQuantity);
            ReportParameter rptTotalNetAmount = new ReportParameter("rptTotalNetAmount", TotalNetAmount.ToString());
            paras.Add(rptTotalNetAmount);
            ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
            paras.Add(rptDateRange);

            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemWiseMenuMixReport", ds.Tables[0]));
            rptViewer1.LocalReport.ReportPath = "./Reports/rptItemWiseMenuMix.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
        }
        //lblWorkPeriod.Text = "Work Period Considered From " + dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString();
    }

    protected void btnReprocess_Click(object sender, EventArgs e)
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        SetData(dStartDate, dEndDate);//AccountManager.GetCurrentBalanceOfAChead(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
        
        //lblWorkPeriod.Text = "Ticket Reprocessed.....";
    }


    private IList<MenuGroupItemInfo> GetData(DateTime dtStart, DateTime dtEnd, int departmentid)
    {
        List<Ticket> tickets = TicketManager.GetTickets(dtStart, dtEnd, 0, 0);
        List<ThreeS.Domain.Models.Menus.MenuItem> menuItemss = MenuItemManager.GetMenuItems(Convert.ToInt32(ddlMenuItem.SelectedValue), ddlGroupItem.SelectedValue);

        var menuItems = departmentid == 0 ? ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss).OrderBy(x => x.ID) : ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, departmentid).OrderBy(x => x.ID);

        IList<MenuGroupItemInfo> Products = new List<MenuGroupItemInfo>();
        foreach (var menuItemInfo in menuItems)
        {
            var s = menuItemss.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });

            DataTable dtProductionCostRecipe = MenuItemManager.ProductionCostRecipe(menuItemInfo.ID, menuItemInfo.Portion);
            decimal PcostR = dtProductionCostRecipe.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostRecipe.Rows[0]["ProductionCost"]) : 0;
            decimal TPcostR = PcostR * menuItemInfo.Quantity;
            decimal PprofitR = menuItemInfo.Amount - TPcostR;
            DataTable dtProductionCostFixed = MenuItemManager.ProductionCostFixed(menuItemInfo.ID, menuItemInfo.Portion);
            decimal PcostF = dtProductionCostFixed.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
            decimal TPcostF = PcostF * menuItemInfo.Quantity;
            decimal PprofitF = menuItemInfo.Amount - TPcostF;
            
            DataSet ds = TicketManager.GetDepartments();
            DataRow drow = ds.Tables[0].NewRow();
            drow["Id"] = "0";
            drow["Name"] = "All";
            ds.Tables[0].Rows.InsertAt(drow, 0);
            DataRow[] rows = ds.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));

            MenuGroupItemInfo q = new MenuGroupItemInfo
            {
                ItemId = menuItemInfo.ID,
                DepartmentName = rows[0]["Name"].ToString(),
                GroupName = s.First().GroupCode,
                ItemName = s.First().Name,
                PortionName = menuItemInfo.Portion,
                Price = Math.Round(menuItemInfo.Price, 2),
                Quantity = Math.Round(menuItemInfo.Quantity, 2),
                NetAmount = Math.Round(menuItemInfo.Amount, 2),
                Gross = Math.Round(menuItemInfo.Price * menuItemInfo.Quantity, 2),
                ProductionCostFixed = Math.Round(PcostF, 2),
                TotalProductionCostFixed = Math.Round(TPcostF, 2),
                ProductionProfitFixed = Math.Round(PprofitF, 2),
                ProductionCostRecipeWise = Math.Round(PcostR, 2),
                TotalProductionCostRecipeWise = Math.Round(TPcostR, 2),
                ProductionProfitRecipeWise = Math.Round(PprofitR, 2),
                Deviation = Math.Round(PprofitF - PprofitR, 2)
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
            //lblWorkPeriod.Text = i.ToString() +" Out of "+ tickets.Count+ " is processed......Ticket No:"+ tick.Id.ToString()+ " Processed......";
        }
    }
   
}

