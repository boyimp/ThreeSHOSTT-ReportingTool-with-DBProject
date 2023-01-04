using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using Newtonsoft.Json;
using BusinessObjects.TicketsManager;

using ThreeS.Modules.BasicReports.Reports;

using BusinessObjects;
using BusinessObjects.ChartsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using ThreeS.Domain.Models.Tickets;


[RoutePrefix("api/timewisechart")]
public class TimeWiseChartController : ApiController
{
    
    [HttpGet]
    [Route("dailyNetSales")]
    public string GetTimeWiseSale(int outletid,int departmentvalue,string fromdate,string todate)
    {
        DataSet dSTimeWiseSales = new DataSet();
        DataSet dSMonthlySales = new DataSet();
        dSTimeWiseSales = ChartsManager.GetTimeWiseSalesForChart(Convert.ToDateTime(fromdate).ToString("M/d/yyyy hh:mm tt"), Convert.ToDateTime(todate).ToString("M/d/yyyy hh:mm tt"), outletid, departmentvalue);
        var Data = (new Microsoft.Reporting.WebForms.ReportDataSource("TimeWiseSalesDataset", dSTimeWiseSales.Tables[0]));
        var jsonString = JsonConvert.SerializeObject(Data.Value);
        return jsonString;
    }
    
}
