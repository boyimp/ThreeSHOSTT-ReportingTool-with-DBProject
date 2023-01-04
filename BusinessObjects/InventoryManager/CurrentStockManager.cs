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
                string sql = string.Format(@"DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100),
											@LastPeriodStartDate DATETIME,
											@LastPeriodEndDate DATETIME,
											@LastWorkPeriodID INT,
											@FirstWorkPeriod int

											SET @StartDate = (SELECT [dbo].[ufsFormat] ((SELECT min(w.startdate) FROM WorkPeriods w WHERE startdate <> enddate and w.startdate > '{0}'),'yyyy-mm-dd hh:mm:ss'))
											SET @LastWorkPeriodID = (SELECT isnull(WorkPeriods.Id, 0) FROM WorkPeriods WHERE StartDate = (SELECT max(w.startdate) FROM WorkPeriods w WHERE startdate <> enddate /*and w.StartDate > '{0}'*/ AND  w.StartDate <= Dateadd(Day,1,'{1}')))
											SET @FirstWorkPeriod = (SELECT isnull(WorkPeriods.Id, 0) FROM WorkPeriods WHERE startdate <> enddate and StartDate = (SELECT min(w.startdate) FROM WorkPeriods w WHERE w.startdate > '{0}'))
											SET @LastPeriodStartDate = (SELECT startdate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)
											SET @LastPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)                                            
											SET @EndDate = (SELECT [dbo].[ufsFormat]((SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID), 'yyyy-mm-dd hh:mm:ss'))

											SELECT isnull(@StartDate, '18 sep 3030') StartDate, isnull(@EndDate, '18 sep 3030') EndDate, isnull(@FirstWorkPeriod, 0) FirstWorkPeriodID, isnull(@LastWorkPeriodID, 0) LastWorkPeriodID", fromDate.ToString("dd MMM yyyy"), toDate.ToString("dd MMM yyyy"));

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
                string sql = string.Format(@"SELECT [dbo].[ufsFormat] (startdate, 'mmm dd yyyy hh:mm AM/PM') StartDate, [dbo].[ufsFormat] (enddate, 'mmm dd yyyy hh:mm AM/PM') EndDate
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

                if (inventoryGroupItem == string.Empty || inventoryItemID == string.Empty || warehouseID == string.Empty) return null;
                string sql = string.Format(@"SELECT i.GroupCode , i.Name Inventory, i.TransactionUnit Unit, i.UnitPrice, 
								[dbo].[ufsFormat](pc.StartDate, 'mmm dd yyyy hh:mm:ss:ms AM/PM') StartDate, i.Id InventoryID, w.Id WarehouseID,
								[dbo].[ufsFormat](pc.EndDate, 'mmm dd yyyy hh:mm:ss:ms AM/PM') EndDate, Convert(Decimal(20,3),p.InStock) OpeningStock, Convert(Decimal(20,3),p.InStock*i.UnitPrice) OpeningStockInValue, w.Name Warehouse,
								p.Purchase 'Purchase/ Transfer', Convert(Decimal(20,3),p.Purchase*i.UnitPrice) PurchaseTransferInValue , p.Consumption, Convert(Decimal(20,3),p.Consumption*i.UnitPrice) ConsumptionInValue, 
								p.StockIn StockIn, Convert(Decimal(20,3),(p.StockIn*i.UnitPrice)) StockInValue,
								p.StockOut StockOut, Convert(Decimal(20,3),(p.StockOut*i.UnitPrice)) StockOutValue, isnull(p.Wastage,0) Wastage , Convert(Decimal(20,3),isnull(p.Wastage,0)*i.UnitPrice) WastageValue, isnull(p.ReturnAmount,0) [Return],  Convert(Decimal(20,3),(isnull(p.ReturnAmount,0)*i.UnitPrice)) ReturnValue,
								(p.InStock + p.Purchase - p.Consumption - p.wastage-p.stockout-p.returnamount) Expected,  Convert(Decimal(20,3),(p.InStock + p.Purchase - p.Consumption- p.wastage-p.stockout-p.returnamount)*i.UnitPrice) ExpectedValue,
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

											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'mmm dd yyyy hh:mm:ss:ms AM/PM'))
											SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'mmm dd yyyy hh:mm:ss:ms AM/PM'))

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

        public static DataSet GetSpecialInvntoryRegister(string inventoryGroupItem
            , string inventoryItemID
            , string warehouseID
            , DateTime dtStart
            , DateTime dtEnd
            , int firstworkperiodid
            , int lastworkperiodid
            , bool fifo
            , string Brand
            , string Vendor
            , string InventoryTakeType
            , bool isForCpmpiled)
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
								i.Id,i.IsIntermediate, i.Barcode, i.STARBrand, i.STARVendor, i.Name, i.transactionunit";
                if (!fifo)
                {
                    string sql = string.Format(@"
											DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100)
											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'mmm dd yyyy hh:mm:ss:ms AM/PM'))
											SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'mmm dd yyyy hh:mm:ss:ms AM/PM'))
											select *, CASE  WHEN IsIntermediate = 0 
											THEN (Opening_Stock+PurchaseORTransfer-PhysicalInventory) 
											Else (Opening_Stock+PurchaseORTransfer-Consumption-PhysicalInventory)
											END as ActualConsumption,
											CASE  WHEN IsIntermediate = 0 
											THEN (Opening_Stock+PurchaseORTransfer-PhysicalInventory)*Last_Unit_Price
											Else (Opening_Stock+PurchaseORTransfer-Consumption-PhysicalInventory)*Last_Unit_Price
											END as ActualConsumptionValue,
											PhysicalInventory-ExpectedQty CountVarianceQty ,(PhysicalInventoryValue-ExpectedValue) CountVarianceValue  from(
												Select
												Id,IsIntermediate,Barcode,Brand,Vendor,StartDate,	EndDate, 	GroupCode,	Inventory, WarehouseID,	
												Warehouse,	InventoryID, Unit,
												UnitPrice Last_Unit_Price, InStock Opening_Stock,InStock*OpeningUnitPrice Opening_StockValue,
												CASE  WHEN IsIntermediate = 0 
												THEN Purchase 
												 Else 0
													END as PurchaseORTransfer,
												CASE  WHEN IsIntermediate = 0 
												THEN Purchase*UnitPrice 
												 Else 0
													END as PurchaseORTransferValue,
												--Purchase 'PurchaseORTransfer', Purchase*UnitPrice PurchaseORTransferValue,
												CASE  WHEN IsIntermediate = 0 
												THEN Consumption 
												 Else 0
													END as Consumption,
												isnull(StockOut,0) StockOut, isnull(StockOut,0)*UnitPrice StockOutValue,
												CASE  WHEN IsIntermediate = 0 
												THEN Consumption*UnitPrice 
												 Else 0
													END as ConsumptionValue,Wastage, Wastage*UnitPrice WastageValue, ReturnAmount,ReturnAmount*UnitPrice ReturnValue,(InStock+Purchase-Consumption-Wastage-ReturnAmount) ExpectedQty,
												(InStock+Purchase-Consumption-Wastage-ReturnAmount)*UnitPrice ExpectedValue, isnull(PhysicalInventory, ClosingStock) PhysicalInventory, (UnitPrice*(isnull(PhysicalInventory, ClosingStock))) PhysicalInventoryValue,
												isnull(ClosingStock ,InStock + Purchase-stockout -Wastage - ReturnAmount- Consumption) ClosingStock,isnull(ClosingStock ,InStock + Purchase-stockout -Wastage - ReturnAmount- Consumption)*UnitPrice ClosingStockValue FROM
												  (
												SELECT @StartDate StartDate,
												@EndDate EndDate,
												i.Id,i.IsIntermediate, i.Barcode, i.STARBrand Brand, i.STARVendor Vendor, i.GroupCode, i.Name Inventory, w.Id WarehouseID, w.Name Warehouse, 
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
					
												sum(p.Purchase) 'Purchase', sum(p.Consumption) Consumption, 
												ClosingStock =(
																SELECT (instock + purchase -isnull(stocktake,0) - isnull(Wastage, 0) - isnull(ReturnAmount, 0) - consumption)
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
												AND pc.Id <= (SELECT max(id) FROM PeriodicConsumptions WHERE WorkPeriodId = {3}) {4} 
								
											 )Q
											)TQ
											ORDER BY  GroupCode, Inventory",
                        dtStart.ToString("dd MMM yyyy hh:mm:ss tt"), dtEnd.ToString("dd MMM yyyy hh:mm:ss tt"),
                        firstworkperiodid, lastworkperiodid, sqlClause);

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
								i.Id,i.IsIntermediate, i.Barcode, i.STARBrand, i.STARVendor, i.Name, i.transactionunit";

                if (!fifo)
                {
                    string sql = string.Format(@"
											DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100)

											SET @StartDate = (SELECT [dbo].[ufsFormat] ('{0}', 'mmm dd yyyy hh:mm:ss:ms AM/PM'))
											SET @EndDate = (SELECT [dbo].[ufsFormat] ('{1}', 'mmm dd yyyy hh:mm:ss:ms AM/PM'))

											select *, (Opening_Stock+PurchaseORTransfer-PhysicalInventory) ActualConsumption,(Opening_Stock+PurchaseORTransfer-PhysicalInventory)*Last_Unit_Price ActualConsumptionValue,
											PhysicalInventory-ExpectedQty CountVarianceQty ,(PhysicalInventoryValue-ExpectedValue) CountVarianceValue  from(
												Select
												Id,IsIntermediate,Barcode,Brand,Vendor,StartDate,	EndDate, 	GroupCode,	Inventory, WarehouseID,	
												Warehouse,	InventoryID, Unit,
												UnitPrice Last_Unit_Price, InStock Opening_Stock,InStock*Unitprice Opening_StockValue, purchase PurchaseORTransfer, Purchase*UnitPrice PurchaseORTransferValue,
												isnull(StockOut,0) StockOut, isnull(StockOut,0)*UnitPrice StockOutValue,Consumption,Consumption*UnitPrice ConsumptionValue,Wastage, Wastage*UnitPrice WastageValue, ReturnAmount,ReturnAmount*UnitPrice ReturnValue,(InStock+Purchase-Consumption-Wastage-ReturnAmount) ExpectedQty,
												(InStock+Purchase-Consumption-Wastage-ReturnAmount)*UnitPrice ExpectedValue, isnull(PhysicalInventory, ClosingStock) PhysicalInventory, (UnitPrice*(isnull(PhysicalInventory, ClosingStock))) PhysicalInventoryValue,
												isnull(ClosingStock ,InStock + Purchase -Wastage - ReturnAmount- Consumption) ClosingStock,isnull(ClosingStock ,InStock + Purchase -Wastage - ReturnAmount- Consumption)*UnitPrice ClosingStockValue FROM
												  (
												SELECT @StartDate StartDate,
												@EndDate EndDate,
												i.Id,i.IsIntermediate, i.Barcode, i.STARBrand Brand, i.STARVendor Vendor, i.GroupCode, i.Name Inventory, w.Id WarehouseID, w.Name Warehouse, 
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
					
												sum(p.Purchase) 'Purchase', sum(p.Consumption) Consumption, 
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
												AND pc.StartDate >= @StartDate
												AND pc.EndDate <= @EndDate {4} 
								
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
                    sql = "select id,Name FROM InventoryItems order by Name";
                else
                    sql = string.Format("select id,Name FROM InventoryItems where GroupCode='{0}' order by Name", search);

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
                    sql = "select id,Name FROM InventoryItems where IsIntermediate = 1 order by Name";
                else
                    sql = string.Format("select id,Name FROM InventoryItems where GroupCode='{0}' and IsIntermediate = 1 order by Name", search);

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
        public static DataSet GetOpeningClosingPurchaseStockValue(string fromDate, string toDate, int warehouseId, string GroupCodes, string Brands)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string sqlCaluse = string.Empty;
                if (warehouseId > 0)
                    sqlCaluse = string.Format(@" and w.Id in ({0})", warehouseId);

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

                if (inventoryGroupItem == string.Empty || inventoryItemID == string.Empty || warehouseID == string.Empty) return null;
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
