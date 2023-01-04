using BusinessObjects.InventoryManager;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Xml;
using BusinessObjects.ReportManager;

public partial class UI_WorkPeriodEndReport : System.Web.UI.Page
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

        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds = CurrentStockManager.GetWorkPeriodsWithWarehouse((Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("yyyy-MM-dd"), (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("yyyy-MM-dd"));

        RadGrid1.DataSource = ds;
        RadGrid1.DataBind();
    }

    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //DataSet ds = new DataSet();
        //DataTable detailTable;
        //detailTable = new DataTable("DetailTable");
        //DataColumn Code = new DataColumn("Code", typeof(string));
        //DataColumn CustomerOrderID = new DataColumn("CustomerOrderID", typeof(string));
        //DataColumn TransferDate = new DataColumn("TransferDate", typeof(string));
        //DataColumn From = new DataColumn("From", typeof(string));
        //DataColumn To = new DataColumn("To", typeof(string));
        //DataColumn TotalValue = new DataColumn("TotalValue", typeof(double));
        //detailTable.Columns.AddRange(new DataColumn[] { Code, CustomerOrderID, TransferDate, From, To, TotalValue });
        //ds.Tables.Add(detailTable);

        //RadGrid1.DataSource = ds;
        //RadGrid1.DataBind();
        DataSet ds = new DataSet();
        ds = CurrentStockManager.GetWorkPeriodsWithWarehouse((Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("yyyy-MM-dd"), (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("yyyy-MM-dd"));

        RadGrid1.DataSource = ds;
        //RadGrid1.DataBind();
    }
    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }
    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        if (e.Column.UniqueName == "WarehouseCID")
        {
            e.Column.Visible = false;
        }
    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.Item != null && (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int WarehouseCID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["WarehouseCID"]);
                string DateRange = "Work Period Considered From "+(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StartDate"]).ToString()+" To "
                                        +(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EndDate"]).ToString();
                switch (e.CommandName)
                {
                    case "Preview":
                        GetData(WarehouseCID, DateRange);
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

    private void GetData(int WarehouseCID, string DateRange)
    {
        DataSet dsWorkPeriodEndDrill = new DataSet();
        dsWorkPeriodEndDrill = CurrentStockManager.GetWorkPeriodEndDrill(WarehouseCID);


        if (dsWorkPeriodEndDrill.Tables[0].Rows.Count > 0)
        {

            try
            {
                List<ReportParameter> paras = ReportManager.GetReportParams();

                ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                paras.Add(rptDateRange);

  

                Session["DataSetName"] = "WorkPeriodEndDrill";
                Session["oReportName"] = Server.MapPath("~/Reports/rptWorkPeriodEndDrill.rdlc");
                Session["oDataSet"] = dsWorkPeriodEndDrill;
                Session["oReportTitle"] = "Stock Transfer Details";
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
    protected void ddlWarehouse_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {

    }
}
