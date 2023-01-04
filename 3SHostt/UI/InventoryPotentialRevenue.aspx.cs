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

/// <summary>
/// Defines the <see cref="UI_InventoryPotentialRevenue" />
/// </summary>
public partial class UI_InventoryPotentialRevenue : System.Web.UI.Page
{
    #region Methods

    /// <summary>
    /// The IsNumeric
    /// </summary>
    /// <param name="col">The col<see cref="DataColumn"/></param>
    /// <returns>The <see cref="bool"/></returns>
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

    /// <summary>
    /// The MsgBox
    /// </summary>
    /// <param name="ex">The ex<see cref="String"/></param>
    /// <param name="pg">The pg<see cref="Page"/></param>
    /// <param name="obj">The obj<see cref="Object"/></param>
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }

    /// <summary>
    /// The BtnSearch_Click
    /// </summary>
    /// <param name="sender">The sender<see cref="object"/></param>
    /// <param name="e">The e<see cref="EventArgs"/></param>
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    /// <summary>
    /// The ChckedChanged
    /// </summary>
    /// <param name="sender">The sender<see cref="object"/></param>
    /// <param name="e">The e<see cref="EventArgs"/></param>
    protected void ChckedChanged(object sender, EventArgs e)
    {
        if (CheckFifoPrice.Checked)
        {
            ddlInventoryItem.Enabled = false;
            ddlGroupItem.Enabled = false;
        }
        else
        {
            ddlInventoryItem.Enabled = true;
            ddlGroupItem.Enabled = true;
        }
    }

    /// <summary>
    /// The DdlBrand_ItemDataBound
    /// </summary>
    /// <param name="o">The o<see cref="object"/></param>
    /// <param name="e">The e<see cref="Telerik.WebControls.RadComboBoxItemDataBoundEventArgs"/></param>
    protected void DdlBrand_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Brand"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Brand"].ToString();
    }

    /// <summary>
    /// The DdlGroupItem_ItemDataBound
    /// </summary>
    /// <param name="o">The o<see cref="object"/></param>
    /// <param name="e">The e<see cref="Telerik.WebControls.RadComboBoxItemDataBoundEventArgs"/></param>
    protected void DdlGroupItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["GroupCode"].ToString();
    }

    /// <summary>
    /// The DdlGroupItem_SelectedIndexChanged
    /// </summary>
    /// <param name="o">The o<see cref="object"/></param>
    /// <param name="e">The e<see cref="Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs"/></param>
    protected void DdlGroupItem_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindInventoryItem(e.Text);
    }

    /// <summary>
    /// The DdlInventoryItem_ItemDataBound
    /// </summary>
    /// <param name="o">The o<see cref="object"/></param>
    /// <param name="e">The e<see cref="Telerik.WebControls.RadComboBoxItemDataBoundEventArgs"/></param>
    protected void DdlInventoryItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }

    /// <summary>
    /// The DdlVendor_ItemDataBound
    /// </summary>
    /// <param name="o">The o<see cref="object"/></param>
    /// <param name="e">The e<see cref="Telerik.WebControls.RadComboBoxItemDataBoundEventArgs"/></param>
    protected void DdlVendor_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Vendor"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Vendor"].ToString();
    }

    /// <summary>
    /// The DdlWarehouse_ItemDataBound
    /// </summary>
    /// <param name="o">The o<see cref="object"/></param>
    /// <param name="e">The e<see cref="Telerik.WebControls.RadComboBoxItemDataBoundEventArgs"/></param>
    protected void DdlWarehouse_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }

    /// <summary>
    /// The Page_Load
    /// </summary>
    /// <param name="sender">The sender<see cref="object"/></param>
    /// <param name="e">The e<see cref="EventArgs"/></param>
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
            BindInvetoryTakeType();
            foreach (RadComboBoxItem itm in ddlTakeType.Items)
            {
                itm.Checked = true;
            }
            BindInventoryItem(ddlGroupItem.SelectedValue);
            BindWarehouse();
            BindBrand();
            foreach (RadComboBoxItem itm in ddlBrand.Items)
            {
                itm.Checked = true;
            }
            BindVendor();
            CheckFifoPrice.Checked = false;
            isForCompiled.Checked = false;
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

    /// <summary>
    /// The BindBrand
    /// </summary>
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
            comboBox.Items.Add(item);
            item.DataBind();
        }
    }

    /// <summary>
    /// The BindData
    /// </summary>
    private void BindData()
    {
        try
        {
            DataSet dsStartEndDate = CurrentStockManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate));
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            int nFisrtWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["FirstWorkPeriodID"]);
            int nLastWorkPeriodID = Convert.ToInt32(dsStartEndDate.Tables[0].Rows[0]["LastWorkPeriodID"]);
            int outletId = 0;
            if (CheckFifoPrice.Checked && (ddlWarehouse.SelectedValue == null || ddlWarehouse.SelectedValue == "0"))
            {
                MsgBox("Please Select a warehouse", this.Page, this);
                return;
            }

            string GroupCodes = string.Empty;
            var checkeditems = ddlGroupItem.CheckedItems;
            foreach (var item in checkeditems)
            {
                GroupCodes = string.IsNullOrEmpty(GroupCodes) ? "'" + item.Value.ToString() + "'" : GroupCodes + ",'" + item.Value.ToString() + "'";
            }

            string InventoryTakeTypes = string.Empty;
            var checkeditemsInventoryTakeType = ddlTakeType.CheckedItems;
            foreach (var item in checkeditemsInventoryTakeType)
            {
                InventoryTakeTypes = string.IsNullOrEmpty(InventoryTakeTypes) ? "'" + item.Value.ToString() + "'" : InventoryTakeTypes + ",'" + item.Value.ToString() + "'";
            }

            string Brands = string.Empty;
            var checkeditemsBrands = ddlBrand.CheckedItems;
            foreach (var item in checkeditemsBrands)
            {
                Brands = string.IsNullOrEmpty(Brands) ? "'" + item.Value.ToString() + "'" : Brands + ",'" + item.Value.ToString() + "'";
            }

            DataSet ds = CurrentStockManager.GetSpecialInvntoryRegister(
                GroupCodes,
                ddlInventoryItem.SelectedValue,
                ddlWarehouse.SelectedValue,
                dStartDate,
                dEndDate,
                nFisrtWorkPeriodID,
                nLastWorkPeriodID,
                CheckFifoPrice.Checked ? true : false,
                Brands,
                ddlVendor.SelectedValue,
                InventoryTakeTypes,
                isForCompiled.Checked);

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
            rptViewer1.LocalReport.DataSources.Clear();
            if (ds.Tables.Count > 0)
            {
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
                    if (!isForCompiled.Checked && tobedeleted)
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                        i--;
                    }
                }

                rptViewer1.LocalReport.DataSources.Add(new ReportDataSource("SpecialInventoryRegister", dt));

                if (isForCompiled.Checked)
                {
                    rptViewer1.LocalReport.ReportPath = "./Reports/rptInventoryPotentialRevenueCompiled.rdlc";
                }
                else
                {
                    rptViewer1.LocalReport.ReportPath = "./Reports/rptInventoryPotentialRevenue.rdlc";
                }
                rptViewer1.LocalReport.EnableExternalImages = true;
                rptViewer1.LocalReport.SetParameters(paras);
                rptViewer1.LocalReport.Refresh();
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && dt.Rows.Count > 0)
                    lblWorkPeriod.Text = "Work Period Considered From " + dt.Rows[0]["StartDate"].ToString() + " TO " + dt.Rows[0]["EndDate"].ToString();
                else
                    lblWorkPeriod.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// The BindGroupItem
    /// </summary>
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

    /// <summary>
    /// The BindInventoryItem
    /// </summary>
    /// <param name="groupItem">The groupItem<see cref="string"/></param>
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

    /// <summary>
    /// The BindInvetoryTakeType
    /// </summary>
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

            comboBox.Items.Add(item);

            item.DataBind();
        }
    }

    /// <summary>
    /// The BindVendor
    /// </summary>
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

    /// <summary>
    /// The BindWarehouse
    /// </summary>
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

    #endregion
}
