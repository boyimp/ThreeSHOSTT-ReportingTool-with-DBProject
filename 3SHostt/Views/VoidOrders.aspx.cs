using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BusinessObjects.TicketsManager;
using ThreeS.Domain.Models.Tickets;
using System.Collections.Generic;
using ThreeS.Domain.Models.Entities;
using System.Xml;
using BusinessObjects.ReportManager;
using BusinessObjects.MenusManager;

public partial class UI_VoidOrders : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("Default.aspx");

        if (!IsPostBack)
        {
            RadGrid1.ClientSettings.Scrolling.AllowScroll = true;
            RadGrid1.ClientSettings.Scrolling.UseStaticHeaders = true;
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
        }
	}

    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

        DataSet VoidOrders = TicketManager.VoidOrders(dStartDate, dEndDate, "Void", 0, string.Empty);
        RadGrid1.DataSource = VoidOrders.Tables["VoidOrders"];
        if (VoidOrders != null && VoidOrders.Tables != null && VoidOrders.Tables.Count > 0 && VoidOrders.Tables[0].Rows.Count > 0)
            lblWorkPeriod.Text = "Work Period Considered From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " TO " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
        else
            lblWorkPeriod.Text = String.Empty;
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
        RadGrid1.MasterTableView.GetColumn("More").Visible = false;
        //210 × 297
        RadGrid1.MasterTableView.Caption = "Void Orders Report" + Environment.NewLine + lblWorkPeriod.Text.ToString();
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
        RadGrid1.ExportSettings.FileName = "VoidOrdersReport";
    }

    protected void RadGrid1_PdfExporting(object sender, GridPdfExportingArgs e)
    {


        e.RawHTML = ReportManager.GetRawHTML(e, "Void Orders Report:" + lblWorkPeriod.Text.ToString()).RawHTML;
    }

    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        e.Column.FilterControlWidth = Unit.Pixel(60);
        if (e.Column.DataType == typeof(decimal))
        {
            ((Telerik.Web.UI.GridBoundColumn)e.Column).FooterText = "Total: ";
            ((Telerik.Web.UI.GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;
            ((Telerik.Web.UI.GridBoundColumn)e.Column).AllowFiltering = false;
        }
        if (e.Column.UniqueName == "id" || e.Column.UniqueName == "TicketId")
        {
            e.Column.Visible = false;
        }
        else
        {
            e.Column.FilterControlWidth = Unit.Pixel(50);
            e.Column.HeaderText.Replace('_', ' ');
            e.Column.HeaderStyle.Wrap = true;
            //e.Column.HeaderStyle.Width = 150;
            //e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
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
   
    private void BindData()
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

        DataSet VoidOrders = TicketManager.VoidOrders(dStartDate, dEndDate, "Void", 0 , string.Empty);
        RadGrid1.DataSource = VoidOrders.Tables["VoidOrders"];
        RadGrid1.DataBind();
        if (VoidOrders != null && VoidOrders.Tables != null && VoidOrders.Tables.Count > 0 && VoidOrders.Tables[0].Rows.Count > 0)
            lblWorkPeriod.Text = "Work Period Considered From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " TO " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
        else
            lblWorkPeriod.Text = String.Empty;
    }


	protected void btnSearch_Click(object sender, EventArgs e)
	{
        BindData();
	}
    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.Item != null && (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int ticketID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketId"]);

                switch (e.CommandName)
                {
                    case "Preview":
                        GetData(ticketID);
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
    private void GetData(int ticketID)
    {
        try
        {
            DataSet dSet = new DataSet();
            DataTable orderDataTable = null;
            DataTable ticketEntitiesDataTable = null;
            DataTable calculationsDataTable = null;
            DataTable paymentsDataTable = null;

            Ticket ticket = TicketManager.GetTicket(ticketID);

            orderDataTable = GetOrders(ticket);
            dSet.Tables.Add(orderDataTable);

            ticketEntitiesDataTable = TicketManager.GetTicketEntities(ticketID);
            dSet.Tables.Add(ticketEntitiesDataTable);

            calculationsDataTable = GetCalculations(ticket);
            dSet.Tables.Add(calculationsDataTable);

            paymentsDataTable = GetPayments(ticket);
            dSet.Tables.Add(paymentsDataTable);

            Session["TicketNo"] = ticket.TicketNumber;
            Session["TicketDate"] = ticket.Date.ToString("dd MMM yyyy");
            Session["DeliveryDate"] = ticket.Date.ToString("dd MMM yyyy");
            Session["TotalTax"] = Convert.ToString(ticket.TicketTaxValue);
            Session["TicketTotal"] = Convert.ToString(ticket.TotalAmount);
            Session["RemainingAmount"] = Convert.ToString(ticket.RemainingAmount);
            Session["oReportName"] = Server.MapPath("~/Reports/Ticket.rdlc");
            Session["oDataSet"] = dSet;
            ResponseHelper.Redirect("~/UI/ReportViewer.aspx", "_blank", "menubar=0,width=1200,height=900");
        }
        catch (Exception ex)
        { throw new Exception(ex.Message); }
    }

    public DataTable GetOrders(Ticket ticket)
    {
        rptDataset.OrdersDataTable dTable = new rptDataset.OrdersDataTable();
        if (ticket != null)
        {
            string menuItem, orderStates;

            try
            {
                foreach (Order ordr in ticket.Orders)
                {
                    menuItem = string.Empty;
                    orderStates = string.Empty;

                    DataRow Rowbody = dTable.NewRow();
                    Rowbody["Quantity"] = ordr.Quantity;

                    menuItem = ordr.MenuItemName + System.Environment.NewLine;
                    foreach (OrderTagValue ordrTagValue in ordr.OrderTagValues)
                    {
                        menuItem += "," + ordrTagValue.TagValue;
                    }
                    Rowbody["MenuItem"] = menuItem;

                    foreach (OrderStateValue ordrStateValue in ordr.OrderStateValues)
                    {
                        if (string.IsNullOrEmpty(orderStates))
                            orderStates = ordrStateValue.State;
                        else
                            orderStates += "," + ordrStateValue.State;
                    }

                    Rowbody["CreatingUserName"] = ordr.CreatingUserName;
                    Rowbody["CreatedDateTime"] = ordr.CreatedDateTime;
                    Rowbody["OrderStates"] = orderStates;
                    Rowbody["UnitPrice"] = ordr.GetVisiblePrice();
                    Rowbody["Price"] = ordr.CalculatePrice ? ordr.GetVisiblePrice() * ordr.Quantity : 0;
                    Rowbody["UnitProductionCost"] = ordr.CalculatePrice ? ordr.UnitProductionCost : 0;
                    DataTable dtProductionCostFixed = MenuItemManager.ProductionCostFixed(ordr.MenuItemId, ordr.PortionName);
                    Rowbody["FixedProductionCost"] = dtProductionCostFixed.Rows.Count > 0 && ordr.CalculatePrice ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
                    Rowbody["Time"] = GetTime(ordr);

                    dTable.Rows.Add(Rowbody);
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        return dTable;
    }

    private string GetTime(Order ordr)
    {
        string time = string.Empty;
        if (ordr.ProductTimerValue != null)
        {
            string TimethresholdHour = ConfigurationManager.AppSettings["TimethresholdHour"];
            string TimethresholdMinute = ConfigurationManager.AppSettings["TimethresholdMinute"];
            int hoursToAdd = 0;
            int MinutesToAdd = 0;
            int.TryParse(TimethresholdHour, out hoursToAdd);
            int.TryParse(TimethresholdMinute, out MinutesToAdd);

            DateTime end = ordr.ProductTimerValue.End != ordr.ProductTimerValue.Start ? ordr.ProductTimerValue.End : DateTime.Now.AddHours(hoursToAdd).AddMinutes(MinutesToAdd);
            time = ordr.ProductTimerValue.Start.ToString("hh:mm tt") + " -" + end.ToString("hh:mm tt") + " (" + ordr.ProductTimerValue.GetTime() + ")";
        }
        return time;
    }

    public DataTable GetCalculations(Ticket ticket)
    {
        rptDataset.CalculationsDataTable dTable = new rptDataset.CalculationsDataTable();
        if (ticket != null)
        {
            foreach (Calculation obj in ticket.Calculations)
            {
                DataRow Rowbody = dTable.NewRow();

                Rowbody["Name"] = obj.Name;
                Rowbody["CalculationAmount"] = obj.CalculationAmount;

                dTable.Rows.Add(Rowbody);
            }
        }
        return dTable;
    }

    public DataTable GetPayments(Ticket ticket)
    {
        rptDataset.PaymentsDataTable dTable = new rptDataset.PaymentsDataTable();
        if (ticket != null)
        {
            foreach (Payment obj in ticket.Payments)
            {
                DataRow Rowbody = dTable.NewRow();

                Rowbody["Name"] = obj.Name;
                Rowbody["Date"] = obj.Date;
                Rowbody["Amount"] = obj.Amount;

                dTable.Rows.Add(Rowbody);
            }
        }
        return dTable;
    }
}
