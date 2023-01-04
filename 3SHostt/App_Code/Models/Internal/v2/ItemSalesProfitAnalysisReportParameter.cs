//In the name of Allah

using System;

namespace ThreeS.Report.v2.Models
{
    public class ItemSalesProfitAnalysisReportParameter
    {
        public bool isExact;
        public DateTime from;
        public DateTime to;
        public int outletId;
        public int departmentId;
        public int menuItemId;
        public string groupCode;
    }//class

}//namespace