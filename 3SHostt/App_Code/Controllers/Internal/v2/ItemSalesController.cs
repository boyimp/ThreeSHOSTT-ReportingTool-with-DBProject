//In the name of Allah

using BusinessObjects.ChartsManager;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Web.Http;
using ThreeS.Report.v2.Models;
using System.Linq;
using BusinessObjects.TicketsManager;
using System.Data;
using ThreeS.Modules.BasicReports.Reports;
using BusinessObjects.MenusManager;
using BusinessObjects.InventoryManager;
using System.Globalization;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/itemSales")]
    public class ItemSalesController : ApiController
    {
        //Author Jewel Hossain
        [HttpGet]
        [Route("GetOpeningStockDetails")]
        public IHttpActionResult GetOpeningStockDetails(DateTime from)
        {
            var result = CurrentStockManager.GetOpeningStock(DateTimeService.GetTimeStamp(from));

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["on"] = from;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetColsingStockDetails")]
        public IHttpActionResult GetColsingStockDetails(DateTime to)
        {
            var result = CurrentStockManager.GetClosingStock(DateTimeService.GetTimeStamp(to));

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["on"] = to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetPurchaseStockDetails")]
        public IHttpActionResult GetPurchaseStockDetails(DateTime from,DateTime to)
        {
            var result = CurrentStockManager.GetPurchaseStock(
                DateTimeService.GetTimeStamp(from),
                DateTimeService.GetTimeStamp(to)
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = from;
            finalResult["toDate"] = to;

            return Ok(finalResult);
        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetInventoryItemGroupCodes")]
        public async Task<IHttpActionResult> GetInventoryItemGroupCodes()
        {
            var result = await MenuItemManager.GetInventoryItemGroupCodes();

            return Ok(result);

        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetBrands")]
        public async Task<IHttpActionResult> GetBrands()
        {
            var result = await MenuItemManager.GetBrands();

            return Ok(result);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetItemSalesReport")]
        public IHttpActionResult GetItemSalesReport(
            ItemSalesReportParameter parameter)
        {
            const int allMenuItemId = 0;
            string allGroupCode = string.Empty;
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            IList<ItemSalesReportInfo> result = TicketManager.GetItemSalesReport(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentId
                );

            if(parameter.menuItemIds[0] != allMenuItemId && parameter.groupCodes[0] == allGroupCode)
            { 
                result = result.Where((element) => parameter.menuItemIds.Contains(element.ItemId)).ToList();
            }//if
            else if(parameter.menuItemIds[0] == allMenuItemId && parameter.groupCodes[0] != allGroupCode)
            {
                result = result.Where((element) => parameter.groupCodes.Contains(element.GroupName)).ToList();
            }//else if
            else if(parameter.menuItemIds[0] != allMenuItemId && parameter.groupCodes[0] != allGroupCode)
            {
                result = result.Where((element) => parameter.groupCodes.Contains(element.GroupName)).ToList();
                result = result.Where((element) => parameter.menuItemIds.Contains(element.ItemId)).ToList();
            }//else if

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetItemSalesProfitLossRecipeReport")]
        public IHttpActionResult GetItemSalesProfitLossRecipeReport(
            ItemSalesProfitLossRecipeReportParameter parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            var result = TicketManager.ItemSalesProfitLossRecipeReport(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentId,
                parameter.menuItemId,
                parameter.groupCode
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetItemSalesProfitAnalysisReport")]
        public IHttpActionResult GetItemSalesProfitAnalysisReport(
            ItemSalesProfitAnalysisReportParameter parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            var result = TicketManager.ItemSalesProfitAnalysisReport(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentId,
                parameter.menuItemId,
                parameter.groupCode
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetProductionCostDrill")]
        public IHttpActionResult GetProductionCostDrill(ProductionCostDrillParameter parameter)
        {
            var result = MenuItemManager.ProductionCostDrill(parameter.menuItemId, parameter.portionName);
            return Ok(result);
        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetProductionCostReport")]
        public async Task<IHttpActionResult> GetProductionCostReport(ProductionCostParameter parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            if (parameter.inventoryGroupCodes.Count == 1 && parameter.inventoryGroupCodes[0] == string.Empty)
            {
                var groupCodes = await MenuItemManager.GetInventoryItemGroupCodes();
                parameter.inventoryGroupCodes = groupCodes.Select(item => item.GroupCode).Cast<string>().ToList();
            }//if

            if (parameter.brands.Count == 1 && parameter.brands[0] == string.Empty)
            {
                var brands = await MenuItemManager.GetBrands();
                parameter.brands = brands.Select(item => item.Brand).Cast<string>().ToList();
            }//if

            var result = MenuItemManager.ProductionCostReport(
                parameter.from,
                parameter.to,
                parameter.inventoryGroupCodes,
                parameter.brands
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetMenuProfiteAnalysisReport")]
        public IHttpActionResult GetMenuProfiteAnalysisReport(MenuProfitAnalysisParameter parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            var result = MenuItemManager.MenuProfitAnalysis(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentId,
                parameter.menuItemId,
                parameter.groupCode,
                parameter.isFixedPrice
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetMenuMixReport")]
        public IHttpActionResult GetMenuMixReport(MenuMixReportParameter parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            var result = MenuItemManager.MenuMixReport(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentId,
                parameter.menuItemId,
                parameter.groupCode
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetOutletWiseItemReport")]
        public IHttpActionResult GetOutletWiseItemReport(OutletWiseItemReportParameter parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            var result = MenuItemManager.OutletWiseItemReport(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentId
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetSalesSummaryReport")]
        public IHttpActionResult GetSalesSummaryReport(SalesSummaryReportParameter parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId =
                    departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var result = MenuItemManager.SalesSummaryReport(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentIds,
                parameter.isTax
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetOrderTagServiceReport")]
        public IHttpActionResult GetOrderTagServiceReport(ParameterFromDateToDateOutletIdDepartmentIdList parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
            }//if
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes =
                    DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.from, parameter.to);
                parameter.from = workPeriodDateTimes.from;
                parameter.to = workPeriodDateTimes.to;
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                DataSet departmentsDataSet = TicketManager.GetDepartments();
                List<int> allDepartmentsId =
                    departmentsDataSet.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<int>("Id")).ToList();
                parameter.departmentIds = allDepartmentsId;
            }//if

            var result = MenuItemManager.OrderTagServiceReport(
                parameter.from,
                parameter.to,
                parameter.outletId,
                parameter.departmentIds
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

    }//class
}//namespace