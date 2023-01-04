using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessObjects.InventoryManager
{
    public class CurrentStockManager
    {
        public static DataSet GetCurrentStocks(string inventoryGroupItem, string inventoryItemID, string warehouseID, string Brand, string Vendor)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql2 = string.Empty;
                if (warehouseID != "0")
                {
                    sql2 += "AND wc.WarehouseId in (" + Convert.ToInt32(warehouseID) + ")";
                }
                if (inventoryItemID != "0")
                {
                    sql2 += "AND pci.InventoryItemId in (" + Convert.ToInt32(inventoryItemID) + ")";
                }
                else if (inventoryGroupItem != "All")
                {
                    sql2 += "AND invItems.GroupCode in ('" + inventoryGroupItem + "')";
                }

                if (Brand != "All")
                {
                    sql2 = sql2 + string.Format(" and invItems.STARBrand = '{0}'", Brand);
                }
                if (Vendor != "All")
                {
                    sql2 = sql2 + string.Format(" and invItems.STARVendor = '{0}'", Vendor);
                }
                string sql = string.Format(@"select w.Warehouse,w.Barcode, w.InventoryItemName,w.inventoryitemid,w.Unit, w.InStock, w.Purchase, 
								w.LatestCost,Cast(isnull(i.AverageCost,0) as DECIMAL(10,2)) AverageCost, w.InventoryConsumption, w.InventoryPrediction, w.CurrentInventory  from
								(
									SELECT w.Name Warehouse, pci.InventoryItemName, invItems.id inventoryitemid,invItems.Barcode,
									pci.UnitName as Unit, 
									pci.InStock,
									pci.Purchase, 
									pci.Cost LatestCost, 
									pci.Consumption as InventoryConsumption, 
									(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
									Convert(Decimal(20,3),((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost)) as CurrentInventory 
									From PeriodicConsumptionItems pci 
									inner join WarehouseConsumptions wc 
									on wc.Id = pci.PeriodicConsumptionId 
									inner join InventoryItems invItems 
									on invItems.Id=pci.InventoryItemId 
									INNER JOIN warehouses w
									ON w.Id = wc.WarehouseId
									where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc)
									{0}
								)w left outer join
								(
									select inventoryItem_id, avg(price) AverageCost
									from inventorytransactions group by inventoryItem_id
								)i
								on i.inventoryItem_id = inventoryitemid
								", sql2);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "CurrentStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetStartAndEndDate(DateTime fromDate, DateTime toDate)
        {
            DataSet ds = new DataSet();
            string searchStr = string.Empty;
            try
            {

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = $@"DECLARE 
								@StartDate  VARCHAR(100),
								@EndDate  VARCHAR(100),
								@LastPeriodStartDate DATETIME,
								@LastPeriodEndDate DATETIME,
								@LastWorkPeriodID INT,
								@FirstWorkPeriod int

								SET @StartDate = (SELECT [dbo].[ufsFormat] ((SELECT min(w.startdate) FROM WorkPeriods w WHERE startdate <> enddate and w.startdate > '{fromDate.ToString("dd MMM yyyy")}'),'yyyy-mm-dd hh:mm:ss'))
								SET @LastWorkPeriodID = (SELECT isnull(WorkPeriods.Id, 0) FROM WorkPeriods WHERE StartDate = (SELECT max(w.startdate) FROM WorkPeriods w WHERE startdate <> enddate /*and w.StartDate > '{fromDate.ToString("dd MMM yyyy")}'*/ AND  w.StartDate <= Dateadd(Day,1,'{toDate.ToString("dd MMM yyyy")}')))
								SET @FirstWorkPeriod = (SELECT isnull(WorkPeriods.Id, 0) FROM WorkPeriods WHERE startdate <> enddate and StartDate = (SELECT min(w.startdate) FROM WorkPeriods w WHERE w.startdate > '{fromDate.ToString("dd MMM yyyy")}'))
								SET @LastPeriodStartDate = (SELECT startdate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)
								SET @LastPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)                                            
								SET @EndDate = (SELECT [dbo].[ufsFormat]((SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID), 'yyyy-mm-dd hh:mm:ss'))

								SELECT isnull(@StartDate, '18 sep 3030') StartDate, isnull(@EndDate, '18 sep 3030') EndDate, isnull(@FirstWorkPeriod, 0) FirstWorkPeriodID, isnull(@LastWorkPeriodID, 0) LastWorkPeriodID";

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);

                da.Fill(ds, "StartAndEndDate"); ;
                dbConn.Close();

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ds;
        }

        public static DataSet GetStartAndEndDateOfLastWorkPeriod()
        {
            DataSet ds = new DataSet();
            string searchStr = string.Empty;
            try
            {

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT [dbo].[ufsFormat] (startdate, 'yyyy-mm-dd hh:mm:ss') StartDate, [dbo].[ufsFormat] (enddate, 'yyyy-mm-dd hh:mm:ss') EndDate
											 FROM WorkPeriods WHERE Id =( SELECT max(id) FROM WorkPeriods WHERE StartDate <> enddate)");

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);

                da.Fill(ds, "StartAndEndDate"); ;
                dbConn.Close();

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ds;
        }

        public static DataSet GetWorkPeriodWiseInvntoryRegister(string inventoryGroupItem, string inventoryItemID, string warehouseID, DateTime dtStart, DateTime dtEnd, int firstworkperiodid, int lastworkperiodid)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                if (inventoryGroupItem == string.Empty || inventoryItemID == string.Empty || warehouseID == string.Empty)
                {
                    return null;
                }

                string sql = string.Format(@"SELECT i.GroupCode , i.Name Inventory, i.TransactionUnit Unit, i.UnitPrice, 
								[dbo].[ufsFormat](pc.StartDate, 'yyyy-mm-dd hh:mm:ss') StartDate, i.Id InventoryID, w.Id WarehouseID,
								[dbo].[ufsFormat](pc.EndDate, 'yyyy-mm-dd hh:mm:ss') EndDate, Convert(Decimal(20,3),p.InStock) OpeningStock, Convert(Decimal(20,3),p.InStock*i.UnitPrice) OpeningStockInValue, w.Name Warehouse,
								p.Purchase 'Purchase/ Transfer', Convert(Decimal(20,3),p.Purchase*i.UnitPrice) PurchaseTransferInValue , p.Consumption, Convert(Decimal(20,3),p.Consumption*i.UnitPrice) ConsumptionInValue, 
								p.StockIn StockIn, Convert(Decimal(20,3),(p.StockIn*i.UnitPrice)) StockInValue,
								p.StockOut StockOut, Convert(Decimal(20,3),(p.StockOut*i.UnitPrice)) StockOutValue, isnull(p.Wastage,0) Wastage , Convert(Decimal(20,3),isnull(p.Wastage,0)*i.UnitPrice) WastageValue, isnull(p.ReturnAmount,0) [Return],  Convert(Decimal(20,3),(isnull(p.ReturnAmount,0)*i.UnitPrice)) ReturnValue,
								(p.InStock + p.Purchase - p.Consumption - p.wastage-p.stockout-p.returnamount-p.StockOut) Expected,  Convert(Decimal(20,3),(p.InStock + p.Purchase - p.Consumption- p.wastage-p.stockout-p.returnamount)*i.UnitPrice) ExpectedValue,
								isnull(p.PhysicalInventory,0) StockTake,  Convert(Decimal(20,3),isnull(p.PhysicalInventory,0)*i.UnitPrice) StockTakeValue
								FROM PeriodicConsumptions pc, WarehouseConsumptions wc,
								PeriodicConsumptionitems p, 
								(
									SELECT i.*,
									UnitPrice = isnull((
															 SELECT Price 
															 FROM InventoryTransactions 
															 WHERE 
															 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
															 AND  id = (
																		 SELECT max(id) FROM InventoryTransactions 
																		 WHERE InventoryItem_Id =  i.id
																		 AND  InventoryTransactions.Unit = i.TransactionUnit
																	   )				                                             
														 ),0)
									from                                          
									inventoryitems i
								)i, warehouses w
								WHERE  pc.Id = wc.PeriodicConsumptionId
								AND wc.Id = p.PeriodicConsumptionId
								AND p.InventoryItemId = i.Id
								AND wc.WarehouseId = w.Id
                                AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
                                AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate
								AND pc.Id >= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {0})
								AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {1})", firstworkperiodid, lastworkperiodid);

                sql += " AND wc.WarehouseId in(" + Convert.ToInt32(warehouseID) + ")";
                sql += " AND i.Id in(" + Convert.ToInt32(inventoryItemID) + ")";
                sql += " AND i.GroupCode in('" + inventoryGroupItem + "')";
                sql += " ORDER BY wc.WarehouseId, i.Id, pc.Id";

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "CurrentStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetInventoryVsRecipesData(string inventoryItemIDs)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();



                string sql = string.Format($@"SELECT 
                                            i.Name InventoryItemName, mi.Name MenuItemName, mip.Name PortionName, r.Name RecipeName
                                            , ri.Quantity, i.BaseUnit,
                                            'Price' = UnitPrice, 'Cost' = ri.Quantity * UnitPrice
                                            FROM MenuItems mi, menuitemportions mip,
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
                                            AND i.Id in ({inventoryItemIDs})
                                            ORDER BY i.Id"
                                            );


                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "CurrentStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetInvntoryRegister(string inventoryGroupItem, string inventoryItemID, string warehouseID, DateTime dtStart, DateTime dtEnd, int firstworkperiodid, int lastworkperiodid)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sqlClause = string.Empty;
                if (warehouseID != "0")
                {
                    sqlClause += " AND w.Id in(" + Convert.ToInt32(warehouseID) + ")";
                }
                if (inventoryItemID != "0")
                {
                    sqlClause += " AND i.Id in(" + Convert.ToInt32(inventoryItemID) + ")";
                }
                else if (inventoryGroupItem != "All")
                {
                    sqlClause += " AND i.GroupCode in('" + inventoryGroupItem + "')";
                }
                sqlClause += @" GROUP BY i.GroupCode, i.Name, w.Id, w.Name, 
								i.Id, i.Name, i.transactionunit";

                string sql = string.Format(@"
											DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100)

											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'yyyy-mm-dd hh:mm:ss'))
											SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'))

											Select
											StartDate,	EndDate, 	GroupCode,	Inventory, WarehouseID,	
											Warehouse,	InventoryID,	Name, Unit,
											UnitPrice LastUnitPrice, InStock OpeningStock, Convert(Decimal(20,3),(InStock*UnitPrice)) OpeningStockInValue, Purchase 'Purchase/ Transfer', 
											Convert(Decimal(20,3),Purchase*UnitPrice) PurchaseTransferInValue,
											Consumption, Convert(Decimal(20,3),Consumption*UnitPrice) ConsumptionInValue,
											Convert(Decimal(20,3),StockIn) StockIn,  Convert(Decimal(20,3),(StockIn*UnitPrice)) StockInValue,
											(StockOut-Consumption) StockOut,Convert(Decimal(20,3),((StockOut-Consumption)*UnitPrice)) StockOutValue,
											isnull(ClosingStock ,InStock + Purchase - Consumption) ClosingStock, 
											Convert(Decimal(20,3),isnull(ClosingStock ,InStock + Purchase - Consumption)*UnitPrice) ClosingStockInValue FROM
											(
												SELECT @StartDate StartDate,
												@EndDate EndDate,
												i.GroupCode, i.Name Inventory, w.Id WarehouseID, w.Name Warehouse, 
												i.Id InventoryID, i.Name, i.transactionunit Unit,
												StockIn =(
															SELECT max(StockIn)
															FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															WHERE 
															pcs.Id = wcs.PeriodicConsumptionId
															AND wcs.Id = ps.PeriodicConsumptionId 
															AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															AND ps.InventoryItemId = i.Id
															AND wcs.warehouseid = w.Id
														),
												StockOut =(
															SELECT max(StockOut)
															FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															WHERE 
															pcs.Id = wcs.PeriodicConsumptionId
															AND wcs.Id = ps.PeriodicConsumptionId 
															AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															AND ps.InventoryItemId = i.Id
															AND wcs.warehouseid = w.Id
														),
												InStock =(
															SELECT max(instock)
															FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															WHERE 
															pcs.Id = wcs.PeriodicConsumptionId
															AND wcs.Id = ps.PeriodicConsumptionId 
															AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															AND ps.InventoryItemId = i.Id
															AND wcs.warehouseid = w.Id
														),
												sum(p.Purchase) 'Purchase', sum(p.Consumption) Consumption, 
												ClosingStock =(
																SELECT (instock+purchase-consumption)
																FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
																WHERE 
																pcs.Id = wcs.PeriodicConsumptionId
																AND wcs.Id = ps.PeriodicConsumptionId 
																AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
																AND ps.InventoryItemId = i.Id
																AND wcs.warehouseid = w.Id
															),
												UnitPrice = isnull((
															 SELECT Price 
															 FROM InventoryTransactions 
															 WHERE 
															 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
															 AND  id = (
																		 SELECT max(id) FROM InventoryTransactions 
																		 WHERE InventoryItem_Id =  i.id
																		 AND  InventoryTransactions.Unit = i.TransactionUnit
																	   )				                                             
														 ),0)	
												FROM 
												PeriodicConsumptions pc,WarehouseConsumptions wc,
												PeriodicConsumptionitems p, inventoryitems i, warehouses w
												WHERE  pc.Id = wc.PeriodicConsumptionId
												AND wc.Id = p.PeriodicConsumptionId
												AND p.InventoryItemId = i.Id
												AND wc.WarehouseId = w.Id  
                                                AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
                                                AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate
												AND pc.Id >= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
												AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3}) {4}
											 )Q
											ORDER BY WarehouseID, GroupCode, Inventory ", dtStart.ToString("dd MMM yyyy hh:mm:ss tt"), dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"), firstworkperiodid, lastworkperiodid, sqlClause);



                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "CurrentStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetSpecialInvntoryRegister(
            string inventoryGroupItem,
            string inventoryItemID,
            string warehouseID,
            DateTime dtStart,
            DateTime dtEnd,
            int firstworkperiodid,
            int lastworkperiodid,
            bool fifo,
            string Brand,
            string Vendor,
            string InventoryTakeType,
            bool isForCpmpiled)
        {
            DataSet ds = new DataSet();
            try
            {
                inventoryGroupItem = inventoryGroupItem.Replace("'s ", "''");
                inventoryItemID = inventoryItemID.Replace("'s ", "''");
                warehouseID = warehouseID.Replace("'s ", "''");
                Brand = Brand.Replace("'s ", "''");
                Vendor = Vendor.Replace("'s ", "''");
                InventoryTakeType = InventoryTakeType.Replace("'s ", "''");

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sqlClause = string.Empty;
                if (warehouseID != "0")
                {
                    sqlClause += " AND w.Id in(" + Convert.ToInt32(warehouseID) + ")";
                }
                if (inventoryItemID != "0")
                {
                    sqlClause += " AND i.Id in(" + Convert.ToInt32(inventoryItemID) + ")";
                }
                //else if (inventoryGroupItem != "All")
                else if (!string.IsNullOrEmpty(inventoryGroupItem))
                {
                    sqlClause += string.Format(" AND i.GroupCode in ({0})", inventoryGroupItem);
                }
                if (!string.IsNullOrEmpty(InventoryTakeType))
                {
                    sqlClause += string.Format(" AND i.InventoryTakeType in ({0})", InventoryTakeType);
                }
                if (!string.IsNullOrEmpty(Brand))
                {
                    sqlClause = sqlClause + string.Format(" and i.STARBrand in ({0})", Brand);
                }
                if (Vendor != "All")
                {
                    sqlClause = sqlClause + string.Format(" and i.STARVendor = '{0}'", Vendor);
                }

                sqlClause += @" GROUP BY i.GroupCode, i.Name, w.Id, w.Name, 
								i.Id,i.PositionValue,i.IsIntermediate, i.Barcode, i.STARBrand, i.STARVendor, i.Name, i.transactionunit, i.StarID";
                if (!fifo)
                {
                    if (isForCpmpiled)
                    {
                        string sql = string.Format(@"
											DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100)
											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'yyyy-mm-dd hh:mm:ss'))
											SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'))
											select TQ.Id
            ,TQ.IsIntermediate
            ,TQ.Barcode
            ,TQ.Brand
            ,TQ.Vendor
            ,TQ.StartDate
            ,TQ.EndDate
            ,TQ.GroupCode
            ,TQ.Inventory
            ,TQ.WarehouseID
            ,TQ.Warehouse
            ,TQ.InventoryID
            ,TQ.Unit
            ,TQ.Last_Unit_Price
            ,TQ.Opening_Stock
            ,TQ.Opening_StockValue
            ,TQ.PurchaseORTransfer
            ,TQ.PurchaseORTransferValue
            ,TQ.Consumption
            ,TQ.StockOut
            ,TQ.StockOutValue
            ,TQ.Free
            ,TQ.ConsumptionValue
            ,TQ.Wastage
            ,TQ.WastageValue
            ,TQ.ReturnAmount
            ,TQ.ReturnValue
            ,TQ.ExpectedQty
            ,TQ.ExpectedValue
            ,TQ.PhysicalInventory
            ,TQ.PhysicalInventoryValue
            ,TQ.ClosingStock
            ,TQ.ClosingStockValue, 
			(Opening_Stock+PurchaseORTransfer-stockout-returnamount-PhysicalInventory)
			ActualConsumption,
			(Opening_StockValue+PurchaseORTransferValue-StockOutValue-ReturnValue-PhysicalInventoryValue)
			ActualConsumptionValue,
			PhysicalInventory-ExpectedQty CountVarianceQty ,
            ((PhysicalInventory*Last_Unit_Price)-(ExpectedQty*Last_Unit_Price)) CountVarianceValue from(
												Select
												Id,
                                                IsIntermediate,
                                                Barcode,
                                                Brand,
                                                Vendor,
                                                StartDate,	
                                                EndDate,
                                                PositionValue, 	
                                                GroupCode,	
                                                Inventory, 
                                                WarehouseID,	
                                                Warehouse,	
                                                InventoryID, 
                                                Unit,
                                                UnitPrice Last_Unit_Price, 
                                                InStock Opening_Stock,
                                                InStock*OpeningUnitPrice Opening_StockValue,
                                                Free,
                                                CASE  WHEN IsIntermediate = 0 
                                                THEN Purchase 
                                                Else 0
                                                END as PurchaseORTransfer,
                                                CASE  WHEN IsIntermediate = 0 
                                                THEN Purchase*UnitPrice 
												Else 0
												END as PurchaseORTransferValue,
												--Purchase 'PurchaseORTransfer', Purchase*UnitPrice PurchaseORTransferValue,
												((CASE  WHEN IsIntermediate = 0 THEN Consumption Else 0 END) - Free) as Consumption,
												isnull(StockOut,0) StockOut, isnull(StockOut,0)*UnitPrice StockOutValue,
												CASE  WHEN IsIntermediate = 0 THEN Consumption*UnitPrice Else 0 END as ConsumptionValue,
                                                Wastage, 
                                                Wastage*UnitPrice WastageValue, 
                                                ReturnAmount,
                                                ReturnAmount*UnitPrice ReturnValue,
                                                (InStock+Purchase-Consumption-Wastage-ReturnAmount-StockOut) ExpectedQty,
                                                (InStock+Purchase-Consumption-Wastage-ReturnAmount-StockOut)*UnitPrice ExpectedValue, 
                                                isnull(PhysicalInventory, ClosingStock) PhysicalInventory, 
                                                (UnitPrice*(isnull(PhysicalInventory, ClosingStock))) PhysicalInventoryValue,
                                                isnull(ClosingStock ,InStock + Purchase-stockout -Wastage - ReturnAmount- Consumption) ClosingStock,
                                                isnull(ClosingStock ,InStock + Purchase-stockout -Wastage - ReturnAmount- Consumption)*UnitPrice ClosingStockValue FROM
												  (
												SELECT @StartDate StartDate,
												@EndDate EndDate,
												i.Id,
                                                i.IsIntermediate, 
                                                i.Barcode, 
                                                i.STARBrand Brand, 
                                                i.STARVendor Vendor,
                                                i.PositionValue, 
                                                i.GroupCode, 
                                                i.Name Inventory, 
                                                w.Id WarehouseID, 
                                                w.Name Warehouse, 
                                                i.Id InventoryID, 
                                                i.Name, 
                                                i.transactionunit Unit,
												StockIn =(
															SELECT max(StockIn)
															FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															WHERE 
															pcs.Id = wcs.PeriodicConsumptionId
															AND wcs.Id = ps.PeriodicConsumptionId 
															AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															AND ps.InventoryItemId = i.Id
															AND wcs.warehouseid = w.Id
														),

												StockOut = isnull((SELECT P.TransferQty
													FROM
													(
														SELECT t.SourceWarehouseId, t.InventoryItem_Id, sum(t.Quantity*t.Multiplier/ it.TransactionUnitMultiplier) TransferQty, sum(t.Quantity*t.Price)TransferValue
														FROM InventoryTransactionDocuments d, InventoryTransactions t, InventoryItems it
														WHERE t.[Date] BETWEEN @StartDate AND @EndDate AND SourceWarehouseId <> 0
																AND TargetWarehouseId <> 0 AND d.Id = t.InventoryTransactionDocumentId
																AND it.Id = t.InventoryItem_Id
																AND d.IsProduction <> 1 GROUP BY InventoryItem_Id, SourceWarehouseId
													) P WHERE P.SourceWarehouseId = w.id AND P.InventoryItem_Id = i.StarID
												), 0),
												InStock =(
															SELECT  isnull(max(instock),0)
															FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															WHERE 
															pcs.Id = wcs.PeriodicConsumptionId
															AND wcs.Id = ps.PeriodicConsumptionId 
															AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															AND ps.InventoryItemId = i.Id
															AND wcs.warehouseid = w.Id
														),
												sum(isnull(p.Wastage, 0)) Wastage ,
												sum(isnull(p.ReturnAmount,0)) ReturnAmount,
                                                Free = isnull((SELECT k.Quantity
											                    FROM
											                    (
												                    SELECT	O.WarehouseId,  O.MenuItemId, m.StarID, SUM(O.Quantity) Quantity
												                    FROM	Orders O INNER JOIN MenuItems M ON M.Id = O.MenuItemId
												                    WHERE	O.CalculatePrice = 0 AND O.DecreaseInventory  = 1
														                    AND O.CreatedDateTime BETWEEN @StartDate AND @EndDate
														                    GROUP BY O.WarehouseId, O.MenuItemId, m.StarID
											                    ) k
											                    WHERE k.WarehouseId = w.Id AND k.StarID = i.StarID
									                    ), 0),
												PhysicalInventory  =(
															SELECT max(PhysicalInventory)
															FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															WHERE 
															pcs.Id = wcs.PeriodicConsumptionId
															AND wcs.Id = ps.PeriodicConsumptionId 
															AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
															AND ps.InventoryItemId = i.Id
															AND wcs.warehouseid = w.Id
														),
					
												sum(p.Purchase) 'Purchase', sum(p.Consumption) Consumption, 
												ClosingStock =(
																SELECT (instock + purchase -isnull(stocktake,0) - isnull(Wastage, 0) - isnull(ReturnAmount, 0)-isnull(StockOut, 0) - consumption)
																FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
																WHERE 
																pcs.Id = wcs.PeriodicConsumptionId
																AND wcs.Id = ps.PeriodicConsumptionId 
																AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
																AND ps.InventoryItemId = i.Id
																AND wcs.warehouseid = w.Id
															),
												UnitPrice = (isnull((
															 SELECT Price 
															 FROM InventoryTransactions 
															 WHERE 
															 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
															 AND  id = (
																		 SELECT max(id) FROM InventoryTransactions 
																		 WHERE InventoryItem_Id =  i.id
																		 AND  InventoryTransactions.Unit = i.TransactionUnit
																		 and InventoryTransactions.Date <= @EndDate
																	   )				                                             
														 ),0)+(isnull((
															 SELECT Price 
															 FROM InventoryTransactions 
															 WHERE 
															 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
															 AND  id = (
																		 SELECT max(id) FROM InventoryTransactions 
																		 WHERE InventoryItem_Id =  i.id
																		 AND  InventoryTransactions.Unit = i.TransactionUnit
																		 and InventoryTransactions.Date <= @EndDate
																	   )				                                             
														 ),0)*0.05)),
												OpeningUnitPrice = isnull((
															 SELECT Price 
															 FROM InventoryTransactions 
															 WHERE 
															 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
															 AND  id = (
																		 SELECT max(id) FROM InventoryTransactions 
																		 WHERE InventoryItem_Id =  i.id
																		 AND  InventoryTransactions.Unit = i.TransactionUnit
																		 and InventoryTransactions.Date < @StartDate
																	   )				                                             
														 ),0)		
												FROM 
												PeriodicConsumptions pc,WarehouseConsumptions wc,
												PeriodicConsumptionitems p, inventoryitems i, warehouses w
												WHERE  pc.Id = wc.PeriodicConsumptionId
												AND wc.Id = p.PeriodicConsumptionId
												AND p.InventoryItemId = i.Id
												AND wc.WarehouseId = w.Id                                
												AND pc.Id >= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
												AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3}) 
                                                AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
                                                AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate
                                                {4} 
								
											 )Q
											)TQ
											ORDER BY PositionValue",
                            dtStart.ToString("dd MMM yyyy hh:mm:ss tt"), dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"),
                            firstworkperiodid, lastworkperiodid, sqlClause);
                        SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                        da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                        da.Fill(ds, "SpecialInventoryRegister");
                    }
                    #region IPRExcludingProducedItems
                    //                 else
                    //                 {

                    //                     string sql = string.Format(@"
                    //								DECLARE 
                    //								@StartDate  VARCHAR(100),
                    //								@EndDate  VARCHAR(100)
                    //								SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'yyyy-mm-dd hh:mm:ss'))
                    //								SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'))
                    //								select TQ.Id
                    //         ,TQ.IsIntermediate
                    //         ,TQ.Barcode
                    //         ,TQ.Brand
                    //         ,TQ.Vendor
                    //         ,TQ.StartDate
                    //         ,TQ.EndDate
                    //         ,TQ.GroupCode
                    //         ,TQ.Inventory
                    //         ,TQ.WarehouseID
                    //         ,TQ.Warehouse
                    //         ,TQ.InventoryID
                    //         ,TQ.Unit
                    //         ,TQ.Last_Unit_Price
                    //         ,TQ.Opening_Stock
                    //         ,TQ.Opening_StockValue
                    //         ,TQ.PurchaseORTransfer
                    //         ,TQ.PurchaseORTransferValue
                    //         ,TQ.Consumption
                    //         ,TQ.StockOut
                    //         ,TQ.StockOutValue
                    //         ,TQ.ConsumptionValue
                    //         ,TQ.Wastage
                    //         ,TQ.WastageValue
                    //         ,TQ.ReturnAmount
                    //         ,TQ.ReturnValue
                    //         ,TQ.ExpectedQty
                    //         ,TQ.ExpectedValue
                    //         ,TQ.PhysicalInventory
                    //         ,TQ.PhysicalInventoryValue
                    //         ,TQ.ClosingStock
                    //         ,TQ.ClosingStockValue, 
                    //(Opening_Stock+PurchaseORTransfer-stockout-returnamount-PhysicalInventory)
                    //ActualConsumption,
                    //(Opening_StockValue+PurchaseORTransferValue-StockOutValue-ReturnValue-PhysicalInventoryValue)
                    //ActualConsumptionValue,
                    //PhysicalInventory-ExpectedQty CountVarianceQty ,
                    //         ((PhysicalInventory*Last_Unit_Price)-(ExpectedQty*Last_Unit_Price)) CountVarianceValue     
                    //                                         from(
                    //									Select
                    //									Id,IsIntermediate,Barcode,Brand,Vendor,StartDate,	EndDate,PositionValue, 	GroupCode,	Inventory, WarehouseID,	
                    //									Warehouse,	InventoryID, Unit,
                    //									UnitPrice Last_Unit_Price, InStock Opening_Stock,InStock*OpeningUnitPrice Opening_StockValue,
                    //									--CASE  WHEN IsIntermediate = 0
                    //                                             CASE when IsIntermediate in (0,1)
                    //									THEN Purchase 
                    //									 Else 0
                    //										END as PurchaseORTransfer,
                    //									--CASE  WHEN IsIntermediate = 0
                    //                                             CASE when IsIntermediate in (0,1)
                    //									THEN Purchase*UnitPrice 
                    //									 Else 0
                    //										END as PurchaseORTransferValue,
                    //									--Purchase 'PurchaseORTransfer', Purchase*UnitPrice PurchaseORTransferValue,
                    //									--CASE  WHEN IsIntermediate = 0
                    //                                             CASE when IsIntermediate in (0,1)
                    //									THEN Consumption 
                    //									 Else 0
                    //										END as Consumption,
                    //									isnull(StockOut,0) StockOut, isnull(StockOut,0)*UnitPrice StockOutValue,
                    //									--CASE  WHEN IsIntermediate = 0
                    //                                             CASE when IsIntermediate in (0,1)
                    //									THEN Consumption*UnitPrice 
                    //									 Else 0
                    //										END as ConsumptionValue,
                    //                                             Wastage, 
                    //                                             Wastage*UnitPrice WastageValue, 
                    //                                             ReturnAmount,
                    //                                             ReturnAmount*UnitPrice ReturnValue,
                    //                                             (InStock+Purchase-Consumption-Wastage-ReturnAmount-isnull(StockOut,0) ) ExpectedQty,
                    //                                             (InStock+Purchase-Consumption-Wastage-ReturnAmount-isnull(StockOut,0) )*UnitPrice ExpectedValue, 
                    //                                             isnull(PhysicalInventory, ClosingStock) PhysicalInventory, 
                    //                                             (UnitPrice*(isnull(PhysicalInventory, ClosingStock))) PhysicalInventoryValue,
                    //                                             isnull(ClosingStock ,InStock + Purchase-isnull(StockOut,0)  -Wastage - ReturnAmount- Consumption) ClosingStock,
                    //                                             isnull(ClosingStock ,InStock + Purchase-isnull(StockOut,0) -Wastage - ReturnAmount- Consumption)*UnitPrice ClosingStockValue FROM
                    //                                             (
                    //									SELECT @StartDate StartDate,
                    //									@EndDate EndDate,
                    //									i.Id,
                    //                                             i.IsIntermediate, 
                    //                                             i.Barcode, 
                    //                                             i.STARBrand Brand, 
                    //                                             i.STARVendor Vendor,
                    //                                             i.PositionValue, 
                    //                                             i.GroupCode, 
                    //                                             i.Name Inventory, 
                    //                                             w.Id WarehouseID, 
                    //                                             w.Name Warehouse, 
                    //									i.Id InventoryID, 
                    //                                             i.Name, 
                    //                                             i.transactionunit Unit,
                    //									StockIn =(
                    //												SELECT max(StockIn)
                    //												FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
                    //												WHERE 
                    //												pcs.Id = wcs.PeriodicConsumptionId
                    //												AND wcs.Id = ps.PeriodicConsumptionId 
                    //												AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
                    //												AND ps.InventoryItemId = i.Id
                    //												AND wcs.warehouseid = w.Id
                    //											),
                    //									sum(isnull(p.StockOut, 0)) StockOut ,
                    //									InStock =(
                    //												SELECT  isnull(max(instock),0)
                    //												FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
                    //												WHERE 
                    //												pcs.Id = wcs.PeriodicConsumptionId
                    //												AND wcs.Id = ps.PeriodicConsumptionId 
                    //												AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
                    //												AND ps.InventoryItemId = i.Id
                    //												AND wcs.warehouseid = w.Id
                    //											),
                    //									sum(isnull(p.Wastage, 0)) Wastage ,
                    //									sum(isnull(p.ReturnAmount,0)) ReturnAmount,
                    //									PhysicalInventory  =(
                    //												SELECT max(PhysicalInventory)
                    //												FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
                    //												WHERE 
                    //												pcs.Id = wcs.PeriodicConsumptionId
                    //												AND wcs.Id = ps.PeriodicConsumptionId 
                    //												AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
                    //												AND ps.InventoryItemId = i.Id
                    //												AND wcs.warehouseid = w.Id
                    //											),

                    //									sum(p.Purchase) 'Purchase', sum(p.Consumption) Consumption, 
                    //									ClosingStock =(
                    //													SELECT (instock + purchase -isnull(stocktake,0) - isnull(Wastage, 0) - isnull(ReturnAmount, 0)- isnull(StockOut, 0)- consumption)
                    //													FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
                    //													WHERE 
                    //													pcs.Id = wcs.PeriodicConsumptionId
                    //													AND wcs.Id = ps.PeriodicConsumptionId 
                    //													AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
                    //													AND ps.InventoryItemId = i.Id
                    //													AND wcs.warehouseid = w.Id
                    //												),
                    //									UnitPrice = isnull((
                    //												 SELECT Price 
                    //												 FROM InventoryTransactions 
                    //												 WHERE 
                    //												 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
                    //												 AND  id = (
                    //															 SELECT max(id) FROM InventoryTransactions 
                    //															 WHERE InventoryItem_Id =  i.id
                    //															 AND  InventoryTransactions.Unit = i.TransactionUnit
                    //															 and InventoryTransactions.Date <= @EndDate
                    //														   )				                                             
                    //											 ),0),
                    //									OpeningUnitPrice = isnull((
                    //												 SELECT Price 
                    //												 FROM InventoryTransactions 
                    //												 WHERE 
                    //												 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
                    //												 AND  id = (
                    //															 SELECT max(id) FROM InventoryTransactions 
                    //															 WHERE InventoryItem_Id =  i.id
                    //															 AND  InventoryTransactions.Unit = i.TransactionUnit
                    //															 and InventoryTransactions.Date < @StartDate
                    //														   )				                                             
                    //											 ),0)		
                    //									FROM 
                    //									PeriodicConsumptions pc,WarehouseConsumptions wc,
                    //									PeriodicConsumptionitems p, inventoryitems i, warehouses w
                    //									WHERE  pc.Id = wc.PeriodicConsumptionId
                    //									AND wc.Id = p.PeriodicConsumptionId
                    //									AND p.InventoryItemId = i.Id
                    //									AND wc.WarehouseId = w.Id                                
                    //									AND pc.Id >= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
                    //									AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
                    //                                             AND p.Produced = 0 --get non-produced items only
                    //                                             AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
                    //                                             AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate
                    //                                             {4} 								
                    //								 )Q
                    //								)TQ
                    //								ORDER BY  PositionValue",
                    //                         dtStart.ToString("dd MMM yyyy hh:mm:ss tt"), dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"),
                    //                         firstworkperiodid, lastworkperiodid, sqlClause);
                    //                     SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    //                     da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                    //                     da.Fill(ds, "SpecialInventoryRegister");
                    //                 }
                    #endregion
                    else //IPR Including Produced Items in different columns.
                    {

                        string sql = string.Format(@"
													DECLARE 
													@StartDate  VARCHAR(100),
													@EndDate  VARCHAR(100)
													SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'yyyy-mm-dd hh:mm:ss'))
													SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'))
													select TQ.Id
					         ,TQ.IsIntermediate
					         ,TQ.Barcode
					         ,TQ.Brand
					         ,TQ.Vendor
					         ,TQ.StartDate
					         ,TQ.EndDate
					         ,TQ.GroupCode
					         ,TQ.Inventory
					         ,TQ.WarehouseID
					         ,TQ.Warehouse
					         ,TQ.InventoryID
					         ,TQ.Unit
					         ,TQ.Last_Unit_Price
					         ,TQ.Opening_Stock
					         ,TQ.Opening_StockValue
					         ,TQ.PurchaseORTransfer
					         ,TQ.PurchaseORTransferValue
                             ,tq.ProductionIn ProductionIn
			                 ,tq.ProductionValue ProductionValues
					         ,TQ.Consumption
					         ,TQ.StockOut
					         ,TQ.StockOutValue
					         ,TQ.ConsumptionValue
					         ,TQ.Wastage
					         ,TQ.WastageValue
					         ,TQ.ReturnAmount
					         ,TQ.ReturnValue
					         ,TQ.ExpectedQty
					         ,TQ.ExpectedValue
					         ,TQ.PhysicalInventory
					         ,TQ.PhysicalInventoryValue
					         ,TQ.ClosingStock
					         ,TQ.ClosingStockValue, 
					(Opening_Stock+PurchaseORTransfer+ProductionIn-stockout-returnamount-PhysicalInventory)
					ActualConsumption,
					(Opening_StockValue+PurchaseORTransferValue+ProductionValue-StockOutValue-ReturnValue-PhysicalInventoryValue)
					ActualConsumptionValue,
					PhysicalInventory-ExpectedQty CountVarianceQty ,
					         ((PhysicalInventory*Last_Unit_Price)-(ExpectedQty*Last_Unit_Price)) CountVarianceValue     
					                                         from(
														Select
														Id,IsIntermediate,Barcode,Brand,Vendor,StartDate,	EndDate,PositionValue, 	GroupCode,	Inventory, WarehouseID,	
														Warehouse,	InventoryID, Unit,
														UnitPrice Last_Unit_Price, InStock Opening_Stock,InStock*OpeningUnitPrice Opening_StockValue,
												--CASE  WHEN IsIntermediate = 0
            --                                    CASE when IsIntermediate in (0,1)
												--THEN Purchase Else 0
												--END as PurchaseORTransfer,

												----CASE  WHEN IsIntermediate = 0
            --                                    CASE when IsIntermediate in (0,1)
												--THEN Purchase*UnitPrice Else 0
												--END as PurchaseORTransferValue,

												--CASE when IsIntermediate in (0,1)
												--THEN Produced Else 0
												--END as 'ProductionIn',

												--CASE when IsIntermediate in (0,1)
												--THEN Produced*UnitPrice Else 0
												--END as 'ProductionValue',
												
												----Purchase 'PurchaseORTransfer', Purchase*UnitPrice PurchaseORTransferValue,
												----CASE  WHEN IsIntermediate = 0


            --                                    CASE when IsIntermediate in (0,1) 
												--THEN Consumption Else 0
												--END as Consumption,

												--CASE when IsIntermediate in (0,1)
												--THEN Consumption*UnitPrice Else 0
												--END as ConsumptionValue,

												Purchase as PurchaseORTransfer,
												Purchase*UnitPrice as PurchaseORTransferValue,
												Produced as ProductionIn,
												Produced*UnitPrice as ProductionValue,
												Consumption as Consumption,
												Consumption*UnitPrice as ConsumptionValue,

												isnull(StockOut,0) StockOut, isnull(StockOut,0)*UnitPrice StockOutValue,
												--CASE  WHEN IsIntermediate = 0
					                                             Wastage, 
					                                             Wastage*UnitPrice WastageValue, 
					                                             ReturnAmount,
					                                             ReturnAmount*UnitPrice ReturnValue,
					                                             (InStock+Purchase+Produced-Consumption-Wastage-ReturnAmount-isnull(StockOut,0) ) ExpectedQty,
					                                             (InStock+Purchase+Produced-Consumption-Wastage-ReturnAmount-isnull(StockOut,0) )*UnitPrice ExpectedValue, 
					                                             isnull(PhysicalInventory, ClosingStock) PhysicalInventory, 
					                                             (UnitPrice*(isnull(PhysicalInventory, ClosingStock))) PhysicalInventoryValue,
					                                             isnull(ClosingStock ,InStock + Purchase+Produced-isnull(StockOut,0)  -Wastage - ReturnAmount- Consumption) ClosingStock,
					                                             isnull(ClosingStock ,InStock + Purchase+Produced-isnull(StockOut,0) -Wastage - ReturnAmount- Consumption)*UnitPrice ClosingStockValue FROM
					                                             (
														SELECT @StartDate StartDate,
														@EndDate EndDate,
														i.Id,
					                                             i.IsIntermediate, 
					                                             i.Barcode, 
					                                             i.STARBrand Brand, 
					                                             i.STARVendor Vendor,
					                                             i.PositionValue, 
					                                             i.GroupCode, 
					                                             i.Name Inventory, 
					                                             w.Id WarehouseID, 
					                                             w.Name Warehouse, 
														i.Id InventoryID, 
					                                             i.Name, 
					                                             i.transactionunit Unit,
														StockIn =(
																	SELECT max(StockIn)
																	FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
																	WHERE 
																	pcs.Id = wcs.PeriodicConsumptionId
																	AND wcs.Id = ps.PeriodicConsumptionId 
																	AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
																	AND ps.InventoryItemId = i.Id
																	AND wcs.warehouseid = w.Id
																),
														sum(isnull(p.StockOut, 0)) StockOut ,
														InStock =(
																	SELECT  isnull(max(instock),0)
																	FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
																	WHERE 
																	pcs.Id = wcs.PeriodicConsumptionId
																	AND wcs.Id = ps.PeriodicConsumptionId 
																	AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
																	AND ps.InventoryItemId = i.Id
																	AND wcs.warehouseid = w.Id
																),
														sum(isnull(p.Wastage, 0)) Wastage ,
														sum(isnull(p.ReturnAmount,0)) ReturnAmount,
														PhysicalInventory  =(
																	SELECT max(PhysicalInventory)
																	FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
																	WHERE 
																	pcs.Id = wcs.PeriodicConsumptionId
																	AND wcs.Id = ps.PeriodicConsumptionId 
																	AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
																	AND ps.InventoryItemId = i.Id
																	AND wcs.warehouseid = w.Id
																),

														--case when p.Produced=0 then sum(p.Purchase) end as 'Purchase',
                                                        sum(p.Purchase-p.Produced) as 'Purchase',

                                                        sum(p.produced)  as 'Produced',

                                                        sum(p.Consumption) Consumption, 
														ClosingStock =(
																		SELECT (instock + purchase -isnull(stocktake,0) - isnull(Wastage, 0) - isnull(ReturnAmount, 0)- isnull(StockOut, 0)- consumption)
																		FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
																		WHERE 
																		pcs.Id = wcs.PeriodicConsumptionId
																		AND wcs.Id = ps.PeriodicConsumptionId 
																		AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
																		AND ps.InventoryItemId = i.Id
																		AND wcs.warehouseid = w.Id
																	),
														UnitPrice = isnull((
																	 SELECT Price 
																	 FROM InventoryTransactions 
																	 WHERE 
																	 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
																	 AND  id = (
																				 SELECT max(id) FROM InventoryTransactions 
																				 WHERE InventoryItem_Id =  i.id
																				 AND  InventoryTransactions.Unit = i.TransactionUnit
																				 and InventoryTransactions.Date <= @EndDate
                                                                                 AND Price != 0
																			   )				                                             
																 ),0),
														OpeningUnitPrice = isnull((
																	 SELECT Price 
																	 FROM InventoryTransactions 
																	 WHERE 
																	 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
																	 AND  id = (
																				 SELECT max(id) FROM InventoryTransactions 
																				 WHERE InventoryItem_Id =  i.id
																				 AND  InventoryTransactions.Unit = i.TransactionUnit
																				 and InventoryTransactions.Date < @StartDate
																			   )				                                             
																 ),0)		
														FROM 
														PeriodicConsumptions pc,WarehouseConsumptions wc,
														PeriodicConsumptionitems p, inventoryitems i, warehouses w
														WHERE  pc.Id = wc.PeriodicConsumptionId
														AND wc.Id = p.PeriodicConsumptionId
														AND p.InventoryItemId = i.Id
														AND wc.WarehouseId = w.Id                                
														AND pc.Id >= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
														AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
					                                             --AND p.Produced = 0 --get non-produced items only
					                                             AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
					                                             AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate
					                                             {4} 								
													 )Q
													)TQ
													ORDER BY  PositionValue",
                            dtStart.ToString("dd MMM yyyy hh:mm:ss tt"), dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"),
                            firstworkperiodid, lastworkperiodid, sqlClause);
                        SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                        da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                        da.Fill(ds, "SpecialInventoryRegister");
                    }

                }
				else
                {
                    if (isForCpmpiled)
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = dbConn;
                            cmd.CommandTimeout = dbConn.ConnectionTimeout;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "BatchInventoryPotentialFORCOMPILED";
                            cmd.Parameters.AddWithValue("@StartDateParam", dtStart.ToString("dd MMM yyyy hh:mm:ss tt"));
                            cmd.Parameters.AddWithValue("@EndDateParam", dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"));
                            cmd.Parameters.AddWithValue("@firstworkperiodid", firstworkperiodid);
                            cmd.Parameters.AddWithValue("@lastworkperiodid", lastworkperiodid);
                            cmd.Parameters.AddWithValue("@warehouseidparam", warehouseID);
                            cmd.Parameters.AddWithValue("@sqlClause", string.Empty);
                            cmd.CommandTimeout = 3600;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(ds);
                            }
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = dbConn;
                            cmd.CommandTimeout = dbConn.ConnectionTimeout;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "BatchInventoryPotential";
                            cmd.Parameters.AddWithValue("@StartDateParam", dtStart.ToString("dd MMM yyyy hh:mm:ss tt"));
                            cmd.Parameters.AddWithValue("@EndDateParam", dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"));
                            cmd.Parameters.AddWithValue("@firstworkperiodid", firstworkperiodid);
                            cmd.Parameters.AddWithValue("@lastworkperiodid", lastworkperiodid);
                            cmd.Parameters.AddWithValue("@warehouseidparam", warehouseID);
                            cmd.Parameters.AddWithValue("@sqlClause", string.Empty);
                            cmd.CommandTimeout = 3600;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(ds);
                            }
                        }
                    }
                }


                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }

        public static DataSet GetSpecialInvntoryPotentialRevenueForSingleProduct(int inventoryItemID
           , int warehouseID
           , DateTime dtStart
           , DateTime dtEnd
           , string Brand
           , string Vendor
           , string InventoryTakeType)
        {
            DataSet ds = new DataSet();
            try
            {
                Brand = Brand.Replace("'s ", "''");
                Vendor = Vendor.Replace("'s ", "''");
                InventoryTakeType = InventoryTakeType.Replace("'s ", "''");

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sqlClause = string.Empty;

                if (!string.IsNullOrEmpty(InventoryTakeType))
                {
                    sqlClause += string.Format(" AND i.InventoryTakeType in ({0})", InventoryTakeType);
                }
                if (!string.IsNullOrEmpty(Brand))
                {
                    sqlClause = sqlClause + string.Format(" and i.STARBrand in ({0})", Brand);
                }
                if (Vendor != "All")
                {
                    sqlClause = sqlClause + string.Format(" and i.STARVendor = '{0}'", Vendor);
                }

                string sql = $@"
											DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100)
											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{dtStart.ToString("dd MMM yyyy hh: mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'))
                                            SET @EndDate = (SELECT [dbo].[ufsFormat] ('{dtEnd.ToString("dd MMM yyyy hh:mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'))
											SELECT 
                                                i.GroupCode,
                                                i.STARVendor,
                                                i.Barcode,
                                                p.UnitName Unit,
                                                p.Cost BatchPrice,
                                                pc.SyncOutletId WarehouseID, 
                                                w.Name Warehouse, 
                                                convert(varchar, pc.StartDate, 106) StartDate, 
                                                convert(varchar, pc.EndDate, 106) EndDate, 
                                                p.InventoryItemId InventoryId, 
                                                p.InventoryItemName Inventory,
                                                p.Instock Opening_Stock, 
                                                p.instockvalue Opening_StockValue, 
                                                p.Purchase PurchaseORTransfer, 
                                                p.PurchaseValue PurchaseORTransferValue, 
                                                p.Produced , 
                                                p.ProducedValue ,
                                                p.PositiveStockAdjustment, 
                                                p.PositiveStockAdjustmentValue,
                                                p.LatestPrice Last_Unit_Price,
                                                p.Consumption, 
                                                p.ConsumptionValue, 
                                                p.ConsumptionCarryForward, 
                                                p.ConsumptionCarryForwardValue,
                                                p.Production, 
                                                p.ProductionValue, 
                                                ProductionPurposeCarryForward, 
                                                ProductionPurposeCarryForwardValue,
                                                p.Wastage, 
                                                p.WastageValue, 
                                                p.WastageCarryForward, 
                                                p.WastageCarryForwardValue,
                                                p.StockOut, 
                                                p.StockOutValue, 
                                                p.StockOutCarryForward, 
                                                p.StockOutCarryForwardValue,
                                                p.ReturnAmount, 
                                                p.ReturnAmountValue ReturnValue, 
                                                p.ReturnAmountCarryForward, 
                                                p.ReturnAmountCarryForwardValue,
                                                p.NegativeStockAdjustment, 
                                                p.NegativeStockAdjustmentValue, 
                                                p.NegativeStockAdjustmentCarryForward, 
                                                p.Instock + p.Purchase + p.Produced - p.ClosingStock ActualConsumption, 
                                                p.instockvalue + p.PurchaseValue + p.ProducedValue - p.ClosingStockValue ActualConsumptionValue,
                                                p.NegativeStockAdjustmentCarryForwardValue,
                                                p.ClosingStock + p.NegativeStockAdjustment - p.PositiveStockAdjustment StandardQty,
                                                p.ClosingStockValue + p.NegativeStockAdjustmentValue - p.PositiveStockAdjustmentValue StandardValue,
                                                p.PhysicalInventory, 
                                                p.PhysicalInventoryValue,
                                                p.ClosingStock, 
                                                p.ClosingStockValue,
                                                p.PositiveStockAdjustment - p.NegativeStockAdjustment VarianceQty,
                                                p.PositiveStockAdjustmentValue - p.NegativeStockAdjustmentValue VarianceValue,
                                                p.BatchInfo
                                                FROM 
                                                PeriodicConsumptions pc,WarehouseConsumptions wc,
                                                PeriodicConsumptionitems p, inventoryitems i, warehouses w
                                                WHERE  pc.Id = wc.PeriodicConsumptionId
                                                AND wc.Id = p.PeriodicConsumptionId
                                                AND p.InventoryItemId = i.Id
                                                AND wc.WarehouseId = w.Id                                               
                                                AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
                                                AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate
                                                AND w.Id = {warehouseID}
                                                AND i.Id = {inventoryItemID}
                                                {sqlClause}
                                                ORDER BY i.Id, pc.id";

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "SpecialInventoryRegister");


                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }

        public static DataSet GetSpecialInvntoryPotentialRevenue(string inventoryItemGroups
           , string inventoryItemIDs
           , string warehouseIDs
           , DateTime dtStart
           , DateTime dtEnd
           , string Brand
           , string Vendor
           , string InventoryTakeType
          )
        {
            DataSet ds = new DataSet();
            try
            {

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                inventoryItemGroups = inventoryItemGroups.Replace("'s ", "''");
                inventoryItemIDs = inventoryItemIDs.Replace("'s ", "''");
                warehouseIDs = warehouseIDs.Replace("'s ", "''");
                Brand = Brand.Replace("'s ", "''");
                Vendor = Vendor.Replace("'s ", "''");
                InventoryTakeType = InventoryTakeType.Replace("'s ", "''");
                string sqlClause = string.Empty;
                if (warehouseIDs == "0")
                {
                    warehouseIDs = "select id from warehouses";
                }
                if(inventoryItemIDs == "0")
                {
                    inventoryItemIDs = "select id from inventoryItems";
                }
                if(!string.IsNullOrEmpty(inventoryItemGroups))
                {
                    sqlClause += string.Format(" AND i.GroupCode in ({0})", inventoryItemGroups);
                }
                if (!string.IsNullOrEmpty(InventoryTakeType))
                {
                    sqlClause += string.Format(" AND i.InventoryTakeType in ({0})", InventoryTakeType);
                }
                if (!string.IsNullOrEmpty(Brand))
                {
                    sqlClause = sqlClause + string.Format(" and i.STARBrand in ({0})", Brand);
                }
                if (Vendor != "All")
                {
                    sqlClause = sqlClause + string.Format(" and i.STARVendor = '{0}'", Vendor);
                }


                string sql = $@"
											DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100),
                                            @DPStartDate Varchar(100),
											@DPEndDate varchar(100);
											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{dtStart.ToString("dd MMM yyyy hh: mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'))
                                            SET @EndDate = (SELECT [dbo].[ufsFormat] ('{dtEnd.ToString("dd MMM yyyy hh:mm:ss tt")}', 'yyyy-mm-dd hh:mm:ss'))
                                            SET @DPStartDate = (SELECT CONVERT(VARCHAR(11), '{dtStart.ToString("dd MMM yyyy hh: mm:ss tt")}', 106) AS [DD MON YYYY])
											SET @DPEndDate = (SELECT CONVERT(VARCHAR(11), '{dtEnd.ToString("dd MMM yyyy hh:mm:ss tt")}', 106) AS [DD MON YYYY])
											SELECT 
                                            i.GroupCode,
                                            i.STARVendor,
                                            i.Barcode,
                                            p.UnitName Unit,
                                            p.Cost BatchPrice,
                                            w.Id WarehouseID, 
                                            w.Name Warehouse, 
                                            CONVERT(VARCHAR(11), StartDate, 106) StartDate , 
                                            CONVERT(VARCHAR(11), EndDate, 106) EndDate, 
                                            p.InventoryItemId InventoryId, 
                                            p.InventoryItemName Inventory,
                                            p.Instock Opening_Stock, 
                                            p.instockvalue Opening_StockValue, 
                                            p.Purchase PurchaseORTransfer, 
                                            p.PurchaseValue PurchaseORTransferValue, 
                                            p.Produced , 
                                            p.ProducedValue ,
                                            p.PositiveStockAdjustment, 
                                            p.PositiveStockAdjustmentValue,
                                            p.Consumption, 
                                            p.ConsumptionValue, 
                                            p.ConsumptionCarryForward, 
                                            p.ConsumptionCarryForwardValue,
                                            p.Production, 
                                            p.ProductionValue,
                                            p.LatestPrice Last_Unit_Price,
                                            ProductionPurposeCarryForward, 
                                            ProductionPurposeCarryForwardValue,
                                            p.Instock + p.Purchase + p.Produced - p.ClosingStock ActualConsumption, 
                                            p.instockvalue + p.PurchaseValue + p.ProducedValue - p.ClosingStockValue ActualConsumptionValue,
                                            p.Wastage, 
                                            p.WastageValue, 
                                            p.WastageCarryForward, 
                                            p.WastageCarryForwardValue,
                                            p.StockOut, 
                                            p.StockOutValue, 
                                            p.StockOutCarryForward, 
                                            p.StockOutCarryForwardValue,
                                            p.ReturnAmount, 
                                            p.ReturnAmountValue ReturnValue, 
                                            p.ReturnAmountCarryForward, 
                                            p.ReturnAmountCarryForwardValue,
                                            p.NegativeStockAdjustment, 
                                            p.NegativeStockAdjustmentValue, 
                                            p.NegativeStockAdjustmentCarryForward, 
                                            p.NegativeStockAdjustmentCarryForwardValue,
                                            p.ClosingStock + p.NegativeStockAdjustment - p.PositiveStockAdjustment StandardQty,
                                            p.ClosingStockValue + p.NegativeStockAdjustmentValue - p.PositiveStockAdjustmentValue StandardValue,
                                            isnull(p.PhysicalInventory,0) PhysicalInventory, 
                                            isnull(p.PhysicalInventoryValue,0) PhysicalInventoryValue,
                                            p.ClosingStock, 
                                            p.ClosingStockValue,
                                            p.PositiveStockAdjustment - p.NegativeStockAdjustment VarianceQty,
                                            p.PositiveStockAdjustmentValue - p.NegativeStockAdjustmentValue VarianceValue,
                                            p.BatchInfo
	                                        INTO ##T
                                            FROM 
                                            PeriodicConsumptions pc,
	                                        WarehouseConsumptions wc,
                                            PeriodicConsumptionitems p, 
	                                        inventoryitems i, 
	                                        warehouses w
                                            WHERE  pc.Id = wc.PeriodicConsumptionId
                                            AND wc.Id = p.PeriodicConsumptionId
                                            AND p.InventoryItemId = i.Id
                                            AND wc.WarehouseId = w.Id                                               
                                            AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
                                            AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate
                                            AND w.Id in ({warehouseIDs})
                                            AND i.Id in ({inventoryItemIDs})
                                            {sqlClause}
                                            ORDER BY i.Id, pc.id,StartDate
	                                        select  MAX(GroupCode) GroupCode,
                                            MAX(STARVendor) STARVendor,
                                            MAX(Barcode) Barcode,
                                            MAX(Unit) Unit,
                                            MAX(Warehouse) Warehouse, 
	                                        CONVERT(VARCHAR(11),min(convert(date, CurrentTable.StartDate, 101)),106) StartDate, 
                                            CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) EndDate, 
                                            InventoryId, 
                                            MAX(Inventory) Inventory,
                                            Opening_Stock = (SELECT Opening_Stock FROM ##T t where t.StartDate=CONVERT(VARCHAR(11),min(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId), 
                                            Opening_StockValue = (SELECT Opening_StockValue FROM ##T t where t.StartDate=CONVERT(VARCHAR(11),min(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId), 
                                            SUM(PurchaseORTransfer) PurchaseORTransfer, 
                                            SUM(PurchaseORTransferValue) PurchaseORTransferValue, 
                                            SUM(Produced) Produced, 
                                            SUM(ProducedValue) ProducedValue,
                                            SUM(PositiveStockAdjustment) PositiveStockAdjustment, 
                                            SUM(PositiveStockAdjustmentValue) PositiveStockAdjustmentValue,
                                            SUM(Consumption) Consumption, 
                                            SUM(ConsumptionValue) ConsumptionValue, 
                                            SUM(ConsumptionCarryForward) ConsumptionCarryForward, 
                                            SUM(ConsumptionCarryForwardValue) ConsumptionCarryForwardValue,
                                            SUM(Production) Production, 
                                            SUM(ProductionValue) Production, 
                                            SUM(ProductionPurposeCarryForward) ProductionPurposeCarryForward, 
                                            SUM(ProductionPurposeCarryForwardValue) ProductionPurposeCarryForwardValue,
                                            SUM(Wastage) Wastage, 
                                            SUM(WastageValue) WastageValue, 
                                            SUM(WastageCarryForward) WastageCarryForward, 
                                            SUM(WastageCarryForwardValue) WastageCarryForwardValue,
                                            SUM(StockOut) StockOut, 
                                            SUM(StockOutValue) StockOutValue, 
                                            SUM(StockOutCarryForward) StockOutCarryForward, 
                                            SUM(StockOutCarryForwardValue) StockOutCarryForwardValue,
                                            SUM(ReturnAmount) ReturnAmount,
                                            SUM(ActualConsumption) ActualConsumption,
                                            SUM(ActualConsumptionValue) ActualConsumptionValue,
                                            SUM(ReturnValue) ReturnValue, 
                                            SUM(ReturnAmountCarryForward) ReturnAmountCarryForward, 
                                            SUM(ReturnAmountCarryForwardValue) ReturnAmountCarryForwardValue,
                                            SUM(NegativeStockAdjustment) NegativeStockAdjustment, 
                                            SUM(NegativeStockAdjustmentValue) NegativeStockAdjustmentValue, 
                                            SUM(NegativeStockAdjustmentCarryForward) NegativeStockAdjustmentCarryForward, 
                                            SUM(NegativeStockAdjustmentCarryForwardValue) NegativeStockAdjustmentCarryForwardValue,
                                            (SELECT BatchPrice FROM ##T  t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId) BatchPrice,
                                            (SELECT Last_Unit_Price FROM ##T  t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId) Last_Unit_Price,
                                            (SELECT ClosingStock FROM ##T  t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId) + SUM(NegativeStockAdjustment) - SUM(PositiveStockAdjustment) StandardQty,
                                            (SELECT ClosingStockValue FROM ##T  t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId) + SUM(NegativeStockAdjustmentValue) - SUM(PositiveStockAdjustmentValue) StandardValue,
	                                        PhysicalInventory = (SELECT PhysicalInventory FROM ##T t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId), 
	                                        PhysicalInventoryValue = (SELECT PhysicalInventoryValue FROM ##T  t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId), 
                                            ClosingStock = (SELECT ClosingStock FROM ##T  t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId), 
                                            ClosingStockValue = (SELECT ClosingStockValue FROM ##T  t where t.StartDate=CONVERT(VARCHAR(11),max(convert(date, CurrentTable.StartDate, 101)),106) and t.WarehouseID=CurrentTable.WarehouseID and t.InventoryId =CurrentTable.InventoryId), 
                                            VarianceQty= SUM(PositiveStockAdjustment) - SUM(NegativeStockAdjustment) ,
                                            VarianceValue= SUM(PositiveStockAdjustmentValue) - SUM(NegativeStockAdjustmentValue)                                           
                                            from ##T CurrentTable
	                                        GROUP BY InventoryId,WarehouseID
                                            Order by InventoryId
	                                        Drop Table ##T";

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "SpecialInventoryRegister");


                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        public static DataSet GetSpecialInvntoryRegisterProduction(string inventoryGroupItem, string inventoryItemID, string warehouseID, DateTime dtStart, DateTime dtEnd, int firstworkperiodid, int lastworkperiodid, bool fifo, string Brand, string Vendor)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sqlClause = string.Empty;
                if (warehouseID != "0")
                {
                    sqlClause += " AND w.Id in(" + Convert.ToInt32(warehouseID) + ")";
                }
                if (inventoryItemID != "0")
                {
                    sqlClause += " AND i.Id in(" + Convert.ToInt32(inventoryItemID) + ")";
                }
                //else if (inventoryGroupItem != "All")
                else if (!string.IsNullOrEmpty(inventoryGroupItem))
                {
                    sqlClause += string.Format(" AND i.GroupCode in ({0})", inventoryGroupItem);
                }
                if (Brand != "All")
                {
                    sqlClause = sqlClause + string.Format(" and i.STARBrand = '{0}'", Brand);
                }
                if (Vendor != "All")
                {
                    sqlClause = sqlClause + string.Format(" and i.STARVendor = '{0}'", Vendor);
                }
                sqlClause += @" GROUP BY i.GroupCode, i.Name, w.Id, w.Name, 
								i.Id,i.PositionValue , i.IsIntermediate, i.Barcode, i.STARBrand, i.STARVendor, i.Name, i.transactionunit";

                if (!fifo)
                {
                    string sql = string.Format(@"
											DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100)

											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'yyyy-mm-dd hh:mm:ss'))
											SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'))

											select *,
                                                (Opening_Stock+PurchaseORTransfer-PhysicalInventory) ActualConsumption,
                                                (Opening_Stock+PurchaseORTransfer-PhysicalInventory)*Last_Unit_Price ActualConsumptionValue,
                                                PhysicalInventory-ExpectedQty CountVarianceQty ,
                                                (PhysicalInventoryValue-ExpectedValue) CountVarianceValue  
                                                from(
												Select
												Id,
                                                IsIntermediate,
                                                Barcode,
                                                Brand,
                                                Vendor,
                                                StartDate,	
                                                EndDate, 	
                                                GroupCode,	
                                                Inventory, 
                                                WarehouseID,	
                                                Warehouse,	
                                                InventoryID, 
                                                Unit,
                                                UnitPrice Last_Unit_Price, 
                                                InStock Opening_Stock,
                                                InStock*Unitprice Opening_StockValue, 
                                                purchase PurchaseORTransfer, 
                                                Purchase*UnitPrice PurchaseORTransferValue,
                                                isnull(StockOut,0) StockOut, 
                                                isnull(StockOut,0)*UnitPrice StockOutValue,
                                                Consumption,
                                                Consumption*UnitPrice ConsumptionValue,
                                                Wastage, 
                                                Wastage*UnitPrice WastageValue, 
                                                ReturnAmount,
                                                ReturnAmount*UnitPrice ReturnValue,
                                                (InStock+Purchase-Consumption-Wastage-ReturnAmount-isnull(StockOut,0)) ExpectedQty,
                                                (InStock+Purchase-Consumption-Wastage-ReturnAmount-isnull(StockOut,0) )*UnitPrice ExpectedValue, 
                                                isnull(PhysicalInventory, ClosingStock) PhysicalInventory, 
                                                (UnitPrice*(isnull(PhysicalInventory, ClosingStock))) PhysicalInventoryValue,
                                                isnull(ClosingStock ,InStock + Purchase -Wastage - ReturnAmount- Consumption -isnull(StockOut,0)) ClosingStock,
                                                isnull(ClosingStock ,InStock + Purchase -Wastage - ReturnAmount- Consumption-isnull(StockOut,0))*UnitPrice ClosingStockValue 
                                                FROM
											    (
												    SELECT @StartDate StartDate,
												    @EndDate EndDate,
												    i.Id,
                                                    i.IsIntermediate, 
                                                    i.Barcode, 
                                                    i.STARBrand Brand, 
                                                    i.STARVendor Vendor, 
                                                    i.GroupCode, 
                                                    i.Name Inventory, 
                                                    w.Id WarehouseID, 
                                                    w.Name Warehouse, 
                                                    i.Id InventoryID, 
                                                    i.Name, 
                                                    i.transactionunit Unit,
												    StockIn =(
															    SELECT max(StockIn)
															    FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															    WHERE 
															    pcs.Id = wcs.PeriodicConsumptionId
															    AND wcs.Id = ps.PeriodicConsumptionId 
															    AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															    AND ps.InventoryItemId = i.Id
															    AND wcs.warehouseid = w.Id
														    ),
												    StockOut =(
															    SELECT max(StockOut)
															    FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															    WHERE 
															    pcs.Id = wcs.PeriodicConsumptionId
															    AND wcs.Id = ps.PeriodicConsumptionId 
															    AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															    AND ps.InventoryItemId = i.Id
															    AND wcs.warehouseid = w.Id
														    ),
												    InStock =(
															    SELECT  isnull(max(instock),0)
															    FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															    WHERE 
															    pcs.Id = wcs.PeriodicConsumptionId
															    AND wcs.Id = ps.PeriodicConsumptionId 
															    AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
															    AND ps.InventoryItemId = i.Id
															    AND wcs.warehouseid = w.Id
														    ),
												    sum(isnull(p.Wastage, 0)) Wastage ,
												    sum(isnull(p.ReturnAmount,0)) ReturnAmount,
												    PhysicalInventory  =(
															    SELECT max(PhysicalInventory)
															    FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
															    WHERE 
															    pcs.Id = wcs.PeriodicConsumptionId
															    AND wcs.Id = ps.PeriodicConsumptionId 
															    AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
															    AND ps.InventoryItemId = i.Id
															    AND wcs.warehouseid = w.Id
														    ),
					
												    sum(p.Purchase) 'Purchase', 
                                                    sum(p.Consumption) Consumption, 
												    ClosingStock =(
																    SELECT (instock + purchase - isnull(Wastage, 0) - isnull(ReturnAmount, 0) - consumption)
																    FROM PeriodicConsumptions pcs, WarehouseConsumptions wcs, PeriodicConsumptionitems ps 
																    WHERE 
																    pcs.Id = wcs.PeriodicConsumptionId
																    AND wcs.Id = ps.PeriodicConsumptionId 
																    AND pcs.Id = (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})
																    AND ps.InventoryItemId = i.Id
																    AND wcs.warehouseid = w.Id
															    ),
												    UnitPrice = isnull((
															     SELECT Price 
															     FROM InventoryTransactions 
															     WHERE 
															     InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
															     AND  id = (
																		     SELECT max(id) FROM InventoryTransactions 
																		     WHERE InventoryItem_Id =  i.id
																		     AND  InventoryTransactions.Unit = i.TransactionUnit
																		     and InventoryTransactions.Date <= @EndDate
																	       )				                                             
														     ),0)	
												    FROM 
												    PeriodicConsumptions pc,WarehouseConsumptions wc,
												    PeriodicConsumptionitems p, inventoryitems i, warehouses w
												    WHERE  pc.Id = wc.PeriodicConsumptionId
												    AND wc.Id = p.PeriodicConsumptionId
												    AND p.InventoryItemId = i.Id
												    AND wc.WarehouseId = w.Id
												    And i.IsIntermediate = 1                                
                                                AND [dbo].[ufsFormat] (pc.StartDate,'yyyy-mm-dd hh:mm:ss') >= @StartDate
                                                AND [dbo].[ufsFormat] (pc.EndDate ,'yyyy-mm-dd hh:mm:ss')<= @EndDate 
                                                {4} 
								
											 )Q
											)TQ
											ORDER BY  GroupCode, Inventory", dtStart.ToString("dd MMM yyyy hh:mm:ss tt"), dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"), firstworkperiodid, lastworkperiodid, sqlClause);

                    SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                    da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                    da.Fill(ds, "SpecialInventoryRegister");
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = dbConn;
                        cmd.CommandTimeout = dbConn.ConnectionTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "BatchInventoryPotential";
                        cmd.Parameters.AddWithValue("@StartDateParam", dtStart.ToString("dd MMM yyyy hh:mm:ss tt"));
                        cmd.Parameters.AddWithValue("@EndDateParam", dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"));
                        cmd.Parameters.AddWithValue("@firstworkperiodid", firstworkperiodid);
                        cmd.Parameters.AddWithValue("@lastworkperiodid", lastworkperiodid);
                        cmd.Parameters.AddWithValue("@warehouseidparam", warehouseID);
                        cmd.Parameters.AddWithValue("@sqlClause", string.Empty);
                        cmd.CommandTimeout = 3600;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }
                    }
                }
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }

        public static DataTable GetWorkPeriodWiseInvntoryLedger(int InventoryID, int WarehouseID, string StartDate, string EndDate)
        {
            DataTable ds = new DataTable("TransferredTo");
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT 
											[dbo].[ufsFormat] (d.Date, 'mmm dd yyyy hh:mm AM/PM') TranDate, d.Name TranName, 
											'SourceWarehouse' = (select Name from warehouses where id = t.SourceWarehouseId), 
											wtarget.Name TargetWarehouseName,
											i.GroupCode, i.Name, t.Quantity TransferredTo, 0.00 TransferredFrom,  t.Unit, t.Price TransferPrice, (t.Quantity*t.Price) TransferTotal
											FROM InventoryTransactionDocuments d, inventorytransactions t, inventoryitems i,  warehouses wtarget
											WHERE 
											d.Id = t.InventoryTransactionDocumentId AND 
											t.InventoryItem_Id = i.Id AND                                            
											wtarget.Id = t.TargetWarehouseId AND
											wtarget.Id = {0} AND
											i.Id = {1} AND 
											d.Date BETWEEN cast('{2}' as datetime) AND cast('{3}' as datetime) ", WarehouseID, InventoryID, StartDate, EndDate);

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


        public static DataTable GetWorkPeriodWiseInvntoryTransferToAnotherWarehouseLedger(int InventoryID, int WarehouseID, string StartDate, string EndDate)
        {
            DataTable ds = new DataTable("TransferredFrom");
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT 
											[dbo].[ufsFormat] (d.Date, 'mmm dd yyyy hh:mm AM/PM') TranDate, d.Name TranName,  
											wsource.Name SourceWarehouse, 
											'TargetWarehouse' = (select Name from warehouses where id = t.targetWarehouseId), 
											i.GroupCode, i.Name, 0.00 TransferredTo, t.Quantity TransferredFrom,  t.Unit, t.Price TransferPrice, t.Quantity*t.Price TransferTotal
											FROM InventoryTransactionDocuments d, inventorytransactions t, inventoryitems i,  warehouses wsource
											WHERE 
											d.Id = t.InventoryTransactionDocumentId AND 
											t.InventoryItem_Id = i.Id AND 
											wsource.Id = t.SourceWarehouseId AND                                           
											wsource.Id = {0} AND
											i.Id = {1} AND 
											d.Date BETWEEN cast('{2}' as datetime) AND cast('{3}' as datetime) ", WarehouseID, InventoryID, StartDate, EndDate);

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

        public static DataTable GetWorkPeriodConsumption(int InventoryID, int WarehouseID, string StartDate, string EndDate)
        {
            DataTable ds = new DataTable("Consumption");
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT 
											o.CreatedDateTime TicketTime,o.CreatingUserName, t.TicketNumber, mi.GroupCode, mi.Name+'('+ 
											mp.Name+')' ItemName, cast(o.Quantity AS INT) OrderQty,
											r.Name RecipeName, /*i.Id InventoryID, i.Name,*/ o.Quantity*ri.Quantity Consumption, i.BaseUnit
											FROM Tickets t, orders o, 
											menuitems mi, menuitemportions mp,
											recipes r, recipeitems ri, inventoryitems i
											WHERE t.Id = o.TicketId
											AND o.MenuItemId = mi.Id
											AND o.DecreaseInventory = 1 
											AND o.PortionName = mp.Name
											AND mi.Id = mp.MenuItemId
											AND mp.Id = r.Portion_Id
											AND r.Id = ri.RecipeId
											AND ri.InventoryItem_Id = i.Id
											AND o.WarehouseId = {0}
											and i.Id = {1}
											AND t.LastUpdateTime >= '{2}'
											AND t.LastUpdateTime <= '{3}'
											Order by o.CreatedDateTime"
                                            , WarehouseID, InventoryID, StartDate, EndDate);

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
        public static DataSet GetInventoryTakeType()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = "select distinct InventoryTakeType FROM InventoryItems where InventoryTakeType is not null";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
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
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = "select distinct GroupCode FROM InventoryItems order by GroupCode";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetProductionGroupItem()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = "select distinct GroupCode FROM InventoryItems where IsIntermediate = 1 order by GroupCode";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetBrand()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = "select distinct STARBrand Brand FROM InventoryItems where STARBrand is not null order by STARBrand";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetVendor()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = "select distinct STARVendor Vendor FROM InventoryItems where STARVendor is not null order by STARVendor";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetInventoryItem(string search)
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
                    sql = "select id,Name FROM InventoryItems order by Name";
                }
                else
                {
                    sql = string.Format("select id,Name FROM InventoryItems where GroupCode='{0}' order by Name", search);
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetProductionInventoryItem(string search)
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
                    sql = "select id,Name FROM InventoryItems where IsIntermediate = 1 order by PositionValue";
                }
                else
                {
                    sql = string.Format("select id,Name FROM InventoryItems where GroupCode='{0}' and IsIntermediate = 1 order by PositionValue", search);
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetOpeningClosingPurchaseStockValue(string fromDate, string toDate, string GroupCodes, string Brands)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(GroupCodes))
                {
                    sqlClause += string.Format(" AND invItems.GroupCode in ({0})", GroupCodes);
                }
                if (!string.IsNullOrEmpty(Brands))
                {
                    sqlClause += string.Format(" AND invItems.STARBrand in ({0})", Brands);
                }
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@" WITH summary AS 
									(
										SELECT p.InventoryItem_Id, 
											   p.Price, p.Unit,
											   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																	 ORDER BY Id DESC) AS rk
										FROM InventoryTransactions p WHERE p.Date < '{0}'
									)

									SELECT --*,
									/*CASE  
									WHEN CurrentStock < 0 THEN 0 
									Else CurrentStock*Cost
									END AS StockValue*/
									sum(CurrentStock*Cost*checksum) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost=Isnull((
												SELECT max(
															CASE  WHEN Unit = BaseUnit THEN Price 
															  Else Price
															  END 
														  )as Price FROM 
														(  
																SELECT s.*
																FROM summary s
																WHERE s.rk = 1
					
														)t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = invItems.Id
											  ), 0), 
										pci.Consumption as InventoryConsumption, 
										(pci.InStock+pci.Purchase-pci.Consumption) as CurrentStock, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0 
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.StartDate < '{0}' {2})
									  )Q;

										SELECT  sum(Quantity*Price) AS TotalPrice  FROM 
											InventoryTransactionDocuments d, inventorytransactions t, inventoryitems invItems, warehouses w
											WHERE d.Id = t.InventoryTransactionDocumentId
											AND sourcewarehouseid = 0
											AND w.Id = targetwarehouseid
											AND invItems.Id = t.InventoryItem_Id
											AND d.date BETWEEN '{0}' AND '{1}' {2};

										WITH summary AS 
										(
											SELECT p.InventoryItem_Id, 
												   p.Price, p.Unit,
												   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																		 ORDER BY Id DESC) AS rk
											FROM InventoryTransactions p WHERE p.Date < '{1}'
										)

										SELECT
										sum(InventoryPrediction*Cost*checksum) AS TotalValueOfStock
										FROM 
										(
											SELECT w.Name Warehouse, pci.InventoryItemName,
											pci.UnitName as Unit, 
											pci.InStock,
											pci.Purchase, 
											Cost=Isnull((
													SELECT max(
																CASE  WHEN Unit = BaseUnit THEN Price 
																  Else Price
																  END 
															  )as Price FROM 
															(  
																	SELECT s.*
																	FROM summary s
																	WHERE s.rk = 1
					
															)t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = invItems.Id
												  ), 0), 
											pci.Consumption as InventoryConsumption, 
											(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
											((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
											CASE  
											WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
											Else 1
											END AS CheckSum
											From PeriodicConsumptionItems pci 
											inner join WarehouseConsumptions wc 
											on wc.Id = pci.PeriodicConsumptionId 
											inner join InventoryItems invItems 
											on invItems.Id=pci.InventoryItemId 
											INNER JOIN warehouses w
											ON w.Id = wc.WarehouseId
											where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.EndDate < '{1}' {2})
										)Q ;", fromDate, toDate, sqlClause);

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
        public static DataSet GetOpeningClosingPurchaseStockValue(string fromDate, string toDate, int warehouseId, string GroupCodes, string Brands)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string sqlCaluse = string.Empty;
                if (warehouseId > 0)
                {
                    sqlCaluse = string.Format(@" and w.Id in ({0})", warehouseId);
                }

                if (!string.IsNullOrEmpty(GroupCodes))
                {
                    sqlCaluse += string.Format(" AND invItems.GroupCode in ({0})", GroupCodes);
                }
                if (!string.IsNullOrEmpty(Brands))
                {
                    sqlCaluse += string.Format(" AND invItems.STARBrand in ({0})", Brands);
                }
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@"
								  select SUM(InStock*Cost) as TotalValueOfStock from (
									SELECT w.Name Warehouse, pci.InventoryItemName, 
									pci.UnitName as Unit, 
									pci.InStock,
									pci.Purchase, 
									Cost = isnull((
															SELECT Price 
															FROM InventoryTransactions 
															WHERE 
															InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
															AND  id = (
																		SELECT max(id) FROM InventoryTransactions 
																		WHERE InventoryItem_Id =  invItems.Id
																		 AND  InventoryTransactions.Unit = invItems.TransactionUnit
																		and InventoryTransactions.Date < '{0}'
																	)				                                             
														),0)	, 
									pci.Consumption as InventoryConsumption, 
									(pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) as CurrentStock, 
									((pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut)*pci.Cost) as CurrentInventory, isnull(pci.physicalInventory,pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) StockTake,
									CASE  
									WHEN (pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) < 0 THEN 0 
									Else 1
									END AS CheckSum,pci.physicalInventory
									From PeriodicConsumptionItems pci 
									inner join WarehouseConsumptions wc 
									on wc.Id = pci.PeriodicConsumptionId 
									inner join InventoryItems invItems 
									on invItems.Id=pci.InventoryItemId 
									INNER JOIN warehouses w
									ON w.Id = wc.WarehouseId
									where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.StartDate <='{0}')
									   {2}

									)Q;

									----StockIn-----
									select SUM(Purchase*Cost) as TotalPrice from (
									SELECT w.Name Warehouse, pci.InventoryItemName, 
									pci.UnitName as Unit, 
									pci.InStock,
									pci.Purchase, 
									Cost = isnull((
															SELECT Price 
															FROM InventoryTransactions 
															WHERE 
															InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
															AND  id = (
																		SELECT max(id) FROM InventoryTransactions 
																		WHERE InventoryItem_Id =  invItems.Id
																		 AND  InventoryTransactions.Unit = invItems.TransactionUnit
																		and InventoryTransactions.Date <= '{1}'
																	)				                                             
														),0)	, 
									pci.Consumption as InventoryConsumption, 
									(pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) as CurrentStock, 
									((pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut)*pci.Cost) as CurrentInventory, isnull(pci.physicalInventory,pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) StockTake,
									CASE  
									WHEN (pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) < 0 THEN 0 
									Else 1
									END AS CheckSum,pci.physicalInventory
									From PeriodicConsumptionItems pci 
									inner join WarehouseConsumptions wc 
									on wc.Id = pci.PeriodicConsumptionId 
									inner join InventoryItems invItems 
									on invItems.Id=pci.InventoryItemId 
									INNER JOIN warehouses w
									ON w.Id = wc.WarehouseId
									where wc.PeriodicConsumptionId IN  (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;

									----Closing-----   
									
									select SUM(stocktake*Cost) as TotalValueOfStock from (
									SELECT w.Name Warehouse, pci.InventoryItemName, 
									pci.UnitName as Unit, 
									pci.InStock,
									pci.Purchase, 
									Cost = isnull((
															SELECT Price 
															FROM InventoryTransactions 
															WHERE 
															InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
															AND  id = (
																		SELECT max(id) FROM InventoryTransactions 
																		WHERE InventoryItem_Id =  invItems.Id
																		 AND  InventoryTransactions.Unit = invItems.TransactionUnit
																		and InventoryTransactions.Date <= '{1}'
																	)				                                             
														),0)	, 
									pci.Consumption as InventoryConsumption, 
									(pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) as CurrentStock, 
									((pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut)*pci.Cost) as CurrentInventory, isnull(pci.physicalInventory,pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) StockTake,
									CASE  
									WHEN (pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) < 0 THEN 0 
									Else 1
									END AS CheckSum,pci.physicalInventory
									From PeriodicConsumptionItems pci 
									inner join WarehouseConsumptions wc 
									on wc.Id = pci.PeriodicConsumptionId 
									inner join InventoryItems invItems 
									on invItems.Id=pci.InventoryItemId 
									INNER JOIN warehouses w
									ON w.Id = wc.WarehouseId
									where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.StartDate <='{1}')
									   {2}

									)Q;
											
									----Wastage-----
									SELECT
									sum(wastage*Cost) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost = isnull((
										SELECT Price 
										FROM InventoryTransactions 
										WHERE 
										InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
										AND  id = (
													SELECT max(id) FROM InventoryTransactions 
													WHERE InventoryItem_Id =  invItems.Id
														AND  InventoryTransactions.Unit = invItems.TransactionUnit
													and InventoryTransactions.Date <= '{1}'
												)				                                             
											),0), 
										pci.Consumption as InventoryConsumption, pci.wastage,
										(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										inner join SyncOutletWarehouses sw
										on w.Id = sw.WarehouseId
										where wc.PeriodicConsumptionId IN (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;

									----StockOut----
									SELECT
									sum(StockOut*Cost) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost = isnull((
										SELECT Price 
										FROM InventoryTransactions 
										WHERE 
										InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
										AND  id = (
													SELECT max(id) FROM InventoryTransactions 
													WHERE InventoryItem_Id =  invItems.Id
														AND  InventoryTransactions.Unit = invItems.TransactionUnit
													and InventoryTransactions.Date <= '{1}'
												)				                                             
											),0), 
										pci.Consumption as InventoryConsumption, pci.stockout,
										(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										inner join SyncOutletWarehouses sw
										on w.Id = sw.WarehouseId
										where wc.PeriodicConsumptionId IN (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;
											
									----Return----
									SELECT
									sum(returnamount*Cost) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost = isnull((
										SELECT Price 
										FROM InventoryTransactions 
										WHERE 
										InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
										AND  id = (
													SELECT max(id) FROM InventoryTransactions 
													WHERE InventoryItem_Id =  invItems.Id
														AND  InventoryTransactions.Unit = invItems.TransactionUnit
													and InventoryTransactions.Date <= '{1}'
												)				                                             
											),0), 
										pci.Consumption as InventoryConsumption, pci.returnamount,
										(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										inner join SyncOutletWarehouses sw
										on w.Id = sw.WarehouseId
										where wc.PeriodicConsumptionId IN (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;
											", fromDate, toDate, sqlCaluse);

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
        public static DataSet GetOpeningClosingPurchaseStockValueWithDepartmentFilter(string fromDate, string toDate, string GroupCodes, string Brands, string departmentIds)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(GroupCodes))
                {
                    sqlClause += string.Format(" AND invItems.GroupCode in ({0})", GroupCodes);
                }
                if (!string.IsNullOrEmpty(Brands))
                {
                    sqlClause += string.Format(" AND invItems.STARBrand in ({0})", Brands);
                }
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@" WITH summary AS 
									(
										SELECT p.InventoryItem_Id, 
											   p.Price, p.Unit,
											   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																	 ORDER BY Id DESC) AS rk
										FROM InventoryTransactions p WHERE p.Date < '{0}'
									)

									SELECT --*,
									/*CASE  
									WHEN CurrentStock < 0 THEN 0 
									Else CurrentStock*Cost
									END AS StockValue*/
									sum(CurrentStock*Cost*checksum) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost=Isnull((
												SELECT max(
															CASE  WHEN Unit = BaseUnit THEN Price 
															  Else Price
															  END 
														  )as Price FROM 
														(  
																SELECT s.*
																FROM summary s
																WHERE s.rk = 1
					
														)t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = invItems.Id
											  ), 0), 
										pci.Consumption as InventoryConsumption, 
										(pci.InStock+pci.Purchase-pci.Consumption) as CurrentStock, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0 
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.StartDate < '{0}' {2})
									  )Q;

										SELECT  sum(Quantity*Price) AS TotalPrice  FROM 
											InventoryTransactionDocuments d, inventorytransactions t, inventoryitems invItems, warehouses w
											WHERE d.Id = t.InventoryTransactionDocumentId
											AND sourcewarehouseid = 0
											AND w.Id = targetwarehouseid
											AND invItems.Id = t.InventoryItem_Id
											AND d.date BETWEEN '{0}' AND '{1}' {2};

										WITH summary AS 
										(
											SELECT p.InventoryItem_Id, 
												   p.Price, p.Unit,
												   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																		 ORDER BY Id DESC) AS rk
											FROM InventoryTransactions p WHERE p.Date < '{1}'
										)

										SELECT
										sum(InventoryPrediction*Cost*checksum) AS TotalValueOfStock
										FROM 
										(
											SELECT w.Name Warehouse, pci.InventoryItemName,
											pci.UnitName as Unit, 
											pci.InStock,
											pci.Purchase, 
											Cost=Isnull((
													SELECT max(
																CASE  WHEN Unit = BaseUnit THEN Price 
																  Else Price
																  END 
															  )as Price FROM 
															(  
																	SELECT s.*
																	FROM summary s
																	WHERE s.rk = 1
					
															)t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = invItems.Id
												  ), 0), 
											pci.Consumption as InventoryConsumption, 
											(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
											((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
											CASE  
											WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
											Else 1
											END AS CheckSum
											From PeriodicConsumptionItems pci 
											inner join WarehouseConsumptions wc 
											on wc.Id = pci.PeriodicConsumptionId 
											inner join InventoryItems invItems 
											on invItems.Id=pci.InventoryItemId 
											INNER JOIN warehouses w
											ON w.Id = wc.WarehouseId
											where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.EndDate < '{1}' {2})
										)Q;", fromDate, toDate, sqlClause);

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
        public static DataSet GetOpeningClosingPurchaseStockValueWithDepartmentFilter(string fromDate, string toDate, int warehouseId, string GroupCodes, string Brands, string DepartmentIds)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string sqlCaluse = string.Empty;
                if (warehouseId > 0)
                {
                    sqlCaluse = string.Format(@" and w.Id in ({0})", warehouseId);
                }

                if (!string.IsNullOrEmpty(GroupCodes))
                {
                    sqlCaluse += string.Format(" AND invItems.GroupCode in ({0})", GroupCodes);
                }
                if (!string.IsNullOrEmpty(Brands))
                {
                    sqlCaluse += string.Format(" AND invItems.STARBrand in ({0})", Brands);
                }
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@"
								  select SUM(InStock*Cost) as TotalValueOfStock from (
									SELECT w.Name Warehouse, pci.InventoryItemName, 
									pci.UnitName as Unit, 
									pci.InStock,
									pci.Purchase, 
									Cost = isnull((
															SELECT Price 
															FROM InventoryTransactions 
															WHERE 
															InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
															AND  id = (
																		SELECT max(id) FROM InventoryTransactions 
																		WHERE InventoryItem_Id =  invItems.Id
																		 AND  InventoryTransactions.Unit = invItems.TransactionUnit
																		and InventoryTransactions.Date < '{0}'
																	)				                                             
														),0)	, 
									pci.Consumption as InventoryConsumption, 
									(pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) as CurrentStock, 
									((pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut)*pci.Cost) as CurrentInventory, isnull(pci.physicalInventory,pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) StockTake,
									CASE  
									WHEN (pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) < 0 THEN 0 
									Else 1
									END AS CheckSum,pci.physicalInventory
									From PeriodicConsumptionItems pci 
									inner join WarehouseConsumptions wc 
									on wc.Id = pci.PeriodicConsumptionId 
									inner join InventoryItems invItems 
									on invItems.Id=pci.InventoryItemId 
									INNER JOIN warehouses w
									ON w.Id = wc.WarehouseId
									where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.StartDate <='{0}')
									   {2}

									)Q;

									----StockIn-----
									select SUM(Purchase*Cost) as TotalPrice from (
									SELECT w.Name Warehouse, pci.InventoryItemName, 
									pci.UnitName as Unit, 
									pci.InStock,
									pci.Purchase, 
									Cost = isnull((
															SELECT Price 
															FROM InventoryTransactions 
															WHERE 
															InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
															AND  id = (
																		SELECT max(id) FROM InventoryTransactions 
																		WHERE InventoryItem_Id =  invItems.Id
																		 AND  InventoryTransactions.Unit = invItems.TransactionUnit
																		and InventoryTransactions.Date <= '{1}'
																	)				                                             
														),0)	, 
									pci.Consumption as InventoryConsumption, 
									(pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) as CurrentStock, 
									((pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut)*pci.Cost) as CurrentInventory, isnull(pci.physicalInventory,pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) StockTake,
									CASE  
									WHEN (pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) < 0 THEN 0 
									Else 1
									END AS CheckSum,pci.physicalInventory
									From PeriodicConsumptionItems pci 
									inner join WarehouseConsumptions wc 
									on wc.Id = pci.PeriodicConsumptionId 
									inner join InventoryItems invItems 
									on invItems.Id=pci.InventoryItemId 
									INNER JOIN warehouses w
									ON w.Id = wc.WarehouseId
									where wc.PeriodicConsumptionId IN  (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;

									----Closing-----   
									
									select SUM(stocktake*Cost) as TotalValueOfStock from (
									SELECT w.Name Warehouse, pci.InventoryItemName, 
									pci.UnitName as Unit, 
									pci.InStock,
									pci.Purchase, 
									Cost = isnull((
															SELECT Price 
															FROM InventoryTransactions 
															WHERE 
															InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
															AND  id = (
																		SELECT max(id) FROM InventoryTransactions 
																		WHERE InventoryItem_Id =  invItems.Id
																		 AND  InventoryTransactions.Unit = invItems.TransactionUnit
																		and InventoryTransactions.Date <= '{1}'
																	)				                                             
														),0)	, 
									pci.Consumption as InventoryConsumption, 
									(pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) as CurrentStock, 
									((pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut)*pci.Cost) as CurrentInventory, isnull(pci.physicalInventory,pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) StockTake,
									CASE  
									WHEN (pci.InStock+pci.Purchase-pci.Consumption-pci.Wastage-pci.ReturnAmount-pci.StockOut) < 0 THEN 0 
									Else 1
									END AS CheckSum,pci.physicalInventory
									From PeriodicConsumptionItems pci 
									inner join WarehouseConsumptions wc 
									on wc.Id = pci.PeriodicConsumptionId 
									inner join InventoryItems invItems 
									on invItems.Id=pci.InventoryItemId 
									INNER JOIN warehouses w
									ON w.Id = wc.WarehouseId
									where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.StartDate <='{1}')
									   {2}

									)Q;
											
									----Wastage-----
									SELECT
									sum(wastage*Cost) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost = isnull((
										SELECT Price 
										FROM InventoryTransactions 
										WHERE 
										InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
										AND  id = (
													SELECT max(id) FROM InventoryTransactions 
													WHERE InventoryItem_Id =  invItems.Id
														AND  InventoryTransactions.Unit = invItems.TransactionUnit
													and InventoryTransactions.Date <= '{1}'
												)				                                             
											),0), 
										pci.Consumption as InventoryConsumption, pci.wastage,
										(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										inner join SyncOutletWarehouses sw
										on w.Id = sw.WarehouseId
										where wc.PeriodicConsumptionId IN (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;

									----StockOut----
									SELECT
									sum(StockOut*Cost) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost = isnull((
										SELECT Price 
										FROM InventoryTransactions 
										WHERE 
										InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
										AND  id = (
													SELECT max(id) FROM InventoryTransactions 
													WHERE InventoryItem_Id =  invItems.Id
														AND  InventoryTransactions.Unit = invItems.TransactionUnit
													and InventoryTransactions.Date <= '{1}'
												)				                                             
											),0), 
										pci.Consumption as InventoryConsumption, pci.stockout,
										(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										inner join SyncOutletWarehouses sw
										on w.Id = sw.WarehouseId
										where wc.PeriodicConsumptionId IN (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;
											
									----Return----
									SELECT
									sum(returnamount*Cost) AS TotalValueOfStock
									FROM 
									(
										SELECT w.Name Warehouse, pci.InventoryItemName,
										pci.UnitName as Unit, 
										pci.InStock,
										pci.Purchase, 
										Cost = isnull((
										SELECT Price 
										FROM InventoryTransactions 
										WHERE 
										InventoryTransactions.InventoryItem_Id = invItems.Id  AND  InventoryTransactions.Unit = invItems.TransactionUnit
										AND  id = (
													SELECT max(id) FROM InventoryTransactions 
													WHERE InventoryItem_Id =  invItems.Id
														AND  InventoryTransactions.Unit = invItems.TransactionUnit
													and InventoryTransactions.Date <= '{1}'
												)				                                             
											),0), 
										pci.Consumption as InventoryConsumption, pci.returnamount,
										(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction, 
										((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory,
										CASE  
										WHEN (pci.InStock+pci.Purchase-pci.Consumption) < 0 THEN 0
										Else 1
										END AS CheckSum
										From PeriodicConsumptionItems pci 
										inner join WarehouseConsumptions wc 
										on wc.Id = pci.PeriodicConsumptionId 
										inner join InventoryItems invItems 
										on invItems.Id=pci.InventoryItemId 
										INNER JOIN warehouses w
										ON w.Id = wc.WarehouseId
										inner join SyncOutletWarehouses sw
										on w.Id = sw.WarehouseId
										where wc.PeriodicConsumptionId IN (select Id from PeriodicConsumptions wc WHERE wc.StartDate >='{0}'  and wc.EndDate <= '{1}')
										{2}
									)Q;
											", fromDate, toDate, sqlCaluse);

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

        public static DataSet GetOpeningStock(string StartDate)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"WITH summary AS 
											(
												SELECT p.InventoryItem_Id, 
													   p.Price, p.Unit,
													   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																			 ORDER BY Id DESC) AS rk
												FROM InventoryTransactions p WHERE p.Date < '{0}'
											)

											SELECT w.Name Warehouse, pci.InventoryItemName,
												pci.UnitName as Unit, 
												pci.InStock,
												pci.Purchase, 
												Cost=Isnull((
														SELECT max(
																	CASE  WHEN Unit = BaseUnit THEN Price 
																	  Else Price
																	  END 
																  )as Price FROM 
																(  
																		SELECT s.*
																		FROM summary s
																		WHERE s.rk = 1
					
																)t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = invItems.Id
													  ), 0), 
												pci.Consumption as InventoryConsumption, 
												(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction
												From PeriodicConsumptionItems pci 
												inner join WarehouseConsumptions wc 
												on wc.Id = pci.PeriodicConsumptionId 
												inner join InventoryItems invItems 
												on invItems.Id=pci.InventoryItemId 
												INNER JOIN warehouses w
												ON w.Id = wc.WarehouseId
												where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.StartDate < '{0}')"
                                            , StartDate);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "OpeningStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetPurchaseStock(string StartDate, string EndDate)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT d.Date, d.Name as DocName, w.Name as WarehouseName, i.Name as InventoryName , t.Quantity, t.Unit, t.Price, Quantity*Price TotalPrice  FROM 
											InventoryTransactionDocuments d, inventorytransactions t, inventoryitems i, warehouses w
											WHERE d.Id = t.InventoryTransactionDocumentId
											AND sourcewarehouseid = 0
											AND w.Id = targetwarehouseid
											AND i.Id = t.InventoryItem_Id
											AND d.date BETWEEN '{0}' AND '{1}'
											ORDER BY d.Date, d.Name , w.Name"
                                            , StartDate, EndDate);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "PurchaseStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetClosingStock(string EndDate)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"WITH summary AS 
											(
												SELECT p.InventoryItem_Id, 
													   p.Price, p.Unit,
													   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																			 ORDER BY Id DESC) AS rk
												FROM InventoryTransactions p WHERE p.Date < '{0}'
											)

											SELECT w.Name Warehouse, pci.InventoryItemName,
											pci.UnitName as Unit, 
											pci.InStock,
											pci.Purchase, 
											Cost=Isnull((
													SELECT max(
																CASE  WHEN Unit = BaseUnit THEN Price 
																  Else Price
																  END 
															  )as Price FROM 
															(  
																	SELECT s.*
																	FROM summary s
																	WHERE s.rk = 1
				
															)t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = invItems.Id
												  ), 0), 
											pci.Consumption as InventoryConsumption, 
											(pci.InStock+pci.Purchase-pci.Consumption) as InventoryPrediction 
											--,((pci.InStock+pci.Purchase-pci.Consumption)*pci.Cost) as CurrentInventory 
											From PeriodicConsumptionItems pci 
											inner join WarehouseConsumptions wc 
											on wc.Id = pci.PeriodicConsumptionId 
											inner join InventoryItems invItems 
											on invItems.Id=pci.InventoryItemId 
											INNER JOIN warehouses w
											ON w.Id = wc.WarehouseId
											where wc.PeriodicConsumptionId IN (select max(Id) from PeriodicConsumptions wc WHERE wc.EndDate < '{0}')"
                                            , EndDate);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "ClosingStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetWorkPeriodsWithWarehouse(string StartDate, string EndDate)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT wc.Id WarehouseCID, CONVERT(VARCHAR(24),w.StartDate,113) StartDate, CONVERT(VARCHAR(24),w.EndDate,113) EndDate, wh.Name WarehouseName  FROM 
												WorkPeriods w,
												PeriodicConsumptions pc, 
												WarehouseConsumptions wc, 
												Warehouses wh
												WHERE 
												pc.Id = wc.PeriodicConsumptionId
												AND w.Id = pc.WorkPeriodId
												AND wh.Id = wc.WarehouseId
												and w.StartDate >= '{0}'
												and w.EndDate <='{1}'"
                                            , StartDate, EndDate);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "WorkPeriodsWithWarehouse");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetWorkPeriodEndDrill(int WarehouseCID)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT  i.Id InventoryID,i.GroupCode, i.Name Inventory,  w.Name Warehouse, 
												i.transactionunit Unit, p.InStock OpeningStock, p.Purchase, 
												p.Consumption, isnull(Wastage, 0) Wastage, isnull(ReturnAmount, 0)ReturnAmount , isnull(p.PhysicalInventory, InStock + Purchase -isnull(Wastage, 0) - isnull(ReturnAmount, 0)- Consumption) PhysicalInventory, P.Remarks
												FROM 
												WarehouseConsumptions wc,
												PeriodicConsumptionitems p, inventoryitems i, warehouses w
												WHERE wc.Id = p.PeriodicConsumptionId
												AND p.InventoryItemId = i.Id
												AND wc.WarehouseId = w.Id      
												AND wc.Id = {0}
												ORDER BY  GroupCode, Inventory"
                                            , WarehouseCID);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "WorkPeriodEndDrill");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetInventoryItemList()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT Id,GroupCode, Name, TransactionUnit Unit, isnull(InventoryTakeType, '' )InventoryTakeType,'' Operation,'' Wastage, '' ReturnAmount,'' StockTake  
											 FROM InventoryItems");

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "InventoryItemList");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetInventoryTransactionDocuments(string StartDate, string EndDate)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT  d.id DocId,d.Name as DocName,d.Date, CONVERT(DECIMAL(20,2),Sum(t.Quantity*t.Price)) as Total  FROM 
											InventoryTransactionDocuments d, inventorytransactions t, inventoryitems i, warehouses w
											WHERE d.Id = t.InventoryTransactionDocumentId
											AND sourcewarehouseid = 0
											AND w.Id = targetwarehouseid
											AND i.Id = t.InventoryItem_Id
											AND d.date BETWEEN '{0}' AND '{1}'
											Group by d.id,d.Name,d.Date
											ORDER BY d.Date, d.Name "
                                            , StartDate, EndDate);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "InventoryTransactionDocuments");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetInventoryTransactions(int DocId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT d.Date, d.Name as DocName, w.Name as WarehouseName, i.Name as InventoryName , t.Quantity, t.Unit, t.Price, CONVERT(DECIMAL(20,2),Quantity*Price) TotalPrice  FROM 
											InventoryTransactionDocuments d, inventorytransactions t, inventoryitems i, warehouses w
											WHERE d.Id = t.InventoryTransactionDocumentId
											AND sourcewarehouseid = 0
											AND w.Id = targetwarehouseid
											AND i.Id = t.InventoryItem_Id
											And d.Id = {0}
											ORDER BY d.Date, d.Name , w.Name "
                                            , DocId);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "InventoryTransactions");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetInventoryCostOfSalesDetail(string inventoryGroupItem, string inventoryItemID, string warehouseID, string dtStart, string dtEnd, int firstworkperiodid, int lastworkperiodid)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                if (inventoryGroupItem == string.Empty || inventoryItemID == string.Empty || warehouseID == string.Empty)
                {
                    return null;
                }

                string sql = string.Format(@"SELECT i.GroupCode , i.Name Inventory, i.TransactionUnit Unit, i.UnitPrice, 
								[dbo].[ufsFormat](pc.StartDate, 'mmm dd yyyy hh:mm:ss:ms AM/PM') StartDate, i.Id InventoryID, w.Id WarehouseID,
								[dbo].[ufsFormat](pc.EndDate, 'mmm dd yyyy hh:mm:ss:ms AM/PM') EndDate, p.InStock OpeningStock, p.InStock*i.UnitPrice OpeningStockInValue, w.Name Warehouse,
								p.Purchase 'Purchase/ Transfer', p.Purchase*i.UnitPrice PurchaseTransferInValue , p.Consumption, p.Consumption*i.UnitPrice ConsumptionInValue, 
								p.StockIn StockIn, (p.StockIn*i.UnitPrice) StockInValue,
								p.StockOut-p.Consumption StockOut, (p.StockOut*i.UnitPrice)StockOutValue,
								(p.InStock + p.Purchase - p.Consumption) Closing,  (p.InStock + p.Purchase - p.Consumption)*i.UnitPrice ClosingInValue,
								Convert(Decimal(10,4),p.Consumption/(
											SELECT ABS(sum(Debit)-sum(Credit)) Balance
											FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
											V.AccountTypeId = T.Id AND A.Id = V.AccountId AND
											T.Name='Sales Accounts' and 
											V.Date  between '{0}' and '{1}'
											GROUP BY V.accounttypeid
											)*100) CostOfSales
								FROM PeriodicConsumptions pc, WarehouseConsumptions wc,
								PeriodicConsumptionitems p, 
								(
									SELECT i.*,
									UnitPrice = isnull((
															 SELECT Price 
															 FROM InventoryTransactions 
															 WHERE 
															 InventoryTransactions.InventoryItem_Id = i.id AND  InventoryTransactions.Unit = i.TransactionUnit
															 AND  id = (
																		 SELECT max(id) FROM InventoryTransactions 
																		 WHERE InventoryItem_Id =  i.id
																		 AND  InventoryTransactions.Unit = i.TransactionUnit
																	   )				                                             
														 ),0)
									from                                          
									inventoryitems i
								)i, warehouses w
								WHERE  pc.Id = wc.PeriodicConsumptionId
								AND wc.Id = p.PeriodicConsumptionId
								AND p.InventoryItemId = i.Id
								AND wc.WarehouseId = w.Id
								AND pc.Id >= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {2})
								AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3})", dtStart, dtEnd, firstworkperiodid, lastworkperiodid);

                sql += " AND wc.WarehouseId in(" + Convert.ToInt32(warehouseID) + ")";
                sql += " AND i.Id in(" + Convert.ToInt32(inventoryItemID) + ")";
                sql += " AND i.GroupCode in('" + inventoryGroupItem + "')";
                sql += " ORDER BY wc.WarehouseId, i.Id, pc.Id";

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "CurrentStock");
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataTable GetSyncWarehouseOutlets()
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = "select * from SyncOutletWarehouses";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.Fill(ds);
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            dt = new DataTable();
            dt = ds.Tables[0];
            return dt;
        }
    }
}
