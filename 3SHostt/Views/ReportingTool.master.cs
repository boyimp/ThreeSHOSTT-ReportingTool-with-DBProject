using System;
using System.Data;
using System.Collections.Generic;
using BusinessObjects.InventoryManager;
using Microsoft.Reporting.WebForms;
using BusinessObjects.ReportManager;
using System.Web;
using ThreeS.Report.v2.Utils;

public partial class Views_ReportingTool : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session[GlobalData.CURRENT_USER] is null)
        {
            Response.Redirect("Default.aspx");
        }//if

        if (Request.QueryString["ReportType"] != null)
        {
            if ( Request.QueryString["ReportType"].Equals("All"))
            {
                Page.Title = "Inventory Potential Revenue";
            }
            else if ( Request.QueryString["ReportType"].Equals("Wastage"))
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
        }

        lblLoggedInPFCompanyUser.InnerText = HttpContext.Current.Session[GlobalData.CURRENT_USER].ToString();
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
    public void ChangeTitle(string newTitle)
    {
        Page.Title = newTitle;
    }
}
