using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using BusinessObjects.AccountsManager;
using BusinessObjects.TicketsManager;
using System.Xml;
using BusinessObjects.ReportManager;
using Microsoft.Reporting.WebForms;


public partial class UI_CurrentBalanceOnAChead : System.Web.UI.Page
{
    private DataSet dsStartEndDate;
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
            LoadOutletsCbo();
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
    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        if (cbExactTime.Checked)
        {
            dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        }
        int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);
        DataSet ds = AccountManager.GetCurrentBalanceOfAChead(dStartDate, dEndDate, outletId);

        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            RadGrid1.DataSource = ds;            
            RadGrid1.GroupingSettings.CaseSensitive = false;
            lblWorkPeriod.Text = "Work Period Considered From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
        }
        else
            lblWorkPeriod.Text = String.Empty;
    }

    protected void ChckedChanged(object sender, EventArgs e)
    {
        if (CheckCurrentWorkPeriod.Checked)
        {
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
        }
        else
        {
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        if (cbExactTime.Checked)
        {
            dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        }
        int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);
        DataSet ds = AccountManager.GetCurrentBalanceOfAChead(dStartDate, dEndDate, outletId);
        RadGrid1.DataSource = ds;
        RadGrid1.GroupingSettings.CaseSensitive = false;
        RadGrid1.DataBind();

        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            lblWorkPeriod.Text = "Work Period Considered From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
        else
            lblWorkPeriod.Text = String.Empty;
    }

    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        if (e.Column.DataType == typeof(decimal))
        {
            ((Telerik.Web.UI.GridBoundColumn)e.Column).FooterText = "Total: ";
            ((Telerik.Web.UI.GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;
            
        }
        if (e.Column.UniqueName == "StartDate" || e.Column.UniqueName == "EndDate" || e.Column.UniqueName == "AccountId")
        {
            e.Column.Visible = false;
        }
        else
        {
            e.Column.FilterControlWidth = 200;
            e.Column.HeaderText.Replace('_', ' ');
            e.Column.HeaderStyle.Wrap = true;
            e.Column.HeaderStyle.Width = 150;
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            e.Column.ItemStyle.BorderStyle = BorderStyle.Solid;
            e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
            e.Column.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
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

        //210 × 297
        RadGrid1.MasterTableView.Caption = "Current Balance of Account Heads" + Environment.NewLine + lblWorkPeriod.Text.ToString();
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
        RadGrid1.ExportSettings.FileName = "CurrentBalanceofAccountHeads";
    }

    protected void RadGrid1_PdfExporting(object sender, GridPdfExportingArgs e)
    {

        e.RawHTML = ReportManager.GetRawHTML(e, "Current Balance of Account Heads:" + lblWorkPeriod.Text.ToString()).RawHTML;
        
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item != null && (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int AccountId = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AccountId"]);
                string AccountName = item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AccountName"].ToString();
                switch (e.CommandName)
                {
                    case "Preview":
                        GetDrillThrough(AccountId, AccountName);
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
    private void GetDrillThrough(int AccountId, string AccountName)
    {
        DataSet ds = new DataSet();
        dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
        DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
        DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
        if (cbExactTime.Checked)
        {
            dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        }
        int outletId = Convert.ToInt32(ddlOutlets.SelectedValue);
        ds = AccountManager.GetAccountDrillThrough(AccountId, dStartDate, dEndDate, outletId);

        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
        {

            try
            {
                DataTable dtOpening = ds.Tables[0];
                DataTable dtLedger = ds.Tables[1];
                DataTable dtClosing = ds.Tables[2];

                decimal DebitTotal = Convert.ToDecimal(dtLedger.Compute("SUM(Debit)", ""));
                decimal CreditTotal = Convert.ToDecimal(dtLedger.Compute("SUM(Credit)", ""));
                decimal BalanceTotal = Convert.ToDecimal(dtLedger.Compute("SUM(Balance)", ""));
                
                DataRow drow = dtLedger.NewRow();
                drow["Date"] = dStartDate;
                drow["Description"] = "Previous Transactions";
                drow["Debit"] =  dtOpening.Rows[0]["Debit"];
                drow["Credit"] =  dtOpening.Rows[0]["Credit"];
                drow["Balance"] =  dtOpening.Rows[0]["Balance"];
                dtLedger.Rows.InsertAt(drow, 0);

                int i = 0;
                foreach (DataRow ledgerRow in dtLedger.Rows)
                {
                    if (i != 0)
                    {
                        dtLedger.Rows[i]["Balance"] = Convert.ToDecimal(dtLedger.Rows[i]["Balance"]) + Convert.ToDecimal(dtLedger.Rows[i-1]["Balance"]);
                    }
                    i++;
                }

                List<ReportParameter> paras = ReportManager.GetReportParams();

                ReportParameter rptAccountName = new ReportParameter("rptAccountName", AccountName);
                paras.Add(rptAccountName);
                ReportParameter rptDebitTotal = new ReportParameter("rptDebitTotal", DebitTotal.ToString());
                paras.Add(rptDebitTotal);
                ReportParameter rptCreditTotal = new ReportParameter("rptCreditTotal", CreditTotal.ToString());
                paras.Add(rptCreditTotal);
                ReportParameter rptBalanceTotal = new ReportParameter("rptBalanceTotal", BalanceTotal.ToString());
                paras.Add(rptBalanceTotal);

                string DateRange = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyyy hh:mm tt") + " to " +
                                                   Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");

                ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                paras.Add(rptDateRange);
                Session["DataSetName"] = "rptDataSet2";
                Session["oReportName"] = Server.MapPath("~/Reports/rptACDrill.rdlc");
                Session["oDataSet"] = ds;
                Session["oReportTitle"] = "AccountDrillThrough";
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

}