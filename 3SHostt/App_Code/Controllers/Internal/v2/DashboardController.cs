//In the name of Allah

using BusinessObjects;
using BusinessObjects.ChartsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using BusinessObjects.TicketsManager;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ThreeS.Domain.Models.Tickets;
using ThreeS.Report.v2.Models;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/dashboard2")] 
    public class Dashboard2Controller : ApiController 
    {

        [HttpGet]
        [Route("GetOutlets")]
        public IHttpActionResult GetOutlets()
        {
            DataSet ds = TicketManager.GetOutlets();
            return Ok(ds.Tables[0]);
        }//func

        [HttpPost]
        [Route("GetWorkPeriodDateTimes")]
        public IHttpActionResult GetWorkPeriodDateTimes(ParameterFromDateToDate parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.fromDate, parameter.toDate);
            return Ok(workPeriodDateTimes);
        }//func

        [HttpPost]
        [Route("GetCurrentWorkPeriodWiseDateTime")]
        public IHttpActionResult GetCurrentWorkPeriodWiseDateTime(ParameterFromDateToDate parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(parameter.fromDate, parameter.toDate);
            return Ok(workPeriodDateTimes);
        }//func

        [HttpPost]
        [Route("GetTotalNumberofTickets")]
        public IHttpActionResult GetTotalNumberofTickets([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            DataSet DsTimeWiseSales = ChartsManager.GetTimeWiseSalesForChart(parameter.fromDate.ToString(),
           parameter.toDate.ToString(), parameter.outletId, parameter.departmentId);
            DataTable _ticketDt = DsTimeWiseSales.Tables[0];
            IEnumerable<Int32> numberOfTicketsList =
                _ticketDt.Rows.OfType<DataRow>().Select(dr => dr.Field<Int32>("NumberOfTickets")).ToList();
            var sum = numberOfTicketsList.Sum(listitem => Convert.ToDecimal(listitem));
            return Ok(sum);
        }//func


        [HttpPost]
        [Route("GetTickets")]
        public IHttpActionResult GetTickets([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            List<Ticket> tickets = TicketManager.GetTickets(parameter.fromDate, parameter.toDate, parameter.departmentId, parameter.outletId);
            return Ok(tickets);
        }//func

       
        [HttpPost]
        [Route("GetDashboardCardValue")]
        public async Task<IHttpActionResult> GetDashboardCardValue(ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter) 
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            TotalNetAndGrossSale totalNetAndGrossSale = await TicketManager.GetTotalNetAndGrossSale(
                                                        parameter.from,
                                                        parameter.to,
                                                        parameter.outletIds,
                                                        parameter.departmentIds
                                                        );

            VoidInfo voidInfo = await TicketManager.GetVoidCountAndAmountSum(
                                                        parameter.from,
                                                        parameter.to,
                                                        parameter.outletIds,
                                                        parameter.departmentIds
                                                        );

            GiftInfo giftInfo = await TicketManager.GetGiftCountAndAmountSum(
                                                        parameter.from,
                                                        parameter.to,
                                                        parameter.outletIds,
                                                        parameter.departmentIds
                                                        );

            DiscountInfo discountInfo = await TicketManager.GetDiscountCountAndAmountSum(
                                                        parameter.from,
                                                        parameter.to,
                                                        parameter.outletIds,
                                                        parameter.departmentIds
                                                        );

            TicketsCountAndNoOfGuest ticketsCountAndNoOfGuest = await TicketManager.GetTicketsCountAndNoOfGuest(
                                                        parameter.from,
                                                        parameter.to,
                                                        parameter.outletIds,
                                                        parameter.departmentIds
                                                        );


            DataSet sds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(DateTimeService.GetTimeStamp(parameter.from), DateTimeService.GetTimeStamp(parameter.to) , string.Empty, string.Empty);
            int OpeningStockValue = string.IsNullOrEmpty(sds.Tables[0].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToInt32(sds.Tables[0].Rows[0]["TotalValueOfStock"]);
            int PurchaseStockValue = string.IsNullOrEmpty(sds.Tables[1].Rows[0]["TotalPrice"].ToString()) ? 0 : Convert.ToInt32(sds.Tables[1].Rows[0]["TotalPrice"]);
            



            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            result["TotalGrossSale"] = totalNetAndGrossSale.GrossTotalSale;
            result["TotalNetSale"] = totalNetAndGrossSale.NetTotalSale;
            result["VoidsCount"] = voidInfo.VoidOrdersCount; 
            result["VoidsAmount"] = voidInfo.VoidOrdersAmount;
            result["GiftsCount"] = giftInfo.GiftOrdersCount;
            result["GiftsAmount"] = giftInfo.GiftOrdersAmountSum;
            result["DiscountCount"] = discountInfo.DiscountedOrdersCount;
            result["DiscountAmount"] = Math.Abs(discountInfo.DiscountedOrdersAmountSum);
            result["TicketsCount"] = ticketsCountAndNoOfGuest.TicketsCount;
            result["NoOfGuest"] = ticketsCountAndNoOfGuest.NoOfGuests;
            result["PerTicketSpend"] = totalNetAndGrossSale.GrossTotalSale
                / (/*(ticketsCountAndNoOfGuest.TicketsCount == 0) ? 1 :*/ ticketsCountAndNoOfGuest.TicketsCount);
            result["PerGuestSpend"] = totalNetAndGrossSale.GrossTotalSale
                / (/*(ticketsCountAndNoOfGuest.NoOfGuests == 0) ? 1 :*/ ticketsCountAndNoOfGuest.NoOfGuests);
            result["stockValue"] = (OpeningStockValue + PurchaseStockValue);


            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        
        [HttpPost]
        [Route("GetProductCagegoryWiseSalesCountInfo")]
        public async Task<IHttpActionResult> GetProductCagegoryWiseSalesCountInfo(ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if(parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0 )
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var result = await TicketManager.GetProductCagegoryWiseSalesCountInfo(
                parameter.from,
                parameter.to,
                parameter.outletIds,
                parameter.departmentIds
                );

            var finalResult = new Dictionary<string,dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

       
        [HttpPost]
        [Route("GetDepartmentWiseSalesInfo")]
        public async Task<IHttpActionResult> GetDepartmentWiseSalesInfo(ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var result = await TicketManager.GetDepartmentWiseSalesInfo(
                parameter.from,
                parameter.to,
                parameter.outletIds,
                parameter.departmentIds
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        
        [HttpPost]
        [Route("GetPaymentMethodWiseSalesInfo")]
        public async Task<IHttpActionResult> GetPaymentMethodWiseSalesInfo(ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var result = await TicketManager.GetPaymentMethodWiseSalesInfo(
                parameter.from,
                parameter.to,
                parameter.outletIds,
                parameter.departmentIds
                );


            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        
        //Get top 10 Product sales info
        [HttpPost]
        [Route("GetProductWiseSalesInfo")]
        public async Task<IHttpActionResult> GetProductWiseSalesInfo(ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var result = await TicketManager.GetProductWiseSalesInfo(
                parameter.from,
                parameter.to,
                parameter.outletIds,
                parameter.departmentIds
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        
        [HttpPost]
        [Route("GetDiscountInfo")]
        public async Task<IHttpActionResult> GetDiscountInfo(ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var result = await TicketManager.GetDiscountInfo(
                parameter.from,
                parameter.to,
                parameter.outletIds,
                parameter.departmentIds
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        //ok
        [HttpPost]
        [Route("GetDailySalesParameterized")]
        public async Task<IHttpActionResult> GetDailySalesParameterized(
            ParameterFromDateToDateOutletIdListDepartmentIdLisMenuItemIdListMenuItemCategoryNameList parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = 
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = 
                    outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = 
                    departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            if (parameter.menuItemIds.Count == 1 && parameter.menuItemIds[0] == 0)
            {
                var menuItems = await MenuItemManager.GetMenuItemNamesAndIds();
                parameter.menuItemIds = menuItems.Select(item => item.Id).Cast<int>().ToList();
            }//if

            if (parameter.menuItemCategoryNames.Count == 1 && parameter.menuItemCategoryNames[0] == string.Empty)
            {
                var menuItemCategories = await MenuItemManager.GetMenuItemCategoriesNamesAndIds();
                parameter.menuItemCategoryNames = menuItemCategories.Select(item => item.Name).Cast<string>().ToList();
            }//if

            var result = await ChartsManager.GetDailySalesParameterized(
                parameter.from, parameter.to, parameter.outletIds,
                parameter.departmentIds,parameter.menuItemIds,parameter.menuItemCategoryNames);

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func


        [HttpPost]
        [Route("GetSalesYearly")]
        public async Task<IHttpActionResult> GetSalesYearly([FromBody] 
        ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = 
                    outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId = 
                    departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var CurrentYear = DateTime.Now.Year;
            string Year = Convert.ToString(CurrentYear - (parameter.NumberOfYears-1));
            Year = "'" + Year + "-01-01 00:00:00.000'";
            var result = await ChartsManager.GetSalesYearly(parameter.outletIds,parameter.departmentIds, Year);
            var TotalYears = await ChartsManager.GetFastAndLastDateOfTickets();
           
            var TotalDataTime = TotalYears.FirstOrDefault();
            DateTime From = Convert.ToDateTime(TotalDataTime.StartDate);
            DateTime To = Convert.ToDateTime(TotalDataTime.EndDate);
            var TotalTime = 1+(To.Year - From.Year);

            var FastAndLastDate = await ChartsManager.GetlastSelectedYearDate(Year);

            var Date = FastAndLastDate.FirstOrDefault();
            DateTime StartDate = Convert.ToDateTime(Date.StartDate);
            DateTime EndDate = Convert.ToDateTime(Date.EndDate);

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = StartDate;
            finalResult["toDate"] = EndDate;
            finalResult["TotalYears"] = TotalTime;

            return Ok(finalResult);
        }//func

        //Lagecy
        [HttpPost]
        [Route("GetTop10SellingItems")]
        public IHttpActionResult GetTop10SellingItems([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            DataSet dates = TicketManager.GetStartAndEndDate(parameter.fromDate, parameter.toDate, false);
            DateTime startDate = Convert.ToDateTime(dates.Tables[0].Rows[0]["StartDate"]);
            DateTime endDate = Convert.ToDateTime(dates.Tables[0].Rows[0]["EndDate"]);

            var tickets = TicketManager.GetTickets(startDate, endDate, parameter.departmentId, parameter.outletId);
            var menuItemss = MenuItemManager.GetMenuItems(Convert.ToInt32(0), "All");
            var departmentid = parameter.departmentId;
            var menuItems = departmentid == 0 
                ? 
                ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss)
                .OrderBy(x => x.ID) 
                : 
                ThreeS.Modules.BasicReports.Reports.MenuGroupBuilder.CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, departmentid)
                .OrderBy(x => x.ID);

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
            //var jsonString = JsonConvert.SerializeObject(top10ItemsTable);
            return Ok(top10ItemsTable);
        }//func



        //Graph no. 1,2
        [HttpGet]
        [Route("GetdailySales/{id:int}")]
        public IHttpActionResult GetDailySales(int id) 
        {
            //id means outletId
            var dayWiseNetSalesDt = ChartsManager.GetLast12MonthSales(id).Tables[0];

            //[
            //  {
            //    "StartDate": "27 Jul 2022",
            //    "TicketTotalAmount": 50.0000,
            //    "Sales": 50.0000
            //  }
            //] 

            return Ok(dayWiseNetSalesDt);
        }//func

        //Graph no. 4
        [HttpGet]
        [Route("GetLast12MonthSalesAccountWise/{id:int}")]
        public IHttpActionResult GetLast12MonthSalesAccountWise(int id)
        {
            //id means outletId
            var monthWiseNetSalesDt = ChartsManager.GetLast12MonthSalesAccountWise(id).Tables[0];

            //[
            //    {
            //    "MonthYear": "November'2021",
            //        "UltimateAccount": "120Coupon",
            //        "Amount": 14880
            //    },
            //    {
            //    "MonthYear": "November'2021",
            //        "UltimateAccount": "Discount",
            //        "Amount": 22004.013
            //    },
            //    {
            //    "MonthYear": "November'2021",
            //        "UltimateAccount": "Food Panda 22% Discount",
            //        "Amount": 90238.0015
            //    }
            //]

            return Ok(monthWiseNetSalesDt);
        }//func

        //Graph no. 5
        [HttpGet]
        [Route("GetLast12MonthSalesPaymentWise/{id:int}")]
        public IHttpActionResult GetLast12MonthSalesPaymentWise(int id)
        {
            //id means outletId
            var monthWiseNetSalesDt = ChartsManager.GetLast12MonthSalesPaymentWise(id).Tables[0];

            //[
            //  {
            //    "MonthYear": "July'2022",
            //    "UltimateAccount": "Bkash",
            //    "Amount": 546.0000
            //  },
            //  {
            //    "MonthYear": "July'2022",
            //    "UltimateAccount": "Cash",
            //    "Amount": 3818.0000
            //  },
            //  {
            //    "MonthYear": "July'2022",
            //    "UltimateAccount": "Visa Card",
            //    "Amount": 220.0000
            //  }
            //]

            return Ok(monthWiseNetSalesDt);
        }//func

        [HttpPost]
        [Route("GetMonthlySalesForDashBoard")]
        public async Task<IHttpActionResult> GetMonthlySalesForDashBoard(
            ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            if (parameter.outletIds == null)
            {
                throw new Exception("outletIds can not be empty");
            }

            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId = 
                    outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            var monthWiseNetSalesDt = await ChartsManager.GetMonthlySalesForDashBoard(parameter.outletIds);

            return Ok(monthWiseNetSalesDt);
        }//func


        //Graph no. 6
        [HttpPost]
        [Route("GetMonthlyNetSales")]
        public async Task<IHttpActionResult> GetMonthlyNetSales(
            ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
        {
            if (parameter.outletIds == null)
            {
                throw new Exception("outletIds can not be empty");
            }

            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                DataSet outletsDataSet = TicketManager.GetOutlets();
                List<int> allOutletsId =
                    outletsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.outletIds = allOutletsId;
            }//if

            var monthWiseNetSalesDt = await ChartsManager.GetLast12MonthSales(parameter.outletIds);

            return Ok(monthWiseNetSalesDt);
        }//func


        [HttpGet]
        [Route("GetAllCategoriesAndMenuItems")]
        public async Task<IHttpActionResult> GetAllCategoriesAndMenuItems()
        {

            var allMenuItems = TicketManager.GetMenuItems();
            if (allMenuItems == null)
            {
                throw new FileNotFoundException("allMenuItems can not be empty");
            }
            List<MenuCategory> AllMenuCategories = new List<MenuCategory>();
            foreach (var menuItem in allMenuItems)
            {
                var menuCategory = AllMenuCategories.Find(x => x.GroupCode == menuItem.GroupCode);
                if (menuCategory == null)
                {
                    var MenuCategory = new MenuCategory();
                    MenuCategory.GroupCode = menuItem.GroupCode;

                    AllMenuCategories.Add(MenuCategory);
                    var Item = new MenuItems();
                    Item.Id = menuItem.Id;
                    Item.Name = menuItem.Name;
                    MenuCategory.Items.Add(Item);

                }
                else
                {
                    var Item = new MenuItems();
                    Item.Id = menuItem.Id;
                    Item.Name = menuItem.Name;
                    menuCategory.Items.Add(Item);

                }
            }
            return Ok(AllMenuCategories);
        }//func
            
    }//class

}//namespace