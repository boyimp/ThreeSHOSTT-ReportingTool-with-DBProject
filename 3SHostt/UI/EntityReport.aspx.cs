using BusinessObjects.MenusManager;
using BusinessObjects.ReportManager;
using BusinessObjects.TicketsManager;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using ThreeS.Domain.Models.Entities;

public partial class UI_EntityReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            BindEntityType();
            BindScreenMenuItem();
            BindEntities(Convert.ToInt32(ddlEntityType.SelectedValue));
        }
    }
    private void BindScreenMenuItem()
    {
        DataSet ds = MenuItemManager.GetScreenMenuCategories();
        ddlScreenMenu.DataSource = ds;
        ddlScreenMenu.DataBind();
        ddlScreenMenu.SelectedIndex = 0;
    }

    private void BindEntityType()
    {
        DataSet ds = EntityManager.GetEntityType();
        ddlEntityType.DataSource = ds;
        ddlEntityType.DataBind();
        ddlEntityType.SelectedIndex = 0;
        ddlScreenMenu.Enabled = false;
        ddlScreenMenu.HideCombo = true;
    }
    private void BindEntities(int EntityTypeId)
    {
        DataSet ds = EntityManager.GetEntities(EntityTypeId);
        ddlEntity.DataSource = ds;
        ddlEntity.DataBind();
        ddlEntity.SelectedIndex = 0;
    }
    protected void ddlEntityType_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
    protected void ddlEntityType_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindEntities(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    protected void ddlEntity_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
    protected void ddlEntity_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }
    protected void ChckedChanged(object sender, EventArgs e)
    {
        if (CheckCurrentWorkPeriod.Checked)
        {
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            btnSearch_Click(sender, e);
        }
        else
        {
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
            btnSearch_Click(sender, e);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindReportViewerData();
    }
    protected void BindReportViewerData()
    {
        try
        {
            if (considerAll.Checked)
            {
                DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
                DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
                DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

                DataSet ds = TicketManager.GetItemSalesEntityAccountWiseForScreenMenu(dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), Convert.ToInt32(ddlEntityType.SelectedValue), Convert.ToInt32(ddlScreenMenu.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<ReportParameter> paras = ReportManager.GetReportParams();

                    ReportParameter rptSelectedScreenMenuCategory = new ReportParameter("rptSelectedScreenMenuCategory", ddlScreenMenu.SelectedItem.Text.ToString());
                    paras.Add(rptSelectedScreenMenuCategory);

                    string DateRange = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyyy hh:mm tt") + " to " +
                                                      Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");

                    ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                    paras.Add(rptDateRange);
                    List<EntityCustomField> EntityCustomFields = null;
                    EntityCustomFields = BusinessObjects.EntitiesManager.EntityManager.GetEntityCustomFields(Convert.ToInt32(ddlEntityType.SelectedValue));
                    Entity entity = BusinessObjects.EntitiesManager.EntityManager.GetEntity(Convert.ToInt32(ddlEntity.SelectedValue));
                    string Header = "Name : " + entity.Name;
                    if (EntityCustomFields.Count > 0)
                    {
                        foreach (EntityCustomField EntityCustomField in EntityCustomFields)
                        {
                            Header = Header + Environment.NewLine + EntityCustomField.Name + " : " + entity.GetCustomData(EntityCustomField.Name);
                        }
                        ReportParameter rptHeader = new ReportParameter("rptHeader", Header);
                        paras.Add(rptHeader);
                    }
                    rptViewer1.LocalReport.DataSources.Clear();
                    rptViewer1.LocalReport.DataSources.Add(new ReportDataSource("Entity_SMC_Wise", ds.Tables[0]));
                    rptViewer1.LocalReport.ReportPath = "./Reports/rptEntitySMICategoryWiseReport.rdlc";
                    rptViewer1.LocalReport.EnableExternalImages = true;
                    rptViewer1.LocalReport.SetParameters(paras);
                    rptViewer1.LocalReport.Refresh();
                }
            }
        
            else
            {
                DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
                DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
                DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

                DataSet ds = TicketManager.GetItemSalesEntityAccountWise(dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), Convert.ToInt32(ddlEntityType.SelectedValue), Convert.ToInt32(ddlEntity.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    decimal SUMPrice = Convert.ToDecimal(ds.Tables[1].Rows[0]["SUMPrice"]);
                    decimal SUMQuantity = Convert.ToDecimal(ds.Tables[1].Rows[0]["SUMQuantity"]);
                    decimal SUMGross = Convert.ToDecimal(ds.Tables[1].Rows[0]["SUMGross"]);
                    decimal SUMGift = Convert.ToDecimal(ds.Tables[1].Rows[0]["SUMGift"]);
                    decimal SUMTotalCollection = Convert.ToDecimal(ds.Tables[1].Rows[0]["SUMTotalCollection"]);

                    List<ReportParameter> paras = ReportManager.GetReportParams();

                    ReportParameter rptSUMPrice = new ReportParameter("rptSUMPrice", SUMPrice.ToString("N2"));
                    paras.Add(rptSUMPrice);
                    ReportParameter rptSUMQuantity = new ReportParameter("rptSUMQuantity", SUMQuantity.ToString("N0"));
                    paras.Add(rptSUMQuantity);
                    ReportParameter rptSUMGross = new ReportParameter("rptSUMGross", SUMGross.ToString("N2"));
                    paras.Add(rptSUMGross);
                    ReportParameter rptSUMGift = new ReportParameter("rptSUMGift", SUMGift.ToString("N2"));
                    paras.Add(rptSUMGift);
                    ReportParameter rptSUMTotalCollection = new ReportParameter("rptSUMTotalCollection", SUMTotalCollection.ToString("N2"));
                    paras.Add(rptSUMTotalCollection);


                    string DateRange = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]).ToString("dd MMM yyyy hh:mm tt") + " to " +
                                                      Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]).ToString("dd MMM yyyy hh:mm tt");

                    ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
                    paras.Add(rptDateRange);
                    List<EntityCustomField> EntityCustomFields = null;
                    EntityCustomFields = BusinessObjects.EntitiesManager.EntityManager.GetEntityCustomFields(Convert.ToInt32(ddlEntityType.SelectedValue));
                    Entity entity = BusinessObjects.EntitiesManager.EntityManager.GetEntity(Convert.ToInt32(ddlEntity.SelectedValue));
                    string Header = "Name : " + entity.Name;
                    if (EntityCustomFields.Count > 0)
                    {
                        foreach (EntityCustomField EntityCustomField in EntityCustomFields)
                        {
                            Header = Header + Environment.NewLine + EntityCustomField.Name + " : " + entity.GetCustomData(EntityCustomField.Name);
                        }
                        ReportParameter rptHeader = new ReportParameter("rptHeader", Header);
                        paras.Add(rptHeader);
                    }
                    rptViewer1.LocalReport.DataSources.Clear();
                    rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemSalesAccountWise", ds.Tables[0]));
                    rptViewer1.LocalReport.ReportPath = "./Reports/rptEntityReport.rdlc";
                    rptViewer1.LocalReport.EnableExternalImages = true;
                    rptViewer1.LocalReport.SetParameters(paras);
                    rptViewer1.LocalReport.Refresh();
                }
            }
        }
        catch// (Exception e)
        {
            
            throw;
        }
        //lblWorkPeriod.Text = "Work Period Considered From " + dsStartEndDate.Tables[0].Rows[0]["StartDate"].ToString() + " TO " + dsStartEndDate.Tables[0].Rows[0]["EndDate"].ToString();
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (considerAll.Checked)
        {
            ddlEntity.Enabled = false;
            ddlScreenMenu.Enabled = true;
            ddlScreenMenu.HideCombo = false;
        }
        else
        {
            ddlEntity.Enabled = true;
            ddlScreenMenu.Enabled = false;
            ddlScreenMenu.HideCombo = true;
        }
    }

    protected void ddlScreenMenu_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    protected void ddlScreenMenu_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
    }
}
