//In the name of Allah


using BusinessObjects.ChartsManager;
using BusinessObjects.InventoryManager;
using BusinessObjects.MenusManager;
using BusinessObjects.TicketsManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ThreeS.Domain.Models.Menus;
using ThreeS.Report.v2.Models;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/chart")]
    public class ChartController : ApiController
    {
        //Author Jewel Hossain
        [HttpGet]
        [Route("GetSyncOutletsWithDeletedSyncOutlets")]
        public async Task<IHttpActionResult> GetSyncOutletsWithDeletedSyncOutlets()
        {
            var result = await ChartsManager.GetSyncOutletsWithDeletedSyncOutlets();

            return Ok(result);

        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetDepartmentsWithDeletedDepartments")]
        public async Task<IHttpActionResult> GetDepartmentsWithDeletedDepartments()
        {
            var result = await ChartsManager.GetDepartmentsWithDeletedDepartments();

            return Ok(result);

        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetMenuItemsWithDeletedMenuItems")]
        public async Task<IHttpActionResult> GetMenuItemsWithDeletedMenuItems()
        {
            var result = await ChartsManager.GetMenuItemsWithDeletedMenuItems();

            return Ok(result);

        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetMenuItemGroupsWithDeletedMenuItemGroups")]
        public async Task<IHttpActionResult> GetMenuItemGroupsWithDeletedMenuItemGroups()
        {
            var result = await ChartsManager.GetMenuItemGroupsWithDeletedMenuItemGroups();

            return Ok(result);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetOutletWiseSalesInfo")]
        public async Task<IHttpActionResult> GetOutletWiseSalesInfo(
            ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
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

            var  result = await ChartsManager.GetOutletWiseSalesInfo( 
                parameter.from,parameter.to,parameter.outletIds,parameter.departmentIds );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetOutletWiseAndPaymentMethodWiseSalesInfo")]
        public async Task<IHttpActionResult> GetOutletWiseAndPaymentMethodWiseSalesInfo(
            ParameterFromDatetoDateOutletIdListDepartmentIdLis parameter)
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


            var result = await ChartsManager.GetOutletWiseAndPaymentMethodWiseSalesInfo(parameter.from,
                parameter.to,parameter.outletIds,parameter.departmentIds); 

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetTimeWiseSalesMultiParameterized")]
        public async Task<IHttpActionResult> GetTimeWiseSalesMultiParameterized(
            ParameterFromDateToDateOutletIdListDepartmentIdLisMenuItemIdListMenuItemCategoryNameList parameter)
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


            if (parameter.outletIds.Count == 1 && parameter.outletIds[0] == 0)
            {
                var allOutletsId = await ChartsManager.GetSyncOutletsWithDeletedSyncOutlets();
                parameter.outletIds = allOutletsId.Select(outlet => outlet.Id).Cast<int>().ToList();
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                var allDepartmentsId = await ChartsManager.GetDepartmentsWithDeletedDepartments();
                parameter.departmentIds = allDepartmentsId.Select(department => department.Id).Cast<int>().ToList();
            }//if

            if (parameter.menuItemIds.Count == 1 && parameter.menuItemIds[0] == 0)
            {
                var menuItems = await ChartsManager.GetMenuItemsWithDeletedMenuItems();
                parameter.menuItemIds = menuItems.Select(item => item.Id).Cast<int>().ToList();
            }//if

            if (parameter.menuItemCategoryNames.Count == 1 && parameter.menuItemCategoryNames[0] == string.Empty)
            {
                var menuItemCategories = await ChartsManager.GetMenuItemGroupsWithDeletedMenuItemGroups();
                parameter.menuItemCategoryNames = menuItemCategories.Select(item => item.GroupCode).Cast<string>().ToList();
            }//if

            var result = await ChartsManager.GetTimeWiseSalesMultiParameterized(
                parameter.from, 
                parameter.to, 
                parameter.outletIds,
                parameter.departmentIds,
                parameter.menuItemIds,
                parameter.menuItemCategoryNames
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetDayTimeWiseSalesMultiParameterized")]
        public async Task<IHttpActionResult> GetDayTimeWiseSalesMultiParameterized(ParameterFromDateToDateOutletIdListDepartmentIdLisMenuItemIdListMenuItemCategoryNameList parameter)
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
                var allOutletsId = await ChartsManager.GetSyncOutletsWithDeletedSyncOutlets();
                parameter.outletIds = allOutletsId.Select(outlet => outlet.Id).Cast<int>().ToList();
            }//if

            if (parameter.departmentIds.Count == 1 && parameter.departmentIds[0] == 0)
            {
                var allDepartmentsId = await ChartsManager.GetDepartmentsWithDeletedDepartments();
                parameter.departmentIds = allDepartmentsId.Select(department => department.Id).Cast<int>().ToList();
            }//if

            if (parameter.menuItemIds.Count == 1 && parameter.menuItemIds[0] == 0)
            {
                var menuItems = await ChartsManager.GetMenuItemsWithDeletedMenuItems();
                parameter.menuItemIds = menuItems.Select(item => item.Id).Cast<int>().ToList();
            }//if

            if (parameter.menuItemCategoryNames.Count == 1 && parameter.menuItemCategoryNames[0] == string.Empty)
            {
                var menuItemCategories = await ChartsManager.GetMenuItemGroupsWithDeletedMenuItemGroups();
                parameter.menuItemCategoryNames = menuItemCategories.Select(item => item.GroupCode).Cast<string>().ToList();
            }//if

            var result = await ChartsManager.GetDayTimeWiseSalesMultiParameterized(
                parameter.from,
                parameter.to,
                parameter.outletIds,
                parameter.departmentIds,
                parameter.menuItemIds,
                parameter.menuItemCategoryNames
                );

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);
        }//func

    }//class
}//namespace

