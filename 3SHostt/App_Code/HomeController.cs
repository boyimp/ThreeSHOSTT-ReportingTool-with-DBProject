using BusinessObjects;
using BusinessObjects.ChartsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using BusinessObjects.TicketsManager;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

/// <summary>
/// Defines the <see cref="HomeController" />
/// </summary>
[RoutePrefix("api/home")]
public class HomeController : ApiController
{
    #region Fields

    /// <summary>
    /// Defines the DsStartEndDate
    /// </summary>
    internal static readonly DataSet DsStartEndDate = TicketManager.GetStartAndEndDate(Convert.ToDateTime(DateTime.Today),
    Convert.ToDateTime(DateTime.Today), true);

    /// <summary>
    /// Defines the DsTimeWiseSales
    /// </summary>
    internal static readonly DataSet DsTimeWiseSales = ChartsManager.GetTimeWiseSalesForChart(_dStartDate.ToString("M/d/yyyy hh:mm tt"),
           _dEndDate.ToString("M/d/yyyy hh:mm tt"), 0, Convert.ToInt32(0));

    /// <summary>
    /// Defines the _dEndDate
    /// </summary>
    internal static DateTime _dEndDate = Convert.ToDateTime(DsStartEndDate.Tables[0].Rows[0]["EndDate"]);

    /// <summary>
    /// Defines the _dStartDate
    /// </summary>
    internal static DateTime _dStartDate = Convert.ToDateTime(DsStartEndDate.Tables[0].Rows[0]["StartDate"]);

    /// <summary>
    /// Defines the _ticketDt
    /// </summary>
    internal DataTable _ticketDt = DsTimeWiseSales.Tables[0];

    #endregion

    #region Methods

    /// <summary>
    /// The DailyNetSales
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("dailyNetSales")]
    public string DailyNetSales()
    {
        try
        {
            var dailyNetSalesDataTable = ChartsManager.GetDailyNetSalesForDashBoard(0).Tables[0];
            var last7DaysSale = dailyNetSalesDataTable.Rows.Cast<System.Data.DataRow>().Take(7).CopyToDataTable();
            var reversedDt = last7DaysSale.Clone();
            for (var row = last7DaysSale.Rows.Count - 1; row >= 0; row--)
            {
                reversedDt.ImportRow(last7DaysSale.Rows[row]);
            }

            var jsonString = JsonConvert.SerializeObject(reversedDt);
            return jsonString;
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// The Delete
    /// </summary>
    /// <param name="id">The id<see cref="int"/></param>
    public void Delete(int id)
    {
    }

    /// <summary>
    /// The Get
    /// </summary>
    /// <param name="id">The id<see cref="int"/></param>
    /// <returns>The <see cref="string"/></returns>
    public string Get(int id)
    {
        return "value";
    }

    /// <summary>
    /// The GetAmountOfVoids
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetAmountOfVoids")]
    public string GetAmountOfVoids()
    {
        var voidOrders = TicketManager.VoidOrders(_dStartDate, _dEndDate, "Void", 0, string.Empty);
        var voidDt = voidOrders.Tables["VoidOrders"];
        IEnumerable<decimal> voidList =
           voidDt.Rows.OfType<DataRow>().Select(dr => dr.Field<decimal>("Total")).ToList();

        decimal sum = voidList.Sum(listitem => Convert.ToDecimal(listitem));
        return sum.ToString();
    }

    /// <summary>
    /// The GetCurrentAccoountWiseReport
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetCurrentAccoountWiseReport")]
    public string GetCurrentAccoountWiseReport()
    {


        var dayWiseDataset = TicketManager.GetTicketsAccountDayWise(
            _dStartDate.ToString("dd MMM yyyy hh:mm:ss tt"), _dEndDate.ToString("dd MMM yyyy hh:mm:ss tt"), 1, 0, string.Empty);
        var jsonString = JsonConvert.SerializeObject(dayWiseDataset.Tables[0]);
        return jsonString;
    }

    /// <summary>
    /// The GetCurrentGrossSale
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetCurrentGrossSale")]
    public string GetCurrentGrossSale()
    {
        try
        {
            var dailyNetSalesDataTable = ChartsManager.GetDailyNetSalesForDashBoard(0).Tables[0];
            IEnumerable<decimal> last30DaysTicketTotalList = dailyNetSalesDataTable.Rows.OfType<DataRow>().Select(dr => dr.Field<decimal>("TicketTotalAmount")).ToList();
            return last30DaysTicketTotalList.Last().ToString();
        }
        catch
        {
            return "0";
        }
    }

    /// <summary>
    /// The GetCurrentNetSale
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetCurrentNetSale")]
    public string GetCurrentNetSale()
    {
        try
        {
            var dailyNetSalesDataTable = ChartsManager.GetDailyNetSalesForDashBoard(0).Tables[0];
            IEnumerable<decimal> last30DaysSalesList = dailyNetSalesDataTable.Rows.OfType<DataRow>().Select(dr => dr.Field<decimal>("Sales")).ToList();
            return last30DaysSalesList.Last().ToString();
        }
        catch
        {
            return "0";
        }
    }

    /// <summary>
    /// The GetCurrentStockValue
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetCurrentStockValue")]
    public string GetCurrentStockValue()
    {
        DataSet ds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(_dStartDate.ToString(), _dEndDate.ToString(), string.Empty, string.Empty);
        int OpeningStockValue = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["TotalValueOfStock"]);
        int PurchaseStockValue = string.IsNullOrEmpty(ds.Tables[1].Rows[0]["TotalPrice"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["TotalPrice"]);
        return (OpeningStockValue + PurchaseStockValue).ToString();
    }

    /// <summary>
    /// The GetMenuWiseCurrentSales
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetMenuWiseCurrentSales")]
    public string GetMenuWiseCurrentSales()
    {
        var tickets = TicketManager.GetTickets(_dStartDate, _dEndDate, 0, 0);
        var menuItemss = MenuItemManager.GetMenuItems(Convert.ToInt32(0), "All");
        const int departmentid = 0;
        var menuItems = departmentid == 0 
            ? ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder
            .CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss)
            .OrderBy(x => x.ID) 
            : ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder
            .CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, 0)
            .OrderBy(x => x.ID);

        IList<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo> products = new List<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo>();
        foreach (var menuItemInfo in menuItems)
        {
            var info = menuItemInfo;
            var s = menuItemss.Where(y => y.Id == info.ID).Select(y => new { y.GroupCode, y.Name });

            var dtProductionCostRecipe = MenuItemManager.ProductionCostRecipe(menuItemInfo.ID, menuItemInfo.Portion);
            var pcostR = dtProductionCostRecipe.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostRecipe.Rows[0]["ProductionCost"]) : 0;
            var TPcostR = pcostR * menuItemInfo.Quantity;
            var pprofitR = menuItemInfo.Amount - TPcostR;
            var dtProductionCostFixed = MenuItemManager.ProductionCostFixed(menuItemInfo.ID, menuItemInfo.Portion);
            var pcostF = dtProductionCostFixed.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
            var TPcostF = pcostF * menuItemInfo.Quantity;
            var pprofitF = menuItemInfo.Amount - TPcostF;

            var ds1 = TicketManager.GetDepartments();
            var drow = ds1.Tables[0].NewRow();
            drow["Id"] = "0";
            drow["Name"] = "All";
            ds1.Tables[0].Rows.InsertAt(drow, 0);
            var rows = ds1.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));

            var q = new ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo
            {
                ItemId = menuItemInfo.ID,
                DepartmentName = rows[0]["Name"].ToString(),
                GroupName = s.First().GroupCode,
                ItemName = s.First().Name,
                PortionName = menuItemInfo.Portion,
                Price = Math.Round(menuItemInfo.Price, 2),
                Quantity = Math.Round(menuItemInfo.Quantity, 2),
                NetAmount = Math.Round(menuItemInfo.Amount, 2),
                Gross = Math.Round(menuItemInfo.Price * menuItemInfo.Quantity, 2),
                ProductionCostFixed = Math.Round(pcostF, 2),
                TotalProductionCostFixed = Math.Round(TPcostF, 2),
                ProductionProfitFixed = Math.Round(pprofitF, 2),
                ProductionCostRecipeWise = Math.Round(pcostR, 2),
                TotalProductionCostRecipeWise = Math.Round(TPcostR, 2),
                ProductionProfitRecipeWise = Math.Round(pprofitR, 2),
                Deviation = Math.Round(pprofitF - pprofitR, 2)
            };
            products.Add(q);
        }
        var ds = DatasetConverter.ToDataSet<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo>(products);
        var datatable = (DataTable)ds.Tables[0];
        var hashSet = new HashSet<string>();

        foreach (DataRow dr in datatable.Rows)
        {
            var itemNo = dr["GroupName"].ToString().Trim();
            hashSet.Add(itemNo);
        }

        var items = new List<string>(hashSet);
        var result = from tab in datatable.AsEnumerable()
                     group tab by tab["GroupName"]
                into groupDt
                     select new
                     {
                         Group = groupDt.Key,
                         Sum = groupDt.Sum((r) => decimal.Parse(r["Gross"].ToString()))
                     };
        var jsonString = JsonConvert.SerializeObject(result);
        return jsonString;
    }

    /// <summary>
    /// The GetNumberOfGuests
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetNumberOfGuests")]
    public string GetNumberOfGuests()
    {
        IEnumerable<Int32> numberOfGuestsList =
        _ticketDt.Rows.OfType<DataRow>().Select(dr => dr.Field<Int32>("NumberOfGuests")).ToList();
        var numofGuests = numberOfGuestsList.Sum(nog => Convert.ToDecimal(nog));
        return numofGuests.ToString();
    }

    /// <summary>
    /// The GetNumberOfVoids
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetNumberOfVoids")]
    public string GetNumberOfVoids()
    {
        var voidOrders = TicketManager.VoidOrders(_dStartDate, _dEndDate, "Void", 0, string.Empty);
        var voidDt = voidOrders.Tables["VoidOrders"];
        IEnumerable<decimal> voidList =
           voidDt.Rows.OfType<DataRow>().Select(dr => dr.Field<decimal>("Total")).ToList();

        return voidList.Count().ToString();
    }

    /// <summary>
    /// The GetStockPurchase
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetStockPurchase")]
    public string GetStockPurchase()
    {
        DataSet ds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(_dStartDate.ToString(), _dEndDate.ToString(), string.Empty, string.Empty);

        int PurchaseStockValue = string.IsNullOrEmpty(ds.Tables[1].Rows[0]["TotalPrice"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["TotalPrice"]);
        return PurchaseStockValue.ToString();
    }

    /// <summary>
    /// The GetTicketsAmount
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetTicketsAmount")]
    public string GetTicketsAmount()
    {
        IEnumerable<Decimal> ticketTotalAmountList =
            _ticketDt.Rows.OfType<DataRow>().Select(dr => dr.Field<Decimal>("GrandTotal")).ToList();
        var sum = ticketTotalAmountList.Sum(listitem => Convert.ToDecimal(listitem));
        return sum.ToString();
    }

    /// <summary>
    /// The GetTotalNumberofTickets
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("GetTotalNumberofTickets")]
    public string GetTotalNumberofTickets()
    {
        IEnumerable<Int32> numberOfTicketsList =
            _ticketDt.Rows.OfType<DataRow>().Select(dr => dr.Field<Int32>("NumberOfTickets")).ToList();
        var sum = numberOfTicketsList.Sum(listitem => Convert.ToDecimal(listitem));
        return sum.ToString();
    }

    /// <summary>
    /// The Post
    /// </summary>
    /// <param name="value">The value<see cref="string"/></param>
    public void Post([FromBody]string value)
    {
    }

    /// <summary>
    /// The Put
    /// </summary>
    /// <param name="id">The id<see cref="int"/></param>
    /// <param name="value">The value<see cref="string"/></param>
    public void Put(int id, [FromBody]string value)
    {
    }

    /// <summary>
    /// The Top10SellingItems
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    [HttpGet]
    [Route("Top10SellingItems")]
    public string Top10SellingItems()
    {

        var tickets = TicketManager.GetTickets(_dStartDate, _dEndDate, 0, 0);
        var menuItemss = MenuItemManager.GetMenuItems(Convert.ToInt32(0), "All");
        var departmentid = 0;
        var menuItems = departmentid == 0 ? ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss).OrderBy(x => x.ID) : ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, 0).OrderBy(x => x.ID);

        IList<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo> products = new List<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo>();
        foreach (var menuItemInfo in menuItems)
        {
            var s = menuItemss.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });

            var dtProductionCostRecipe = MenuItemManager.ProductionCostRecipe(menuItemInfo.ID, menuItemInfo.Portion);
            var pcostR = dtProductionCostRecipe.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostRecipe.Rows[0]["ProductionCost"]) : 0;
            var TPcostR = pcostR * menuItemInfo.Quantity;
            var pprofitR = menuItemInfo.Amount - TPcostR;
            var dtProductionCostFixed = MenuItemManager.ProductionCostFixed(menuItemInfo.ID, menuItemInfo.Portion);
            var pcostF = dtProductionCostFixed.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
            var TPcostF = pcostF * menuItemInfo.Quantity;
            var pprofitF = menuItemInfo.Amount - TPcostF;

            var ds1 = TicketManager.GetDepartments();
            var drow = ds1.Tables[0].NewRow();
            drow["Id"] = "0";
            drow["Name"] = "All";
            ds1.Tables[0].Rows.InsertAt(drow, 0);
            var rows = ds1.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));

            var q = new ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo
            {
                ItemId = menuItemInfo.ID,
                DepartmentName = rows[0]["Name"].ToString(),
                GroupName = s.First().GroupCode,
                ItemName = s.First().Name,
                PortionName = menuItemInfo.Portion,
                Price = Math.Round(menuItemInfo.Price, 2),
                Quantity = Math.Round(menuItemInfo.Quantity, 2),
                NetAmount = Math.Round(menuItemInfo.Amount, 2),
                Gross = Math.Round(menuItemInfo.Price * menuItemInfo.Quantity, 2),
                ProductionCostFixed = Math.Round(pcostF, 2),
                TotalProductionCostFixed = Math.Round(TPcostF, 2),
                ProductionProfitFixed = Math.Round(pprofitF, 2),
                ProductionCostRecipeWise = Math.Round(pcostR, 2),
                TotalProductionCostRecipeWise = Math.Round(TPcostR, 2),
                ProductionProfitRecipeWise = Math.Round(pprofitR, 2),
                Deviation = Math.Round(pprofitF - pprofitR, 2)
            };
            products.Add(q);
        }
        var ds = DatasetConverter.ToDataSet<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo>(products);
        var datatable = (DataTable)ds.Tables[0];
        var myDataView = datatable.DefaultView;
        myDataView.Sort = "Quantity DESC";
        datatable = myDataView.ToTable();
        var top10ItemsTable = datatable.Rows.Cast<System.Data.DataRow>().Take(10).CopyToDataTable();
        var jsonString = JsonConvert.SerializeObject(top10ItemsTable);
        return jsonString;
    }

    #endregion
}
