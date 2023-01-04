using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BusinessObjects.TicketsManager;
using ThreeS.Modules.BasicReports.Reports;
using BusinessObjects;
using BusinessObjects.ChartsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using ThreeS.Domain.Models.Tickets;
using Microsoft.Reporting.WebForms;
using BusinessObjects.ReportManager;
public partial class Views_Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty((string)Session["CurrentUser"]))
            Response.Redirect("Default.aspx");
        lblLoggedInPFCompanyUser.InnerText = Session["CurrentUser"].ToString();
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