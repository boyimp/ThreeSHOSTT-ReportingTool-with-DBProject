//In the name of Allah

using System;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using BusinessObjects.InventoryManager;
using Microsoft.Reporting.WebForms;
using BusinessObjects.ReportManager;
using System.Web;
using ThreeS.Report.v2.Utils;

public partial class UI_Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session[GlobalData.CURRENT_USER] is null)
        {
            Response.Redirect("./");
        }//if
        
    }

    protected void InventoryList_onClick(object sender, EventArgs e)
    {
        DataSet dsInventoryItemList = new DataSet();
        dsInventoryItemList = CurrentStockManager.GetInventoryItemList();

        try
        {
            List<ReportParameter> paras = ReportManager.GetReportParams();

            Session["DataSetName"] = "InventoryItemList";
            Session["oReportName"] = Server.MapPath("~/Reports/rptInventoryItemList.rdlc");
            Session["oDataSet"] = dsInventoryItemList;
            Session["oReportTitle"] = "Inventory Item List";
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

}
