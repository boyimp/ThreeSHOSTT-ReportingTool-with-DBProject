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

public partial class UI_InventoryTransactionDateWise : System.Web.UI.Page
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
        ds = CurrentStockManager.GetInventoryTransactionDocuments((Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("yyyy-MM-dd"), (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("yyyy-MM-dd"));

        RadGrid1.DataSource = ds;
        RadGrid1.DataBind();
    }

    protected void RadGrid1_NeedDataSource1(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        ds = CurrentStockManager.GetInventoryTransactionDocuments((Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("yyyy-MM-dd"), (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("yyyy-MM-dd"));

        RadGrid1.DataSource = ds;
        //RadGrid1.DataBind();
    }
    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }
    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        if (e.Column.UniqueName == "DocId")
        {
            e.Column.Visible = false;
        }
        if (e.Column.DataType == typeof(decimal) || e.Column.DataType == typeof(int))
        {
            //e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            ((Telerik.Web.UI.GridBoundColumn)e.Column).FooterText = "Total: ";
            ((Telerik.Web.UI.GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;
        }
    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.Item != null && (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int DocId = Convert.ToInt32(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DocId"]);
                string Date = (item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Date"]).ToString();
                string DocName = (item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DocName"]).ToString();
                //string DateRange = "Work Period Considered From "+(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StartDate"]).ToString()+" To "
                //                        +(item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EndDate"]).ToString();
                switch (e.CommandName)
                {
                    case "Preview":
                        GetData(DocId, Date, DocName);
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

    private void GetData(int DocId, string Date, string DocName)
    {
        DataSet dsInventoryTransactions = new DataSet();
        dsInventoryTransactions = CurrentStockManager.GetInventoryTransactions(DocId);


        if (dsInventoryTransactions.Tables[0].Rows.Count > 0)
        {

            try
            {

                List<ReportParameter> paras = ReportManager.GetReportParams();
                ReportParameter rptDate = new ReportParameter("rptDate", Date);
                paras.Add(rptDate);
                ReportParameter rptDocName = new ReportParameter("rptDocName", DocName);
                paras.Add(rptDocName);


                Session["DataSetName"] = "InventoryTransactions";
                Session["oReportName"] = Server.MapPath("~/Reports/rptInventoryTransactions.rdlc");
                Session["oDataSet"] = dsInventoryTransactions;
                Session["oReportTitle"] = "Inventory Transactions Document";
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
