using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObjects.InventoryManager;
using Telerik.Web.UI;
using System.Xml;
using Microsoft.Reporting.WebForms;
using BusinessObjects.ReportManager;

public partial class UI_InventoryVsRecipes : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            BindInventoryItem();
            foreach (RadComboBoxItem itm in ddlInventoryItem.Items)
            {
                itm.Checked = true;
            }
        }
        //BindData();
    }

    private void BindData()
    {
        try
        {
            string intentoryitems = "";
            var checkeditems = ddlInventoryItem.CheckedItems;
            foreach (var item in checkeditems)
            {
                intentoryitems = string.IsNullOrEmpty(intentoryitems) ? "'" + item.Value.ToString() + "'" : intentoryitems + ",'" + item.Value.ToString() + "'";
            }

            DataSet ds = CurrentStockManager.GetInventoryVsRecipesData(intentoryitems);
            List<ReportParameter> paras = ReportManager.GetReportParams();
            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("SpecialInventoryRegister", ds.Tables[0]));
            rptViewer1.LocalReport.ReportPath = "./Reports/InventoryVsRecipes.rdlc";
            rptViewer1.LocalReport.EnableExternalImages = true;
            rptViewer1.LocalReport.SetParameters(paras);
            rptViewer1.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }


  

    private void BindInventoryItem()
    {
        DataSet ds = CurrentStockManager.GetInventoryItem(string.Empty);
        ddlInventoryItem.DataSource = ds;
        ddlInventoryItem.DataBind();
        ddlInventoryItem.SelectedValue = "0";
    }

   


    protected void ddlInventoryItem_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
    {
        
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }


    protected void btnSearch_Click1(object sender, EventArgs e)
    {
      
    }





    protected void ddlInventoryItem_ItemDataBound1(object sender, RadComboBoxItemEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["id"].ToString();
    }
}
