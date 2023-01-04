//In the name of Allah

using BusinessObjects.ChartsManager;
using BusinessObjects.TicketsManager;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThreeS.Report.v2.Models;
using System.Data;
using Telerik.Web.UI.com.hisoftware.api2;
using ThreeS.Domain.Models.Tickets;
using Elmah.Assertions;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/sales")]
    public class SalesController : ApiController
    {
        //Author Jewel Hossain
        [HttpPost]
        [Route("GetWorkPeriodReport")]
        public IHttpActionResult GetWorkPeriodReport(
            ParameterFromDateToDateOutletIdDepartmentIdList parameter)
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

            var result = TicketManager.GetWorkPeriodReport(
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

        [HttpPost]
        [Route("GetInvoiceWiseSales")]
        public async Task<IHttpActionResult> GetInvoiceWiseSales(ParameterFromDateToDateDepertmentIdOutletId parameter)
        {
            DateTime defaultDateTime = new DateTime();
            if (parameter.fromDate == defaultDateTime & parameter.toDate == defaultDateTime)
            {
                WorkPeriodDateTimes currentWorkPeriodDateTimes = DateTimeService.GenerateCurrentWorkPeriodWiseDateTime(DateTime.Now, DateTime.Now);
                parameter.fromDate = currentWorkPeriodDateTimes.from;
                parameter.toDate = currentWorkPeriodDateTimes.to;
            }
            else if (!parameter.isExact)
            {
                WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.fromDate, parameter.toDate);
                parameter.fromDate = workPeriodDateTimes.from;
                parameter.toDate = workPeriodDateTimes.to;
            }//if

            var Resutl = TicketManager.GetTickets(parameter.fromDate, parameter.toDate, parameter.departmentId, parameter.onlyOpenTickets, parameter.outletId);

            var finalResult = new Dictionary<string, dynamic>();

            finalResult["result"] = Resutl;
            finalResult["fromDate"] = parameter.fromDate;
            finalResult["toDate"] = parameter.toDate;

            return Ok(finalResult);
        }//func

        [HttpPost]
        [Route("GetTicketDetailsByTicketId")]
        public async Task<IHttpActionResult> GetTicketDetailsByTicketId(MenuItem parameter)
        {
            var TicketId = parameter.Id;
            try
            {
                DataSet dSet = new DataSet();
                DataTable ClintEvent = new DataTable();
                DataTable orderDataTable = null;
                DataTable ticketEntitiesDataTable = null;
                DataTable calculationsDataTable = null;
                DataTable paymentsDataTable = null;

                Ticket ticket = TicketManager.GetTicket(TicketId);

                var customerEntity = ticket.TicketEntities.FirstOrDefault(i => i.EntityTypeName.Contains("Customer"));
                  
                var eventEntity = ticket.TicketEntities.FirstOrDefault(i => i.EntityTypeName.Contains("Events"));

                var Colbody = new DataColumn("Client Name", typeof(String));
                ClintEvent.Columns.Add(Colbody);
                var Colbody1 = new DataColumn("Department", typeof(String));
                ClintEvent.Columns.Add(Colbody1);
                var Colbody2 = new DataColumn("Company", typeof(String));
                ClintEvent.Columns.Add(Colbody2);
                var Colbody3 = new DataColumn("Phone", typeof(String));
                ClintEvent.Columns.Add(Colbody3);
                var Colbody4 = new DataColumn("Address", typeof(String));
                ClintEvent.Columns.Add(Colbody4);
                var Colbody5 = new DataColumn("EventName", typeof(String));
                ClintEvent.Columns.Add(Colbody5);

                var Colbody6 = new DataColumn("TicketNo", typeof(String));
                ClintEvent.Columns.Add(Colbody6);
                var Colbody7 = new DataColumn("TicketDate", typeof(String));
                ClintEvent.Columns.Add(Colbody7);
                var Colbody8 = new DataColumn("DeliveryDate", typeof(String));
                ClintEvent.Columns.Add(Colbody8);
                var Colbody9 = new DataColumn("TotalTax", typeof(String));
                ClintEvent.Columns.Add(Colbody9);
                var Colbody10 = new DataColumn("TicketTotal", typeof(String));
                ClintEvent.Columns.Add(Colbody10);
                var Colbody11 = new DataColumn("RemainingAmount", typeof(String));
                ClintEvent.Columns.Add(Colbody11);


                DataRow Rowbody = ClintEvent.NewRow();
                Rowbody["Client Name"] = customerEntity == null ? string.Empty : customerEntity.EntityName;
                Rowbody["Department"] = customerEntity == null ? string.Empty : customerEntity.GetCustomDataFromEntity("Department");
                Rowbody["Company"] = customerEntity == null ? string.Empty : customerEntity.GetCustomData("Company");
                Rowbody["Phone"] = customerEntity == null ? string.Empty : customerEntity.GetCustomData("Phone");
                Rowbody["Address"] = customerEntity == null ? string.Empty : customerEntity.GetCustomData("Address");
                Rowbody["EventName"] = eventEntity == null ? string.Empty : eventEntity.EntityName;

                Rowbody["TicketNo"] = ticket.TicketNumber == null ? string.Empty : ticket.TicketNumber;
                Rowbody["TicketDate"] = ticket.Date == null ? string.Empty : ticket.Date.ToString("dd MMM yyyy");
                Rowbody["DeliveryDate"] = ticket.Date == null ? string.Empty : ticket.Date.ToString("dd MMM yyyy");
                Rowbody["TotalTax"] = ticket.TicketTaxValue == null ? string.Empty : Convert.ToString(ticket.TicketTaxValue);
                Rowbody["TicketTotal"] = ticket.TotalAmount == null ? string.Empty : Convert.ToString(ticket.TotalAmount);
                Rowbody["RemainingAmount"] = ticket.RemainingAmount == null ? string.Empty : Convert.ToString(ticket.RemainingAmount);

                ClintEvent.Rows.Add(Rowbody);
                
                dSet.Tables.Add(ClintEvent);

                orderDataTable = TicketManager.GetOrders(ticket);
                dSet.Tables.Add(orderDataTable);

                ticketEntitiesDataTable = TicketManager.GetTicketEntities(TicketId);
                dSet.Tables.Add(ticketEntitiesDataTable);

                calculationsDataTable = TicketManager.GetCalculations(ticket);
                dSet.Tables.Add(calculationsDataTable);

                paymentsDataTable = TicketManager.GetPayments(ticket);
                dSet.Tables.Add(paymentsDataTable);

                return Ok(dSet);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
       
        }//func
    }//class
}//namespace