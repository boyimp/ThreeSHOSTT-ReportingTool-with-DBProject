using BusinessObjects.AccountsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.ReportManager;
using BusinessObjects.TicketsManager;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ThreeS.Domain.Models.Tickets;

public partial class UI_IncomeStatement : System.Web.UI.Page
{
    private DataSet _dsDepartments;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {

            BindGroupItem();
            foreach (RadComboBoxItem itm in ddlGroupItem.Items)
            {
                itm.Checked = true;
            }
            BindBrand();
            foreach (RadComboBoxItem itm in ddlBrand.Items)
            {
                itm.Checked = true;
            }
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            rbSalesView.Checked = true;
            rbStockView.Checked = false;
            LoadOutletsCbo();
            foreach (RadComboBoxItem itm in ddlOutlets.Items)
            {
                itm.Checked = true;
            }

            BindDepartment();
            foreach (RadComboBoxItem itm in ddlTicketType.Items)
            {
                itm.Checked = true;
            }
        }
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
    private void LoadOutletsCbo()
    {
        DataSet ds = TicketManager.GetOutlets();
        RadComboBox comboBox = (RadComboBox)ddlOutlets;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["Name"].ToString();
            item.Value = row["Id"].ToString();
            item.Attributes.Add("Name", row["Name"].ToString());
            comboBox.Items.Add(item);
            item.DataBind();
        }
    }
    protected void ddlOutlets_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
    private void BindDepartment()
    {
        DataSet ds = TicketManager.GetDepartments();
        RadComboBox comboBox = (RadComboBox)ddlTicketType;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["Name"].ToString();
            item.Value = row["Id"].ToString();
            item.Attributes.Add("Name", row["Name"].ToString());
            comboBox.Items.Add(item);
            item.DataBind();
        }
    }
    private void BindGroupItem()
    {
        DataSet ds = CurrentStockManager.GetGroupItem();
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
    private void BindBrand()
    {
        DataSet ds = CurrentStockManager.GetBrand();
        RadComboBox comboBox = (RadComboBox)ddlBrand;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["Brand"].ToString();
            item.Value = row["Brand"].ToString();
            item.Attributes.Add("Brand", row["Brand"].ToString());
            comboBox.Items.Add(item);
            item.DataBind();
        }
    }
    private void BindData()
    {
        try
        {
            bool IsShort = false;
            string sReportTitle = "Income Statement";
            DataSet ds = null;
            decimal dGrandTotal = 0;
            double dTotalExpense = 0;
            double dProfitLoss = 0;
            double CostOfProducton = 0;
            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
            _dsDepartments = TicketManager.GetDepartments();
            if (cbExactTime.Checked)
            {
                dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            }
            string GroupCodes = string.Empty;
            var checkeditems = ddlGroupItem.CheckedItems;
            foreach (var item in checkeditems)
            {
                GroupCodes = string.IsNullOrEmpty(GroupCodes) ? "'" + item.Value.ToString() + "'" : GroupCodes + ",'" + item.Value.ToString() + "'";
            }

            string Brands = string.Empty;
            var checkedBrands = ddlBrand.CheckedItems;
            foreach (var item in checkedBrands)
            {
                Brands = string.IsNullOrEmpty(Brands) ? "'" + item.Value.ToString() + "'" : Brands + ",'" + item.Value.ToString() + "'";
            }

            dProfitLoss = (double)dGrandTotal - dTotalExpense - CostOfProducton;


            List<ReportParameter> paras = ReportManager.GetReportParams(0);
            string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
                                               dEndDate.ToString("dd MMM yyyy hh:mm tt");
            ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
            paras.Add(rptDateRange);
            if (rbSalesView.Checked && !rbStockView.Checked)
            {
                IsShort = true;
                sReportTitle = "Sales Statement";
            }
            ReportParameter rptIsShort = new ReportParameter("rptIsShort", IsShort.ToString());
            paras.Add(rptIsShort);
            ReportParameter rptTitle = new ReportParameter("rptTitle", sReportTitle);
            paras.Add(rptTitle);
            ReportParameter rptSUMTotalAmount = new ReportParameter("rptSUMTotalAmount", dProfitLoss.ToString());
            paras.Add(rptSUMTotalAmount);
            ReportParameter rptSUMPrice = new ReportParameter("rptSUMPrice", CostOfProducton.ToString());
            paras.Add(rptSUMPrice);

            rptViewer1.LocalReport.DataSources.Clear();
            ds = GetIncomeStatementDataset(dStartDate, dEndDate, GroupCodes, Brands);
            foreach (DataTable dt in ds.Tables)
            {
                rptViewer1.LocalReport.DataSources.Add(new ReportDataSource(dt.TableName, dt));
            }
            rptViewer1.LocalReport.ReportPath = "./Reports/rptIncomeStatement.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataSet GetIncomeStatementDataset(DateTime dStartDate, DateTime dEndDate, string GroupCodes, string Brands)
    {
        DataSet dsFinal = new DataSet();
        DataTable dtSalesFinal = new DataTable("Sales");
        dtSalesFinal.Columns.AddRange(new DataColumn[] { new DataColumn("OutletName")
            , new DataColumn("Sales")
            , new DataColumn("Net", typeof(decimal))
            , new DataColumn("Sorting", typeof(int))
        });
        DataTable dtIncomesFinal = new DataTable("Incomes");
        dtIncomesFinal.Columns.AddRange(new DataColumn[] { new DataColumn("OutletName")
            , new DataColumn("Sales")
            , new DataColumn("Net", typeof(decimal))
            , new DataColumn("Sorting", typeof(int))
        });
        DataTable dtProductionCost = new DataTable("ProductionCost");
        dtProductionCost.Columns.AddRange(new DataColumn[] { new DataColumn("OutletName")
            , new DataColumn("Sales")
            , new DataColumn("Net", typeof(decimal))
        });
        DataTable dtAccounts = new DataTable("Accounts");
        dtAccounts.Columns.AddRange(new DataColumn[] { new DataColumn("OutletName")
            , new DataColumn("Sales")
            , new DataColumn("Net", typeof(decimal))
            , new DataColumn("Sorting", typeof(int))
        });
        DataTable dtWastage = new DataTable("Wastage");
        dtWastage.Columns.AddRange(new DataColumn[] { new DataColumn("OutletName")
            , new DataColumn("Sales")
            , new DataColumn("Net", typeof(decimal))
        });
        double dProductionOverheadPerc = Convert.ToDouble(txtProductionOverhead.Text);
        double dMarketingCostPerc = Convert.ToDouble(txtMarketingCost.Text);
        double dDepriciationPerc = Convert.ToDouble(txtDepreciation.Text);
        double dRoyalty = Convert.ToDouble(txtRoyalty.Text);
        string Ids = string.Empty;
        var checkeditems = ddlTicketType.CheckedItems;
        foreach (var item in checkeditems)
        {
            Ids = string.IsNullOrEmpty(Ids) ? item.Value.ToString() : Ids + ',' + item.Value.ToString();
        }
        var checkedOutlets = ddlOutlets.CheckedItems;
        IEnumerable<Department> oDepartments = TicketManager.GetDepartmentCollection();
        DataSet dtTicketType = TicketManager.GetTicketType();
        DataSet dtCalculationType = TicketManager.GetCalculationTypes();
        DataSet dtTaxTemplate = TicketManager.GetTaxTemplates();
        foreach (var itm in checkedOutlets)
        {
            int OutletId = Convert.ToInt32(itm.Value);
            decimal dNetTotal = 0;
            decimal dGrossTotal = 0;
            double dTotalExpense = 0;
            double dProfitLoss = 0;
            if (OutletId == 0)
                continue;
            string OutletName = Convert.ToString(itm.Text);            
            List<Ticket> oTickets = TicketManager.GetTicketsFaster(OutletId, Ids, dStartDate, dEndDate);
            DataTable dtSales = new DataTable("Sales");
            List<Ticket> oValidTickets = oTickets.Where(x => x.TotalAmount >= 0).ToList();

            dtSales = TicketManager.CreateTicketTypeInfoShortOptimized(dtCalculationType,
                dtTicketType,
                dtTaxTemplate,
                oTickets, 
                oValidTickets, 
                oDepartments , 
                ref dNetTotal, 
                ref dGrossTotal, 
                OutletId
            );

            dtSales.TableName = "Sales";

            if (dtSales.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSales.Rows)
                {
                    dtSalesFinal.Rows.Add(new object[] { OutletName
                        , dr["Sales"].ToString()
                        , Convert.ToDouble(dr["Net"]).ToString("0.00")
                        , Convert.ToInt32(dr["Sorting"]).ToString()
                    });
                }
            }
            else
            {
                dtSalesFinal.Rows.Add(new object[] { OutletName, "Gross Total", 0.ToString("0.00") });
            }
            DataTable dtIncomes = new DataTable("Incomes");
            dtIncomes = TicketManager.GetIncomes(oTickets);
            dtIncomes.TableName = "Incomes";

            if (dtIncomes.Rows.Count > 0)
            {
                foreach (DataRow dr in dtIncomes.Rows)
                {
                    dtIncomesFinal.Rows.Add(new object[] { OutletName
                        , dr["Sales"].ToString()
                        , Convert.ToDouble(dr["Net"]).ToString("0.00")
                        , Convert.ToInt32(dr["Sorting"]).ToString()
                    });
                }
            }

            if (rbStockView.Checked && !rbSalesView.Checked)
            {
                DataSet ds = new DataSet();
                ds = GetProductionCostTable(dStartDate, dEndDate, OutletId, GroupCodes, Brands);
                double OpeningStockValue = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["TotalValueOfStock"]);
                double PurchaseStockValue = string.IsNullOrEmpty(ds.Tables[1].Rows[0]["TotalPrice"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[1].Rows[0]["TotalPrice"]);
                double ClosingStockValue = string.IsNullOrEmpty(ds.Tables[2].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[2].Rows[0]["TotalValueOfStock"]);
                double WastageValue = string.IsNullOrEmpty(ds.Tables[3].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[3].Rows[0]["TotalValueOfStock"]);
                double StockOutValue = string.IsNullOrEmpty(ds.Tables[4].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[4].Rows[0]["TotalValueOfStock"]);
                double ReturnValue = string.IsNullOrEmpty(ds.Tables[5].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[5].Rows[0]["TotalValueOfStock"]);
                double Consumption = (OpeningStockValue + PurchaseStockValue - ReturnValue - StockOutValue) - ClosingStockValue;
                double OtherProductionOverhead = Consumption * (dProductionOverheadPerc / 100);
                double CostOfProduction = Consumption + OtherProductionOverhead;
                double MarketingCost = CostOfProduction * (dMarketingCostPerc / 100);
                double Depreciation = CostOfProduction * (dDepriciationPerc / 100);
                double Royalty = (double)dGrossTotal * (dRoyalty / 100);

                dtProductionCost.Rows.Add(new object[] { OutletName, "Opening Stock", OpeningStockValue.ToString("0.00") });
                dtProductionCost.Rows.Add(new object[] { OutletName, "Purchase-Transfer In", PurchaseStockValue.ToString("0.00") });
                dtProductionCost.Rows.Add(new object[] { OutletName, "Closing Stock", ClosingStockValue.ToString("0.00") });
                dtProductionCost.Rows.Add(new object[] { OutletName, "Consumption", Consumption.ToString("0.00") });
                dtProductionCost.Rows.Add(new object[] { OutletName, "Other Production Overhead", OtherProductionOverhead.ToString("0.00") });
                dtProductionCost.Rows.Add(new object[] { OutletName, "Total Cost of Production", CostOfProduction.ToString("0.00") });

                dtWastage.Rows.Add(new object[] { OutletName, "Discard(Wastage)", WastageValue.ToString("0.00") });

                DataTable dtTemp = new DataTable("Temp");
                dtTemp = AccountManager.GetAccoutWiseBalance("Expenditure", string.Empty,
                     dStartDate.ToString("dd MMM yyyy hh:mm tt"), dEndDate.ToString("dd MMM yyyy hh:mm tt"), OutletId).Tables[0];

                foreach (DataRow dr in dtTemp.Rows)
                {
                    dtAccounts.Rows.Add(new object[] { OutletName, dr["AccountName"].ToString(), Convert.ToDouble(dr["Balance"]).ToString("0.00"), 1.ToString() });
                    dTotalExpense = dTotalExpense + Convert.ToDouble(dr["Balance"]);
                }

                double dTotalCost = dTotalExpense + CostOfProduction + MarketingCost + Depreciation + Royalty;
                dProfitLoss = (double)dNetTotal - dTotalCost;
                dtAccounts.Rows.Add(new object[] { OutletName, "Marketing Cost", MarketingCost.ToString("0.00"), 2.ToString() });
                dtAccounts.Rows.Add(new object[] { OutletName, "Depreciation", Depreciation.ToString("0.00"), 3.ToString() });
                dtAccounts.Rows.Add(new object[] { OutletName, "Royalty", Royalty.ToString("0.00"), 3.ToString() });
                dtAccounts.Rows.Add(new object[] { OutletName, "Total Cost", dTotalCost.ToString("0.00"), 4.ToString() });
                dtAccounts.Rows.Add(new object[] { OutletName, "Profit/Loss", dProfitLoss.ToString("0.00"), 5.ToString() });
            }
        }
        dsFinal.Tables.Add(dtSalesFinal.Copy());
        dsFinal.Tables.Add(dtIncomesFinal.Copy());
        dsFinal.Tables.Add(dtProductionCost.Copy());
        dsFinal.Tables.Add(dtAccounts.Copy());
        dsFinal.Tables.Add(dtWastage.Copy());
        return dsFinal;
    }

    private DataSet GetProductionCostTable(DateTime dStartDate, DateTime dEndDate, int outletId, string GroupCodes, string Brands)
    {
        DataSet ds = new DataSet();
        if (outletId > 0)
        {
            DataTable dtSyncWarehouses = CurrentStockManager.GetSyncWarehouseOutlets();
            DataRow[] rows = dtSyncWarehouses.Select(string.Format("OutletId={0}", outletId));
            int warehouseId = Convert.ToInt32(rows[0]["WarehouseId"]);
            ds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(dStartDate.ToString("dd MMM yyyy hh:mm tt"), dEndDate.ToString("dd MMM yyyy hh:mm tt"), warehouseId, GroupCodes, Brands);
        }
        else
        {
            ds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(dStartDate.ToString("dd MMM yyyy hh:mm tt"), dEndDate.ToString("dd MMM yyyy hh:mm tt"), GroupCodes, Brands);
        }
        return ds;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void ddlOutlets_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        var checkedOutlets = ddlOutlets.CheckedItems;

        foreach (var itm in checkedOutlets)
        {
            int OutletId = Convert.ToInt32(itm.Value);
        }
    }
}
