using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeS.Modules.BasicReports.Reports
{
    public class MenuItemSellInfo
    {
        //public string GroupName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }

    public class MenuItemSpentTimeSellInfo
    {
        public string GroupName { get; set; }
        public int ID { get; set; }        
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountWithOrderTag { get; set; }
        public decimal TimeSpent { get; set; }
        //Added
        public string Portion { get; set; }
        public int DepartmentID { get; set; }        

    }

    public class WaiterMenuItemSpentTimeSellInfo
    {
        public string Waiter { get; set; }
        public string GroupName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }       
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal TimeSpent { get; set; }
        //Added
        public string Portion { get; set; }
        public int DepartmentID { get; set; }

    }

    public class DepartmentWiseSellInfo
    {
        //public string GroupName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
    
}

