using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;

using BusinessObjects.ChartsManager;
using BusinessObjects.TicketsManager;
using Newtonsoft.Json;
using System.Web.Http;


[RoutePrefix("api/dashboard")]
public class DashboardController : ApiController
{
    [HttpGet]
    [Route("GetOutlets")]
    public string GetOutlets()
    {
        DataSet ds = TicketManager.GetOutlets();
        var jsonString = JsonConvert.SerializeObject(ds.Tables[0]);
        return jsonString;
    }

    [HttpGet]
    [Route("GetdailySales/{id:int}")]
    public string GetDailySales(int id)
    {
        var dayWiseNetSalesDt = ChartsManager.GetDailyNetSalesForDashBoard(id).Tables[0];

        var jsonString = JsonConvert.SerializeObject(dayWiseNetSalesDt);
        return jsonString;
    }

    [HttpGet]
    [Route("GetMonthlyNetSales/{id:int}")]
    public string GetMonthlyNetSales(int id)
    {
        var monthWiseNetSalesDt = ChartsManager.GetLast12MonthSales(id).Tables[0];
        monthWiseNetSalesDt.Columns["MonthYear"].ColumnName = "StartDate";
        monthWiseNetSalesDt.Columns["Amount"].ColumnName = "Sales";
        monthWiseNetSalesDt.AcceptChanges();
        var jsonString = JsonConvert.SerializeObject(monthWiseNetSalesDt);
        return jsonString;
    }

    [HttpGet]
    [Route("GetMonthlyGrossSales/{id:int}")]
    public string GetMonthlyGrossSales(int id)
    {
        var monthWiseGrossSalesDt = ChartsManager.GetLast12MonthSales(id).Tables[0];

            monthWiseGrossSalesDt.Columns["MonthYear"].ColumnName = "StartDate";
        monthWiseGrossSalesDt.Columns["Amount"].ColumnName = "Sales";
        monthWiseGrossSalesDt.AcceptChanges();
        var jsonString = JsonConvert.SerializeObject(monthWiseGrossSalesDt);
        return jsonString;
    }

    [HttpGet]
    [Route("GetdailyNetSales/{id:int}")]
    public string GetDailyNetSales(int id)
    {
        var dayWiseNetSalesDt = ChartsManager.GetDailyNetSalesForDashBoard(id).Tables[0];
        dayWiseNetSalesDt.Columns.Remove("Sales");
        dayWiseNetSalesDt.Columns["TicketTotalAmount"].ColumnName = "Sales";
        dayWiseNetSalesDt.AcceptChanges();

        var jsonString = JsonConvert.SerializeObject(dayWiseNetSalesDt);

        return jsonString;
    }
}
