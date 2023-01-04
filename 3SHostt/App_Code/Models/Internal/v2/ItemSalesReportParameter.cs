//In the name of Allah

using System;
using System.Collections.Generic;

namespace ThreeS.Report.v2.Models
{
    public class ItemSalesReportParameter
    {
        public bool isExact;
        public DateTime from;
        public DateTime to;
        public int outletId;
        public int departmentId;
        public List<int> menuItemIds;
        public List<string> groupCodes;
    }//class

}//namespace