using BusinessObjects.InventoryManager;
using BusinessObjects.ReportManager;
using BusinessObjects.TicketsManager;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;

public partial class UI_InventoryItemWisePotential : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Page.Title = " Inventory Item Wise Potential";

        if (!IsPostBack)
        {
            BindGroupItem();
            foreach (RadComboBoxItem itm in ddlGroupItem.Items)
            {
                itm.Checked = true;
            }
            BindInvetoryTakeType();           
            BindInventoryItem(ddlGroupItem.SelectedValue);
            BindWarehouse();
            BindBrand();
           
            BindVendor();
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
    }


    public bool IsNumeric(DataColumn col)
    {
        if (col == null)
            return false;
        // Make this const
        var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
        typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
        typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};
        return numericTypes.Contains(col.DataType);
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
        ddlInventoryItem.Enabled = true;
        ddlGroupItem.Enabled = true;        
    }

    private void BindGroupItem()
    {
        DataSet ds = CurrentStockManager.GetGroupItem();
        RadComboBox comboBox = (RadComboBox)ddlGroupItem;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem
            {
                Text = row["GroupCode"].ToString(),
                Value = row["GroupCode"].ToString()
            };
            item.Attributes.Add("GroupCode", row["GroupCode"].ToString());

            comboBox.Items.Add(item);
            item.DataBind();
        }
    }
    private void BindInvetoryTakeType()
    {
        DataSet ds = CurrentStockManager.GetInventoryTakeType();
        RadComboBox comboBox = (RadComboBox)ddlTakeType;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem
            {
                Text = row["InventoryTakeType"].ToString(),
                Value = row["InventoryTakeType"].ToString()
            };
            item.Attributes.Add("InventoryTakeType", row["InventoryTakeType"].ToString());
            item.Checked = false;
            comboBox.Items.Add(item);
            item.DataBind();
        }
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
    private void BindBrand()
    {
        DataSet ds = CurrentStockManager.GetBrand();
        RadComboBox comboBox = ddlBrand;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem
            {
                Text = row["Brand"].ToString(),
                Value = row["Brand"].ToString()
            };
            item.Attributes.Add("Brand", row["Brand"].ToString());
            item.Checked = false;
            comboBox.Items.Add(item);
            item.DataBind();
        }
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
    protected void DdlGroupItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
    }

    protected void DdlGroupItem_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindInventoryItem(e.Text);
    }

    protected void DdlInventoryItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }

    protected void DdlWarehouse_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }
    protected void DdlBrand_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Brand"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Brand"].ToString();
    }
    protected void DdlVendor_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Vendor"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Vendor"].ToString();
    }


    protected void btn_ShowNewReport_Click(object sender, EventArgs e)
    {
        ShowInventoryPotentialRevenue();
    }

    private void ShowInventoryPotentialRevenue()
    {
        try
        {
            DataSet dsStartEndDate = CurrentStockManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);           

            string GroupCodes = string.Empty;
            string InventoryTakeTypes = string.Empty;
            string Brands = string.Empty;

            var checkeditems = ddlGroupItem.CheckedItems;
            foreach (var item in checkeditems)
            {
                GroupCodes = string.IsNullOrEmpty(GroupCodes) ? "'" + item.Value.ToString() + "'" : GroupCodes + ",'" + item.Value.ToString() + "'";
            }

            var checkeditemsInventoryTakeType = ddlTakeType.CheckedItems;
            foreach (var item in checkeditemsInventoryTakeType)
            {
                InventoryTakeTypes = string.IsNullOrEmpty(InventoryTakeTypes) ? "'" + item.Value.ToString() + "'" : InventoryTakeTypes + ",'" + item.Value.ToString() + "'";
            }

            var checkeditemsBrands = ddlBrand.CheckedItems;
            foreach (var item in checkeditemsBrands)
            {
                Brands = string.IsNullOrEmpty(Brands) ? "'" + item.Value.ToString() + "'" : Brands + ",'" + item.Value.ToString() + "'";
            }
            DataSet ds = new DataSet();
            if (cbIsforProductLedger.Checked)
            {
                if (ddlInventoryItem.SelectedValue == "0")
                {
                    MsgBox("Please Select single Inventory Item for ledger report", this.Page, this);
                    return;
                }
                if (ddlWarehouse.SelectedValue == "0")
                {
                    MsgBox("Please Select single Warehouse for ledger report", this.Page, this);
                    return;
                }

                ds = CurrentStockManager.GetSpecialInvntoryPotentialRevenueForSingleProduct(int.Parse(ddlInventoryItem.SelectedValue),
                    int.Parse(ddlWarehouse.SelectedValue),
                    dStartDate,
                    dEndDate,
                    Brands,
                    ddlVendor.SelectedValue,
                    InventoryTakeTypes
                );                
            }           
            else
            {
                ds = CurrentStockManager.GetSpecialInvntoryPotentialRevenue(GroupCodes,
                      ddlInventoryItem.SelectedValue,
                      ddlWarehouse.SelectedValue,
                      dStartDate,
                      dEndDate,
                      Brands,
                      ddlVendor.SelectedValue,
                      InventoryTakeTypes                  
                );
            }
            List<ReportParameter> paras = new List<ReportParameter>();
            if (ddlWarehouse.SelectedValue != "0")
            {
                int outletid = TicketManager.GetOutletIdForWarehouse(int.Parse(ddlWarehouse.SelectedValue));
                paras = ReportManager.GetReportParams(outletid);
            }
            else
            {
                paras = ReportManager.GetReportParams();
            }
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

            rptViewer1.LocalReport.DataSources.Clear();
            DataTable dt = ds.Tables[0];
            List<string> numericColumns = new List<string>();
            List<string> overlookColumns = new List<string> {
                "Id",
                "IsIntermediate",
                "WarehouseID",
                "InventoryID"
            };
            dt.AcceptChanges();
            foreach (DataColumn column in dt.Columns)
            {
                if (IsNumeric(column))
                {
                    var match = overlookColumns
                            .FirstOrDefault(stringToCheck => stringToCheck.Contains(column.ColumnName));
                    if (match == null)
                    {
                        numericColumns.Add(column.ColumnName);
                    }
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool tobedeleted = true;
                foreach (var column in numericColumns)
                {
                    if (
                        Double.Parse(string.IsNullOrEmpty(dt.Rows[i][column].ToString())
                                        ? "0.00"
                                        : dt.Rows[i][column].ToString()
                                    ) != 0
                        )
                    {
                        tobedeleted = false;
                        break;
                    }
                }
                if (tobedeleted)
                {
                    dt.Rows.Remove(dt.Rows[i]);
                    i--;
                }
            }
            rptViewer1.LocalReport.DataSources.Add(new ReportDataSource("SpecialInventoryRegister", dt));
            rptViewer1.LocalReport.ReportPath = "./Reports/InventoryPotentialRevenueIndivisdualProduct.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && dt.Rows.Count > 0)
                lblWorkPeriod.Text = "Work Period Considered From " + dt.Rows[0]["StartDate"].ToString() + " TO " + dt.Rows[0]["EndDate"].ToString();
            else
                lblWorkPeriod.Text = string.Empty;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
