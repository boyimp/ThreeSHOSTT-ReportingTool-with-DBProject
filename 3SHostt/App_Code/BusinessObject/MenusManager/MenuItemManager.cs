using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using ThreeS.Report.v2.Controllers;
using Dapper;
using BusinessObjects.InventoryManager;
using System.Globalization;
using ThreeS.Domain.Models.Menus;
using ThreeS.Modules.BasicReports.Reports;

namespace BusinessObjects.MenusManager
{
    public class MenuItemManager 
    {
        //Author Jewel Hossain
        public static dynamic OrderTagServiceReport(
            DateTime from,
            DateTime to,
            int outletId,
            List<int> departmentIds
            )
        {

            string departments = StringService.DepartmentIdsToString(departmentIds);

            DataSet dsOrderTags = new DataSet();
            DataTable dtOrderTags = new DataTable("OrderTags");
            dtOrderTags.Columns.AddRange(new DataColumn[] { new DataColumn("GroupCode"), new DataColumn("MenuItemName"), new DataColumn("Quantity", typeof(decimal)), new DataColumn("Total", typeof(decimal)) });
            List<ThreeS.Domain.Models.Tickets.Ticket> oTickets = TicketsManager.TicketManager.GetTicketsFaster(outletId, departments, from, to);
            List<ThreeS.Domain.Models.Tickets.Ticket> oValidTickets = oTickets.Where(x => x.TotalAmount >= 0).ToList();
            //var orderTags = TicketManager.GetOrderTagService(oValidTickets);
            var orderTags = oValidTickets
                   .SelectMany(x => x.Orders.Where(y => !string.IsNullOrEmpty(y.OrderTags)))
                   .SelectMany(x => x.GetOrderTagValues(y => y.MenuItemId == 0).Select(y => new { TagName = y.TagValue, x.MenuItemName, x.Quantity, x.Value }))
                   .GroupBy(x => new { x.TagName, x.MenuItemName })
                   .Select(x => new { x.Key.TagName, x.Key.MenuItemName, Quantity = x.Sum(y => y.Quantity), Value = x.Sum(y => y.Value) }).ToList();

            foreach (var item in orderTags)
            {
                dtOrderTags.Rows.Add(new object[] { item.TagName.ToString(), item.MenuItemName, item.Quantity.ToString("N2"), item.Value.ToString("N2") });
            }

            dsOrderTags.Tables.Add(dtOrderTags.Copy());

            return dsOrderTags;
        }//func

        //Author Jewel Hossain
        public static dynamic SalesSummaryReport(
            DateTime from,
            DateTime to,
            int outletId,
            List<int> departmentIds,
            bool isTax
            )
        {
            string departments = StringService.DepartmentIdsToString( departmentIds );
            if (isTax)
            {
                return TicketsManager.TicketManager.GetSalesSummaryTaxReport(from, to, departments, outletId);
            }//if
            else
            {
                return TicketsManager.TicketManager.GetSalesSummaryReport(from, to, departments, outletId);
            }//else
        }//func

        //Author Jewel Hossain
        public static dynamic OutletWiseItemReport(
            DateTime from,
            DateTime to,
            int outletId,
            int departmentId
            )
        {
            DataSet ds = GetMenuItemOutletWise(from, to, departmentId.ToString(), outletId);
            DataSet dsCalculations = GetCalculationsOutLetWise(from, to);
            DataSet dsTotal = GetTotalOutletWise(from, to);

            var result = new Dictionary<string, dynamic>();
            result["ItemSalesOutletWise"] = ds.Tables[0];
            result["ItemCalculationsOutletWise"] = dsCalculations.Tables[0];
            result["ItemTotalOutletWise"] = dsTotal.Tables[0];

            return result;
        }//func

        //Author Jewel Hossain
        public static dynamic MenuMixReport(
            DateTime from,
            DateTime to,
            int outletId,
            int departmentId,
            int menuItem,
            string groupCode
            )
        {
            if (groupCode == string.Empty)
            {
                groupCode = "All";
            }//if

            List<ThreeS.Domain.Models.Tickets.Ticket> tickets = TicketsManager.TicketManager.GetTicketsFaster(outletId, departmentId == 0 ? string.Empty : departmentId.ToString(), from, to);
            List<ThreeS.Domain.Models.Menus.MenuItem> menuItemss = GetMenuItemsFaster(menuItem, groupCode);

            var menuItems = departmentId == 0
                ? ThreeS.Modules.BasicReports.Reports.
                MenuGroupBuilder.CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss)
                .OrderBy(x => x.ID)
            : ThreeS.Modules.BasicReports.Reports
                .MenuGroupBuilder.CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, departmentId)
                .OrderBy(x => x.ID);

            IList<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo> Products = 
                new List<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo>();
            foreach (var menuItemInfo in menuItems)
            {
                var s = menuItemss.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });

                DataTable dtProductionCostRecipe = ProductionCostRecipe(menuItemInfo.ID, menuItemInfo.Portion);
                decimal PcostR = dtProductionCostRecipe.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostRecipe.Rows[0]["ProductionCost"]) : 0;
                decimal TPcostR = PcostR * menuItemInfo.Quantity;
                decimal PprofitR = menuItemInfo.Amount - TPcostR;
                DataTable dtProductionCostFixed = ProductionCostFixed(menuItemInfo.ID, menuItemInfo.Portion);
                decimal PcostF = dtProductionCostFixed.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
                decimal TPcostF = PcostF * menuItemInfo.Quantity;
                decimal PprofitF = menuItemInfo.Amount - TPcostF;

                DataSet ds = TicketsManager.TicketManager.GetDepartments();
                DataRow drow = ds.Tables[0].NewRow();
                drow["Id"] = "0";
                drow["Name"] = "All";
                ds.Tables[0].Rows.InsertAt(drow, 0);
                DataRow[] rows = ds.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));

                MenuGroupItemInfo q = new MenuGroupItemInfo
                {
                    ItemId = menuItemInfo.ID,
                    DepartmentName = rows[0]["Name"].ToString(),
                    GroupName = s.First().GroupCode,
                    ItemName = s.First().Name,
                    PortionName = menuItemInfo.Portion,
                    Price = Math.Round(menuItemInfo.Price, 2),
                    Quantity = Math.Round(menuItemInfo.Quantity, 2),
                    NetAmount = Math.Round(menuItemInfo.AmountWithOrderTag, 2),
                    Gross = Math.Round(menuItemInfo.Price * menuItemInfo.Quantity, 2),
                    ProductionCostFixed = Math.Round(PcostF, 2),
                    TotalProductionCostFixed = Math.Round(TPcostF, 2),
                    ProductionProfitFixed = Math.Round(PprofitF, 2),
                    ProductionCostRecipeWise = Math.Round(PcostR, 2),
                    TotalProductionCostRecipeWise = Math.Round(TPcostR, 2),
                    ProductionProfitRecipeWise = Math.Round(PprofitR, 2),
                    Deviation = Math.Round(PprofitF - PprofitR, 2)
                };
                Products.Add(q);
            }//foreach

            decimal totalQuantity = Products.Sum(product => product.Quantity);
            decimal totalNetAmount = Products.Sum(product => product.NetAmount);

            var result = new List<Dictionary<string,dynamic>>(); 
            
            foreach (var product in Products)
            {
                var element = new Dictionary<string, dynamic>();

                decimal quantityRation = product.Quantity / (totalQuantity / 100);
                decimal netAmountRation = product.NetAmount / (totalNetAmount / 100);

                element["department"] = product.DepartmentName;
                element["group"] = product.GroupName;
                element["item"] = product.ItemName;
                element["portion"] = product.PortionName;
                element["quantity"] = product.Quantity;
                element["quantity%"] = decimal.Round(quantityRation, 2, MidpointRounding.AwayFromZero);
                element["netAmount"] = product.NetAmount;
                element["netAmount%"] = decimal.Round(netAmountRation, 2, MidpointRounding.AwayFromZero);

                result.Add(element);
            }//foreach

            return result;
        }//func

        //Author Jewel Hossain
        public static dynamic MenuProfitAnalysis(
            DateTime from, 
            DateTime to, 
            int outletId,
            int departmentId, 
            int menuItem,
            string groupCode,
            bool isFixedPrice
            )
        {
            if (groupCode == string.Empty)
            {
                groupCode = "All";
            }//if

            List<ThreeS.Domain.Models.Tickets.Ticket> tickets = TicketsManager.TicketManager.GetTickets(from, to, departmentId, outletId);
            List<MenuItem> menuItemss = GetMenuItems(menuItem, groupCode);

            var menuItems = departmentId == 0 ?
                MenuGroupBuilder
                .CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss)
                .OrderBy(x => x.ID) :
                MenuGroupBuilder
                .CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, departmentId)
                .OrderBy(x => x.ID);

            IList<MenuMix2GroupItemInfo> Products = 
                new List<MenuMix2GroupItemInfo>();

            foreach (var menuItemInfo in menuItems)
            {
                var s = menuItemss.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });

                decimal dGross = Math.Round(menuItemInfo.Price * menuItemInfo.Quantity, 2);
                DataTable dtProductionCostRecipe = ProductionCostRecipe(menuItemInfo.ID, menuItemInfo.Portion);
                decimal dPcostR = dtProductionCostRecipe.Rows.Count > 0 ? 
                    Convert.ToDecimal(dtProductionCostRecipe.Rows[0]["ProductionCost"]) : 0;
                decimal dTPcostR = dPcostR * menuItemInfo.Quantity;
                decimal dPprofitR = dGross - dTPcostR;
                DataTable dtProductionCostFixed = ProductionCostFixed(menuItemInfo.ID, menuItemInfo.Portion);
                decimal dPcostF = dtProductionCostFixed.Rows.Count > 0 ? 
                    Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
                decimal dTPcostF = dPcostF * menuItemInfo.Quantity;
                decimal dPprofitF = menuItemInfo.Amount - dTPcostF;

                DataSet ds = TicketsManager.TicketManager.GetDepartments();
                DataRow drow = ds.Tables[0].NewRow();
                drow["Id"] = "0";
                drow["Name"] = "All";
                ds.Tables[0].Rows.InsertAt(drow, 0);
                DataRow[] rows = ds.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));

                if (isFixedPrice)
                {
                    MenuMix2GroupItemInfo q = new MenuMix2GroupItemInfo
                    {
                        ItemId = menuItemInfo.ID,
                        DepartmentName = rows[0]["Name"].ToString(),
                        GroupName = s.First().GroupCode,
                        ItemName = s.First().Name,
                        PortionName = menuItemInfo.Portion,
                        Price = Math.Round(menuItemInfo.Price, 2),
                        Quantity = Math.Round(menuItemInfo.Quantity, 2),
                        NetAmount = Math.Round(menuItemInfo.Amount, 2),
                        Gross = dGross,
                        ProductionCostFixed = Math.Round(dPcostF, 2),
                        TotalProductionCostFixed = Math.Round(dTPcostF, 2),
                        ProductionProfitFixed = Math.Round(dPprofitF, 2),
                        ProductionCostRecipeWise = Math.Round(dPcostR, 2),
                        TotalProductionCostRecipeWise = Math.Round(dTPcostR, 2),
                        ProductionProfitRecipeWise = Math.Round(dPprofitR, 2),
                        Deviation = Math.Round(dPprofitF - dPprofitR, 2),
                        TotalCOSPercentage = dGross == 0 ? 0 : Math.Round((dTPcostF * 100) / dGross, 2),
                        GrossProfitPercentage = dGross == 0 ? 0 : Math.Round((dPprofitF / dGross) * 100, 2)
                    };
                    Products.Add(q);
                }//if
                else//Recipe price wise
                {
                    MenuMix2GroupItemInfo q = new MenuMix2GroupItemInfo
                    {
                        ItemId = menuItemInfo.ID,
                        DepartmentName = rows[0]["Name"].ToString(),
                        GroupName = s.First().GroupCode,
                        ItemName = s.First().Name,
                        PortionName = menuItemInfo.Portion,
                        Price = Math.Round(menuItemInfo.Price, 2),
                        Quantity = Math.Round(menuItemInfo.Quantity, 2),
                        NetAmount = Math.Round(menuItemInfo.Amount, 2),
                        Gross = dGross,
                        ProductionCostFixed = Math.Round(dPcostF, 2),
                        TotalProductionCostFixed = Math.Round(dTPcostF, 2),
                        ProductionProfitFixed = Math.Round(dPprofitF, 2),
                        ProductionCostRecipeWise = Math.Round(dPcostR, 2),
                        TotalProductionCostRecipeWise = Math.Round(dTPcostR, 2),
                        ProductionProfitRecipeWise = Math.Round(dPprofitR, 2),
                        Deviation = Math.Round(dPprofitF - dPprofitR, 2),
                        TotalCOSPercentage = dGross == 0 ? 0 : Math.Round((dTPcostR * 100) / dGross, 2),
                        GrossProfitPercentage = dGross == 0 ? 0 : Math.Round((dPprofitR / dGross) * 100, 2)
                    };
                    Products.Add(q);
                }//else
            }//foreach

            //new code
            var result = new List<Dictionary<string, dynamic>>();
            decimal totalPrice = Products.Sum(product => product.Gross);

            foreach (var product in Products)
            {
                var element = new Dictionary<string, dynamic>();

                decimal totalPriceRatio = product.Gross / (totalPrice / 100);

                if (isFixedPrice)
                {
                    element["ItemId"] = product.ItemId;
                    element["ItemName"] = product.ItemName;
                    element["DepartmentName"] = product.DepartmentName;
                    element["GroupName"] = product.GroupName;
                    element["PortionName"] = product.PortionName;
                    element["Price"] = product.Price;
                    element["Quantity"] = product.Quantity;
                    element["TotalPrice"] = product.Gross;
                    element["ItemSales%"] = decimal.Round(totalPriceRatio, 2, MidpointRounding.AwayFromZero);
                    element["ProductionCostFixed"] = product.ProductionCostFixed;
                    element["COS%"] = product.TotalCOSPercentage;
                    element["TotalProductionCostFixed"] = product.TotalProductionCostFixed;
                    element["TotalCOS%"] = product.TotalCOSPercentage;
                    element["ProductionProfitFixed"] = product.ProductionProfitFixed;
                    element["GrossProfit%"] = product.GrossProfitPercentage;

                    result.Add(element);
                }//if
                else
                {
                    element["ItemId"] = product.ItemId;
                    element["ItemName"] = product.ItemName;
                    element["DepartmentName"] = product.DepartmentName;
                    element["GroupName"] = product.GroupName;
                    element["PortionName"] = product.PortionName;
                    element["Price"] = product.Price;
                    element["Quantity"] = product.Quantity;
                    element["TotalPrice"] = product.Gross;
                    element["ItemSales%"] = decimal.Round(totalPriceRatio, 2, MidpointRounding.AwayFromZero);
                    element["ProductionCostRecipeWise"] = product.ProductionCostRecipeWise;
                    element["COS%"] = product.TotalCOSPercentage;
                    element["TotalProductionCostRecipeWise"] = product.TotalProductionCostRecipeWise;
                    element["TotalCOS%"] = product.TotalCOSPercentage;
                    element["ProductionProfitRecipeWise"] = product.ProductionProfitRecipeWise;
                    element["GrossProfit%"] = product.GrossProfitPercentage;

                    result.Add(element);
                }//else
                
            }//foreach

            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetInventoryItemGroupCodes()
        {

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT DISTINCT [InventoryItems].[GroupCode] FROM [InventoryItems] WHERE [InventoryItems].[GroupCode] IS NOT NULL";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetBrands()
        {

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT Distinct [InventoryItems].[STARBrand] AS [Brand] FROM [InventoryItems]";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public static DataTable ProductionCostReport(DateTime from,DateTime to,List<string> groupCodes,List<string> brands)
        {
            string groupCodesAsParameter = StringService.MenuItemCategorisToString(groupCodes);
            string brandsAsParameter = StringService.BrandsToString(brands);
            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            DataSet ds = CurrentStockManager.GetOpeningClosingPurchaseStockValue(fromTimeStamp, toTimeStamp, groupCodesAsParameter, brandsAsParameter);

            double OpeningStockValue = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["TotalValueOfStock"]);
            double PurchaseStockValue = string.IsNullOrEmpty(ds.Tables[1].Rows[0]["TotalPrice"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[1].Rows[0]["TotalPrice"]);
            double ClosingStockValue = string.IsNullOrEmpty(ds.Tables[2].Rows[0]["TotalValueOfStock"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[2].Rows[0]["TotalValueOfStock"]);
            double CostOfProducton = (OpeningStockValue + PurchaseStockValue) - ClosingStockValue;

            DataTable table = new DataTable();
            table.Columns.AddRange(new DataColumn[] { new DataColumn("ID"), new DataColumn("Description"), new DataColumn("Value") });
            table.Rows.Add(new object[] { 0, "Opening Stock on " + (Convert.ToDateTime(from)).ToString("dd MMM yyyy"), OpeningStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
            table.Rows.Add(new object[] { 1, "Purchase from " + (Convert.ToDateTime(from)).ToString("dd MMM yyyy") + " To " + (Convert.ToDateTime(to)).ToString("dd MMM yyyy"), PurchaseStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
            table.Rows.Add(new object[] { 2, "Closing Stock on " + (Convert.ToDateTime(to)).ToString("dd MMM yyyy"), ClosingStockValue.ToString("#,#", CultureInfo.InvariantCulture) });
            table.Rows.Add(new object[] { 3, "Cost of Production ", CostOfProducton.ToString("#,#", CultureInfo.InvariantCulture) });
            return table;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetMenuItemNamesAndIds()
        {

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT DISTINCT [Id],[Name] FROM [MenuItems]";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetMenuItemCategoriesNamesAndIds()
        {

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT DISTINCT [Id],[Name] FROM [ScreenMenuCategories];";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        public static List<MenuItem> GetMenuItems()
        {
            DataSet ds = new DataSet();
            List<MenuItem> objects = new List<MenuItem>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT  * FROM MenuItems");
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();


                MenuItem obj = null;

                while (reader.Read())
                {
                    obj = new MenuItem();
                    obj.Id = (int)reader["Id"];
                    obj.GroupCode = (reader["GroupCode"] == DBNull.Value) ? String.Empty : (string)reader["GroupCode"];
                    obj.Barcode = (reader["Barcode"] == DBNull.Value) ? String.Empty : (string)reader["Barcode"];
                    obj.Tag = (reader["Tag"] == DBNull.Value) ? String.Empty : (string)reader["Tag"];
                    obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];

                    objects.Add(obj);
                }
                reader.Close();

                objects = GetMenuItemPortion(dbConn, objects);

                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return objects;
        }
        public static List<MenuItem> GetMenuItems(int sMenuItemId, string sGroupCode)
        {
            List<MenuItem> objects = new List<MenuItem>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                string sClause = string.Empty;
                if (sGroupCode !="All")
                {
                    sClause = "and groupcode = '"+sGroupCode+"'";
                }

                if (sMenuItemId != 0)
                {
                    sClause = "and id =" + sMenuItemId;
                }

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                
                string sqlString = string.Format(@"SELECT  * FROM MenuItems m 
                                                   where m.id=m.id {0}", sClause);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();


                MenuItem obj = null;

                while (reader.Read())
                {
                    obj = new MenuItem();
                    obj.Id = (int)reader["Id"];
                    obj.GroupCode = (reader["GroupCode"] == DBNull.Value) ? String.Empty : (string)reader["GroupCode"];
                    obj.Barcode = (reader["Barcode"] == DBNull.Value) ? String.Empty : (string)reader["Barcode"];
                    obj.Tag = (reader["Tag"] == DBNull.Value) ? String.Empty : (string)reader["Tag"];
                    obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];

                    objects.Add(obj);
                }
                reader.Close();

                objects = GetMenuItemPortion(dbConn, objects);

                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return objects;
        }

        public static DataSet GetScreenMenuCategories()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT  * FROM ScreenMenuCategories");
                SqlDataAdapter da = new SqlDataAdapter(sqlString, dbConn);
                da.Fill(ds);
                dbConn.Close();

             
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ds;
        }
        private static List<MenuItem> GetMenuItemPortion(SqlConnection dbConn, List<MenuItem> menuItems)
        {
            List<MenuItemPortion> objects;

            foreach (MenuItem menuItem in menuItems)
            {
                string sqlString = string.Format(@"select * from MenuItemPortions where MenuItemId={0}", menuItem.Id);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();

                objects = new List<MenuItemPortion>();
                MenuItemPortion obj = null;
                while (reader.Read())
                { 
                    obj = new MenuItemPortion();
                    obj.Id = (int)reader["Id"];
                    obj.MenuItemId = (int)(reader["MenuItemId"]);
                    obj.Multiplier = (int)(reader["Multiplier"]);
                    obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];

                    objects.Add(obj);
                }

                menuItem.Portions = objects;
                reader.Close();
                GetMenuItemprice(dbConn, objects);
            }

            return menuItems;
        }
        private static List<MenuItemPortion> GetMenuItemprice(SqlConnection dbConn, List<MenuItemPortion> menuItemPortions)
        {
            List<MenuItemPrice> objects;

            foreach (MenuItemPortion menuItemportion in menuItemPortions)
            {
                string sqlString = string.Format(@"select * from MenuItemPrices where MenuItemPortionId={0}", menuItemportion.Id);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();

                objects = new List<MenuItemPrice>();
                MenuItemPrice obj = null;
                while (reader.Read())
                {
                    obj = new MenuItemPrice();
                    obj.Id = (int)reader["Id"];
                    obj.MenuItemPortionId = (int)(reader["MenuItemPortionId"]);
                    obj.Price = (decimal)(reader["Price"]);
                    obj.PriceTag = (reader["PriceTag"] == DBNull.Value) ? String.Empty : (string)reader["PriceTag"];
                    objects.Add(obj);
                }

                menuItemportion.Prices = objects;
                reader.Close();

            }

            return menuItemPortions;
        }
        public static List<MenuItem> GetMenuItemsFaster(int sMenuItemId, string sGroupCode)
        {
            List<MenuItem> objects = new List<MenuItem>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                string sClause = string.Empty;
                if (sGroupCode != "All")
                {
                    sClause = "and groupcode = '" + sGroupCode + "'";
                }

                if (sMenuItemId != 0)
                {
                    sClause = "and id =" + sMenuItemId;
                }

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT  * FROM MenuItems m 
                                                   where m.id=m.id {0} Order By PositionValue", sClause);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();


                MenuItem obj = null;

                while (reader.Read())
                {
                    obj = new MenuItem();
                    obj.Id = (int)reader["Id"];
                    obj.GroupCode = (reader["GroupCode"] == DBNull.Value) ? String.Empty : (string)reader["GroupCode"];
                    obj.Barcode = (reader["Barcode"] == DBNull.Value) ? String.Empty : (string)reader["Barcode"];
                    obj.Tag = (reader["Tag"] == DBNull.Value) ? String.Empty : (string)reader["Tag"];
                    obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];

                    objects.Add(obj);
                }
                reader.Close();

                objects = GetAllMenuItemPortions(dbConn, objects);

                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return objects;
        }
        private static List<MenuItem> GetAllMenuItemPortions(SqlConnection dbConn, List<MenuItem> menuItems)
        {
            List<MenuItemPortion> objects;
            string menuItemIds = string.Join(",", menuItems.Select(x => x.Id));
            string sqlString = string.Format(@"select * from MenuItemPortions where MenuItemId in ({0})", menuItemIds);
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            SqlDataReader reader = dbCommand.ExecuteReader();

            objects = new List<MenuItemPortion>();
            MenuItemPortion obj = null;
            while (reader.Read())
            {
                obj = new MenuItemPortion();
                obj.Id = (int)reader["Id"];
                obj.MenuItemId = (int)(reader["MenuItemId"]);
                obj.Multiplier = (int)(reader["Multiplier"]);
                obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];
                objects.Add(obj);
            }
           
            reader.Close();
            List<MenuItemPortion> menuItemPortions = GetAllMenuItemPortionsWithPrices(dbConn, objects);
            foreach (MenuItemPortion menuItemPortion in menuItemPortions)
            {
                menuItems.Where(x => x.Id == menuItemPortion.MenuItemId).ToList().ForEach(i => i.Portions.Add(menuItemPortion));
            }
            return menuItems;
        }
        private static List<MenuItemPortion> GetAllMenuItemPortionsWithPrices(SqlConnection dbConn, List<MenuItemPortion> menuItemPortions)
        {
            List<MenuItemPrice> objects;
            string menuItemPortionIds = string.Join(",", menuItemPortions.Select(x => x.Id));
            string sqlString = string.Format(@"select * from MenuItemPrices where MenuItemPortionId in ({0})", menuItemPortionIds);
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            SqlDataReader reader = dbCommand.ExecuteReader();

            objects = new List<MenuItemPrice>();
            MenuItemPrice obj = null;
            while (reader.Read())
            {
                obj = new MenuItemPrice();
                obj.Id = (int)reader["Id"];
                obj.MenuItemPortionId = (int)(reader["MenuItemPortionId"]);
                obj.Price = (decimal)(reader["Price"]);
                obj.PriceTag = (reader["PriceTag"] == DBNull.Value) ? String.Empty : (string)reader["PriceTag"];
                objects.Add(obj);
                //MenuItemPortion menuItemPortion = menuItemPortions.Where(x => x.Id == obj.MenuItemPortionId).FirstOrDefault();
                menuItemPortions.Where(x => x.Id == obj.MenuItemPortionId).ToList().ForEach(i => i.Prices.Add(obj));
            }
            reader.Close();

            return menuItemPortions;
        }
        public static DataSet GetRecipes(int sMenuItemId, string sGroupCode)
        {
            DataSet ds = new DataSet();
            try
            {
                string sClause = string.Empty;
                if (sGroupCode != "All")
                {
                    sClause = "and mi.groupcode = '" + sGroupCode + "'";
                }

                if (sMenuItemId != 0)
                {
                    sClause = "and mi.id =" + sMenuItemId;
                }

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"   SELECT 
                                                mi.GroupCode,r.Name RecipeName,mi.Id MenuItemId, mi.Name MenuItemName, mip.Name PortionName, mpr.Price SellingPrice, r.FixedCost,
                                                i.Name InventoryItemName, ri.Quantity, i.BaseUnit,
                                                'Price' = UnitPrice, 'Cost' = ri.Quantity * UnitPrice
                                                FROM MenuItems mi, menuitemportions mip, MenuItemPrices mpr,
                                                Recipes r, recipeitems ri, inventoryitems i,
                                                (
	                                                SELECT i.Id, i.Name, isnull(UnitPrice, 0)UnitPrice 
	                                                FROM InventoryItems i LEFT OUTER JOIN
	                                                (
		                                                SELECT l.inventoryItem_id, t.Price/Multiplier UnitPrice
		                                                FROM 
		                                                (
			                                                SELECT inventoryItem_id ,max(id) id FROM InventoryTransactions GROUP BY inventoryItem_id
		                                                )l,
		                                                (
			                                                SELECT * FROM InventoryTransactions 
		                                                )t
		                                                WHERE l.Id = t.id
	                                                )ItemPrice
	                                                ON ItemPrice.inventoryItem_id = i.Id
                                                )InventoryPrice
                                                WHERE
                                                mi.Id = mip.MenuItemId AND mip.Id = r.Portion_Id
                                                AND  r.Id = ri.RecipeId AND ri.InventoryItem_Id = i.Id
                                                AND InventoryPrice.id = i.Id
                                                AND mip.Id = mpr.MenuItemPortionId
                                                {0}
                                               Order by mi.GroupCode,mi.Name, mip.Name,i.Name", sClause);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "Recipe");
                dbConn.Close();

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;

        }
        public static DataTable ProductionCostRecipe(int MenuItemId, string PortionName)
        {
            DataTable ds = new DataTable("ProductionCostRecipe");
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = $@" SELECT  distinct MenuItemId, MenuItemName,CONVERT(DECIMAL(20,2),isnull(Sum(t.Quantity*t.Price), 0)) ProductionCost  FROM 
						                                                (    
							                                                SELECT mi.Id MenuItemId,mi.Name MenuItemName, mip.Name PortionName, r.Name RecipeName, i.Name InventoryItemName, ri.Quantity, i.BaseUnit,
							                                                'Price' =(
										                                                SELECT max(
													                                                CASE  WHEN Unit = BaseUnit THEN Price 
												                                                      Else Price / TransactionUnitMultiplier
												                                                      END 
												                                                   )as Price FROM 
													                                                (  
															                                                SELECT s.*
															                                                FROM (SELECT p.InventoryItem_Id, 
																												   p.Price, p.Unit,
																												   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																																		 ORDER BY Id DESC) AS rk
																											FROM InventoryTransactions p)s
															                                                WHERE s.rk = 1
													
													                                                )t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = i.Id
									                                                  )
							                                                FROM MenuItems mi, menuitemportions mip,
							                                                Recipes r, recipeitems ri, inventoryitems i
							                                                WHERE
							                                                mi.Id = mip.MenuItemId AND mip.Id = r.Portion_Id
							                                                AND  r.Id = ri.RecipeId AND ri.InventoryItem_Id = i.Id
							                                               -- AND mip.Name = 'Normal' 
							                                               AND mip.MenuItemId = {MenuItemId} and mip.Name = '{PortionName.Replace("'", "''")}'
							                                                
						                                                ) T
						                                                group by MenuItemId,MenuItemName";

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();


            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataTable ProductionCostFixed(int MenuItemId, string PortionName)
        {
            DataTable ds = new DataTable("ProductionCostFixed");
            try
            {
                if (MenuItemId == 850)
                {

                }
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@" SELECT   MenuItemId, MenuItemName, mAx(Price) FixedProductionCost  FROM 
                                            (    
                                                SELECT mi.Id MenuItemId,mi.Name MenuItemName, mip.Name PortionName, r.Name RecipeName,   r.FixedCost    Price
                                                FROM MenuItems mi, menuitemportions mip, Recipes r
                                                WHERE
                                                mi.Id = mip.MenuItemId AND mip.Id = r.Portion_Id   
                                               AND mip.MenuItemId = {0} and mip.Name = '{1}'
    
                                            ) T
                                            group by MenuItemId,MenuItemName", MenuItemId, PortionName.Replace("'", "''"));

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();


            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet ProductionCostDrill(int MenuItemId, string PortionName)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@" WITH summary AS 
                                                (
                                                    SELECT p.InventoryItem_Id, 
                                                            p.Price, p.Unit,
                                                            ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
                                                                                    ORDER BY Id DESC) AS rk
                                                    FROM InventoryTransactions p
                                                    --WHERE InventoryItem_Id IN (612,615,730)
                                                    )

                                                    SELECT *, Quantity*Price Total  FROM 
                                                    (    
                                                    SELECT mi.Name MenuItemName, mip.Name PortionName, r.Name RecipeName, i.Name InventoryItemName, ri.Quantity, i.BaseUnit,r.FixedCost,
                                                    'Price' =isnull((
                                                                SELECT max(
                                                                            CASE  WHEN Unit = BaseUnit THEN Price 
                                                                                Else Price / TransactionUnitMultiplier
                                                                                END 
                                                                            )as Price FROM 
                                                                            (  
                                                                                    SELECT s.*
                                                                                    FROM summary s
                                                                                    WHERE s.rk = 1

                                                                            )t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = i.Id
                                                                ), 0)
                                                    FROM MenuItems mi, menuitemportions mip,
                                                    Recipes r, recipeitems ri, inventoryitems i
                                                    WHERE
                                                    mi.Id = mip.MenuItemId AND mip.Id = r.Portion_Id
                                                    AND  r.Id = ri.RecipeId AND ri.InventoryItem_Id = i.Id
                                                    --AND mip.Name = Orders.PortionName AND orders.MenuItemId = mi.Id
                                                    AND mi.Id ={0} and mip.Name = '{1}'
                                                ) T", MenuItemId, PortionName.Replace("'", "''"));

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "ProductionCostDrill");
                dbConn.Close();


            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetMenuItemList( string department)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"   select m.id Id,d.Id DepartmentId,d.Name DepartmentName,isNull(m.GroupCode,' ')GroupCode,isNull(m.Barcode,' ')Barcode,m.Name,mp2.Name PortionName,mp.Price
                                                from Menuitems m,[MenuItemPrices] mp,[MenuItemPortions] mp2,Departments d, TicketTypes t, ScreenMenus S, ScreenMenuItems smi, ScreenMenuCategories smc
                                                where m.id = mp2.menuitemid
                                                and mp2.id = mp.MenuItemPortionId
                                                and d.TicketTypeId = t.id
                                                and t.ScreenMenuId = s.id
                                                and s.id = smc.ScreenMenuId
                                                and smc.id = smi.ScreenMenuCategoryId
                                                and smi.MenuItemId = m.id
                                                {0}
                                                order by m.id", department);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "MenuItemList");
                dbConn.Close();


            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        //Author Jewel Hossain
        public static DataSet GetMenuItemListByDepertmentId(int departmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"   select m.id Id,d.Id DepartmentId,d.Name DepartmentName,isNull(m.GroupCode,' ')GroupCode,isNull(m.Barcode,' ')Barcode,m.Name,mp2.Name PortionName,mp.Price
                                                from Menuitems m,[MenuItemPrices] mp,[MenuItemPortions] mp2,Departments d, TicketTypes t, ScreenMenus S, ScreenMenuItems smi, ScreenMenuCategories smc
                                                where m.id = mp2.menuitemid
                                                and mp2.id = mp.MenuItemPortionId
                                                and d.TicketTypeId = t.id
                                                and t.ScreenMenuId = s.id
                                                and s.id = smc.ScreenMenuId
                                                and smc.id = smi.ScreenMenuCategoryId
                                                and smi.MenuItemId = m.id
                                                and d.id = {0}
                                                order by m.id", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "MenuItemList");
                dbConn.Close();


            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetGroupItem()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                sql = "select distinct GroupCode from MenuItems order by GroupCode";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetMenuItem(string search)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                if (string.IsNullOrEmpty(search))
                {
                    sql = "select distinct ID, Name from MenuItems order by Name";
                }
                else
                {
                    sql = string.Format("select distinct id,Name FROM MenuItems where GroupCode='{0}' order by Name", search);
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetMenuItemTag()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                sql = "select distinct GroupCode from MenuItems order by GroupCode";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetMenuItemOutletWise(DateTime startDate, DateTime endDate, string DepartmentIds, int outletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                //string sqlClause = string.Empty;
                //if (!string.IsNullOrEmpty(DepartmentIds))
                //{
                //    sqlClause = string.Format(@"and t.DepartmentId in ({0})", DepartmentIds);
                //}
                //if (outletId > 0)
                //{
                //    sqlClause = sqlClause + string.Format(@" and t.SyncOutletId = {0}", outletId);
                //}

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
               
                //sql = string.Format(@"Select o.MenuItemId, o.MenuGroupName GroupCode, o.MenuItemName, o.PortionName, o.price UnitPrice,sum(o.Quantity) Quantity,sum(o.Total) Total,  so.Name OutletName
                //                        from  Orders o
                //                         , tickets t, SyncOutlets so
                //                        where o.TicketId = t.Id
                //                        and t.SyncOutletId = so.Id
                //                        and t.[Date] between '{0}' and '{1}'
                //                        {2}
                //                        group by o.MenuItemId, o.MenuGroupName, o.MenuItemName, o.PortionName, o.price, so.Name
                //                        order by o.MenuItemName, o.PortionName,o.Price, so.Name", startDate.ToString(), endDate.ToString(), sqlClause);


                sql = string.Format(@"SELECT	o.MenuItemId, o.MenuGroupName GroupCode, o.MenuItemName, o.PortionName,
		                                        o.price UnitPrice, sum(o.Quantity) Quantity, sum(o.Total) Total,  so.Name OutletName

                                        FROM	Orders o,
		                                        tickets t,
		                                        SyncOutlets so

                                        WHERE	o.TicketId = t.Id
		                                        AND t.SyncOutletId = so.Id
		                                        AND t.[Date] BETWEEN '{0}' AND '{1}'
		                                        AND o.CalculatePrice = 1
		                                        GROUP BY o.MenuItemId, o.MenuGroupName, o.MenuItemName, o.PortionName, o.price, so.Name
		                                        ORDER BY so.Name, o.MenuItemName, o.PortionName, o.Price", startDate.ToString(), endDate.ToString());
                
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }



        public static DataSet GetCalculationsOutLetWise(DateTime startDate, DateTime endDate)
        {
            DataSet dsOutletWise = new DataSet();
            try
            {
                string queryString = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                queryString = string.Format(@"SELECT	s.Name OutletName, c.Name CalculationName, SUM(c.CalculationAmount) AS CalculationAmount
                                                FROM	dbo.Calculations c
		                                                JOIN  dbo.Tickets t ON c.TicketId = t.Id
		                                                JOIN dbo.SyncOutlets s ON s.Id  = t.SyncOutletId
                                                WHERE	t.[Date] BETWEEN '{0}' AND '{1}'
		                                                GROUP BY s.Name, c.Name ORDER BY s.Name, c.Name;", startDate.ToString(), endDate.ToString());



                SqlDataAdapter da = new SqlDataAdapter(queryString, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(dsOutletWise);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception($"{ ex }");
            }

            return dsOutletWise;
        }


        public static DataSet GetTotalOutletWise(DateTime startDate, DateTime endDate)
        {
            DataSet dstotalOutletWise = new DataSet();
            try
            {
                string queryString = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                queryString = string.Format(@"SELECT	so.Name OutletName, SUM(TotalAmount) TotalAmount
                                                FROM	tickets t, SyncOutlets so
                                                WHERE	t.SyncOutletId = so.Id
		                                                AND [Date] BETWEEN '{0}' AND '{1}'
		                                                GROUP BY so.Name ORDER BY so.Name", startDate.ToString(), endDate.ToString());


                SqlDataAdapter da = new SqlDataAdapter(queryString, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(dstotalOutletWise);
                dbConn.Close();

            }
            catch (SqlException ex)
            {

                throw new Exception($"{ ex }");
            }

            return dstotalOutletWise;
        } 


        public static DataSet GetMenuItemScreenMenuWise(DateTime startDate, DateTime endDate, string DepartmentIds, int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = string.Format(@"and d.Id in ({0})", DepartmentIds);
                }
                if (OutletId > 0)
                {
                    sqlClause = sqlClause + string.Format(@" and t.SyncOutletId = {0}", OutletId);
                }
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@"Select * , (Quantity/SUM(Quantity) over())*100 QuantityPerc, (Gross/SUM(Gross) over())*100 TotalPerc from (
										select DepartmentName, GroupName, SubMenu, SUM(Quantity) Quantity, SUM(Total) Gross from (
												select d.Name DepartmentName, smc.Name GroupName, IsNULL(LTRIM(RTRIM(smi.SubMenuTag)), '-') SubMenu, o.Quantity, (o.Total+(o.OrderTagPrice*o.Quantity)) Total
												from tickets t, orders o, TicketTypes tt, Departments d, ScreenMenus sm, 
												ScreenMenuCategories smc, ScreenMenuItems smi
												where t.Id = o.TicketId
												and t.TicketTypeId = tt.Id
												and tt.Id = d.TicketTypeId
												and tt.ScreenMenuId = sm.Id
												and sm.Id = smc.ScreenMenuId 
												and smc.Id = smi.ScreenMenuCategoryId
												and o.MenuItemId = smi.MenuItemId
												and t.[Date] between '{0}' and '{1}'
                                                {2}
										)Q
										group by DepartmentName, GroupName, SubMenu
										
									)T
									order by DepartmentName, GroupName, SubMenu", startDate.ToString("dd MMM yyy hh:mm tt"), endDate.ToString("dd MMM yyy hh:mm tt"), sqlClause);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetTargetAchievement(DateTime startDate, DateTime endDate)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string sqlClause = string.Empty;
                //if (DepartmentId > 0)
                //{
                //    sqlClause = string.Format(@"and t.DepartmentId = {0}", DepartmentId);
                //}
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@"If(OBJECT_ID('tempdb..#TranDate') Is Not Null)
                                    Begin
                                        Drop Table #TranDate
                                    END

                                    DECLARE 
                                    @DateFrom smalldatetime, 
                                    @DateTo smalldatetime;
                                    SET @DateFrom= '{0}';
                                    SET @DateTo='{1}';

                                    create table #TranDate
                                    (
                                        SalesDate DATETIME  
                                    );

                                    WITH T(date)
                                    AS
                                    ( 
	                                    SELECT @DateFrom 
	                                    UNION ALL
	                                    SELECT DateAdd(day,1,T.date) FROM T WHERE T.date < @DateTo
                                    )

                                    INSERT INTO #TranDate
                                    SELECT date FROM T OPTION (MAXRECURSION 32767);

                                    SELECT *,
                                    CASE WHEN DailyTarget > 0 THEN
                                    DailySales/DailyTarget*100
                                    ELSE 0
                                    END SalesAchievement,--****
                                    CASE WHEN TargetInPcs > 0 THEN
                                    SalesCount/TargetInPcs*100
                                    ELSE 0
                                    END 'Combo_Achievement'--****
                                    FROM 
                                    (
	                                    SELECT 
	                                    Target.SalesDate, OutletId, OutletName, isnull(TotalAmount, 0) DailySales, 
	                                    isnull(TicketCount, 0) TicketCount, isnull(BPO, 0) BPO, isnull(SalesCount, 0) SalesCount,
										Case WHEN isnull(PerformanceDays, 0) > 0 THEN
										isnull(TargetInPcs, 0)/isnull(PerformanceDays, 0) 
										END TargetInPcs,
	                                    CASE WHEN  isnull(PerformanceDays, 0) > 0 THEN
	                                    isnull(TargetInValue, 0)/isnull(PerformanceDays, 0) 
	                                    ELSE 0
	                                    END DailyTarget,
	                                    isnull(Combo_Sales_Count, 0) Combo_Sales_Count,--****
										Case WHEN isnull(PerformanceDays, 0) > 0 THEN
	                                    isnull(Combo_Target_Count, 0)/ isnull(PerformanceDays, 0)
										ELSE 0
										END Combo_Target_Count--****
	                                    FROM 
	                                    (
		                                    SELECT d.*, t.*, Ordr.*, Combo_Target_Count, ti.TargetInPcs FROM 
		                                    (
			                                    SELECT #TranDate.*, o.Id OutletId, o.Name OutletName
			                                    FROM #TranDate , SyncOutlets o
		                                    )d
		                                    LEFT OUTER JOIN 
		                                    (
			                                    SELECT ot.StartDate, ot.EndDate, Month, 
			                                    Year, i.SyncOutlet_Id, sum(i.TargetInPcs*i.MenuItemPrice) TargetInValue,
			                                    DATEDIFF(d, DateAdd(d,-1,StartDate), EndDate) PerformanceDays
			                                    FROM 
			                                    OutletWiseTargets ot, OutletWiseTargetItems i
			                                    where ot.ID = i.OutletWiseTargetId
			                                    GROUP BY ot.StartDate, ot.EndDate, PerformanceDays, Month, Year, SyncOutlet_Id
		                                    ) t
		                                    ON d.OutletID = t.SyncOutlet_Id AND 
		                                    d.SalesDate >= t.StartDate AND d.SalesDate <= t.EndDate
		                                    LEFT OUTER JOIN
		                                    (
			                                    SELECT CAST(convert(varchar, [CreatedDateTime], 106) AS DATETIME) OrderDate, mi.Tag, --****
			                                    t.SyncOutletId, count(*) SalesCount, count(*) 'Combo_Sales_Count'
			                                    FROM tickets t, orders o, MenuItems mi
			                                    WHERE o.MenuItemId = mi.Id
			                                    AND o.CreatedDateTime BETWEEN @DateFrom AND @DateTo
			                                    AND o.DecreaseInventory = 1
			                                    AND o.CalculatePrice = 1
			                                    AND t.Id = o.TicketId
			                                    AND mi.Tag = 'Combo'--****
			                                    GROUP BY 
			                                    CAST(convert(varchar, [CreatedDateTime], 106) AS DATETIME), mi.Tag--****
				                                    , t.SyncOutletId	
		                                    )Ordr
		                                    ON d.SalesDate = Ordr.OrderDate
		                                    AND d.OutletID = Ordr.SyncOutletId
		                                    LEFT OUTER JOIN 
		                                    (
			                                    SELECT ot.StartDate, ot.EndDate, Month,	   
			                                    Year, i.SyncOutlet_Id, sum(i.TargetInPcs)TargetInPcs, sum(i.TargetInPcs)'Combo_Target_Count'
			                                    FROM 
			                                    OutletWiseTargets ot, OutletWiseTargetItems i, MenuItems mi, MenuItemPortions p
			                                    where ot.ID = i.OutletWiseTargetId
			                                    AND mi.Id = p.MenuItemId
			                                    AND i.MenuItemPortion_Id = p.Id
			                                    AND mi.Tag = 'Combo'--****
			                                    GROUP BY ot.StartDate, ot.EndDate, PerformanceDays, Month, Year, SyncOutlet_Id
		                                    ) ti
		                                    ON d.OutletID = ti.SyncOutlet_Id AND 
		                                    d.SalesDate >= ti.StartDate AND d.SalesDate <= ti.EndDate
		
	                                    )Target LEFT OUTER JOIN 
	                                    (
		                                    SELECT CAST(convert(varchar, [date], 106) AS DATETIME) SalesDate, SyncOutletId,  sum(TotalAmount) TotalAmount, count(*) TicketCount,
		                                    CASE WHEN count(*) > 0
		                                    THEN sum(TotalAmount)/count(*)
		                                    ELSE 0
		                                    END BPO
		                                    FROM tickets t
		                                    WHERE TotalAmount > 0	
		                                    GROUP BY CAST(convert(varchar, [date], 106) AS DATETIME), SyncOutletId	
	                                    ) Sales
	                                    on
	                                    Target.SalesDate = Sales.SalesDate
	                                    AND Target.OutletId = Sales.SyncOutletId	
                                    )TVA
                                    ORDER BY TVA.SalesDate, TVA.OutletId
                                    If(OBJECT_ID('tempdb..#TranDate') Is Not Null)
                                    Begin
                                        Drop Table #TranDate
                                    END
                                    ", startDate.ToString("dd MMM yyyy"), endDate.ToString("dd MMM yyyy"));

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
    }
}
