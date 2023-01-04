using BusinessObjects.ChartsManager;
using BusinessObjects.TicketsManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ThreeS.Domain.Models.Tickets;
using ThreeS.Report.v2.Models;

//In the name of Allah

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/markdown")]
    public class MarkdownController : ApiController
    {
        //Jahin Hasan Chowdhury 
        [HttpPost]
        [Route("GetGiftDetailsReport")]
        public IHttpActionResult GetGiftDetailsReport(
              ParameterFromDateToDateOutletIdDepartmentIdList parameter)
        {
            bool currentWorkPeriod = false;
            DateTime defaultDateTime = new DateTime();
            if (parameter.from == defaultDateTime & parameter.to == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes =
                    DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.from = currentWorkPeriodDateTimes.from;
                parameter.to = currentWorkPeriodDateTimes.to;
                currentWorkPeriod = true;
            }

            DataSet dsStartEndDate = TicketManager.GetStartAndEndDate(parameter.from, parameter.to, currentWorkPeriod);

            DateTime dStartDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["StartDate"]);
            DateTime dEndDate = Convert.ToDateTime(dsStartEndDate.Tables[0].Rows[0]["EndDate"]);

            DataSet ds = TicketManager.VoidOrders(dStartDate, dEndDate, "Gift", 0, string.Empty);

            

            return Ok(ds);
        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetGiftDetailsMultiparameterReport")]
        public async Task<IHttpActionResult> GetGiftDetailsMultiparameterReport(
            GiftDetailsParameter parameter)
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


            var result = await TicketManager.GiftDetailsMultiparameterReport(parameter.from,
                parameter.to, parameter.outletIds, parameter.departmentIds);

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpPost]
        [Route("GetVoidDetailsMultiparameterReport")]
        public async Task<IHttpActionResult> GetVoidDetailsMultiparameterReport(
            VoidDetailsParameter parameter)
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


            var result = await TicketManager.VoidDetailsMultiparameterReport(parameter.from,
                parameter.to, parameter.outletIds, parameter.departmentIds);

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = result;
            finalResult["fromDate"] = parameter.from;
            finalResult["toDate"] = parameter.to;

            return Ok(finalResult);

        }//func

        //Author Jewel Hossain
        [HttpGet]
        [Route("GetVoidAnalysisReport")]
        public IHttpActionResult GetVoidAnalysisReport(int ticketId)
        {
            DataSet result = TicketManager.VoidAnalysisReport(ticketId);

            return Ok(result);

        }//func

    }//class
}//namespace