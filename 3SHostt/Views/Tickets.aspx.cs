using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BusinessObjects.TicketsManager;
using BusinessObjects.MenusManager;
using ThreeS.Domain.Models.Tickets;
using System.Collections.Generic;
using BusinessObjects.ReportManager;
using Microsoft.Reporting.WebForms;

public partial class UI_Tickets : System.Web.UI.Page
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
            BindTicketType();
            LoadOutletsCbo();
            //DataSet ds = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            //DateTime dStartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"]);
            //DateTime dEndDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"]);
            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    lblWorkPeriod.Text = "Last Closed Working Period " + Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyy hh:mm:sstt") + " TO " + Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyy hh:mm:sstt");
            //    dtpFromDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
            //    dtpToDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"].ToString());
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
    protected void RadGrid1_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);
        if (cbExactTime.Checked)
        {
            dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        }
        DataSet ds = TicketManager.GetTickets(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue), chkOpen.Checked, outletId);
        RadGrid1.DataSource = ds;
        RadGrid1.GroupingSettings.CaseSensitive = false;
        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //RadGrid1.DataBind();
            lblWorkPeriod.Text = "Work Period Considered From " + Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyy hh:mm:ss tt") + " to " + Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyy hh:mm:ss tt");
        }
        else
            lblWorkPeriod.Text = String.Empty;
    }

    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item != null && (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int ticketID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id"]);

                switch (e.CommandName)
                {
                    case "Preview":
                        GetData(ticketID);
                        break;
                    case "InvoicePreview":
                        GetInvoiceData(ticketID);
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
        RadGrid1.MasterTableView.GetColumn("More2").Visible = false;

        //210 × 297
        RadGrid1.MasterTableView.Caption = "Ticket Reports:" + Environment.NewLine + lblWorkPeriod.Text.ToString();
        RadGrid1.ExportSettings.Pdf.Subject = "My Data";
        RadGrid1.ExportSettings.Pdf.PageTitle = "This is a page title";

        //RadGrid1.ExportSettings.Pdf.Page = "Ticket Reports:" + Environment.NewLine + lblWorkPeriod.Text.ToString();
        RadGrid1.ExportSettings.Pdf.PageLeftMargin = Unit.Parse("5mm");
        RadGrid1.ExportSettings.Pdf.PageTopMargin = Unit.Parse("10mm");
        RadGrid1.ExportSettings.Pdf.PageRightMargin = Unit.Parse("5mm");
        RadGrid1.ExportSettings.Pdf.PageBottomMargin = Unit.Parse("5mm");

        RadGrid1.ExportSettings.Pdf.PageHeight = Unit.Parse("210mm");
        RadGrid1.ExportSettings.Pdf.PageWidth = Unit.Parse("297mm");

        RadGrid1.ExportSettings.ExportOnlyData = true;
        RadGrid1.ExportSettings.IgnorePaging = true;
        RadGrid1.ExportSettings.OpenInNewWindow = true;
        RadGrid1.ExportSettings.FileName = "Ticket";
    }

    protected void RadGrid1_PdfExporting(object sender, GridPdfExportingArgs e)
    {

        RadGrid1.MasterTableView.ShowFooter = false;
        GridFooterItem footer = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
        string t1 = footer["TotalAmount"].Text.Split(':')[1];
        string t2 = footer["No_Of_Guests"].Text.Split(':')[1];

        //string Sums ="<div><table><tr width=\"50px\"><td width=\"50px\"></td> <td width=\"50px\"></td><td width=\"50px\"></td><td width=\"50px\">Total No.of Guest:"+t1+"</td><td width=\"50px\">Ticket Total:"+t2+"</td></tr></table></div>";
        //string sums = "<div>" + t1 ++ t2 + "</div>";
        //e.RawHTML = e.RawHTML + sums;
        e.RawHTML = ReportManager.GetRawHTML(e, "Ticket Reports:" + lblWorkPeriod.Text.ToString()).RawHTML;



        //XmlDocument xml = new XmlDocument();
        //xml.Load(Server.MapPath("~") + "/CompanyInfo.xml");
        //string CompanyName = xml.SelectSingleNode("/Company/CompanyName").InnerText;
        //string CompanyAddress = xml.SelectSingleNode("/Company/CompanyAddress").InnerText;
        //string ContactNumber = xml.SelectSingleNode("/Company/ContactNumber").InnerText;
        //string VatReg = xml.SelectSingleNode("/Company/VatReg").InnerText;

        //string sCompanyLogo = "<div><img src=\"../images/CompanyLogo.png\" style=\"width:90px;height:90px;\"/></div>";
        //string strCompanyName = "<div width=\"100%\" style=\"text-align:left;font-size:13px;font-family:Verdana;\">" + CompanyName + "</div>";
        //string strCompanyAddress = "<div width=\"100%\" style=\"text-align:left;font-size:10px;font-family:Verdana;\">" + CompanyAddress + "</div>";
        //string strContactNumber = "<div width=\"100%\" style=\"text-align:left;font-size:10px;font-family:Verdana;\">" + ContactNumber + "</div>";
        //string strVatReg = "<div width=\"100%\" style=\"text-align:left;font-size:10px;font-family:Verdana;\">" + VatReg + "</div>";
        //string strHeader = "<div width=\"100%\" style=\"text-align:left;font-size:13px;font-family:Verdana;\">" + "Ticket Reports:" + Environment.NewLine + lblWorkPeriod.Text.ToString() + "</div>";
        //string strFooter = "<div width=\"100%\" style=\"font-size:10px;font-family:Verdana;\">Powered By</div>";
        //string strFooterLogo = "<div width=\"100%\"><img src=\"../images/logo.jpg\"/></div>";

        //e.RawHTML = sCompanyLogo + strCompanyName + strCompanyAddress + strContactNumber + strVatReg + strHeader + e.RawHTML + strFooter+strFooterLogo;
    }

    protected void RadGrid1_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        e.Column.FilterControlWidth = Unit.Pixel(60);
        if (e.Column.DataType == typeof(decimal) || e.Column.DataType == typeof(int))
        {
            //e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.FooterStyle.HorizontalAlign = HorizontalAlign.Right;


            ((GridBoundColumn)e.Column).FooterText = "Total: ";
            ((GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;

        }
        if (e.Column.UniqueName == "id" || e.Column.UniqueName == "StartDate" || e.Column.UniqueName == "EndDate")
        {
            e.Column.Visible = false;
        }
        else
        {
            e.Column.FilterControlWidth = Unit.Pixel(50);
            e.Column.HeaderText.Replace('_', ' ');
            e.Column.HeaderStyle.Wrap = true;
            //e.Column.HeaderStyle.Width = 100;
            //e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
        }
    }

    private void BindData()
    {
        DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);
        if (cbExactTime.Checked)
        {
            dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        }
        DataSet ds = TicketManager.GetTickets(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue), chkOpen.Checked, outletId);
        RadGrid1.DataSource = ds;
        RadGrid1.GroupingSettings.CaseSensitive = false;
        RadGrid1.GroupingSettings.CaseSensitive = false;
        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            RadGrid1.DataBind();
            lblWorkPeriod.Text = "Work Period Considered From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
        }
        else
            lblWorkPeriod.Text = String.Empty;
    }

    private void BindTicketType()
    {
        DataSet ds = TicketManager.GetTicketType();
        DataRow drow = ds.Tables[0].NewRow();
        drow["Id"] = "0";
        drow["Name"] = "All";
        ds.Tables[0].Rows.InsertAt(drow, 0);
        ddlTicketType.DataSource = ds;
        ddlTicketType.DataBind();
        ddlTicketType.SelectedValue = "0";
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RadGrid1.DataSource = new string[] { };
        RadGrid1.DataBind();
        BindData();
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

    private void GetInvoiceData(int ticketID)
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

            TicketEntity customerEntity = null;
            customerEntity = ticket.TicketEntities.FirstOrDefault(i => i.EntityTypeName.Contains("Customer"));
            TicketEntity eventEntity = null;
            eventEntity = ticket.TicketEntities.FirstOrDefault(i => i.EntityTypeName.Contains("Events"));


            List<ReportParameter> paras = ReportManager.GetReportParams();
            ReportParameter TicketNo = new ReportParameter("TicketNo", ticket.TicketNumber.ToString());
            paras.Add(TicketNo);
            ReportParameter TicketDate = new ReportParameter("TicketDate", ticket.Date.ToString("dd MMM yyyy"));
            paras.Add(TicketDate);
            ReportParameter DeliveryDate = new ReportParameter("DeliveryDate", ticket.Date.ToString("dd MMM yyyy"));
            paras.Add(DeliveryDate);
            ReportParameter TotalTax = new ReportParameter("TotalTax", Convert.ToString(ticket.TicketTaxValue));
            paras.Add(TotalTax);
            ReportParameter TicketTotal = new ReportParameter("TicketTotal", Convert.ToString(ticket.TotalAmount));
            paras.Add(TicketTotal);
            ReportParameter RemainingAmount = new ReportParameter("RemainingAmount", Convert.ToString(ticket.RemainingAmount));
            paras.Add(RemainingAmount);

            string ClientDetails = "Client Name : " + (customerEntity == null ? string.Empty : customerEntity.EntityName) + Environment.NewLine +
                                   "Department : " + (customerEntity == null ? string.Empty : customerEntity.GetCustomDataFromEntity("Department")) + Environment.NewLine +
                                   "Company : " + (customerEntity == null ? string.Empty : customerEntity.GetCustomData("Company")) + Environment.NewLine +
                                   "Address : " + (customerEntity == null ? string.Empty : customerEntity.GetCustomData("Address")) + Environment.NewLine +
                                   "Cell/Phone : " + (customerEntity == null ? string.Empty : customerEntity.GetCustomData("Phone")) + Environment.NewLine +
                                   "Event : " + (eventEntity == null ? string.Empty : eventEntity.EntityName);

            ReportParameter rptClientDetails = new ReportParameter("rptClientDetails", ClientDetails);
            paras.Add(rptClientDetails);

            string TicketDetail = "Invoice No : " + ticket.TicketNumber.ToString() + Environment.NewLine +
                                   "Invoice Date : " + ticket.Date.ToString("dd MMM yyyy") + Environment.NewLine +
                                   "PO No : "  + Environment.NewLine +
                                   "PO Date : "  + Environment.NewLine +
                                   "Delivery Date : " + ticket.DeliveryDate.ToString("dd MMM yyyy");
            ReportParameter rptTicketDetails = new ReportParameter("rptTicketDetails", TicketDetail);
            paras.Add(rptTicketDetails);
            //ReportParameter RemainingAmount = new ReportParameter("TotalTax", Convert.ToString(ticket.TicketTaxValue));
            //paras.Add(TotalTax);
            Session["DataSetName"] = "rptDataset";
            Session["oReportName"] = Server.MapPath("~/Reports/rptTicketInvoice.rdlc");
            Session["oDataSet"] = dSet;
            Session["oReportTitle"] = "Sales Invoice";
            Session["ReportParams"] = paras;
            ResponseHelper.Redirect("~/UI/ReportViewerAlter.aspx", "_blank", "menubar=0,width=900,height=900");
            //Page.ClientScript.RegisterStartupScript(
            //                     this.GetType(), "OpenWindow",
            //                     "window.open('./ReportViewerAlter.aspx','_newtab');", true);
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
                if (obj.CalculationAmount == 0)
                    continue;
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
    protected void RadGrid1_HTMLExporting(object sender, GridHTMLExportingEventArgs e)
    {

    }
    protected void RadGrid1_GridExporting(object sender, GridExportingArgs e)
    {
    }
}
