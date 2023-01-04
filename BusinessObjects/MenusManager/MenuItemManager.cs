using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Domain.Models.Menus;
using System.Data.SqlClient;
using System.Data;

namespace BusinessObjects.MenusManager
{
    public class MenuItemManager
    {
        public static List<MenuItem> GetMenuItems()
        {
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
                    sClause = "and groupcode = '"+sGroupCode+"'";
                if (sMenuItemId != 0)
                    sClause = "and id =" + sMenuItemId;
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
                    sClause = "and groupcode = '" + sGroupCode + "'";
                if (sMenuItemId != 0)
                    sClause = "and id =" + sMenuItemId;
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
                //MenuItemPortion menuItemPortion = menuItemPortions.Where(x => x.Id == obj.MenuItemPortionId).SingleOrDefault();
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
                    sClause = "and mi.groupcode = '" + sGroupCode + "'";
                if (sMenuItemId != 0)
                    sClause = "and mi.id =" + sMenuItemId;

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
                string sql = string.Format(@" SELECT  distinct MenuItemId, MenuItemName,CONVERT(DECIMAL(20,2),isnull(Sum(t.Quantity*t.Price), 0)) ProductionCost  FROM 
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
                    sql = "select distinct ID, Name from MenuItems order by Name";
                else
                    sql = string.Format("select distinct id,Name FROM MenuItems where GroupCode='{0}' order by Name", search);

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
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = string.Format(@"and t.DepartmentId in ({0})", DepartmentIds);
                }
                if (outletId > 0)
                    sqlClause = sqlClause + string.Format(@" and t.SyncOutletId = {0}", outletId);
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
               
                sql = string.Format(@"Select o.MenuItemId, o.MenuGroupName GroupCode, o.MenuItemName, o.PortionName, o.price UnitPrice,sum(o.Quantity) Quantity,sum(o.Total) Total,  so.Name OutletName
                                        from  Orders o
                                         , tickets t, SyncOutlets so
                                        where o.TicketId = t.Id
                                        and t.SyncOutletId = so.Id
                                        and t.[Date] between '{0}' and '{1}'
                                        {2}
                                        group by o.MenuItemId, o.MenuGroupName, o.MenuItemName, o.PortionName, o.price, so.Name
                                        order by o.MenuItemName, o.PortionName,o.Price, so.Name", startDate.ToString(), endDate.ToString(), sqlClause);
                
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
