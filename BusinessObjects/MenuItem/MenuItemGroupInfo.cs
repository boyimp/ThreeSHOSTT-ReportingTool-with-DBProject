namespace ThreeS.Modules.BasicReports.Reports
{
    public class MenuItemGroupInfo
    {
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal QuantityRate { get; set; }
        public decimal GrossAmount { get; set; }
    }

    public class MenuGroupItemInfo
    {
        public int ItemId { get; set; }
        public string DepartmentName { get; set; }
        public string GroupName { get; set; }
        public string ItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Gross { get; set; }
        public decimal ProductionCostFixed { get; set; }
        public decimal TotalProductionCostFixed { get; set; }
        public decimal ProductionProfitFixed { get; set; } 
        public decimal ProductionCostRecipeWise { get; set; }
        public decimal TotalProductionCostRecipeWise { get; set; }
        public decimal ProductionProfitRecipeWise { get; set; }
        public decimal Deviation { get; set; }
    }
    public class ItemSalesReportInfo
    {
        public int ItemId { get; set; }
        public string DepartmentName { get; set; }
        public string GroupName { get; set; }
        public string ItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Gross { get; set; }
    }
    public class ItemProfitLossRecipeInfo
    {
        public int ItemId { get; set; }
        public string DepartmentName { get; set; }
        public string GroupName { get; set; }
        public string ItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Gross { get; set; }
        public decimal ProductionCostRecipeWise { get; set; }
        public decimal TotalProductionCostRecipeWise { get; set; }
        public decimal ProductionProfitRecipeWise { get; set; }
    }

    public class ItemProfitLossFixedInfo
    {
        public int ItemId { get; set; }
        public string DepartmentName { get; set; }
        public string GroupName { get; set; }
        public string ItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Gross { get; set; }
        public decimal ProductionCostFixed { get; set; }
        public decimal TotalProductionCostFixed { get; set; }
        public decimal ProductionProfitFixed { get; set; }
    }
    public class MenuMix2GroupItemInfo
    {
        public int ItemId { get; set; }
        public string DepartmentName { get; set; }
        public string GroupName { get; set; }
        public string ItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Gross { get; set; }
        public decimal ProductionCostFixed { get; set; }
        public decimal TotalProductionCostFixed { get; set; }
        public decimal ProductionProfitFixed { get; set; }
        public decimal ProductionCostRecipeWise { get; set; }
        public decimal TotalProductionCostRecipeWise { get; set; }
        public decimal ProductionProfitRecipeWise { get; set; }
        public decimal Deviation { get; set; }
        public decimal TotalCOSPercentage { get; set; }
        public decimal GrossProfitPercentage { get; set; }
    }
    public class WaiterMenuGroupItemInfo
    {
        public int ItemId { get; set; }
        public string Waiter { get; set; }
        public string GroupName { get; set; }
        public string ItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Gross { get; set; }
        //public decimal ProductionCost { get; set; }
        //public decimal TotalProductionCost { get; set; }
        //public decimal ProductionProfit { get; set; }
    }

}