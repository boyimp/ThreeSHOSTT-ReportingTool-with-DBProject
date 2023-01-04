//In the name of Allah

using System;
using System.Collections.Generic;

namespace ThreeS.Report.v2.Models
{
    public class SalesSummaryReportParameter
    {
        public bool isExact;
        public DateTime from;
        public DateTime to;
        public int outletId;
        public List<int> departmentIds;
        public bool isTax;
    }//class

}//namespace