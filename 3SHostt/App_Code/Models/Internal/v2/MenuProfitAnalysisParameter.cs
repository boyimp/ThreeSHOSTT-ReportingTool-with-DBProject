//In the name of Allah

using System;
using System.Collections.Generic;

namespace ThreeS.Report.v2.Models
{
    public class MenuProfitAnalysisParameter
    {
        public bool isExact;
        public DateTime from;
        public DateTime to;
        public int outletId;
        public int departmentId;
        public int menuItemId;
        public string groupCode;
        public bool isFixedPrice;
    }//class

}//namespace