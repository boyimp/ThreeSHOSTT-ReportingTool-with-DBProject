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

public partial class UI_CancelOrders : System.Web.UI.Page
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
        }
	}

    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
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
        RadGrid1.MasterTableView.Caption = "Cancel Orders Report" + Environment.NewLine + lblWorkPeriod.Text.ToString();
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
        RadGrid1.ExportSettings.FileName = "CancelOrdersReport";
    }

    protected void RadGrid1_PdfExporting(object sender, GridPdfExportingArgs e)
    {


        e.RawHTML = ReportManager.GetRawHTML(e, "Cancel Orders Report:" + lblWorkPeriod.Text.ToString()).RawHTML;
    }

    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        e.Column.FilterControlWidth = Unit.Pixel(40);
        if (e.Column.DataType == typeof(decimal))
        {
            ((Telerik.Web.UI.GridBoundColumn)e.Column).FooterText = "Total: ";
            ((Telerik.Web.UI.GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;
            ((Telerik.Web.UI.GridBoundColumn)e.Column).AllowFiltering = false;
        }
        if (e.Column.UniqueName == "id")
        {
            e.Column.Visible = false;
        }
        //else
        //{
        //    //e.Column.FilterControlWidth = 100;
        //    //e.Column.HeaderText.Replace('_', ' ');
        //    //e.Column.HeaderStyle.Wrap = true;
        //    //e.Column.HeaderStyle.Width = 150;
        //    e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
        //    e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
        //    e.Column.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
        //}
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

        DataSet VoidOrders = TicketManager.VoidOrders(dStartDate, dEndDate, "Cancel", 0, string.Empty);
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
}
