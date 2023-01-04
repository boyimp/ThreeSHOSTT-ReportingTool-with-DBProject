using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObjects.InventoryManager;
using Telerik.Web.UI;
using Microsoft.Reporting.WebForms;
using System.Xml;
using BusinessObjects.ReportManager;
using BusinessObjects.TicketsManager;

public partial class UI_InventoryCostOfSalesTotal : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("Default.aspx");

        if (!IsPostBack)
        {
  
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
        //BindData();
    }

    private void BindData()
    {
        try
        {
            DataSet dsStartEndDate = CurrentStockManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            int nFisrtWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["FirstWorkPeriodID"]);
            int nLastWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["LastWorkPeriodID"]);

            DataSet ds = CurrentStockManager.GetSpecialInvntoryRegister(ddlGroupItem.SelectedValue, ddlInventoryItem.SelectedValue, ddlWarehouse.SelectedValue, dStartDate, dEndDate, nFisrtWorkPeriodID, nLastWorkPeriodID, false, "All", "All", string.Empty,false);

            DataSet TicketTotal = TicketManager.GetTicketsAccountWise(dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), 1, 0,String.Empty);

            decimal TotalCost = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("SUM(ActualConsumptionValue)", "")),2);
            decimal TotalSales = Math.Round(Convert.ToDecimal(TicketTotal.Tables[1].Rows[0]["SUMTotalAmount"]),2);
            decimal TotalCOSPerc = Math.Round((TotalCost / TotalSales) * 100,2);

            decimal TotalCountVarianceValue = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("SUM(CountVarianceValue)", "")),2);
            decimal TotalCountVarianceValuePerc = Math.Round((TotalCountVarianceValue / TotalSales) * 100,2);

            decimal TotalWastage = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("SUM(WastageValue)", "")),2);
            decimal TotalWastagePerc = Math.Round((TotalWastage / TotalSales) * 100,2);

            decimal TotalTheoCost = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("SUM(ConsumptionValue)", "")),2);
            decimal TotalTheoCostPerc = Math.Round((TotalTheoCost / TotalSales) * 100,2);
            
            List<ReportParameter> paras = ReportManager.GetReportParams();

            ReportParameter rptTotalCost = new ReportParameter("rptTotalCost", TotalCost.ToString());
            paras.Add(rptTotalCost);
            ReportParameter rptTotalSales = new ReportParameter("rptTotalSales", TotalSales.ToString());
            paras.Add(rptTotalSales);
            ReportParameter rptTotalCOSPerc = new ReportParameter("rptTotalCOSPerc", TotalCOSPerc.ToString());
            paras.Add(rptTotalCOSPerc);

            ReportParameter rptCountVarianceValue = new ReportParameter("rptCountVarianceValue", TotalCountVarianceValue.ToString());
            paras.Add(rptCountVarianceValue);
            ReportParameter rptCountVarianceValuePerc = new ReportParameter("rptCountVarianceValuePerc", TotalCountVarianceValuePerc.ToString());
            paras.Add(rptCountVarianceValuePerc);

            ReportParameter rptWastageValue = new ReportParameter("rptWastageValue", TotalWastage.ToString());
            paras.Add(rptWastageValue);
            ReportParameter rptWastageValuePerc = new ReportParameter("rptWastageValuePerc", TotalWastagePerc.ToString());
            paras.Add(rptWastageValuePerc);

            ReportParameter rptTheoCostValue = new ReportParameter("rptTheoCostValue", TotalTheoCost.ToString());
            paras.Add(rptTheoCostValue);
            ReportParameter rptTheoCostValuePerc = new ReportParameter("rptTheoCostValuePerc", TotalTheoCostPerc.ToString());
            paras.Add(rptTheoCostValuePerc);

            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("SpecialInventoryRegister", ds.Tables[0]));
            rptViewer1.LocalReport.ReportPath = "./Reports/rptInventoryCostOfSalesTotal.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();


            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                lblWorkPeriod.Text = "Work Period Considered From " + ds.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + ds.Tables[0].Rows[0]["EndDate"].ToString();
            else
                lblWorkPeriod.Text = string.Empty;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

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

}
