//In the name of Allah

using System;
using System.Collections.Generic;

namespace ThreeS.Report.v2.Models
{
    public class ProductionCostParameter
    {
        public bool isExact;
        public DateTime from;
        public DateTime to;
        public List<string> inventoryGroupCodes;
        public List<string> brands;
    }//class

}//namespace