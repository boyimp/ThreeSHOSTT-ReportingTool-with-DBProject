using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BusinessObjects.InventoryManager;
using Microsoft.Reporting.WebForms;
using BusinessObjects.ReportManager;
using System.Diagnostics;
using ThreeS.Report.v2.Utils;

public partial class UI_Home : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        Debug.WriteLine("session Home: " + HttpContext.Current.Session[GlobalData.CURRENT_USER]);
		if(HttpContext.Current.Session[GlobalData.CURRENT_USER].ToString() == null)
		{
            Response.Redirect("Default.aspx");
        }
        //if (string.IsNullOrEmpty(HttpContext.Current.Session["Username"].ToString()))
			//Response.Redirect("Default.aspx");
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
