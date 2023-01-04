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
using BusinessObjects.SyncManager;
using Newtonsoft.Json.Linq;
using BusinessObjects.WebPushManager;

public partial class UI_PushNotification : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");

        if (!IsPostBack)
        {
            BindData();
        }
	}
    private void BindData()
    {
        try
        {
            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(DateTime.MinValue, DateTime.MinValue, true);
            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dMaxEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["MaxEndDate"]);
            //lblWorkPeriod.Text = "Last Central Work Period Ended : " + dMaxEndDate.ToString("dd MMM yyyy hh:mm tt");
            DataSet ds = SyncManager.GetSyncOutletStatus();
            //RadGrid1.DataSource = ds;
            //RadGrid1.DataBind();
            //RadGrid1.GroupingSettings.CaseSensitive = false;
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void RadGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        e.Column.FilterControlWidth = Unit.Pixel(40);
        //if (e.Column.DataType == typeof(decimal))
        //{            
        //    ((Telerik.Web.UI.GridBoundColumn)e.Column).FooterText = "Total: ";
        //    ((Telerik.Web.UI.GridBoundColumn)e.Column).Aggregate = Telerik.Web.UI.GridAggregateFunction.Sum;            
        //}        
        if (e.Column.UniqueName == "Id" || e.Column.UniqueName == "OutletInfo")
        {
            e.Column.Visible = false;
        }
        else
        {
            e.Column.FilterControlWidth = 100;
            e.Column.HeaderText.Replace('_', ' ');
            e.Column.HeaderStyle.Wrap = true;
            e.Column.HeaderStyle.Width = 150;
            //e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
            //e.Column.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = SyncManager.GetSyncOutletStatus();
        //RadGrid1.DataSource = ds;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string endpiontJson = lblHidden.Value;
        string IsSubscribed = lblIsSubscribed.Value.ToLower();
        string currentUser = SessionManager.CurrentUser;
        WebPushClientModel webPushCient = new WebPushClientModel();
        JToken token = JObject.Parse(endpiontJson);

        webPushCient.PushEndpoint = (string)token.SelectToken("endpoint");
        webPushCient.P256DH = (string)token.SelectToken("keys.p256dh");
        webPushCient.Auth = (string)token.SelectToken("keys.auth");
        webPushCient.UserName = currentUser;
        try
        {
            if (IsSubscribed.Equals("true"))
                WebPushManager.Insert(webPushCient);
            else
                WebPushManager.Delete(webPushCient);

            lblMessage.Text = "Saved Succesfully";
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
}
