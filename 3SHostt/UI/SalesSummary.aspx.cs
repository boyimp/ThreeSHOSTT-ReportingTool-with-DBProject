using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BusinessObjects.TicketsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using ThreeS.Domain.Models.Tickets;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using BusinessObjects.ReportManager;
using Microsoft.Reporting.WebForms;

public partial class UI_SalesSummary: System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(SessionManager.CurrentUser))
			Response.Redirect("Default.aspx");

		if (!IsPostBack)
		{
			dtpFromDate.SelectedDate = DateTime.Today;
			dtpToDate.SelectedDate = DateTime.Today;
			dtpFromDate.Enabled = false;
			dtpToDate.Enabled = false;
			rbTAXView.Checked = false;
			rbSalesSummary.Checked = true;
			LoadOutletsCbo();
			BindDepartment();
			foreach (RadComboBoxItem itm in ddlTicketType.Items)
			{
				itm.Checked = true;
			}
			BindData();
		}
	}

	protected void ChckedChanged(object sender, EventArgs e)
	{
		if (CheckCurrentWorkPeriod.Checked)
		{
			dtpFromDate.Enabled = false;
			dtpToDate.Enabled = false;
			BindData();
		}
		else
		{
			dtpFromDate.Enabled = true;
			dtpToDate.Enabled = true;
			BindData();
		}
	}
	private void LoadOutletsCbo()
	{
		DataSet ds = TicketManager.GetOutlets();
		DataRow drow = ds.Tables[0].NewRow();
		drow["Id"] = "0";
		drow["Name"] = "All";
		ds.Tables[0].Rows.InsertAt(drow, 0);
		ddlOutlets.DataSource = ds;
		ddlOutlets.DataBind();
		ddlOutlets.SelectedValue = "0";
	}
	protected void ddlOutlets_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
	{
		e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
		e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
	}
	private void BindDepartment()
	{
		DataSet ds = TicketManager.GetDepartments();
		//ddlTicketType.DataSource = ds;
		//ddlTicketType.DataBind();
		//ddlTicketType.SelectedValue = "0";
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

	//protected void ddlTicketType_ItemDataBound(object o, Telerik.WebControls.RadComboBoxItemDataBoundEventArgs e)
	//{
	//    e.Item.Text = ((DataRowView)e.Item.DataItem)["Name"].ToString();
	//    e.Item.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
	//}
	private void BindData()
	{
		try
		{
			int OutletId = Convert.ToInt32(ddlOutlets.SelectedValue);
			bool IsShort = false;
			DataSet ds = null;
			DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
			DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
			DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
			if (cbExactTime.Checked)
			{
				dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
				dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
			}   

			string Ids = string.Empty;
			var checkeditems = ddlTicketType.CheckedItems;
			foreach (var item in checkeditems)
			{
				Ids = string.IsNullOrEmpty(Ids) ? item.Value.ToString() : Ids + ',' + item.Value.ToString();
			}

			ds = TicketManager.GetSalesSummaryReport(dStartDate, dEndDate, Ids, OutletId);//(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue), chkOpen.Checked);      
			
			List<ReportParameter> paras = ReportManager.GetReportParams(OutletId);

			string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
											   dEndDate.ToString("dd MMM yyyy hh:mm tt");

			ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
			paras.Add(rptDateRange);

			ReportParameter rptIsShort = new ReportParameter("rptIsShort", IsShort.ToString());
			paras.Add(rptIsShort);

			rptViewer1.LocalReport.DataSources.Clear();
			rptViewer1.Reset();
			foreach (DataTable dt in ds.Tables)
			{
				rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet_" + dt.TableName, dt));
			}
			rptViewer1.LocalReport.ReportPath = "./Reports/rptSalesSummaryReport.rdlc";
			rptViewer1.LocalReport.EnableExternalImages = true;
			rptViewer1.LocalReport.SetParameters(paras);
			rptViewer1.LocalReport.Refresh();
		}
		catch (Exception)
		{
			
			throw;
		}
	}
	private void BindDataTAX()
	{
		try
		{
			int OutletId = Convert.ToInt32(ddlOutlets.SelectedValue);
			bool IsShort = false;
			DataSet ds = null;
			DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(dtpFromDate.SelectedDate), Convert.ToDateTime(dtpToDate.SelectedDate), CheckCurrentWorkPeriod.Checked);
			DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
			DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);
			if (cbExactTime.Checked)
			{
				dStartDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
				dEndDate = Convert.ToDateTime(dtpToDate.SelectedDate);
			}


			string Ids = string.Empty;
			var checkeditems = ddlTicketType.CheckedItems;
			foreach (var item in checkeditems)
			{
				Ids = string.IsNullOrEmpty(Ids) ? item.Value.ToString() : Ids + ',' + item.Value.ToString();
			}

			ds = TicketManager.GetSalesSummaryTaxReport(dStartDate, dEndDate, Ids, OutletId);//(dStartDate, dEndDate, Convert.ToInt32(ddlTicketType.SelectedValue), chkOpen.Checked);      

			List<ReportParameter> paras = ReportManager.GetReportParams(OutletId);

			string DateRange = dStartDate.ToString("dd MMM yyyy hh:mm tt") + " to " +
											   dEndDate.ToString("dd MMM yyyy hh:mm tt");

			ReportParameter rptDateRange = new ReportParameter("rptDateRange", DateRange);
			paras.Add(rptDateRange);

			ReportParameter rptIsShort = new ReportParameter("rptIsShort", IsShort.ToString());
			paras.Add(rptIsShort);

			rptViewer1.LocalReport.DataSources.Clear();
			rptViewer1.Reset();
			foreach (DataTable dt in ds.Tables)
			{
				rptViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet_" + dt.TableName, dt));
			}
			rptViewer1.LocalReport.ReportPath = "./Reports/rptSalesSummaryTAX.rdlc";
			rptViewer1.LocalReport.EnableExternalImages = true;
			rptViewer1.LocalReport.SetParameters(paras);
			rptViewer1.LocalReport.Refresh();
		}
		catch (Exception)
		{

			throw;
		}
	}
	protected void btnSearch_Click(object sender, EventArgs e)
	{
		if (rbSalesSummary.Checked && !rbTAXView.Checked)
		{
		   BindData();
		}
		if (!rbSalesSummary.Checked && rbTAXView.Checked)
		{
			BindDataTAX();
		}
	}	
}
