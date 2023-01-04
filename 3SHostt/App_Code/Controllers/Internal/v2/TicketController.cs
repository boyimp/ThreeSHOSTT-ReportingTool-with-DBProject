//In the name of Allah

using BusinessObjects.TicketsManager;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using ThreeS.Domain.Models.Tickets;
using ThreeS.Report.v2.Models;
using ThreeS.Report.v2.Utils;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/ticket")]
    public class TicketController : ApiController 
    {

        [HttpGet]
        [Route("GetDepartments")]
        public IHttpActionResult GetDepartments()
        {
            DataSet ds = TicketManager.GetDepartments();
            return Ok(ds.Tables[0]);
        }//func

        [HttpGet]
        [Route("GetPaymentType")]
        public IHttpActionResult GetPaymentType()
        {
            DataSet ds = TicketManager.GetPaymentType();
            return Ok(ds.Tables[0]);
        }//func

        [HttpGet]
        [Route("GetTicketType")]
        public IHttpActionResult GetTicketType()
        {
            DataSet ds = TicketManager.GetTicketType();
            return Ok(ds.Tables[0]);
        }//func

        [HttpGet]
        [Route("GetTaxTemplates")]
        public IHttpActionResult GetTaxTemplates()
        {
            DataSet ds = TicketManager.GetTaxTemplates();
            return Ok(ds.Tables[0]);
        }//func

        [HttpGet]
        [Route("GetCalculationTypes")]
        public IHttpActionResult GetCalculationTypes()
        {
            DataSet ds = TicketManager.GetCalculationTypes();
            return Ok(ds.Tables[0]);
        }//func

        [HttpGet]
        [Route("GetPaymentNames")]
        public IHttpActionResult GetPaymentNames()
        {
            List<string> paymentNames = TicketManager.GetPaymentNames();
            return Ok(paymentNames);
        }//func

        [HttpGet]
        [Route("GetDepartmentCollection")]
        public IHttpActionResult GetDepartmentCollection()
        {
            IEnumerable<Department> departmentCollections = TicketManager.GetDepartmentCollection();
            return Ok(departmentCollections);
        }//func

        [HttpGet]
        [Route("GetObjectCalculationTypes")]
        public IHttpActionResult GetObjectCalculationTypes() 
        {
            IEnumerable<CalculationType> objectCalculationTypes = TicketManager.GetObjectCalculationTypes();
            return Ok(objectCalculationTypes);
        }//func

        [HttpGet]
        [Route("GetMenuItems")]
        public IHttpActionResult GetMenuItems()
        {
            IEnumerable<MenuItem> menuItems = TicketManager.GetMenuItems();
            return Ok(menuItems);
        }//func

        //untested
        [HttpPost]
        [Route("GetSalesCalculationTable")]
        public IHttpActionResult GetSalesCalculationTable([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataTable salesCalculationTable = TicketManager.GetSalesCalculationTable(
                parameter.outletId.ToString(),
                parameter.departmentId.ToString(),
                workPeriodDateTimes.from,
                workPeriodDateTimes.to
                );
            return Ok(salesCalculationTable);
        }//func

        //untested
        [HttpPost]
        [Route("GetMenuGroupAccountDetails")]
        public IHttpActionResult GetMenuGroupAccountDetails([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet menuGroupAccountDetails = TicketManager.GetMenuGroupAccountDetails(
                parameter.outletId.ToString(), 
                parameter.departmentId.ToString(), 
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to
                );

            return Ok(menuGroupAccountDetails);
        }//func

        //untested
        [HttpPost]
        [Route("GetMenuItemsAccountDetails")]
        public IHttpActionResult GetMenuItemsAccountDetails([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet menuItemsAccountDetails = TicketManager.GetMenuItemsAccountDetails(
                parameter.outletId.ToString(), 
                parameter.departmentId.ToString(), 
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to
                );
            return Ok(menuItemsAccountDetails);
        }//func

        //untested
        [HttpPost]
        [Route("GetWorkperiodReprotRefunds")]
        public IHttpActionResult GetWorkperiodReprotRefunds([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataTable workperiodReprotRefunds = TicketManager.GetWorkperiodReprotRefunds(
                parameter.outletId.ToString(), 
                parameter.departmentId.ToString(), 
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to
                );

            return Ok(workperiodReprotRefunds);
        }//func

        
        //untested
        [HttpPost]
        [Route("GetSalesSummaryReport")]
        public IHttpActionResult GetSalesSummaryReport([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet salesSummaryReport = TicketManager.GetSalesSummaryReport(
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to,
                parameter.departmentId.ToString(),
                parameter.outletId
                );

            return Ok(salesSummaryReport);
        }//func

        //untested
        [HttpPost]
        [Route("GetSalesSummaryReport")]
        public IHttpActionResult GetSalesSummaryTaxReport([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet salesSummaryTaxReport = TicketManager.GetSalesSummaryTaxReport(
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to, 
                parameter.departmentId.ToString(), 
                parameter.outletId
                );

            return Ok(salesSummaryTaxReport);
        }//func

        //untested
        [HttpPost]
        [Route("GetItemSalesReportShort")]
        public IHttpActionResult GetItemSalesReportShort([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet itemSalesReportShort = TicketManager.GetItemSalesReportShort(
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to, 
                parameter.departmentId.ToString(), 
                parameter.outletId
                );

            return Ok(itemSalesReportShort);
        }//func

        //untested
        [HttpPost]
        [Route("GetAdvanceDue")]
        public IHttpActionResult GetAdvanceDue([FromBody] ParameterFromDateToDate parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet advanceDue = TicketManager.GetAdvanceDue(
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to
                );

            return Ok(advanceDue.Tables[0]);
        }//func

        //untested
        [HttpPost]
        [Route("GetWorkPeriodRange")]
        public IHttpActionResult GetWorkPeriodRange([FromBody] ParameterFromDateToDate parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet workPeriodRange = TicketManager.GetWorkPeriodRange(
                workPeriodDateTimes.from.ToString(GlobalData.DATE_TIEM_FORMATE), 
                workPeriodDateTimes.to.ToString(GlobalData.DATE_TIEM_FORMATE)
                );

            return Ok(workPeriodRange);
        }//func

        //untested
        [HttpPost]
        [Route("GetItemSalesAccountWise")]
        public IHttpActionResult GetItemSalesAccountWise([FromBody] ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet workperiodReprotRefunds = TicketManager.GetItemSalesAccountWise(
                workPeriodDateTimes.from.ToString(GlobalData.DATE_TIEM_FORMATE), 
                workPeriodDateTimes.to.ToString(GlobalData.DATE_TIEM_FORMATE),
                parameter.outletId, 
                parameter.departmentId
                );

            return Ok(workperiodReprotRefunds);
        }//func
        



    }//class
}//namespace