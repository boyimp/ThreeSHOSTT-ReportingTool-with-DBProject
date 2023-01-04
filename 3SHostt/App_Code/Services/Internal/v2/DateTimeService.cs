//In the name of Allah

using BusinessObjects.TicketsManager;
using System;
using System.Data;
using ThreeS.Report.v2.Models;

namespace ThreeS.Report.v2.Controllers
{ 
    public class DateTimeService
    {
        public static WorkPeriodDateTimes GenerateWorkPeriodWiseDateTime(DateTime from,DateTime to)
        {
            WorkPeriodDateTimes workPeriodDateTimes = new WorkPeriodDateTimes();
            DataSet dates = TicketManager.GetStartAndEndDate(from, to , false);
            workPeriodDateTimes.from = Convert.ToDateTime(dates.Tables[0].Rows[0]["StartDate"]);
            workPeriodDateTimes.to = Convert.ToDateTime(dates.Tables[0].Rows[0]["EndDate"]);
            DateTime maxDateTime = new DateTime(3030, 09, 18, 00, 00, 00);
            
            if(workPeriodDateTimes.from.Year == maxDateTime.Year || workPeriodDateTimes.to.Year == maxDateTime.Year)
            {
                if(from.Date == to.Date)
                {
                    workPeriodDateTimes.to = from.Date.AddHours(23).AddMinutes(59);
                }
                else
                {
                    workPeriodDateTimes.to = to.Date.AddHours(23).AddMinutes(59);
                }
                workPeriodDateTimes.from = from.Date;
            }
            return workPeriodDateTimes;
        }//func

        public static WorkPeriodDateTimes GenerateCurrentWorkPeriodWiseDateTime(DateTime form, DateTime to)
        {
            WorkPeriodDateTimes workPeriodDateTimes = new WorkPeriodDateTimes();
            
            DataSet dates = TicketManager.GetStartAndEndDate(form, to, true);
            workPeriodDateTimes.from = Convert.ToDateTime(dates.Tables[0].Rows[0]["StartDate"]);
            workPeriodDateTimes.to = Convert.ToDateTime(dates.Tables[0].Rows[0]["EndDate"]);

            return workPeriodDateTimes;
        }//func

        public static string GetTimeStamp(DateTime date) => date.ToString("yyyy-MM-dd HH:mm:ss");
    }//class
}//namespace