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

public partial class UI_InventoryPotentialRevenueProduction : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (Request.QueryString["ReportType"].Equals("All"))
        {
            Page.Title = "Inventory Potential Revenue";
        }
        else if (Request.QueryString["ReportType"].Equals("Wastage"))
        {
            Page.Title = "Wastage Report";
        }
        else if (Request.QueryString["ReportType"].Equals("TheoUsage"))
        {
            Page.Title = "Consumption/Theoritical Usage Report";
        }
        else if (Request.QueryString["ReportType"].Equals("CountVariance"))
        {
            Page.Title = "Count Variance Report";
        }

        if (!IsPostBack)
        {
  
            BindGroupItem();
            foreach (RadComboBoxItem itm in ddlGroupItem.Items)
            {
                itm.Checked = true;
            }
            BindInventoryItem(ddlGroupItem.SelectedValue);
            BindWarehouse();
            BindBrand();
            BindVendor();
            CheckFifoPrice.Checked = false;
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
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
            if (CheckFifoPrice.Checked && (ddlWarehouse.SelectedValue == null || ddlWarehouse.SelectedValue == "0"))
            {
                MsgBox("Please Select a warehouse", this.Page, this);
                return;
            }

            string GroupCodes = string.Empty;
            var checkeditems = ddlGroupItem.CheckedItems;
            foreach (var item in checkeditems)
            {
                GroupCodes = string.IsNullOrEmpty(GroupCodes) ? "'"+item.Value.ToString()+"'" : GroupCodes + ",'" + item.Value.ToString()+"'";
            }

            DataSet ds = CurrentStockManager.GetSpecialInvntoryRegisterProduction(GroupCodes, ddlInventoryItem.SelectedValue, ddlWarehouse.SelectedValue, 
                dStartDate, dEndDate, nFisrtWorkPeriodID, nLastWorkPeriodID, CheckFifoPrice.Checked ? true : false, ddlBrand.SelectedValue, ddlVendor.SelectedValue);

            
            List<ReportParameter> paras = ReportManager.GetReportParams();
            
            if (Request.QueryString["ReportType"].Equals("All"))
            {
                ReportParameter rptVisibilityOther = new ReportParameter("rptVisibilityOther", "1");
                paras.Add(rptVisibilityOther);
                ReportParameter rptVisibilityWastage = new ReportParameter("rptVisibilityWastage", "1");
                paras.Add(rptVisibilityWastage);
                ReportParameter rptVisibilityTheoUsage = new ReportParameter("rptVisibilityTheoUsage", "1");
                paras.Add(rptVisibilityTheoUsage);
                ReportParameter rptVisibilityCountVariance = new ReportParameter("rptVisibilityCountVariance", "1");
                paras.Add(rptVisibilityCountVariance);
                ReportParameter rptVisibilityOpeningStock = new ReportParameter("rptVisibilityOpeningStock", "1");
                paras.Add(rptVisibilityOpeningStock);
                ReportParameter rptVisibilityExpected = new ReportParameter("rptVisibilityExpected", "1");
                paras.Add(rptVisibilityExpected);
                ReportParameter rptVisibilityStockTake = new ReportParameter("rptVisibilityStockTake", "1");
                paras.Add(rptVisibilityStockTake);
                ReportParameter rptReportTitle = new ReportParameter("rptReportTitle", "Inventory Potential Revenue");
                paras.Add(rptReportTitle);
            }
            else if (Request.QueryString["ReportType"].Equals("Wastage"))
            {
                ReportParameter rptVisibilityWastage = new ReportParameter("rptVisibilityWastage", "1");
                paras.Add(rptVisibilityWastage);
                ReportParameter rptVisibilityTheoUsage = new ReportParameter("rptVisibilityTheoUsage", "0");
                paras.Add(rptVisibilityTheoUsage);
                ReportParameter rptVisibilityCountVariance = new ReportParameter("rptVisibilityCountVariance", "0");
                paras.Add(rptVisibilityCountVariance);
                ReportParameter rptVisibilityOther = new ReportParameter("rptVisibilityOther", "0");
                paras.Add(rptVisibilityOther);
                ReportParameter rptVisibilityOpeningStock = new ReportParameter("rptVisibilityOpeningStock", "0");
                paras.Add(rptVisibilityOpeningStock);
                ReportParameter rptVisibilityExpected = new ReportParameter("rptVisibilityExpected", "0");
                paras.Add(rptVisibilityExpected);
                ReportParameter rptVisibilityStockTake = new ReportParameter("rptVisibilityStockTake", "0");
                paras.Add(rptVisibilityStockTake);
                ReportParameter rptReportTitle = new ReportParameter("rptReportTitle", "Wastage Report");
                paras.Add(rptReportTitle);
            }
            else if (Request.QueryString["ReportType"].Equals("TheoUsage"))
            {
                ReportParameter rptVisibilityTheoUsage = new ReportParameter("rptVisibilityTheoUsage", "1");
                paras.Add(rptVisibilityTheoUsage);
                ReportParameter rptVisibilityWastage = new ReportParameter("rptVisibilityWastage", "0");
                paras.Add(rptVisibilityWastage);
                ReportParameter rptVisibilityCountVariance = new ReportParameter("rptVisibilityCountVariance", "0");
                paras.Add(rptVisibilityCountVariance);
                ReportParameter rptVisibilityOther = new ReportParameter("rptVisibilityOther", "0");
                paras.Add(rptVisibilityOther);
                ReportParameter rptVisibilityOpeningStock = new ReportParameter("rptVisibilityOpeningStock", "0");
                paras.Add(rptVisibilityOpeningStock);
                ReportParameter rptVisibilityExpected = new ReportParameter("rptVisibilityExpected", "0");
                paras.Add(rptVisibilityExpected);
                ReportParameter rptVisibilityStockTake = new ReportParameter("rptVisibilityStockTake", "0");
                paras.Add(rptVisibilityStockTake);
                ReportParameter rptReportTitle = new ReportParameter("rptReportTitle", "Consumption/Theoritical Report");
                paras.Add(rptReportTitle);
            }
            else if (Request.QueryString["ReportType"].Equals("CountVariance"))
            {
                ReportParameter rptVisibilityCountVariance = new ReportParameter("rptVisibilityCountVariance", "1");
                paras.Add(rptVisibilityCountVariance);
                ReportParameter rptVisibilityTheoUsage = new ReportParameter("rptVisibilityTheoUsage", "0");
                paras.Add(rptVisibilityTheoUsage);
                ReportParameter rptVisibilityWastage = new ReportParameter("rptVisibilityWastage", "0");
                paras.Add(rptVisibilityWastage);
                ReportParameter rptVisibilityOther = new ReportParameter("rptVisibilityOther", "0");
                paras.Add(rptVisibilityOther);
                ReportParameter rptVisibilityOpeningStock = new ReportParameter("rptVisibilityOpeningStock", "0");
                paras.Add(rptVisibilityOpeningStock);
                ReportParameter rptVisibilityExpected = new ReportParameter("rptVisibilityExpected", "0");
                paras.Add(rptVisibilityExpected);
                ReportParameter rptVisibilityStockTake = new ReportParameter("rptVisibilityStockTake", "0");
                paras.Add(rptVisibilityStockTake);
                ReportParameter rptReportTitle = new ReportParameter("rptReportTitle", "Count Variance Report");
                paras.Add(rptReportTitle);
            }
            else if (Request.QueryString["ReportType"].Equals("StockTake"))
            {
                ReportParameter rptVisibilityCountVariance = new ReportParameter("rptVisibilityCountVariance", "1");
                paras.Add(rptVisibilityCountVariance);
                ReportParameter rptVisibilityTheoUsage = new ReportParameter("rptVisibilityTheoUsage", "0");
                paras.Add(rptVisibilityTheoUsage);
                ReportParameter rptVisibilityWastage = new ReportParameter("rptVisibilityWastage", "0");
                paras.Add(rptVisibilityWastage);
                ReportParameter rptVisibilityOther = new ReportParameter("rptVisibilityOther", "0");
                paras.Add(rptVisibilityOther);
                ReportParameter rptVisibilityOpeningStock = new ReportParameter("rptVisibilityOpeningStock", "1");
                paras.Add(rptVisibilityOpeningStock);
                ReportParameter rptVisibilityExpected = new ReportParameter("rptVisibilityExpected", "1");
                paras.Add(rptVisibilityExpected);
                ReportParameter rptVisibilityStockTake = new ReportParameter("rptVisibilityStockTake", "1");
                paras.Add(rptVisibilityStockTake);
                ReportParameter rptReportTitle = new ReportParameter("rptReportTitle", "Stock Take Report");
                paras.Add(rptReportTitle);
            }
            
            //ReportParameter rptTO = new ReportParameter("rptTO", TOCode);

            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("SpecialInventoryRegister", ds.Tables[0]));
            rptViewer1.LocalReport.ReportPath = "./Reports/rptInventoryPotentialRevenue.rdlc";
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
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }
    protected void ChckedChanged(object sender, EventArgs e)
    {
        if (CheckFifoPrice.Checked)
        {
            ddlInventoryItem.Enabled = false;
            ddlGroupItem.Enabled = false;
            //BindData();
        }
        else
        {
            ddlInventoryItem.Enabled = true;
            ddlGroupItem.Enabled = true;
            //BindData();
        }
    }

    private void BindGroupItem()
    {
        //DataSet ds = CurrentStockManager.GetGroupItem();
        //DataRow drow = ds.Tables[0].NewRow();
        //drow["GroupCode"] = "All";
        //ds.Tables[0].Rows.InsertAt(drow, 0);
        //ddlGroupItem.DataSource = ds;
        //ddlGroupItem.DataBind();
        //ddlGroupItem.SelectedValue = "All";
        DataSet ds = CurrentStockManager.GetProductionGroupItem();
        //ddlTicketType.DataSource = ds;
        //ddlTicketType.DataBind();
        //ddlTicketType.SelectedValue = "0";
        RadComboBox comboBox = (RadComboBox)ddlGroupItem;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["GroupCode"].ToString();
            item.Value = row["GroupCode"].ToString();
            item.Attributes.Add("GroupCode", row["GroupCode"].ToString());

            comboBox.Items.Add(item);

            item.DataBind();
        }
    }

    private void BindInventoryItem(string groupItem)
    {
        DataSet ds = CurrentStockManager.GetProductionInventoryItem(groupItem == "All" ? string.Empty : groupItem);
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

}
