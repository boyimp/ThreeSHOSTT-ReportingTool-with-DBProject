//In the name of Allah
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ThreeS.Domain.Models.Accounts;
using ThreeS.Domain.Models.Menus;
using ThreeS.Domain.Models.Tickets;
using Dapper;
using ThreeS.Report.v2.Models;
using ThreeS.Report.v2.Controllers;
using BusinessObjects.MenusManager;
using System.Configuration;
using ThreeS.Modules.BasicReports.Reports;

namespace BusinessObjects.TicketsManager
{
    public class TicketManager
    {
        //Author Jewel Hossain
        public static DataSet VoidAnalysisReport(int ticketId)
        {
            DataSet dSet = new DataSet();

            Ticket ticket = GetTicket(ticketId);

            rptDataset.OrdersDataTable dTable = new rptDataset.OrdersDataTable();
            if (ticket != null)
            {
                string menuItem, orderStates;

                try
                {
                    foreach (Order ordr in ticket.Orders)
                    {
                        menuItem = string.Empty;
                        orderStates = string.Empty;

                        DataRow Rowbody = dTable.NewRow();
                        Rowbody["Quantity"] = ordr.Quantity;

                        //menuItem = ordr.MenuItemName + System.Environment.NewLine;
                        menuItem = ordr.MenuItemName ;
                        foreach (OrderTagValue ordrTagValue in ordr.OrderTagValues)
                        {
                            menuItem += "," + ordrTagValue.TagValue;
                        }//forEach
                        Rowbody["MenuItem"] = menuItem;

                        foreach (OrderStateValue ordrStateValue in ordr.OrderStateValues)
                        {
                            if (string.IsNullOrEmpty(orderStates)) orderStates = ordrStateValue.State;
                            else orderStates += "," + ordrStateValue.State;
                        }//forEach

                        Rowbody["CreatingUserName"] = ordr.CreatingUserName;
                        Rowbody["CreatedDateTime"] = ordr.CreatedDateTime;
                        Rowbody["OrderStates"] = orderStates;
                        Rowbody["UnitPrice"] = ordr.GetVisiblePrice();
                        Rowbody["Price"] = ordr.CalculatePrice ? ordr.GetVisiblePrice() * ordr.Quantity : 0;
                        Rowbody["UnitProductionCost"] = ordr.CalculatePrice ? ordr.UnitProductionCost : 0;
                        DataTable dtProductionCostFixed = MenuItemManager.ProductionCostFixed(ordr.MenuItemId, ordr.PortionName);
                        Rowbody["FixedProductionCost"] = dtProductionCostFixed.Rows.Count > 0 && ordr.CalculatePrice ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
                        Rowbody["Time"] = GetTime(ordr);

                        dTable.Rows.Add(Rowbody);
                    }//forEach
                }//try
                catch (Exception ex)
                { 
                    throw new Exception(ex.Message); 
                }//catch
            }//if

            DataTable orderDataTable = dTable;
            dSet.Tables.Add(orderDataTable);

            DataTable ticketEntitiesDataTable = GetTicketEntities(ticketId);
            dSet.Tables.Add(ticketEntitiesDataTable);

            DataTable calculationsDataTable = GetCalculations(ticket);
            dSet.Tables.Add(calculationsDataTable);

            DataTable paymentsDataTable = GetPayments(ticket);
            dSet.Tables.Add(paymentsDataTable);

            return dSet;
        }//func


         //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GiftDetailsMultiparameterReport(
            DateTime from, 
            DateTime to, 
            List<int> outlets, 
            List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"--In the name of Allah


                        WITH
                            [TicketState]
                                AS
                                    (
                                        SELECT
                                            [tkt].[SyncOutletId],
                                            [tkt].[Id],
                                            [tkt].[TicketNumber],
                                            [tkt].[Date],
                                            [tkt].[TotalAmount],
                                            CAST(tkt.Note AS NVARCHAR(100)) AS [Note],
                                            [tkt].[IsClosed],
                                            [tkt].[LastUpdateTime],
                                            JSON_VALUE(TicketStates.Value, '$.S') AS [TicketStates]
                                        FROM [Tickets] AS [tkt]
                                    LEFT OUTER JOIN [TicketTypes] AS [tktType] ON [tktType].[Id] = [tkt].[TicketTypeId] 
                                    CROSS APPLY OPENJSON(convert(NVARCHAR(max),tkt.TicketStates)) AS [TicketStates]
                                        WHERE [tkt].[Date] BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                            AND [tkt].[SyncOutletId] IN ({outletsAsParameter})
                                            AND [tkt].[DepartmentId] IN({departmentsAsParameter})
                                                )
                                            SELECT [TEMPORARY_2].*,
                                                    [OrderId],
                                                    [Entry_By],
                                                    [Order_Time],
                                                    [Action_Type],
                                                    [Approved_By],
                                                    [Login_By],
                                                    [Action_Time],
                                                    [Reason],
                                                    [Menu_Item_Name] AS [Menu_Item],
                                                    [Quantity],
                                                    [Price],
                                                    [Product_Price_Total] AS [Total]
                                            FROM
                                                (
                                                SELECT
                                                    [TEMPORARY_1].[OutletName],
                                                    [TEMPORARY_1].[Id] AS [TicketId],
                                                    [TEMPORARY_1].[TicketNumber],
                                                    'Customers'= (Isnull(
                                                                (
                                                                    SELECT isnull(Name, '') AS [Name]
                                                                    FROM [Entities], [TicketEntities]
                                                                        WHERE [Entities].[Id] = [TicketEntities].[EntityId]
                                                                            AND [TicketEntities].[EntityTypeId] =1 --Customer = 1 
                                                                            AND [TicketEntities].[Ticket_Id] = [TEMPORARY_1].[Id]
                                                                ), '')),
                                                    'Tables'= (Isnull    (   
                                                                (
                                                                SELECT isnull(Name, '') AS [Name]
                                                                FROM [Entities], [TicketEntities]
                                                                    WHERE [Entities].[Id] = [TicketEntities].[EntityId]
                                                                        AND [TicketEntities].[EntityTypeId] IN (2,7) --Table = 2 
                                                                        AND [TicketEntities].[Ticket_Id] = [TEMPORARY_1].[Id]
                                                                ), '')),
                                                    'Waiters'= (Isnull(
                                                                (
                                                                SELECT isnull(Name, '') AS [Name]
                                                                FROM [Entities], [TicketEntities]
                                                                    WHERE [Entities].[Id] = [TicketEntities].[EntityId]
                                                                        AND [TicketEntities].[EntityTypeId] IN (3,6) --Waiter = 3
                                                                        AND [TicketEntities].[Ticket_Id] = [TEMPORARY_1].[Id]
                                                                ), '')),
                                                    FORMAT(CAST([TEMPORARY_1].[Date] AS datetime),'dd-MMM-yyyy hh:mm:ss tt','en-us') AS [Date],
                                                    LTRIM(RIGHT(CONVERT(VARCHAR(20), [TEMPORARY_1].Date, 100), 7)) AS [Opening],
                                                    LTRIM(RIGHT(CONVERT(VARCHAR(20), [TEMPORARY_1].[LastUpdateTime], 100), 7)) AS [Closing],
                                                    [TEMPORARY_1].[Note],
                                                    [TEMPORARY_1].[StateName] AS [TicketStates]
                                                FROM
                                                    (
                                                        SELECT [SyncOutlets].[Name] AS [OutletName], 
                                                                [a].[Id], 
                                                                [TicketNumber], 
                                                                [Date], 
                                                                [TotalAmount], 
                                                                [Note], 
                                                                [IsClosed], 
                                                                [LastUpdateTime],
                                                    [StateName] = 
                                                        STUFF((SELECT ', ' + [TicketStates]
                                                        FROM [TicketState] 
                                                        WHERE [TicketState] .[Id] = [a].[Id]
                                                        FOR XML PATH('')), 1, 2, '')
                                                    FROM [TicketState] AS [a], [SyncOutlets]
                                                        WHERE [a].LastUpdateTime BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                            AND [SyncOutlets].[Id]=[a].[SyncOutletId]

                                                    GROUP BY [SyncOutlets].[Name], 
                                                                [a].[Id],  
                                                                [TicketNumber], 
                                                                [Date], [TotalAmount], 
                                                                [Note],   [IsClosed],   [LastUpdateTime]                  
                                                    ) AS [TEMPORARY_1]

                                            GROUP BY    [TEMPORARY_1].[OutletName], 
                                                            [TEMPORARY_1].[Id], 
                                                            [TEMPORARY_1].[TicketNumber], 
                                                            [TEMPORARY_1].[Date], 
                                                            [TEMPORARY_1].[LastUpdateTime], 
                                                            [TEMPORARY_1].[TotalAmount], 
                                                            [TEMPORARY_1].[Note], 
                                                            [TEMPORARY_1].[StateName], 
                                                            [TEMPORARY_1].[Isclosed]
                            
                                                    )AS [TEMPORARY_2] INNER JOIN
                                                    (
                                                        SELECT

                                                                [Orders].[Id] AS [OrderId],
                                                                [TicketId],
                                                                [Tickets].[TicketNumber] AS [Ticket_Number],
                                                                [Tickets].[LastOrderDate] AS [Ticket_Date_Time],
                                                                FORMAT(CAST(orders.CreatedDateTime AS DATETIME),'dd-MMM-yyyy hh:mm:ss tt','en-us') AS [Order_Time],
                                                                [orders].[CreatingUserName] AS [Entry_By],
                                                                JSON_VALUE(OrderStates.value, '$.S') AS [Action_Type],
                                                                JSON_VALUE(OrderStates.value, '$.UN') AS [Login_By],
                                                                JSON_VALUE(OrderStates.value, '$.AN') AS [Approved_By],
                                                                Format(CAST(JSON_VALUE(OrderStates.value, '$.SD') AS DATETIME),'dd-MMM-yyyy hh:mm:ss tt','en-us') AS [Action_Time],
                                                                ISNULL(JSON_VALUE(OrderStates.value, '$.SV'),'') AS [Reason],
                                                                [Orders].[MenuItemName] AS [Menu_Item_Name],
                                                                [Price],
                                                                CAST(Quantity AS INT) AS [Quantity],
                                                                CAST((Quantity*Price) AS DECIMAL(18,2)) AS [Product_Price_Total]
                                            FROM [Tickets] , [Orders] CROSS APPLY OPENJSON(convert(nvarchar(max),[Orders].[OrderStates])) AS [OrderStates]
                                                    WHERE   [Tickets].[Id] = [Orders].[TicketId]
                                                    AND JSON_VALUE([OrderStates].[Value], '$.S') in ('Gift') --'%Void%'
                                                    --AND orders.CreatedDateTime between @FDate and @TDate  
                                                    AND [Tickets].[Id] IN
                                                    (   
                                                        SELECT [Tickets].[Id]
                                                        FROM [Tickets]
                                                        WHERE [Tickets].[Date] BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                        AND Tickets.SyncOutletId IN({outletsAsParameter})
                                                        AND [Tickets].[DepartmentId] IN({departmentsAsParameter})
                                            )
                                 
                                        ) AS [TickOrder]
                                            ON [TEMPORARY_2].[TicketId] = [TickOrder].[TicketId]
                                            ORDER BY [Order_Time] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> VoidDetailsMultiparameterReport(
            DateTime from,
            DateTime to,
            List<int> outlets,
            List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"--In the name of Allah


                        WITH
                            [TicketState]
                                AS
                                    (
                                        SELECT
                                            [tkt].[SyncOutletId],
                                            [tkt].[Id],
                                            [tkt].[TicketNumber],
                                            [tkt].[Date],
                                            [tkt].[TotalAmount],
                                            CAST(tkt.Note AS NVARCHAR(100)) AS [Note],
                                            [tkt].[IsClosed],
                                            [tkt].[LastUpdateTime],
                                            JSON_VALUE(TicketStates.Value, '$.S') AS [TicketStates]
                                        FROM [Tickets] AS [tkt]
                                    LEFT OUTER JOIN [TicketTypes] AS [tktType] ON [tktType].[Id] = [tkt].[TicketTypeId] 
                                    CROSS APPLY OPENJSON(convert(NVARCHAR(max),tkt.TicketStates)) AS [TicketStates]
                                        WHERE [tkt].[Date] BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                            AND [tkt].[SyncOutletId] IN ({outletsAsParameter})
                                            AND [tkt].[DepartmentId] IN({departmentsAsParameter})
                                                )
                                            SELECT [TEMPORARY_2].*,
                                                    [OrderId],
                                                    [Entry_By],
                                                    [Order_Time],
                                                    [Action_Type],
                                                    [Approved_By],
                                                    [Login_By],
                                                    [Action_Time],
                                                    [Reason],
                                                    [Menu_Item_Name] AS [Menu_Item],
                                                    [Quantity],
                                                    [Price],
                                                    [Product_Price_Total] AS [Total]
                                            FROM
                                                (
                                                SELECT
                                                    [TEMPORARY_1].[OutletName],
                                                    [TEMPORARY_1].[Id] AS [TicketId],
                                                    [TEMPORARY_1].[TicketNumber],
                                                    'Customers'= (Isnull(
                                                                (
                                                                    SELECT isnull(Name, '') AS [Name]
                                                                    FROM [Entities], [TicketEntities]
                                                                        WHERE [Entities].[Id] = [TicketEntities].[EntityId]
                                                                            AND [TicketEntities].[EntityTypeId] =1 --Customer = 1 
                                                                            AND [TicketEntities].[Ticket_Id] = [TEMPORARY_1].[Id]
                                                                ), '')),
                                                    'Tables'= (Isnull    (   
                                                                (
                                                                SELECT isnull(Name, '') AS [Name]
                                                                FROM [Entities], [TicketEntities]
                                                                    WHERE [Entities].[Id] = [TicketEntities].[EntityId]
                                                                        AND [TicketEntities].[EntityTypeId] IN (2,7) --Table = 2 
                                                                        AND [TicketEntities].[Ticket_Id] = [TEMPORARY_1].[Id]
                                                                ), '')),
                                                    'Waiters'= (Isnull(
                                                                (
                                                                SELECT isnull(Name, '') AS [Name]
                                                                FROM [Entities], [TicketEntities]
                                                                    WHERE [Entities].[Id] = [TicketEntities].[EntityId]
                                                                        AND [TicketEntities].[EntityTypeId] IN (3,6) --Waiter = 3
                                                                        AND [TicketEntities].[Ticket_Id] = [TEMPORARY_1].[Id]
                                                                ), '')),
                                                    FORMAT(CAST([TEMPORARY_1].[Date] AS datetime),'dd-MMM-yyyy hh:mm:ss tt','en-us') AS [Date],
                                                    LTRIM(RIGHT(CONVERT(VARCHAR(20), [TEMPORARY_1].Date, 100), 7)) AS [Opening],
                                                    LTRIM(RIGHT(CONVERT(VARCHAR(20), [TEMPORARY_1].[LastUpdateTime], 100), 7)) AS [Closing],
                                                    [TEMPORARY_1].[Note],
                                                    [TEMPORARY_1].[StateName] AS [TicketStates]
                                                FROM
                                                    (
                                                        SELECT [SyncOutlets].[Name] AS [OutletName], 
                                                                [a].[Id], 
                                                                [TicketNumber], 
                                                                [Date], 
                                                                [TotalAmount], 
                                                                [Note], 
                                                                [IsClosed], 
                                                                [LastUpdateTime],
                                                    [StateName] = 
                                                        STUFF((SELECT ', ' + [TicketStates]
                                                        FROM [TicketState] 
                                                        WHERE [TicketState] .[Id] = [a].[Id]
                                                        FOR XML PATH('')), 1, 2, '')
                                                    FROM [TicketState] AS [a], [SyncOutlets]
                                                        WHERE [a].LastUpdateTime BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                            AND [SyncOutlets].[Id]=[a].[SyncOutletId]

                                                    GROUP BY [SyncOutlets].[Name], 
                                                                [a].[Id],  
                                                                [TicketNumber], 
                                                                [Date], [TotalAmount], 
                                                                [Note],   [IsClosed],   [LastUpdateTime]                  
                                                    ) AS [TEMPORARY_1]

                                            GROUP BY    [TEMPORARY_1].[OutletName], 
                                                            [TEMPORARY_1].[Id], 
                                                            [TEMPORARY_1].[TicketNumber], 
                                                            [TEMPORARY_1].[Date], 
                                                            [TEMPORARY_1].[LastUpdateTime], 
                                                            [TEMPORARY_1].[TotalAmount], 
                                                            [TEMPORARY_1].[Note], 
                                                            [TEMPORARY_1].[StateName], 
                                                            [TEMPORARY_1].[Isclosed]
                            
                                                    )AS [TEMPORARY_2] INNER JOIN
                                                    (
                                                        SELECT

                                                                [Orders].[Id] AS [OrderId],
                                                                [TicketId],
                                                                [Tickets].[TicketNumber] AS [Ticket_Number],
                                                                [Tickets].[LastOrderDate] AS [Ticket_Date_Time],
                                                                FORMAT(CAST(orders.CreatedDateTime AS DATETIME),'dd-MMM-yyyy hh:mm:ss tt','en-us') AS [Order_Time],
                                                                [orders].[CreatingUserName] AS [Entry_By],
                                                                JSON_VALUE(OrderStates.value, '$.S') AS [Action_Type],
                                                                JSON_VALUE(OrderStates.value, '$.UN') AS [Login_By],
                                                                JSON_VALUE(OrderStates.value, '$.AN') AS [Approved_By],
                                                                Format(CAST(JSON_VALUE(OrderStates.value, '$.SD') AS DATETIME),'dd-MMM-yyyy hh:mm:ss tt','en-us') AS [Action_Time],
                                                                ISNULL(JSON_VALUE(OrderStates.value, '$.SV'),'') AS [Reason],
                                                                [Orders].[MenuItemName] AS [Menu_Item_Name],
                                                                [Price],
                                                                CAST(Quantity AS INT) AS [Quantity],
                                                                CAST((Quantity*Price) AS DECIMAL(18,2)) AS [Product_Price_Total]
                                            FROM [Tickets] , [Orders] CROSS APPLY OPENJSON(convert(nvarchar(max),[Orders].[OrderStates])) AS [OrderStates]
                                                    WHERE   [Tickets].[Id] = [Orders].[TicketId]
                                                    AND JSON_VALUE([OrderStates].[Value], '$.S') in ('Void') --'%Void%'
                                                    --AND orders.CreatedDateTime between @FDate and @TDate  
                                                    AND [Tickets].[Id] IN
                                                    (   
                                                        SELECT [Tickets].[Id]
                                                        FROM [Tickets]
                                                        WHERE [Tickets].[Date] BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                        AND Tickets.SyncOutletId IN({outletsAsParameter})
                                                        AND [Tickets].[DepartmentId] IN({departmentsAsParameter})
                                            )
                                 
                                        ) AS [TickOrder]
                                            ON [TEMPORARY_2].[TicketId] = [TickOrder].[TicketId]
                                            ORDER BY [Order_Time] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public static IList<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo> ItemSalesProfitAnalysisReport(
            DateTime from, 
            DateTime to, 
            int outletId, 
            int departmentId, 
            int menuItemId, 
            string groupCode)
        {
            if (groupCode == string.Empty)
            {
                groupCode = "All";
            }//if
            List<Ticket> tickets = GetTicketsFaster(outletId, departmentId == 0 ? string.Empty : departmentId.ToString(), from, to);
            List<ThreeS.Domain.Models.Menus.MenuItem> menuItemss = MenuItemManager.GetMenuItemsFaster(menuItemId, groupCode);

            var menuItems = departmentId == 0 ? 
                ThreeS.Modules.BasicReports.Reports
                .MenuGroupBuilder
                .CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss)
                .OrderBy(x => x.ID) : 
                ThreeS.Modules.BasicReports.Reports
                .MenuGroupBuilder
                .CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, departmentId)
                .OrderBy(x => x.ID);

            IList<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo> Products = new List<ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo>();
            foreach (var menuItemInfo in menuItems)
            {
                var s = menuItemss.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });

                DataTable dtProductionCostRecipe = MenuItemManager
                    .ProductionCostRecipe(menuItemInfo.ID, menuItemInfo.Portion);
                decimal PcostR = dtProductionCostRecipe.Rows.Count > 0 ? 
                    Convert.ToDecimal(dtProductionCostRecipe.Rows[0]["ProductionCost"]) : 0;
                decimal TPcostR = PcostR * menuItemInfo.Quantity;
                decimal PprofitR = menuItemInfo.AmountWithOrderTag - TPcostR;
                DataTable dtProductionCostFixed = MenuItemManager
                    .ProductionCostFixed(menuItemInfo.ID, menuItemInfo.Portion);
                decimal PcostF = dtProductionCostFixed.Rows.Count > 0 ? 
                    Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
                decimal TPcostF = PcostF * menuItemInfo.Quantity;
                decimal PprofitF = menuItemInfo.AmountWithOrderTag - TPcostF;
                DataSet ds = TicketManager.GetDepartments();
                DataRow drow = ds.Tables[0].NewRow();
                drow["Id"] = "0";
                drow["Name"] = "All";
                ds.Tables[0].Rows.InsertAt(drow, 0);
                DataRow[] rows = ds.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));
                ThreeS.Modules.BasicReports.Reports
                    .MenuGroupItemInfo menuGroupItemInfo = new ThreeS.Modules.BasicReports.Reports.MenuGroupItemInfo
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
                Products.Add(menuGroupItemInfo);
            }
            return Products;
        }//func

        //Author Jewel Hossain
        public static IList<ItemProfitLossRecipeInfo> ItemSalesProfitLossRecipeReport(
            DateTime from, 
            DateTime to,
            int outletId, 
            int departmentId,
            int menuItemId,
            string groupCode)
        {
            if(groupCode == string.Empty)
            {
                groupCode = "All";
            }//if
            List<Ticket> tickets = GetTicketsFaster(outletId, departmentId == 0 ? string.Empty : departmentId.ToString(), from, to);
            List<ThreeS.Domain.Models.Menus.MenuItem> menuItemss = MenuItemManager.GetMenuItemsFaster(menuItemId, groupCode);

            var menuItems = departmentId == 0 ? 
                ThreeS.Modules.BasicReports.Reports
                .MenuGroupBuilder.
                CalculateMenuItemsWithTimerAllDepartment(tickets, menuItemss)
                .OrderBy(x => x.ID) : 
                ThreeS.Modules.BasicReports.Reports
                .MenuGroupBuilder
                .CalculateMenuItemsWithTimerWithDepartment(tickets, menuItemss, departmentId)
                .OrderBy(x => x.ID);

            IList<ItemProfitLossRecipeInfo> Products = new List<ItemProfitLossRecipeInfo>();
            foreach (var menuItemInfo in menuItems)
            {
                var s = menuItemss.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });
                DataTable dtProductionCostRecipe = MenuItemManager.ProductionCostRecipe(menuItemInfo.ID, menuItemInfo.Portion);
                decimal PcostR = dtProductionCostRecipe.Rows.Count > 0 ? Convert.ToDecimal(dtProductionCostRecipe.Rows[0]["ProductionCost"]) : 0;
                decimal TPcostR = PcostR * menuItemInfo.Quantity;
                decimal PprofitR = menuItemInfo.AmountWithOrderTag - TPcostR;

                DataSet ds = GetDepartments();
                DataRow drow = ds.Tables[0].NewRow();
                drow["Id"] = "0";
                drow["Name"] = "All";
                ds.Tables[0].Rows.InsertAt(drow, 0);
                DataRow[] rows = ds.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(menuItemInfo.DepartmentID)));

                ItemProfitLossRecipeInfo itemProfitLossRecipeInfo = new ItemProfitLossRecipeInfo
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
                    ProductionCostRecipeWise = Math.Round(PcostR, 2),
                    TotalProductionCostRecipeWise = Math.Round(TPcostR, 2),
                    ProductionProfitRecipeWise = Math.Round(PprofitR, 2),
                };
                Products.Add(itemProfitLossRecipeInfo);
            }
            return Products;
        }//func

        //Author Jewel Hossain
        public static IList<ItemSalesReportInfo> GetItemSalesReport(
            DateTime from, 
            DateTime to, 
            int outletId, 
            int departmentId)
        {
            var menuItems = GetItemIssue(outletId, departmentId == 0 ? string.Empty : departmentId.ToString(), from, to);
            IList<ItemSalesReportInfo> Products = new List<ItemSalesReportInfo>();
            DataSet ds = GetDepartments();

            foreach (DataRow menuItemInfo in menuItems.Rows)
            {
                DataRow drow = ds.Tables[0].NewRow();
                drow["Id"] = "0";
                drow["Name"] = "All";
                ds.Tables[0].Rows.InsertAt(drow, 0);
                DataRow[] rows = ds.Tables[0].Select(string.Format("Id='{0}'", Convert.ToInt32(departmentId)));

                ItemSalesReportInfo itemSalesReportInfo = new ItemSalesReportInfo
                {
                    ItemId = Convert.ToInt32(menuItemInfo["MenuItemId"]),
                    DepartmentName = rows[0]["Name"].ToString(),
                    GroupName = menuItemInfo["GroupCode"].ToString(),
                    ItemName = menuItemInfo["MenuItemName"].ToString(),
                    PortionName = menuItemInfo["PortionName"].ToString(),
                    Price = Math.Round(Convert.ToDecimal(menuItemInfo["price"]), 4),
                    Quantity = Math.Round(Convert.ToDecimal(menuItemInfo["Quantity"]), 4),
                    NetAmount = Math.Round(Convert.ToDecimal(menuItemInfo["Gross"]), 4),
                    LineItemValue = Math.Round(Convert.ToDecimal(menuItemInfo["LineItemValue"]), 4),
                    Gross = Math.Round(Convert.ToDecimal(menuItemInfo["NetAmount"]), 4)
                };
                Products.Add(itemSalesReportInfo);
            }
            return Products;
        }//func

        //Author Jewel Hossain
        public static DataSet GetWorkPeriodReport(DateTime from, DateTime to, int outlet, List<int> departments)
        {
            const bool isShort = false;
            string departmentsAsParameter = StringService.DepartmentIdsToString(departments);
            DataSet result = GetReport(from,to,departmentsAsParameter,outlet,isShort);
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<VoidInfo> GetVoidCountAndAmountSum(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query =
                $@"SELECT 
                COUNT(*) AS [VoidOrdersCount],
                ISNULL( SUM([Orders].[Quantity] * [Orders].[Price]), 0 ) AS [VoidOrdersAmount]
                FROM [Tickets],[Orders]
                WHERE [Orders].[TicketId] = [Tickets].[Id]
                --AND [Tickets].[IsClosed] = 1
                AND [Orders].[CalculatePrice] = 0
                AND [Orders].[DecreaseInventory] = 0
                AND [Orders].[DepartmentId] IN ({departmentsAsParameter})
                AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                AND [Tickets].[Date] >= '{fromTimeStamp}'
                AND [Tickets].[Date] <= '{toTimeStamp}' ";

            db.Open();
            var voids = await db.QuerySingleAsync<VoidInfo>(query);
            db.Close();
            return voids;
        }//func

        //Author Jewel Hossain
        public async static Task<GiftInfo> GetGiftCountAndAmountSum(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT 
                            COUNT(*) AS [GiftOrdersCount],
                            ISNULL( SUM([Orders].[Quantity] * [Orders].[Price]), 0 ) AS [GiftOrdersAmountSum]
                            FROM [Tickets],[Orders]
                            WHERE [Orders].[TicketId] = [Tickets].[Id]
                            AND [Tickets].[IsClosed] = 1
                            AND [Orders].[CalculatePrice] = 0
                            AND [Orders].[DecreaseInventory] = 1
                            AND [Orders].[DepartmentId] IN ({departmentsAsParameter})
                            AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                            AND [Tickets].[Date] >= '{fromTimeStamp}'
                            AND [Tickets].[Date] <= '{toTimeStamp}' ";

            db.Open();
            var giftInfo = await db.QuerySingleAsync<GiftInfo>(query);
            db.Close();
            return giftInfo;
        }//func

        //Author Jewel Hossain
        public async static Task<DiscountInfo> GetDiscountCountAndAmountSum(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT 
                                COUNT(*) AS [DiscountedOrdersCount],
                                SUM([CalculationAmount]) AS [DiscountedOrdersAmountSum]
                                FROM [Tickets],[Calculations]
                                WHERE [Calculations].[TicketId] = [Tickets].[Id]
                                --AND [Tickets].[IsClosed] = 1
                                AND [Calculations].[DecreaseAmount] = 1
                                AND [Calculations].[Name] NOT LIKE '%Auto%'
                                And [Calculations].[Name] NOT LIKE '%Auto Round%'
	                            And [Calculations].[Name] NOT LIKE '%Round%'
                                AND [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                                AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                AND [Tickets].[Date] >= '{fromTimeStamp}'
                                AND [Tickets].[Date] <= '{toTimeStamp}'";

            db.Open();
            var discountInfo = await db.QuerySingleAsync<DiscountInfo>(query);
            db.Close();
            return discountInfo;
        }//func

        //Author Jewel Hossain 
        public async static Task<TicketsCountAndNoOfGuest> GetTicketsCountAndNoOfGuest(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT 
                                COUNT(*) AS TicketsCount,
                                SUM(case
								when NoOfGuests = 0 then 2

                                else NoOfGuests
                                end) AS NoOfGuests
                                FROM [Tickets]
                                --WHERE [Tickets].[IsClosed] = 1
                                WHERE [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                                AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                AND [Tickets].[Date] >= '{fromTimeStamp}'
                                AND [Tickets].[Date] <= '{toTimeStamp}'";

            db.Open();
            var ticketsCountAndNoOfGuest = await db.QuerySingleAsync<TicketsCountAndNoOfGuest>(query);
            db.Close();
            return ticketsCountAndNoOfGuest;
        }//func

        //Author Jewel Hossain 
        public async static Task<TotalNetAndGrossSale> GetTotalNetAndGrossSale(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"SELECT 
                                SUM([TotalAmount]) AS [GrossTotalSale],
                                SUM([PlainSum]) AS [NetTotalSale] 
                                FROM [Tickets]
                                WHERE [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                                AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                AND [Tickets].[Date] >= '{fromTimeStamp}'
                                AND [Tickets].[Date] <= '{toTimeStamp}'";

            db.Open();
            var totalNetAndGrossSale = await db.QuerySingleAsync<TotalNetAndGrossSale>(query);
            db.Close();
            return totalNetAndGrossSale;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetProductCagegoryWiseSalesCountInfo(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets) 
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"WITH
                                [TEMPORARY]
                                            AS
                                                (
                                                    SELECT [MenuItems].[GroupCode] AS [ProductCategory],
                                                            sum([Orders].[Quantity]) AS [Quantity],
                                                            sum([Orders].PlainTotal) AS [GrossAmount],
                                                            sum(NetTicketUnitPrice * [Orders].[Quantity]) AS [NetAmount]
                                                    FROM [Departments], [Tickets], [Orders], [MenuItems]
                                                        WHERE [Tickets].[Id] = [Orders].[TicketId]
                                                        AND [MenuItems].[Id] = [Orders].[MenuItemId]
                                                        AND [Orders].[CalculatePrice] = 1
                                                        AND [Departments].[Id] = [Orders].[DepartmentId]
                                                        AND [Orders].[CalculatePrice] = 1
                                                        AND [Orders].[DepartmentId] IN ({departmentsAsParameter})
                                                        AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                                        AND [Tickets].Date BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                    GROUP BY [MenuItems].[GroupCode]
                                                )
                            SELECT
                                    [ProductCategory],
                                    [Quantity],
                                    CAST(100 / 
                                        ((SUM([TEMPORARY].[Quantity]) OVER ()+(0.0)) / 
                                        [TEMPORARY].[Quantity]) AS DECIMAL(30,2)) AS [Quantity(%)],
                                    [GrossAmount],
                                    CAST(100 / 
                                        ((SUM([TEMPORARY].[GrossAmount]) OVER ()+(0.0)) / 
                                        [TEMPORARY].[GrossAmount]) AS DECIMAL(30,2)) AS [GrossAmount(%)],
                                    [NetAmount] ,
                                    CAST(100 / 
                                        ((SUM([TEMPORARY].[NetAmount]) OVER ()+(0.0)) / 
                                        [TEMPORARY].[NetAmount]) AS DECIMAL(30,2)) AS [NetAmount(%)]
                            FROM [TEMPORARY]
                            ORDER BY [GrossAmount] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetDepartmentWiseSalesInfo(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets) 
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"WITH
                            [TEMPORARY]
                                    AS
                                        (
                                            SELECT
                                                Count([TicketTypes].[Name]) AS [Quantity],
                                                CAST(SUM([Tickets].[PlainSum]) AS DECIMAL(30,2)) AS [Amount],
                                                [TicketTypes].[Name] AS [Name]
                                            FROM [Tickets]
                                            INNER JOIN [TicketTypes]
                                                On [Tickets].[TicketTypeId] = [TicketTypes].[Id]
                                                AND [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                                                AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                                AND [Tickets].[Date] 
                                            BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                            GROUP BY [TicketTypes].[Name]
                                        )
                            SELECT
                                [TEMPORARY].[Name] AS [Department],
                                [TEMPORARY].[Quantity] ,
                                [TEMPORARY].[Amount],
                                CAST(100 / ((SUM([TEMPORARY].[Amount]) OVER ()+(0.0)) / [TEMPORARY].[Amount]) AS DECIMAL(30,2)) AS [Amount(%)]
                            FROM [TEMPORARY]
                            ORDER BY [TEMPORARY].[Quantity] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetPaymentMethodWiseSalesInfo(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"WITH
                            [TEMPORARY]
                                AS    
                                    (
                                        SELECT
                                            Count([Payments].[Name]) AS [Quantity],
                                            CAST(SUM([Payments].[Amount]) AS DECIMAL(30,2)) AS [Amount],
                                            [Payments].[Name] AS [PaymentMethod]
                                        FROM [Payments] INNER JOIN [Tickets]
                                            ON [Payments].[TicketId] = [Tickets].[Id]
                                            WHERE [Tickets].[IsClosed] = 1
                                            AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                            AND [Tickets].[Date] >= '{fromTimeStamp}'
                                            AND [Tickets].[Date] <= '{toTimeStamp}'
                                            GROUP BY [Payments].[Name]
                                    )
                            SELECT
                                [TEMPORARY].[PaymentMethod],
                                [TEMPORARY].[Quantity],
                                [TEMPORARY].[Amount],
                                CAST(100 / ((SUM([TEMPORARY].[Amount]) OVER ()+(0.0)) / [TEMPORARY].[Amount]) AS DECIMAL(30,2)) AS [Amount(%)]
                            FROM [TEMPORARY]
                            ORDER BY [TEMPORARY].[Quantity] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetProductWiseSalesInfo(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"WITH
                                [TEMPORARY]
                                            AS
                                                (
                                                     SELECT [Orders].[MenuItemName] AS [Product],
                                                            [Orders].[PortionName],
                                                            [Orders].[Price] AS [UnitPrice],
                                                            sum([Orders].[Quantity]) AS [Quantity],
                                                            sum([Orders].PlainTotal) AS [GrossAmount],
                                                            sum(NetTicketUnitPrice * [Orders].[Quantity]) AS [NetAmount]
                                                    FROM [Departments], [Tickets], [Orders], [MenuItems]
                                                        WHERE [Tickets].[Id] = [Orders].[TicketId]
                                                        AND [MenuItems].[Id] = [Orders].[MenuItemId]
                                                        AND [Orders].[CalculatePrice] = 1
                                                        AND [Departments].[Id] = [Orders].[DepartmentId]
                                                        AND [Orders].[CalculatePrice] = 1
                                                        AND [Orders].[DepartmentId] IN ({departmentsAsParameter})
                                                        AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                                        AND [Tickets].Date BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                    GROUP BY [Orders].[MenuItemName],[Orders].[PortionName],[Orders].[Price]
                                                )
                                SELECT
                                    TOP(10)
                                    [Product],
                                    [PortionName],
                                    [UnitPrice],
                                    [Quantity],
                                    CAST(100 / 
                                        ((SUM([TEMPORARY].[Quantity]) OVER ()+(0.0)) / 
                                        [TEMPORARY].[Quantity]) AS DECIMAL(30,2)) AS [Quantity(%)],
                                    [GrossAmount],
                                    CAST(100 / 
                                        ((SUM([TEMPORARY].[GrossAmount]) OVER ()+(0.0)) / 
                                        [TEMPORARY].[GrossAmount]) AS DECIMAL(30,2)) AS [Gross(%)],
                                    [NetAmount] ,
                                    CAST(100 / 
                                        ((SUM([TEMPORARY].[NetAmount]) OVER ()+(0.0)) / 
                                        [TEMPORARY].[NetAmount]) AS DECIMAL(30,2)) AS [Net(%)]
                                FROM [TEMPORARY]
                                ORDER BY [Quantity] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        //Author Jewel Hossain
        public async static Task<IEnumerable<dynamic>> GetDiscountInfo(DateTime from, DateTime to, List<int> outlets, List<int> departments)
        {
            string outletsAsParameter = "0";
            foreach (int outlet in outlets)
            {
                outletsAsParameter += "," + outlet.ToString();
            }//foreach

            string departmentsAsParameter = "0";
            foreach (int department in departments)
            {
                departmentsAsParameter += "," + department.ToString();
            }//foreach

            string fromTimeStamp = DateTimeService.GetTimeStamp(from);
            string toTimeStamp = DateTimeService.GetTimeStamp(to);

            string connectionString = DBUtility.GetConnectionString();
            IDbConnection db = new SqlConnection(connectionString);
            var query = $@"WITH
                            [TEMPORARY]
                                        AS
                                            (
                                                SELECT
                                                    [Calculations].[Name] AS [Name],
                                                    COUNT([Calculations].[Name]) AS [Count],
                                                    ABS(SUM([CalculationAmount])) AS [Amount]
                                                FROM [Tickets], [Calculations]
                                                    WHERE [Calculations].[TicketId] = [Tickets].[Id]
                                                    AND Calculations.[DecreaseAmount] = 1
                                                    AND [Calculations].Name not like '%Auto%'
                                                    And [Calculations].[Name] NOT LIKE '%Auto Round%'
	                                                And [Calculations].[Name] NOT LIKE '%Round%'
                                                    AND [Tickets].[DepartmentId] IN ({departmentsAsParameter})
                                                    AND [Tickets].[SyncOutletId] IN ({outletsAsParameter})
                                                    AND [Tickets].[Date] 
                                                BETWEEN '{fromTimeStamp}' AND '{toTimeStamp}'
                                                GROUP BY [Calculations].[Name]
                                            )
                            SELECT
                                [TEMPORARY].[Name] ,
                                [TEMPORARY].[Count],
                                [TEMPORARY].[Amount],
                                CAST(100 / ((SUM([TEMPORARY].[Amount]) OVER ()+(0.0)) / [TEMPORARY].[Amount]) AS DECIMAL(30,2)) AS [Amount(%)]
                            FROM [TEMPORARY]
                            ORDER BY [TEMPORARY].[Amount] DESC";

            db.Open();
            var result = await db.QueryAsync(query);
            db.Close();
            return result;
        }//func

        public static DataSet GetTickets(DateTime fromDate, DateTime toDate, int TicketType, bool isOnlyOpenTicket, int OutletId)
        {
            DataSet ds = new DataSet();

            string sFeedback = string.Empty;
            string searchStr = string.Empty;

            try
            {
                sFeedback += ":1:::";
                sFeedback += fromDate.ToString() + "::::";
                sFeedback += toDate.ToString() + "::::";

                if (TicketType != 0)
                {
                    searchStr = string.Format(@"tktType.Id = {0} AND ", TicketType);
                }

                if (isOnlyOpenTicket)
                {
                    searchStr += @" tkt.IsClosed = 0 AND ";
                }

                string dbConnString = DBUtility.GetConnectionString();
                string sql = string.Empty;

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                if (OutletId == 0)
                {
                    sql = string.Format(@"
					DECLARE 
					@StartDate  VARCHAR(100),
					@EndDate  VARCHAR(100)
 
					SET @StartDate = (SELECT [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'))
					SET @EndDate = (SELECT [dbo].[ufsFormat] ('{2}', 'yyyy-mm-dd hh:mm:ss'))

					declare @stmt nvarchar(max)
					select @stmt =	isnull(@stmt + ', ', '') +
									'max(case when Name = ''' + Name + ''' then EntityName else null end) as ' + quotename(name)
									from EntityTypes

					DECLARE 
					@datestmt NVARCHAR(max),
					@DateCondition NVARCHAR(max)

					SET @datestmt =(SELECT ''''+ @StartDate + '''' + ' StartDate, '+'''' + @EndDate+''''+ ' EndDate')
					SET @DateCondition = 'Between' + ''''+ @StartDate + '''' + ' AND '+ '''' + @EndDate+''''
					--SELECT @datestmt

					select @stmt = 'SELECT StartDate, EndDate,
					t.id,t.TicketNumber, t.Department_Name,t.SettledBy,' + @stmt + ',t.Date, LTRIM(RIGHT(CONVERT(VARCHAR(20), t.Date, 100), 7)) as Opening,

					--CASE t.IsClosed
					--    WHEN 0 THEN LTRIM(RIGHT(CONVERT(VARCHAR(20), getdate(), 100), 7)) 
					--      ELSE LTRIM(RIGHT(CONVERT(VARCHAR(20), t.LastUpdateTime, 100), 7)) 
					--   END 
					--as Closing,
					LTRIM(RIGHT(CONVERT(VARCHAR(20), t.LastUpdateTime, 100), 7)) Closing,

					--CASE t.IsClosed
					--      WHEN 0 THEN DATEDIFF(MINUTE,t.Date,getdate()) 
					--      ELSE DATEDIFF(MINUTE,t.Date,t.LastUpdateTime) 
					--   END 
					--as MinutesSpend
					DATEDIFF(MINUTE,t.Date,t.LastUpdateTime) as MinutesSpend
					,cast(No_Of_Guests as int) No_Of_Guests, t.TotalAmount, t.Note, t.Isclosed from 
					(
						select' + @datestmt + ',
						tkt.Id,tkt.TicketNumber, d.Name Department_Name,isNull(tkt.SettledBy, '' '') as SettledBy, tkt.Date,tkt.TotalAmount,CAST(tkt.Note AS NVARCHAR(100)) Note,tkt.IsClosed, tkt.LastUpdateTime, EntType.Name,tktEnt.EntityName, NoOfGuests No_Of_Guests
						from Tickets as tkt 
						LEFT OUTER join TicketTypes as tktType on tktType.Id = tkt.TicketTypeId 
						LEFT OUTER join  TicketEntities as tktEnt on tktEnt.Ticket_Id=tkt.Id
						LEFT OUTER join EntityTypes as EntType on EntType.Id = tktEnt.EntityTypeId
						Left Outer join departments d on d.ID = tkt.DepartmentID
						WHERE {0} Date ' +   @DateCondition +'	
					)t 
					group by t.StartDate, t.EndDate, t.id,t.TicketNumber, t.Department_Name, t.Date,t.LastUpdateTime,t.TotalAmount, t.No_Of_Guests, t.Note,t.Isclosed, t.SettledBy order by t.Isclosed'

					exec dbo.sp_executesql
					@stmt", searchStr, fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"));
                }
                else
                {
                    sql = string.Format(@"
					DECLARE 
					@StartDate  VARCHAR(100),
					@EndDate  VARCHAR(100)
 
					SET @StartDate = (SELECT [dbo].[ufsFormat] ('{1}', 'yyyy-mm-dd hh:mm:ss'))
					SET @EndDate = (SELECT [dbo].[ufsFormat] ('{2}', 'yyyy-mm-dd hh:mm:ss'))

					declare @stmt nvarchar(max)
					select @stmt =	isnull(@stmt + ', ', '') +
									'max(case when Name = ''' + Name + ''' then EntityName else null end) as ' + quotename(name)
									from EntityTypes

					DECLARE 
					@datestmt NVARCHAR(max),
					@DateCondition NVARCHAR(max)

					SET @datestmt =(SELECT ''''+ @StartDate + '''' + ' StartDate, '+'''' + @EndDate+''''+ ' EndDate')
					SET @DateCondition = 'Between' + ''''+ @StartDate + '''' + ' AND '+ '''' + @EndDate+''''
					--SELECT @datestmt

					select @stmt = 'SELECT StartDate, EndDate,
					t.id,t.TicketNumber, t.Department_Name,t.SettledBy,' + @stmt + ',t.Date, LTRIM(RIGHT(CONVERT(VARCHAR(20), t.Date, 100), 7)) as Opening,

					--CASE t.IsClosed
					--    WHEN 0 THEN LTRIM(RIGHT(CONVERT(VARCHAR(20), getdate(), 100), 7)) 
					--      ELSE LTRIM(RIGHT(CONVERT(VARCHAR(20), t.LastUpdateTime, 100), 7)) 
					--   END 
					--as Closing,
					LTRIM(RIGHT(CONVERT(VARCHAR(20), t.LastUpdateTime, 100), 7)) Closing,

					--CASE t.IsClosed
					--      WHEN 0 THEN DATEDIFF(MINUTE,t.Date,getdate()) 
					--      ELSE DATEDIFF(MINUTE,t.Date,t.LastUpdateTime) 
					--   END 
					--as MinutesSpend
					DATEDIFF(MINUTE,t.Date,t.LastUpdateTime) as MinutesSpend
					,cast(No_Of_Guests as int) No_Of_Guests, t.TotalAmount, t.Note, t.Isclosed from 
					(
						select' + @datestmt + ',
						tkt.Id,tkt.TicketNumber, d.Name Department_Name,isNull(tkt.SettledBy, '' '') as SettledBy, tkt.Date,tkt.TotalAmount,CAST(tkt.Note AS NVARCHAR(100)) Note,
						tkt.IsClosed, tkt.LastUpdateTime, EntType.Name,tktEnt.EntityName, NoOfGuests No_Of_Guests, SyncOutletId
						from Tickets as tkt 
						LEFT OUTER join TicketTypes as tktType on tktType.Id = tkt.TicketTypeId 
						LEFT OUTER join  TicketEntities as tktEnt on tktEnt.Ticket_Id=tkt.Id
						LEFT OUTER join EntityTypes as EntType on EntType.Id = tktEnt.EntityTypeId
						Left Outer join departments d on d.ID = tkt.DepartmentID
						WHERE {0} Date ' +   @DateCondition +'
	
					)t 
					where t.SyncOutletId = {3}    
					group by t.StartDate, t.EndDate, t.id,t.TicketNumber, t.Department_Name, t.Date,t.LastUpdateTime,t.TotalAmount, 
					t.No_Of_Guests, t.Note,t.Isclosed, t.SettledBy, t.SyncOutletId
					order by t.Isclosed'

					exec dbo.sp_executesql
					@stmt", searchStr, fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"), OutletId);
                }

                sFeedback += ":3:" + "::::";
                sFeedback += fromDate.ToString("dd MMM yyyy hh:mm:ss tt") + "::::";
                sFeedback += toDate.ToString("dd MMM yyyy hh:mm:ss tt") + "::::";
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                sFeedback += ":5" + "::::";
                da.Fill(ds, "Tickets");
                sFeedback += ":5" + "::::";
                dbConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + sFeedback);
            }

            return ds;
        }

        public static DataSet VoidOrders(DateTime fromDate, DateTime toDate, string sOrderStatus, int OutletId, string DepartmentIds)
        {
            DataSet ds = new DataSet();
            string searchStr = string.Empty;
            try
            {
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = string.Format(@" and tickets.DepartmentId in ({0})", DepartmentIds);
                }

                if (OutletId > 0)
                {
                    sqlClause = sqlClause + string.Format(@"and tickets.SyncOutletId = {0}", OutletId);
                }

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"declare @stmt nvarchar(max)
											select @stmt =
															isnull(@stmt + ', ', '') +
															'max(case when Name = ''' + Name + ''' then EntityName else null end) as ' + quotename(name)
															from EntityTypes

											select @stmt = '
											SELECT Tick.*,
													id,
													Entry_By,                                                     
													[dbo].[ufsFormat](Order_Date_Time, ''mmm dd yyyy hh:mm AM/PM'') Order_Time,
													{2}_By,
													CASE {2}_Time
														WHEN NULL THEN '''' 
														ELSE [dbo].[ufsFormat]({2}_Time, ''mmm dd yyyy hh:mm AM/PM'') 
														END 
													as {2}_Time, Reason,
													Menu_Item_Name Menu_Item,
													Quantity, 
													Price, 
													Product_Price_Total Total FROM 
											(
												select
												 t.Id TicketId, t.TicketNumber,' + @stmt + ', [dbo].[ufsFormat](t.Date, ''mmm dd yyyy hh:mm AM/PM'')Date, LTRIM(RIGHT(CONVERT(VARCHAR(20), t.Date, 100), 7)) as Opening,
												LTRIM(RIGHT(CONVERT(VARCHAR(20), t.LastUpdateTime, 100), 7)) as Closing, t.Note, t.TicketStates from 
												(
													select
													tkt.Id,tkt.TicketNumber,tkt.Date,tkt.TotalAmount,CAST(tkt.Note AS NVARCHAR(100)) Note, CAST(tkt.TicketStates AS NVARCHAR(4000)) TicketStates,
													tkt.IsClosed, tkt.LastUpdateTime, EntType.Name,tktEnt.EntityName
													from Tickets as tkt 
													LEFT OUTER join TicketTypes as tktType on tktType.Id = tkt.TicketTypeId 
													LEFT OUTER join  TicketEntities as tktEnt on tktEnt.Ticket_Id=tkt.Id
													LEFT OUTER join EntityTypes as EntType on EntType.Id = tktEnt.EntityTypeId
													where tkt.LastUpdateTime between ''{0}'' and ''{1}''	
												)t 
												group by t.id,t.TicketNumber,t.Date,t.LastUpdateTime,t.TotalAmount, t.Note, t.TicketStates, t.Isclosed
											)Tick INNER JOIN 
											(
												SELECT Orders.Id id, TicketId, Tickets.TicketNumber Ticket_Number, 
												tickets.LastOrderDate Ticket_Date_Time, 
												orders.CreatedDateTime Order_Date_Time,
												orders.CreatingUserName Entry_By,
												isnull(orders.AutomationUserName,'''') {2}_By,'''' ''Reason'',
												orders.AutomationDateTime {2}_Time,
												Orders.MenuItemName Menu_Item_Name, Price, 
												cast(Quantity AS int) Quantity, cast((Quantity*Price) AS DECIMAL(18,2)) Product_Price_Total
												FROM Tickets , orders
												WHERE tickets.Id = orders.TicketId
												AND orders.OrderStates LIKE ''%{2}%''
												AND orders.CreatedDateTime
												between ''{0}'' and ''{1}''	
												{3}
											)TickOrder
											on tick.TicketId = tickorder.ticketid
											ORDER BY Order_Time desc
											'
											exec dbo.sp_executesql
											@stmt 
												", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"), sOrderStatus, sqlClause);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "VoidOrders");
                dbConn.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["TicketStates"] = Ticket.GetStates(Convert.ToString(dr["TicketStates"]));
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
            
            return ds;
        }
        public static DataSet VoidOrdersGroupWise(DateTime fromDate, DateTime toDate, string sOrderStatus, int OutletId, string DepartmentIds)
        {
            DataSet ds = new DataSet();
            string searchStr = string.Empty;
            try
            {
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = string.Format(@" and tickets.DepartmentId in ({0})", DepartmentIds);
                }

                if (OutletId > 0)
                {
                    sqlClause = sqlClause + string.Format(@"and tickets.SyncOutletId = {0}", OutletId);
                }

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT 0 TicketId, m.GroupCode GroupName, SUM(cast((Quantity*Price) AS DECIMAL(18,2)))  Amount, SUM(Quantity) Quantity
											FROM Tickets , orders, MenuItems m
											WHERE tickets.Id = orders.TicketId
											and m.Id = orders.MenuItemId
											AND orders.OrderStates LIKE '%{2}%'
											AND orders.CreatedDateTime
											between '{0}' and '{1}'	
											{3}
											group by m.GroupCode", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"), sOrderStatus, sqlClause);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "VoidOrders");
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
											 FROM WorkPeriods WHERE Id =( SELECT max(id) FROM WorkPeriods)");

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "StartAndEndDate"); ;
                dbConn.Close();

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ds;
        }

        public static DataSet GetStartAndEndDate(DateTime fromDate, DateTime toDate, bool bIsCurrentWorkPeriod)
        {
            DataSet ds = new DataSet();
            string searchStr = string.Empty;
            try
            {

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Empty;
                if (!bIsCurrentWorkPeriod)
                {
                    sql = string.Format(@"DECLARE 
											@StartDate  VARCHAR(100),
											@EndDate  VARCHAR(100),
											@LastPeriodStartDate DATETIME,
											@LastPeriodEndDate DATETIME,
											@LastWorkPeriodID INT

											SET @StartDate = (SELECT [dbo].[ufsFormat] ((SELECT min(w.startdate) FROM WorkPeriods w WHERE w.startdate >= '{0}'), 'yyyy-mm-dd hh:mm:ss'))
											SET @LastWorkPeriodID = (SELECT WorkPeriods.Id FROM WorkPeriods WHERE StartDate = (SELECT max(w.startdate) FROM WorkPeriods w WHERE w.StartDate >= '{0}' AND  w.StartDate < Dateadd(Day,1,'{1}')))
											SET @LastPeriodStartDate = (SELECT startdate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)
											SET @LastPeriodEndDate = (SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID)                                            
											IF @LastPeriodEndDate = @LastPeriodStartDate
											BEGIN
												SET @EndDate = (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)
											END
											ELSE
											BEGIN 
												SET @EndDate = (SELECT [dbo].[ufsFormat]((SELECT enddate FROM WorkPeriods WHERE Id = @LastWorkPeriodID), 'yyyy-mm-dd hh:mm:ss'))
											END 

											SELECT isnull(@StartDate, '18 sep 3030') StartDate, isnull(@EndDate, '18 sep 3030') EndDate
											", fromDate.ToString("dd MMM yyyy"), toDate.ToString("dd MMM yyyy"));
                }
                else
                {
                    sql = string.Format(@"DECLARE 
										@StartDate  DateTime,
										@EndDate  DateTime,
										@MaxEndDate DateTime

										SET @StartDate = (SELECT max(startdate) FROM WorkPeriods)
										SET @EndDate = (SELECT max(EndDate) FROM WorkPeriods)
										SET @MaxEndDate = @EndDate

										IF @StartDate = @EndDate
										BEGIN
											SET @MaxEndDate = (SELECT max(EndDate) FROM WorkPeriods where StartDate<>EndDate)
											SET @EndDate = (SELECT [dbo].[ufsFormat](DATEADD(second,1,max(Date)), 'yyyy-mm-dd hh:mm:ss') FROM AccountTransactionValues)
										END
										SELECT [dbo].[ufsFormat](@StartDate, 'yyyy-mm-dd hh:mm:ss') StartDate, [dbo].[ufsFormat](@EndDate, 'yyyy-mm-dd hh:mm:ss') EndDate,[dbo].[ufsFormat](@MaxEndDate, 'yyyy-mm-dd hh:mm:ss') MaxEndDate");
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds, "StartAndEndDate");
                dbConn.Close();

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ds;
        }

        public static DataTable GetTicketEntities(int ticketId)
        {
            DataTable ds = new DataTable("TicketEntities");
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"select entType.EntityName as EntityType,tktEnt.EntityName from ticketentities tktEnt
							inner join EntityTypes entType
							on entType.Id=tktEnt.EntityTypeId
							where tktEnt.Ticket_Id={0}", ticketId);

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

        public static DataSet GetCalculationTypes()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string clause = string.Empty;

                //if (calculationTypeId > 0)
                //    clause = string.Format(" Where Id = {0}", calculationTypeId);

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format("select * FROM CalculationTypes");

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

        public static DataSet GetTaxTemplates()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format("select * FROM TaxTemplates");

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

        //ok
        public static DataSet GetTicketType()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                sql = string.Format("select id,Name FROM TicketTypes order by name");
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

        //ok
        public static DataSet GetPaymentType()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format("select id,Name FROM PaymentTypes");

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

        //ok
        public static DataSet GetDepartments()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = $@"select id,Name--,Tag
									FROM Departments order by name";

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

        //ok
        public static DataSet GetOutlets()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@"select id, /*(Name +'-'+Cast(LastSyncedOutletTime as varchar))*/ Name FROM SyncOutlets where isActive<>0");

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

        public static int GetOutletIdForWarehouse(int warehouseId)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();
                    sql = $@"select isnull(outletid,0) outletid from SyncOutletWarehouses where warehouseid={warehouseId}";
                    using (var reader = new SqlCommand(sql, dbConn)
                    {
                        CommandType = CommandType.Text
                    }.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            int.TryParse(reader["outletid"].ToString(), out int outletId);
                            return outletId;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }


        public static Ticket GetTicket(int ticketIdid)
        {
            Ticket ticket = null;
            try
            {
                int TransactionDocument_Id = 0;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = $@"SELECT  * FROM tickets where id={ticketIdid}";
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();
                while (reader.Read())
                {
                    ticket = new Ticket();
                    ticket.Id = (int)reader["Id"];
                    ticket.LastUpdateTime = Convert.ToDateTime(reader["LastUpdateTime"]);
                    ticket.TicketNumber = (string)reader["TicketNumber"];
                    ticket.Date = Convert.ToDateTime(reader["Date"]);
                    ticket.LastOrderDate = Convert.ToDateTime(reader["LastOrderDate"]);
                    ticket.LastPaymentDate = Convert.ToDateTime(reader["LastPaymentDate"]);
                    ticket.IsClosed = (bool)reader["IsClosed"];
                    ticket.IsLocked = (bool)reader["IsLocked"];
                    ticket.RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]);
                    ticket.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    ticket.DepartmentId = (int)reader["DepartmentId"];
                    ticket.TicketTypeId = (int)reader["TicketTypeId"];
                    ticket.Note = (reader["Note"] == DBNull.Value) ? string.Empty : (string)reader["Note"];
                    ticket.TicketTags = (reader["TicketTags"] == DBNull.Value) ? string.Empty : (string)reader["TicketTags"];
                    ticket.TicketStates = (reader["TicketStates"] == DBNull.Value) ? string.Empty : (string)reader["TicketStates"];
                    ticket.ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"]);
                    ticket.TaxIncluded = (bool)reader["TaxIncluded"];
                    ticket.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                    ticket.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                    TransactionDocument_Id = (reader["TransactionDocument_Id"] == DBNull.Value) ? 0 : (int)reader["TransactionDocument_Id"];
                    ticket.NoOfGuests = Convert.ToInt32(reader["NoOfGuests"]);
                    ticket.Sum = Convert.ToDecimal(reader["Sum"]);
                    ticket.PreTaxServicesTotal = Convert.ToDecimal(reader["PreTaxServicesTotal"]);
                    ticket.PostTaxServicesTotal = Convert.ToDecimal(reader["PostTaxServicesTotal"]);
                    ticket.PreTaxDiscountServicesTotal = Convert.ToDecimal(reader["PreTaxDiscountServicesTotal"]);
                    ticket.PostTaxDiscountServicesTotal = Convert.ToDecimal(reader["PostTaxDiscountServicesTotal"]);
                    ticket.PlainSum = Convert.ToDecimal(reader["PlainSum"]);
                    ticket.PlainSumForEndOfPeriod = Convert.ToDecimal(reader["PlainSumForEndOfPeriod"]);
                    ticket.TaxTotal = Convert.ToDecimal(reader["TaxTotal"]);
                    ticket.ServiceChargeTotal = Convert.ToDecimal(reader["ServiceChargeTotal"]);
                    ticket.ActiveTimerAmount = Convert.ToDecimal(reader["ActiveTimerAmount"]);
                    ticket.ActiveTimerClosedAmount = Convert.ToDecimal(reader["ActiveTimerClosedAmount"]);
                    ticket.ZoneId = (int)reader["ZoneId"];
                }
                reader.Close();
                DataTable dtMenuItemPortionCost = GetAllMenuItemPortionCost().Tables[0];
                ticket.Orders = GetOrders(dbConn, ticketIdid, dtMenuItemPortionCost);
                ticket.Calculations = GetCalculations(dbConn, ticketIdid);
                ticket.Payments = GetPayments(dbConn, ticketIdid);
                ticket.TransactionDocument = GetTransactionDocument(dbConn, TransactionDocument_Id);
                ticket.TicketEntities = GetTicketEntity(dbConn, ticketIdid);
                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ticket;
        }

        public static void UpdateTicket(Ticket ticket)
        {

            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();
            foreach (Order oO in ticket.Orders)
            {
                string sqlString = $@"UPDATE dbo.Orders
													SET 
													ActiveTimerAmount = {oO.ActiveTimerAmount},
													PlainTotal = {oO.PlainTotal},
													Total = {oO.Total},
													Value = {oO.Value},
													GetPriceValue = {oO.GetPriceValue},
													GrossUnitPrice = {oO.GrossUnitPrice},
													GrossPrice = {oO.GrossPrice},
													OrderTagPrice = {oO.OrderTagPrice}
													WHERE Id = {oO.Id} ";
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                int NoOfRows = dbCommand.ExecuteNonQuery();
            }
            dbConn.Close();

        }

        private static List<Order> GetOrders(SqlConnection dbConn, int ticketId, DataTable dtCosting)
        {
            string sqlString = $@"select * FROM orders where TicketId={ticketId}";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            if (dbConn.State == ConnectionState.Closed) dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            List<Order> orders = new List<Order>();
            Order order = null;
            while (reader.Read())
            {
                order = new Order();
                order.Id = (int)reader["Id"];
                order.TicketId = (int)reader["TicketId"];
                order.WarehouseId = (int)reader["WarehouseId"];
                order.DepartmentId = (int)reader["DepartmentId"];
                order.MenuItemId = (int)reader["MenuItemId"];
                order.MenuItemName = (reader["MenuItemName"] == DBNull.Value) ? string.Empty : (string)reader["MenuItemName"];
                order.PortionName = (reader["PortionName"] == DBNull.Value) ? string.Empty : (string)reader["PortionName"];
                order.Price = Convert.ToDecimal(reader["Price"]);
                order.Quantity = Convert.ToDecimal(reader["Quantity"]);
                order.PortionCount = (int)reader["PortionCount"];
                order.Locked = (bool)reader["Locked"];
                order.CreatedDateTime = Convert.ToDateTime(reader["CreatedDateTime"]);
                order.CalculatePrice = (bool)reader["CalculatePrice"];
                order.DecreaseInventory = (bool)reader["DecreaseInventory"];
                order.OrderNumber = (int)reader["OrderNumber"];
                order.AccountTransactionTypeId = (int)reader["AccountTransactionTypeId"];
                order.ProductTimerValueId = (reader["ProductTimerValueId"] == DBNull.Value) ? 0 : (int)reader["ProductTimerValueId"];
                order.PriceTag = (reader["PriceTag"] == DBNull.Value) ? string.Empty : (string)reader["PriceTag"];
                order.CreatingUserName = (reader["CreatingUserName"] == DBNull.Value) ? string.Empty : (string)reader["CreatingUserName"];
                order.Tag = (reader["Tag"] == DBNull.Value) ? string.Empty : (string)reader["Tag"];
                order.Taxes = (reader["Taxes"] == DBNull.Value) ? string.Empty : (string)reader["Taxes"];
                order.OrderTags = (reader["OrderTags"] == DBNull.Value) ? string.Empty : (string)reader["OrderTags"];
                order.OrderStates = (reader["OrderStates"] == DBNull.Value) ? string.Empty : (string)reader["OrderStates"];
                order.ActiveTimerAmount = Convert.ToDecimal(reader["ActiveTimerAmount"]);
                order.PlainTotal = Convert.ToDecimal(reader["PlainTotal"]);
                order.Total = Convert.ToDecimal(reader["Total"]);
                order.Value = Convert.ToDecimal(reader["Value"]);
                order.GetPriceValue = Convert.ToDecimal(reader["GetPriceValue"]);
                order.GrossUnitPrice = Convert.ToDecimal(reader["GrossUnitPrice"]);
                order.GrossPrice = Convert.ToDecimal(reader["GrossPrice"]);
                order.OrderTagPrice = Convert.ToDecimal(reader["OrderTagPrice"]);
                order.MenuGroupName = (reader["MenuGroupName"] == DBNull.Value) ? string.Empty : (string)reader["MenuGroupName"];
                order.CalculationTypes = (reader["CalculationTypes"] == DBNull.Value) ? string.Empty : (string)reader["CalculationTypes"];
                //DataRow[] dr = dtCosting.Select(string.Format("MenuItemId = {0} and PortionName ='{1}'",order.MenuItemId,order.PortionName));
                DataRow[] dr = dtCosting.AsEnumerable()
                    .Where(row => row.Field<int>("MenuItemId") == order.MenuItemId && row.Field<string>("PortionName") == order.PortionName).ToArray<DataRow>();
                order.UnitProductionCost = dr.Count() > 0 ? Convert.ToDecimal(dr[0]["ProductionCost"]) : 0;
                //order.UnitProductionCost = dtTable.get this menu item portion costing;
                //order.UnitProductionCost = Convert.ToDecimal(reader["UnitProductionCost"]);
                orders.Add(order);
            }
            reader.Close();
            if (dbConn.State == ConnectionState.Open) dbConn.Close();
            GetProductTimerValue(dbConn, orders);

            return orders;
        }
        private static List<Order> GetAllOrders(SqlConnection dbConn, DateTime fromDate, DateTime toDate, DataTable dtCosting)
        {
            string sqlString = $@"select * from Orders where Ticketid in (SELECT Id FROM tickets where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}')";
            List<Order> orders = dbConn.Query<Order>(sqlString, null, null, true, 3600, null).ToList();
            return orders;
        }

        private static List<Order> GetProductTimerValue(SqlConnection dbConn, List<Order> orders)
        {
            Parallel.ForEach(orders, (ordr) =>
            {
                if (ordr.ProductTimerValueId.HasValue && ordr.ProductTimerValueId > 0)
                {
                    string sqlString = $@"select top 1 * from ProductTimerValues where Id=@ProductTimerValueId";
                    SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                    dbCommand.Parameters.AddWithValue("@ProductTimerValueId", ordr.ProductTimerValueId);
                    dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                    IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);

                    ProductTimerValue prdTmvalue = null;
                    if (reader.Read())
                    {
                        prdTmvalue = new ProductTimerValue();
                        prdTmvalue.Id = (int)reader["Id"];
                        prdTmvalue.ProductTimerId = (int)reader["ProductTimerId"];
                        prdTmvalue.PriceType = (int)reader["PriceType"];
                        prdTmvalue.PriceDuration = (decimal)reader["PriceDuration"];
                        prdTmvalue.MinTime = (decimal)reader["MinTime"];
                        prdTmvalue.TimeRounding = (decimal)reader["TimeRounding"];
                        prdTmvalue.Start = (DateTime)reader["Start"];
                        prdTmvalue.End = (DateTime)reader["End"];

                        ordr.ProductTimerValue = prdTmvalue;
                    }
                    reader.Close();
                }
            });

            //foreach (Order ordr in orders)
            //{
            //    if (ordr.ProductTimerValueId.HasValue&&ordr.ProductTimerValueId>0)
            //    {
            //        string sqlString = $@"select * from ProductTimerValues where Id={ordr.ProductTimerValueId}";
            //        SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            //        dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            //        IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);

            //        ProductTimerValue prdTmvalue = null;
            //        if (reader.Read())
            //        {
            //            prdTmvalue = new ProductTimerValue();
            //            prdTmvalue.Id = (int)reader["Id"];
            //            prdTmvalue.ProductTimerId = (int)reader["ProductTimerId"];
            //            prdTmvalue.PriceType = (int)reader["PriceType"];
            //            prdTmvalue.PriceDuration = (decimal)reader["PriceDuration"];
            //            prdTmvalue.MinTime = (decimal)reader["MinTime"];
            //            prdTmvalue.TimeRounding = (decimal)reader["TimeRounding"];
            //            prdTmvalue.Start = (DateTime)reader["Start"];
            //            prdTmvalue.End = (DateTime)reader["End"];

            //            ordr.ProductTimerValue = prdTmvalue;
            //        }
            //        reader.Close();
            //    }
            //}

            return orders;
        }

        private static List<Calculation> GetCalculations(SqlConnection dbConn, int ticketId)
        {
            string sqlString = $@"select * from Calculations where TicketId = {ticketId}";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            if (dbConn.State == ConnectionState.Closed) dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            List<Calculation> objects = new List<Calculation>();
            Calculation obj = null;
            while (reader.Read())
            {
                obj = new Calculation();
                obj.Id = (int)reader["Id"];
                obj.TicketId = (int)reader["TicketId"];
                obj.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                obj.Order = (int)reader["Order"];
                obj.CalculationTypeId = (int)reader["CalculationTypeId"];
                obj.AccountTransactionTypeId = (int)reader["AccountTransactionTypeId"];
                obj.CalculationType = (int)reader["CalculationType"];
                obj.IncludeTax = (bool)reader["IncludeTax"];
                obj.Dynamic = (bool)reader["Dynamic"];
                obj.IsTax = (bool)reader["IsTax"];
                obj.IsDiscount = (bool)reader["IsDiscount"];
                obj.IncludeOtherCalculations = (bool)reader["IncludeOtherCalculations"];
                obj.DecreaseAmount = (bool)reader["DecreaseAmount"];
                obj.UsePlainSum = (bool)reader["UsePlainSum"];
                obj.Amount = (decimal)reader["Amount"];
                obj.CalculationAmount = (decimal)reader["CalculationAmount"];
                obj.CalculationTypeMap = (reader["CalculationTypeMap"] == DBNull.Value) ? string.Empty : (string)reader["CalculationTypeMap"];
                obj.CalculationSumValue = (decimal)reader["CalculationSumValue"];
                objects.Add(obj);
            }

            reader.Close();
            if (dbConn.State == ConnectionState.Open) dbConn.Close();
            return objects;
        }

        private static List<Payment> GetPayments(SqlConnection dbConn, int ticketId)
        {
            string sqlString = $@"select * from Payments where TicketId={ticketId}";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            if (dbConn.State == ConnectionState.Closed) dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            List<Payment> objects = new List<Payment>();
            Payment obj = null;
            while (reader.Read())
            {
                obj = new Payment();
                obj.Id = (int)reader["Id"];
                obj.TicketId = (int)reader["TicketId"];
                obj.PaymentTypeId = (int)reader["PaymentTypeId"];
                obj.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                obj.Date = (DateTime)reader["Date"];
                obj.AccountTransactionId = (int)reader["AccountTransactionId"];
                obj.Amount = (decimal)reader["Amount"];

                objects.Add(obj);

            }

            reader.Close();
            if (dbConn.State == ConnectionState.Open) dbConn.Close();
            return objects;
        }
        private static List<Calculation> GetAllCalculations(SqlConnection dbConn, DateTime fromDate, DateTime toDate)
        {
            string sqlString = $@"select * from Calculations where  Calculations.CalculationAmount <> 0 and Ticketid in (SELECT Id FROM tickets where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}')";
            List<Calculation> objects = dbConn.Query<Calculation>(sqlString, null, null, true, 3600, null).ToList();
            return objects;
        }

        private static List<Payment> GetAllPayments(SqlConnection dbConn, DateTime fromDate, DateTime toDate)
        {
            string sqlString = $@"select * from Payments where Ticketid in (SELECT Id FROM tickets where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}')";
            List<Payment> objects = dbConn.Query<Payment>(sqlString, null, null, true, 3600, null).ToList();
            return objects;
        }
        private static List<TicketEntity> GetTicketEntity(SqlConnection dbConn, int ticketId)
        {

            string sql = $@"select te.*, et.EntityName EntityTypeName, e.CustomData CustomDataFromEntity from TicketEntities te, EntityTypes et, Entities e 
										where te.EntityTypeId = et.Id and
										e.Id = te.EntityId and
										te.Ticket_Id={ticketId}";
            SqlCommand dbCommand = new SqlCommand(sql, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            if (dbConn.State == ConnectionState.Closed) dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();
            List<TicketEntity> objects = new List<TicketEntity>();
            TicketEntity obj = null;
            while (reader.Read())
            {
                obj = new TicketEntity();
                obj.Id = (int)reader["Id"];
                obj.EntityTypeId = (int)reader["EntityTypeId"];
                obj.EntityId = (int)reader["EntityId"];
                obj.EntityName = (string)reader["EntityName"];
                obj.AccountId = (int)reader["AccountId"];
                obj.AccountTypeId = (int)reader["AccountTypeId"];
                obj.EntityCustomData = (string)reader["EntityCustomData"];
                obj.EntityTypeName = (string)reader["EntityTypeName"];
                obj.CustomDataFromEntity = (string)reader["CustomDataFromEntity"];

                objects.Add(obj);

            }

            reader.Close();
            if (dbConn.State == ConnectionState.Open) dbConn.Close();
            return objects;
        }

        private static AccountTransactionDocument GetTransactionDocument(SqlConnection dbConn, int TransactionDocument_Id)
        {
            string sqlString = $@"select * from AccountTransactionDocuments where id={TransactionDocument_Id}";
            //       return dbConn.Query<AccountTransactionDocument, AccountTransaction, AccountTransactionValue>(sqlString, splitOn: "AccountTransactionDocumentId").FirstOrDefault();
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            if (dbConn.State == ConnectionState.Closed) dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            AccountTransactionDocument obj = null;
            while (reader.Read())
            {
                obj = new AccountTransactionDocument();
                obj.Id = (int)reader["Id"];
                obj.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                obj.Date = (DateTime)reader["Date"];
            }
            reader.Close();

            obj.AccountTransactions = GetAccountTransactions(dbConn, obj.Id);
            if (dbConn.State == ConnectionState.Open) dbConn.Close();
            return obj;
        }

        private static List<AccountTransaction> GetAccountTransactions(SqlConnection dbConn, int id)
        {
            string sqlString = $@"select * from AccountTransactions where AccountTransactionDocumentId={id}";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            SqlDataReader reader = dbCommand.ExecuteReader();

            List<AccountTransaction> objects = new List<AccountTransaction>();
            AccountTransaction obj = null;
            while (reader.Read())
            {
                obj = new AccountTransaction();
                obj.Id = (int)reader["Id"];
                obj.AccountTransactionDocumentId = (int)reader["AccountTransactionDocumentId"];
                obj.Amount = (decimal)reader["Amount"];
                obj.ExchangeRate = (decimal)reader["ExchangeRate"];
                obj.AccountTransactionTypeId = (int)reader["AccountTransactionTypeId"];
                obj.SourceAccountTypeId = (int)reader["SourceAccountTypeId"];
                obj.TargetAccountTypeId = (int)reader["TargetAccountTypeId"];
                obj.IsReversed = (bool)reader["IsReversed"];
                obj.Reversable = (bool)reader["Reversable"];
                obj.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];

                objects.Add(obj);

            }
            reader.Close();

            return GetAccountTransactionValue(dbConn, objects);
        }

        private static List<AccountTransaction> GetAccountTransactionValue(SqlConnection dbConn, List<AccountTransaction> acTrns)
        {
            foreach (AccountTransaction acTrn in acTrns)
            {
                string sqlString = $@"select * from AccountTransactionValues where AccountTransactionId={acTrn.Id}";
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();

                List<AccountTransactionValue> objects = new List<AccountTransactionValue>();
                AccountTransactionValue obj = null;
                while (reader.Read())
                {
                    obj = new AccountTransactionValue();
                    obj.Id = (int)reader["Id"];
                    obj.AccountTransactionId = (int)reader["AccountTransactionId"];
                    obj.AccountTransactionDocumentId = (int)reader["AccountTransactionDocumentId"];
                    obj.AccountTypeId = (int)reader["AccountTypeId"];
                    obj.AccountId = (int)reader["AccountId"];
                    obj.Date = (DateTime)reader["Date"];
                    obj.Debit = (decimal)reader["Debit"];
                    obj.Credit = (decimal)reader["Credit"];
                    obj.Exchange = (decimal)reader["Exchange"];
                    obj.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                    objects.Add(obj);
                }
                reader.Close();
                acTrn.AccountTransactionValues = objects;
            }

            return acTrns;
        }

        private static List<AccountTransactionDocument> GetAllTransactionDocument(SqlConnection dbConn, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string sqlString = $@"select * from AccountTransactionDocuments where id in (SELECT TransactionDocument_Id FROM tickets where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}')";
                List<AccountTransactionDocument> accountrandocs = dbConn.Query<AccountTransactionDocument>(sqlString, null, null, true, 3600, null).ToList();
                List<AccountTransaction> accounttran = GetAllAccountTransactions(dbConn, fromDate, toDate);
                foreach (AccountTransactionDocument actrandoc in accountrandocs)
                {
                    actrandoc.AccountTransactions = accounttran.Where(x => x.AccountTransactionDocumentId == actrandoc.Id).ToList();
                }
                return accountrandocs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static List<AccountTransaction> GetAllAccountTransactions(SqlConnection dbConn, DateTime fromDate, DateTime toDate)
        {
            string sqlString = $@"select * from AccountTransactions where AccountTransactionDocumentId in (SELECT TransactionDocument_Id FROM tickets where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}')";
            List<AccountTransaction> objects = dbConn.Query<AccountTransaction>(sqlString, null, null, true, 3600, null).ToList();
            return objects;// GetAllAccountTransactionValue(dbConn, objects, fromDate, toDate);
        }

        private static List<AccountTransaction> GetAllAccountTransactionValue(SqlConnection dbConn, List<AccountTransaction> acTrns, DateTime fromDate, DateTime toDate)
        {
            string actranIds = string.Empty;
            actranIds = string.Join(",", acTrns.Select(x => x.Id));
            string sqlString = $@"select * from AccountTransactionValues where credit + debit > 0 and AccountTransactionId in (select Id from AccountTransactions where AccountTransactionDocumentId in (SELECT TransactionDocument_Id FROM tickets where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}'))";
            List<AccountTransactionValue> listatvs = dbConn.Query<AccountTransactionValue>(sqlString).ToList(); //reader.Parse<AccountTransactionValue>().ToList();		
            acTrns.ForEach(t =>
            {
                t.AccountTransactionValues.AddRange(listatvs.Where(v => v.AccountTransactionId == t.Id));
            });
            return acTrns;
        }

        public static List<Ticket> GetTickets(int outletId, string departmentIds, DateTime fromDate, DateTime toDate)
        {
            List<Ticket> tickets = new List<Ticket>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(departmentIds))
                {
                    sqlClause = string.Format(@" and DepartmentId in ({0})", departmentIds);
                }

                if (outletId > 0)
                {
                    sqlClause += string.Format(@" and SyncOutletId = {0}", outletId);
                }

                string sqlString = $@"SELECT * FROM tickets 
													where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}' 
													and id=id {sqlClause}";

                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();

                tickets = reader.Parse<Ticket>().ToList();
                // Ticket ticket = null;

                //while (reader.Read())
                //{
                //    ticket = new Ticket
                //    {
                //        Id = (int)reader["Id"],
                //        LastUpdateTime = Convert.ToDateTime(reader["LastUpdateTime"]),
                //        TicketNumber = (string)reader["TicketNumber"],
                //        Date = Convert.ToDateTime(reader["Date"]),
                //        LastOrderDate = Convert.ToDateTime(reader["LastOrderDate"]),
                //        LastPaymentDate = Convert.ToDateTime(reader["LastPaymentDate"]),
                //        IsClosed = (bool)reader["IsClosed"],
                //        IsLocked = (bool)reader["IsLocked"],
                //        RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]),
                //        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                //        DepartmentId = (int)reader["DepartmentId"],
                //        TicketTypeId = (int)reader["TicketTypeId"],
                //        Note = (reader["Note"] == DBNull.Value) ? string.Empty : (string)reader["Note"],
                //        TicketTags = (reader["TicketTags"] == DBNull.Value) ? string.Empty : (string)reader["TicketTags"],
                //        TicketStates = (reader["TicketStates"] == DBNull.Value) ? string.Empty : (string)reader["TicketStates"],
                //        ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"]),
                //        TaxIncluded = (bool)reader["TaxIncluded"],
                //        Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"],
                //        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                //        TransactionDocument_Id = (reader["TransactionDocument_Id"] == DBNull.Value) ? 0 : (int)reader["TransactionDocument_Id"],
                //        ReturnAmount = Convert.ToDecimal(reader["ReturnAmount"]),
                //        NoOfGuests = Convert.ToInt32(reader["NoOfGuests"]),
                //        //Sum, PreTaxServicesTotal, PostTaxServicesTotal, PlainSum, PlainSumForEndOfPeriod, TaxTotal, ServiceChargeTotal, ActiveTimerAmount, ActiveTimerClosedAmount
                //        Sum = Convert.ToDecimal(reader["Sum"]),
                //        PreTaxServicesTotal = Convert.ToDecimal(reader["PreTaxServicesTotal"]),
                //        PostTaxServicesTotal = Convert.ToDecimal(reader["PostTaxServicesTotal"]),
                //        PlainSum = Convert.ToDecimal(reader["PlainSum"]),
                //        PlainSumForEndOfPeriod = Convert.ToDecimal(reader["PlainSumForEndOfPeriod"]),
                //        TaxTotal = Convert.ToDecimal(reader["TaxTotal"]),
                //        ServiceChargeTotal = Convert.ToDecimal(reader["ServiceChargeTotal"]),
                //        ActiveTimerAmount = Convert.ToDecimal(reader["ActiveTimerAmount"]),
                //        ActiveTimerClosedAmount = Convert.ToDecimal(reader["ActiveTimerClosedAmount"]),
                //        SyncOutletId = (reader["SyncOutletId"] == DBNull.Value) ? 0 : (int)reader["SyncOutletId"],
                //        ZoneId = (int)reader["ZoneId"]
                //    };
                //    tickets.Add(ticket);
                //}
                reader.Close();
                DataTable dtMenuItemPortionCost = GetAllMenuItemPortionCost().Tables[0];
                Parallel.ForEach(tickets, (tkt) =>
                 {
                     tkt.Orders = GetOrders(dbConn, tkt.Id, dtMenuItemPortionCost);
                     tkt.Calculations = GetCalculations(dbConn, tkt.Id);
                     tkt.Payments = GetPayments(dbConn, tkt.Id);
                     tkt.TransactionDocument = GetTransactionDocument(dbConn, tkt.TransactionDocument_Id);
                 });

                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return tickets;
        }


        public static DataTable GetItemIssue(int outletId, string departmentIds, DateTime fromDate, DateTime toDate)
        {

            DataSet ds = new DataSet();
            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();
            string sqlClause = string.Empty;
            if (!string.IsNullOrEmpty(departmentIds))
            {
                sqlClause = string.Format(@" AND o.DepartmentId in ({0})", departmentIds);
            }
            if (outletId > 0)
            {
                sqlClause += $@" AND SyncOutletId = {outletId}";
            }
            string sqlString = $@"SELECT m.GroupCode, MenuItemId, MenuItemName, PortionName,  price,
								sum(o.Quantity) Quantity, sum(o.Total)LineItemValue, sum(o.PlainTotal) Gross, sum(NetTicketUnitPrice * o.Quantity) NetAmount
								FROM Departments d , Tickets t, Orders o, menuitems  m
								WHERE
								t.Id = o.TicketId
								and m.Id = o.MenuItemId
								AND CalculatePrice = 1
								AND d.Id = o.DepartmentId
								AND t.Date BETWEEN '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' AND '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}' {sqlClause}
								GROUP BY m.GroupCode, MenuItemId, MenuItemName, PortionName, price";

            SqlDataAdapter da = new SqlDataAdapter(sqlString, dbConn);
            da.Fill(ds);
            return ds.Tables[0];
        }
        public static double GetVATIncludingDenominator(int OutletId)
        {
            return DBUtility.GetVATIncludingDenominator(OutletId);
        }

        public static List<Ticket> GetTicketsFaster(int outletId, string departmentIds, DateTime fromDate, DateTime toDate)
        {
            List<Ticket> tickets = new List<Ticket>();
            int i = 0;
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                string ticketIds = string.Empty;
                string trandocIds = string.Empty;
                SqlConnection dbConn = new SqlConnection(dbConnString);
                DataSet ds = new DataSet();
                dbConn.Open();
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(departmentIds))
                {
                    sqlClause = string.Format(@" AND DepartmentId in ({0})", departmentIds);
                }
                if (outletId > 0)
                {
                    sqlClause += $@" AND SyncOutletId = {outletId}";
                }
                string sqlString = $@"SELECT * FROM tickets where Date between '{fromDate.ToString("dd MMM yyyy hh:mm:ss tt")}' and '{toDate.ToString("dd MMM yyyy hh:mm:ss tt")}' {sqlClause}";
                tickets = dbConn.Query<Ticket>(sqlString, null, null, true, 3600, null).ToList();
                if (tickets.Count < 1)
                    return tickets;
                DataTable dtMenuItemPortionCost = GetAllMenuItemPortionCost().Tables[0];
                ticketIds = string.Join(",", tickets.Select(x => x.Id.ToString()));
                List<AccountTransactionDocument> allTranDocs = GetAllTransactionDocument(dbConn, fromDate, toDate);
                List<Order> allOrders = GetAllOrders(dbConn, fromDate, toDate, dtMenuItemPortionCost);
                List<Calculation> allCalculations = GetAllCalculations(dbConn, fromDate, toDate);
                List<Payment> allPayments = GetAllPayments(dbConn, fromDate, toDate);

                tickets.ForEach(ticket =>
                {
                    try
                    {
                        i = ticket.Id;
                        ticket.Orders = allOrders.Where(x => x.TicketId == ticket.Id).ToList();
                        ticket.Calculations = allCalculations.Where(x => x.TicketId == ticket.Id).ToList();
                        ticket.Payments = allPayments.Where(x => x.TicketId == ticket.Id).ToList();
                        ticket.TransactionDocument = allTranDocs.Where(x => x.Id == ticket.TransactionDocument_Id).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + ": " + i.ToString()); ;
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ": " + i.ToString());
            }
            return tickets;
        }

        //public static List<Ticket> GetTickets(DateTime fromDate, DateTime toDate, int departmentId)
        //{
        //    List<Ticket> tickets = new List<Ticket>();
        //    try
        //    {
        //        string dbConnString = DBUtility.GetConnectionString();
        //        SqlConnection dbConn = new SqlConnection(dbConnString);
        //        dbConn.Open();
        //        string sqlString = string.Empty;
        //        if(departmentId == 0)
        //            sqlString = string.Format(@"SELECT * FROM tickets where Date between '{0}' and '{1}'", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"));
        //        else
        //            sqlString = string.Format(@"SELECT * FROM tickets where Date between '{0}' and '{1}' and DepartmentId = {2} ", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"), departmentId);
        //        SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
        //        SqlDataReader reader = dbCommand.ExecuteReader();


        //        Ticket ticket = null;

        //        while (reader.Read())
        //        {
        //            ticket = new Ticket();
        //            ticket.Id = (int)reader["Id"];
        //            ticket.LastUpdateTime = Convert.ToDateTime(reader["LastUpdateTime"]);
        //            ticket.TicketNumber = (string)(reader["TicketNumber"]);
        //            ticket.Date = Convert.ToDateTime(reader["Date"]);
        //            ticket.LastOrderDate = Convert.ToDateTime(reader["LastOrderDate"]);
        //            ticket.LastPaymentDate = Convert.ToDateTime(reader["LastPaymentDate"]);
        //            ticket.IsClosed = (bool)(reader["IsClosed"]);
        //            ticket.IsLocked = (bool)(reader["IsLocked"]);
        //            ticket.RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]);
        //            ticket.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
        //            ticket.DepartmentId = (int)(reader["DepartmentId"]);
        //            ticket.TicketTypeId = (int)(reader["TicketTypeId"]);
        //            ticket.Note = (reader["Note"] == DBNull.Value) ? String.Empty : (string)reader["Note"];
        //            ticket.TicketTags = (reader["TicketTags"] == DBNull.Value) ? String.Empty : (string)reader["TicketTags"];
        //            ticket.TicketStates = (reader["TicketStates"] == DBNull.Value) ? String.Empty : (string)reader["TicketStates"];
        //            ticket.ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"]);
        //            ticket.TaxIncluded = (bool)(reader["TaxIncluded"]);
        //            ticket.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];
        //            ticket.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
        //            ticket.TransactionDocument_Id = (reader["TransactionDocument_Id"] == DBNull.Value) ? 0 : (int)reader["TransactionDocument_Id"];
        //            ticket.ReturnAmount = Convert.ToDecimal(reader["ReturnAmount"]);
        //            ticket.NoOfGuests = Convert.ToInt32(reader["NoOfGuests"]);
        //            //Sum, PreTaxServicesTotal, PostTaxServicesTotal, PlainSum, PlainSumForEndOfPeriod, TaxTotal, ServiceChargeTotal, ActiveTimerAmount, ActiveTimerClosedAmount
        //            ticket.Sum = Convert.ToDecimal(reader["Sum"]);
        //            ticket.PreTaxServicesTotal = Convert.ToDecimal(reader["PreTaxServicesTotal"]);
        //            ticket.PostTaxServicesTotal = Convert.ToDecimal(reader["PostTaxServicesTotal"]);
        //            ticket.PlainSum = Convert.ToDecimal(reader["PlainSum"]);
        //            ticket.PlainSumForEndOfPeriod = Convert.ToDecimal(reader["PlainSumForEndOfPeriod"]);
        //            ticket.TaxTotal = Convert.ToDecimal(reader["TaxTotal"]);
        //            ticket.ServiceChargeTotal = Convert.ToDecimal(reader["ServiceChargeTotal"]);
        //            ticket.ActiveTimerAmount = Convert.ToDecimal(reader["ActiveTimerAmount"]);
        //            ticket.ActiveTimerClosedAmount = Convert.ToDecimal(reader["ActiveTimerClosedAmount"]);
        //            ticket.SyncOutletId = (reader["SyncOutletId"] == DBNull.Value) ? 0 : (int)reader["SyncOutletId"];
        //            tickets.Add(ticket);
        //        }
        //        reader.Close();
        //        DataTable dtMenuItemPortionCost = GetAllMenuItemPortionCost().Tables[0];
        //        foreach (Ticket tkt in tickets)
        //        {
        //            tkt.Orders = GetOrders(dbConn, tkt.Id, dtMenuItemPortionCost);
        //            tkt.Calculations = GetCalculations(dbConn, tkt.Id);
        //            tkt.Payments = GetPayments(dbConn, tkt.Id);
        //            tkt.TransactionDocument = GetTransactionDocument(dbConn, tkt.TransactionDocument_Id);
        //        }

        //        dbConn.Close();
        //    }
        //    catch (Exception ex)
        //    { throw new Exception(ex.Message); }

        //    return tickets;
        //}

        //ok
        public static IEnumerable<Department> GetDepartmentCollection()
        {
            List<Department> departments = new List<Department>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT * FROM Departments");
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();
                Department department = null;

                while (reader.Read())
                {
                    department = new Department();
                    department.Id = (int)reader["Id"];
                    department.SortOrder = (int)reader["SortOrder"];
                    department.PriceTag = (reader["PriceTag"] == DBNull.Value) ? string.Empty : (string)reader["PriceTag"];
                    department.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                    department.WarehouseId = (int)reader["WarehouseId"];
                    department.TicketTypeId = (int)reader["TicketTypeId"];
                    department.TicketCreationMethod = (int)reader["TicketCreationMethod"];
                    department.Tag = (reader["Tag"] == DBNull.Value) ? string.Empty : (string)reader["Tag"];
                    departments.Add(department);
                }
                reader.Close();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return departments;
        }

        //ok
        public static IEnumerable<MenuItem> GetMenuItems()
        {
            List<MenuItem> menuitems = new List<MenuItem>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT * FROM MenuItems");
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();
                MenuItem menuitem = null;
                /*
				 * Id                INT IDENTITY (357,1) NOT NULL,
					GroupCode         NVARCHAR (4000) COLLATE SQL_Latin1_General_CP1_CI_AS,
					Barcode           NVARCHAR (4000) COLLATE SQL_Latin1_General_CP1_CI_AS,
					Tag               NVARCHAR (4000) COLLATE SQL_Latin1_General_CP1_CI_AS,
					Name              NVARCHAR (4000) COLLATE SQL_Latin1_General_CP1_CI_AS,
					StarID            INT,
					StarCode          NVARCHAR (4000) COLLATE SQL_Latin1_General_CP1_CI_AS,
					InventoryTakeType NVARCHAR (4000) COLLATE SQL_Latin1_General_CP1_CI_AS,
				 */

                while (reader.Read())
                {
                    menuitem = new MenuItem();
                    menuitem.Id = (int)reader["Id"];
                    menuitem.GroupCode = (reader["GroupCode"] == DBNull.Value) ? string.Empty : (string)reader["GroupCode"];
                    menuitem.Barcode = (reader["Barcode"] == DBNull.Value) ? string.Empty : (string)reader["Barcode"];
                    menuitem.Tag = (reader["Tag"] == DBNull.Value) ? string.Empty : (string)reader["Tag"];
                    menuitem.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                    menuitems.Add(menuitem);
                }
                reader.Close();

                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return menuitems;
        }
        public static IEnumerable<TaxTemplate> GetObjectTaxTemplates()
        {
            List<TaxTemplate> menuitems = new List<TaxTemplate>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT * FROM TaxTemplates");
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();
                TaxTemplate menuitem = null;

                while (reader.Read())
                {
                    menuitem = new TaxTemplate();
                    menuitem.Id = (int)reader["Id"];
                    menuitem.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                    menuitem.AccountTransactionType_Id = (int)reader["AccountTransactionType_Id"];
                    menuitems.Add(menuitem);
                }
                reader.Close();
                foreach (TaxTemplate tkt in menuitems)
                {
                    tkt.AccountTransactionType = GetAccountTransactionTypes(dbConn, tkt.AccountTransactionType_Id);
                }
                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return menuitems;
        }

        //ok
        public static IEnumerable<CalculationType> GetObjectCalculationTypes()
        {
            List<CalculationType> menuitems = new List<CalculationType>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT * FROM CalculationTypes");
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();
                CalculationType menuitem = null;

                while (reader.Read())
                {
                    menuitem = new CalculationType();
                    menuitem.Id = (int)reader["Id"];
                    menuitem.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                    menuitem.IsDiscount = (bool)reader["IsDiscount"];
                    menuitem.IsTax = (bool)reader["IsTax"];
                    menuitem.IsSD = (bool)reader["IsSD"];
                    menuitem.IsServiceCharge = (bool)reader["IsServiceCharge"];
                    menuitem.DecreaseAmount = (bool)reader["DecreaseAmount"];
                    menuitem.AccountTransactionType_Id = (int)reader["AccountTransactionType_Id"];
                    menuitems.Add(menuitem);
                }

                reader.Close();
                foreach (CalculationType tkt in menuitems)
                {
                    tkt.AccountTransactionType = GetAccountTransactionTypes(dbConn, tkt.AccountTransactionType_Id);
                }
                dbConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return menuitems;
        }

        //ok
        public static List<string> GetPaymentNames()
        {
            List<string> menuitems = new List<string>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sqlString = string.Format(@"select distinct name from payments");
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();
                string menuitem = string.Empty;

                while (reader.Read())
                {
                    menuitem = (reader["Name"] == DBNull.Value) ? "[Undefined]" : (string)reader["Name"];
                    menuitems.Add(menuitem);
                }

                reader.Close();
                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return menuitems;
        }
        private static AccountTransactionType GetAccountTransactionTypes(SqlConnection dbConn, int taxTemplateId)
        {
            string sqlString = string.Format(@"select * FROM AccountTransactionTypes where Id = {0};", taxTemplateId);
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            SqlDataReader reader = dbCommand.ExecuteReader();

            List<AccountTransactionType> accountTransactionTypes = new List<AccountTransactionType>();
            AccountTransactionType accountTransactionType = null;

            while (reader.Read())
            {
                accountTransactionType = new AccountTransactionType();
                accountTransactionType.Id = (int)reader["Id"];
                accountTransactionType.SortOrder = (int)reader["SortOrder"];
                accountTransactionType.SourceAccountTypeId = (int)reader["SourceAccountTypeId"];
                accountTransactionType.TargetAccountTypeId = (int)reader["TargetAccountTypeId"];
                accountTransactionType.DefaultSourceAccountId = (int)reader["DefaultSourceAccountId"];
                accountTransactionType.DefaultTargetAccountId = (int)reader["DefaultTargetAccountId"];
                accountTransactionType.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                accountTransactionTypes.Add(accountTransactionType);
            }

            reader.Close();
            return accountTransactionTypes.First();
        }
        private static List<AccountTransactionType> GetAccountTransactionTypes()
        {
            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();
            string sqlString = string.Format(@"select * FROM AccountTransactionTypes;");
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
            SqlDataReader reader = dbCommand.ExecuteReader();

            List<AccountTransactionType> accountTransactionTypes = new List<AccountTransactionType>();
            AccountTransactionType accountTransactionType = null;
            while (reader.Read())
            {
                accountTransactionType = new AccountTransactionType();
                accountTransactionType.Id = (int)reader["Id"];
                accountTransactionType.SortOrder = (int)reader["SortOrder"];
                accountTransactionType.SourceAccountTypeId = (int)reader["SourceAccountTypeId"];
                accountTransactionType.TargetAccountTypeId = (int)reader["TargetAccountTypeId"];
                accountTransactionType.DefaultSourceAccountId = (int)reader["DefaultSourceAccountId"];
                accountTransactionType.DefaultTargetAccountId = (int)reader["DefaultTargetAccountId"];
                accountTransactionType.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                accountTransactionTypes.Add(accountTransactionType);
            }
            reader.Close();
            dbConn.Close();
            return accountTransactionTypes;
        }
        //ok
        public static DataSet GetAdvanceDue(DateTime fromDate, DateTime toDate)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format(@"
				SELECT 
				'CustomerCashPayment' = (SELECT isnull(sum(isnull(debit, 0)), 0) CustomerCashPayment
										FROM AccountTransactionValues 
										WHERE 
										[Date] between '{0}' and '{1}'
										AND AccountId = 5 
										AND Name = 'Customer Cash Payment'),
				'CustomerCreditPayment' = (SELECT isnull(sum(isnull(debit, 0)), 0) CustomerCreditPayment
										FROM AccountTransactionValues
										WHERE 
										[Date] between '{0}' and '{1}'
										AND AccountId = 6 
										AND Name = 'Customer Credit Card Payment')
				", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"));

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

        public static List<Ticket> GetTickets(DateTime fromDate, DateTime toDate, int departmentid, int OutletId)
        {
            List<Ticket> tickets = new List<Ticket>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                string sAdditionalClause = string.Empty;

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                if (departmentid > 0)
                {
                    sAdditionalClause = string.Format(" and departmentid = {0}", departmentid);
                }

                if (OutletId > 0)
                {
                    sAdditionalClause += string.Format("and SyncOutletId ={0}", OutletId);
                }

                string sqlString = string.Format(@"SELECT * FROM tickets where Date between '{0}' and '{1}' {2}", fromDate.ToString("dd MMM yyyy hh:mm:ss tt"), toDate.ToString("dd MMM yyyy hh:mm:ss tt"), sAdditionalClause);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                dbCommand.CommandTimeout = dbConn.ConnectionTimeout;
                SqlDataReader reader = dbCommand.ExecuteReader();

                Ticket ticket = null;
                DataSet ds = GetTickets(fromDate: fromDate, toDate: toDate, TicketType: 0, isOnlyOpenTicket: false, OutletId: 0);

                while (reader.Read())
                {
                    ticket = new Ticket();
                    ticket.Id = (int)reader["Id"];
                    ticket.LastUpdateTime = Convert.ToDateTime(reader["LastUpdateTime"]);
                    ticket.TicketNumber = (string)reader["TicketNumber"];
                    ticket.Date = Convert.ToDateTime(reader["Date"]);
                    ticket.LastOrderDate = Convert.ToDateTime(reader["LastOrderDate"]);
                    ticket.LastPaymentDate = Convert.ToDateTime(reader["LastPaymentDate"]);
                    ticket.IsClosed = (bool)reader["IsClosed"];
                    ticket.IsLocked = (bool)reader["IsLocked"];
                    ticket.RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]);
                    ticket.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    ticket.DepartmentId = (int)reader["DepartmentId"];
                    ticket.TicketTypeId = (int)reader["TicketTypeId"];
                    ticket.Note = (reader["Note"] == DBNull.Value) ? string.Empty : (string)reader["Note"];
                    ticket.TicketTags = (reader["TicketTags"] == DBNull.Value) ? string.Empty : (string)reader["TicketTags"];
                    ticket.TicketStates = (reader["TicketStates"] == DBNull.Value) ? string.Empty : (string)reader["TicketStates"];
                    ticket.ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"]);
                    ticket.TaxIncluded = (bool)reader["TaxIncluded"];
                    ticket.Name = (reader["Name"] == DBNull.Value) ? string.Empty : (string)reader["Name"];
                    ticket.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                    ticket.TransactionDocument_Id = (reader["TransactionDocument_Id"] == DBNull.Value) ? 0 : (int)reader["TransactionDocument_Id"];
                    ticket.ReturnAmount = Convert.ToDecimal(reader["ReturnAmount"]);
                    ticket.NoOfGuests = Convert.ToInt32(reader["NoOfGuests"]);
                    ticket.Sum = Convert.ToDecimal(reader["Sum"]);
                    ticket.PreTaxServicesTotal = Convert.ToDecimal(reader["PreTaxServicesTotal"]);
                    ticket.PostTaxServicesTotal = Convert.ToDecimal(reader["PostTaxServicesTotal"]);
                    ticket.PlainSum = Convert.ToDecimal(reader["PlainSum"]);
                    ticket.PlainSumForEndOfPeriod = Convert.ToDecimal(reader["PlainSumForEndOfPeriod"]);
                    ticket.TaxTotal = Convert.ToDecimal(reader["TaxTotal"]);
                    ticket.ServiceChargeTotal = Convert.ToDecimal(reader["ServiceChargeTotal"]);
                    ticket.ActiveTimerAmount = Convert.ToDecimal(reader["ActiveTimerAmount"]);
                    ticket.ActiveTimerClosedAmount = Convert.ToDecimal(reader["ActiveTimerClosedAmount"]);
                    ticket.ZoneId = (int)reader["ZoneId"];


                    try
                    {
                        DataRow[] result = ds.Tables[0].Select("id =" + ticket.Id);
                        ticket.Waiter = result[0]["Waiter"].ToString() == string.Empty ? "NA" : result[0]["Waiter"].ToString();
                    }
                    catch
                    {
                        ticket.Waiter = "NA";
                    }

                    tickets.Add(ticket);
                }
                reader.Close();
                DataTable dtMenuItemPortionCost = GetAllMenuItemPortionCost().Tables[0];
                foreach (Ticket tkt in tickets)
                {
                    tkt.Orders = GetOrders(dbConn, tkt.Id, dtMenuItemPortionCost);
                    tkt.Calculations = GetCalculations(dbConn, tkt.Id);
                    tkt.Payments = GetPayments(dbConn, tkt.Id);
                    tkt.TransactionDocument = GetTransactionDocument(dbConn, tkt.TransactionDocument_Id);
                }

                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return tickets;
        }
        public static DataSet GetAllMenuItemPortionCost()
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"
											WITH summary AS 
											(
												SELECT p.InventoryItem_Id, 
													   p.Price, p.Unit,
													   ROW_NUMBER() OVER(PARTITION BY p.InventoryItem_Id 
																			 ORDER BY Id DESC) AS rk
												FROM InventoryTransactions p
											)


											SELECT MenuItemID, PortionName, sum(isnull(Quantity, 0)*isnull(Price, 0)) ProductionCost
											FROM 
											(
												SELECT mi.Id, MenuItemID, mi.Name MenuItemName, mip.Name PortionName, mip.id MenuPortionId, r.Name RecipeName, i.Name InventoryItemName, ri.Quantity, i.BaseUnit,
												'Price' =(
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
														  )
												FROM MenuItems mi, menuitemportions mip,
												Recipes r, recipeitems ri, inventoryitems i
												WHERE
												mi.Id = mip.MenuItemId AND mip.Id = r.Portion_Id
												AND  r.Id = ri.RecipeId AND ri.InventoryItem_Id = i.Id
											)Q
											GROUP BY MenuItemID, PortionName");

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
        //ok
        public static DataSet GetWorkPeriodRange(string StartDate, string EndDate)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"SELECT StartDate = [dbo].[ufsFormat] ((SELECT min(w.startdate) FROM WorkPeriods w WHERE w.StartDate > '{0}'), 'mmm dd yyyy hh:mm AM/PM'),
											EndDate = [dbo].[ufsFormat] ((SELECT enddate FROM WorkPeriods WHERE StartDate = (SELECT max(w.startdate) FROM WorkPeriods w WHERE w.StartDate <= Dateadd(Day, 1, '{1}'))), 'mmm dd yyyy hh:mm AM/PM')", StartDate, EndDate);

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
        //ok
        public static DataSet GetItemSalesAccountWise(string StartDate, string EndDate, int DepartmentId, int SyncOutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();

                string sqlClause = string.Empty;
                string sqlClause2 = string.Empty;
                if (SyncOutletId > 0)
                {
                    sqlClause = string.Format(@" and SyncOutletId = {0}", SyncOutletId);
                    sqlClause2 = string.Format(@" and tick.SyncOutletId = {0}", SyncOutletId);
                }
                if (DepartmentId > 0)
                {
                    sqlClause = sqlClause + string.Format(@" and t.DepartmentId = {0}", DepartmentId);
                    sqlClause2 = sqlClause2 + string.Format(@" and tick.DepartmentId = {0}", DepartmentId);
                }

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                #region Old
                //     string sql1 = string.Format(@"select DepartmentName,MenuItemID,
                //                                 'GroupCode' =
                //(

                //	SELECT max(GroupCode) from MenuItems
                //	where id = t.MenuItemID
                //), 
                //                                 MenuItemName, PortionName,Price,
                //                                   'TotalCollection' =
                //                                 (
                //                                  	SELECT sum(Collection) FROM 
                //                                  	(
                //			SELECT o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
                //			CASE WHEN tick.PlainSum = 0 THEN 0
                //			ELSE  o.Total/tick.PlainSum*tick.TotalAmount
                //			END Collection
                //			FROM 
                //			Orders o, tickets tick
                //			WHERE O.ticketid = tick.ID
                //			AND tick.[Date] between '{0}' and '{1}'
                //			AND o.DecreaseInventory = 1															
                //		)l
                //		WHERE l.menuitemid = t.menuitemid
                //		AND l.MenuItemName = t.menuitemname
                //		AND l.portionname = t.portionname
                //		AND l.departmentid = t.DepartmentID
                //		AND l.price = t.Price
                //                                         --AND t.UltimateAccount = 'Sales'
                //		GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
                //                                 ),

                //                                 'Quantity' = 
                //                                 (
                //                                  	SELECT sum(quantity) 
                //                                      FROM Orders, Tickets tick 
                //                                      WHERE orders.DepartmentID = t.DepartmentID 
                //                                      AND tick.Id = Orders.TicketId
                //                                      AND menuitemid = t.menuitemid
                //                                      AND MenuItemName = t.menuitemname
                //                                      AND portionname = t.portionname
                //                                      AND Price = t.Price
                //                                      AND Orders.DecreaseInventory = 1	
                //                                      AND tick.[Date] between '{0}' and '{1}'
                //                                 ),
                //                                 'Gross' = 
                //                                 (
                //                                      SELECT sum(PlainTotal+OrderTagPrice*Quantity) 
                //                                      FROM Orders, Tickets tick 
                //                                      WHERE orders.DepartmentID = t.DepartmentID 
                //                                      AND tick.Id = Orders.TicketId
                //                                      AND menuitemid = t.menuitemid
                //                                      AND MenuItemName = t.menuitemname
                //                                      AND portionname = t.portionname
                //                                      AND Price = t.Price
                //                                      AND Orders.DecreaseInventory = 1	
                //                                      AND tick.[Date] between '{0}' and '{1}'
                //                                 ),
                //                                 'Gift' = 
                //                                 (
                //                                      SELECT sum((PlainTotal+OrderTagPrice*Quantity)-Total) 
                //                                      FROM Orders, Tickets tick 
                //                                      WHERE orders.DepartmentID = t.DepartmentID 
                //                                      AND tick.Id = Orders.TicketId
                //                                      AND menuitemid = t.menuitemid
                //                                      AND MenuItemName = t.menuitemname
                //                                      AND portionname = t.portionname
                //                                      AND Price = t.Price
                //                                      AND Orders.DecreaseInventory = 1	
                //                                      AND tick.[Date] between '{0}' and '{1}'
                //                                 ),
                //                                 SortOrder =
                //                                 case when UltimateAccount is null then 0
                //                                 else SortOrder
                //                                 end,
                //                                 'OrderTotal' =
                //                                 (
                //                                  	SELECT sum(OrderTotal) FROM 
                //                                  	(
                //			SELECT o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
                //			max(OrderTotal)OrderTotal
                //			FROM 
                //			TicketItemview o														
                //			where o.[ticketDate] between '{0}' and '{1}'
                //			group by o.departmentid, menuitemid, MenuItemName, portionname, o.Price
                //		)l
                //		WHERE l.menuitemid = t.menuitemid
                //		AND l.MenuItemName = t.menuitemname
                //		AND l.portionname = t.portionname
                //		AND l.departmentid = t.DepartmentID
                //		AND l.price = t.Price
                //		GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
                //                                 ),
                //                                 isnull(UltimateAccount, '_') UltimateAccount,
                //                                 SUM(DistinctAmount) DistinctAmount
                //                                 from TicketItemView t
                //                                 where T.TicketDate between '{0}' and '{1}'
                //                                 {2}
                //                                 --AND MenuItemName = 'Affogato'
                //                                 group by DepartmentID, DepartmentName,MenuItemID, menuitemname, portionname, UltimateAccount, SortOrder,price
                //                                 order by menuitemname,DepartmentName,Portionname,SortOrder", StartDate,EndDate, sqlClause); 
                #endregion

                string sql1 = string.Format(@"Select * from (
												select MenuItemID,
														'GroupCode' =
														(
			
															SELECT max(GroupCode) from MenuItems
															where id = t.MenuItemID
														), 
														MenuItemName, PortionName,Price,
															'TotalCollection' =
														(
																SELECT sum(Collection) FROM 
																(
																	SELECT menuitemid, MenuItemName, portionname, o.Price,
																	CASE WHEN tick.PlainSum = 0 THEN 0
																	ELSE  o.Total/tick.PlainSum*tick.TotalAmount
																	END Collection
																	FROM 
																	Orders o, tickets tick
																	WHERE O.ticketid = tick.ID
																	AND tick.[Date] between '{0}' and '{1}'
																	AND o.DecreaseInventory = 1	
																	{3}													
																)l
																WHERE l.menuitemid = t.menuitemid
																AND l.MenuItemName = t.menuitemname
																AND l.portionname = t.portionname
									
																AND l.price = t.Price
																--AND t.UltimateAccount = 'Sales'
																GROUP BY menuitemid, MenuItemName, PortionName, Price																								
														),
											
														'Quantity' = 
														(
																SELECT sum(quantity) 
																FROM Orders, Tickets tick 
																WHERE  tick.Id = Orders.TicketId
																AND menuitemid = t.menuitemid
																AND MenuItemName = t.menuitemname
																AND portionname = t.portionname
																AND Price = t.Price
																AND Orders.DecreaseInventory = 1
																AND tick.[Date] between '{0}' and '{1}'
																{3}	
														),
														'Gross' = 
														(
																SELECT sum(PlainTotal+OrderTagPrice*Quantity) 
																FROM Orders, Tickets tick 
																WHERE  tick.Id = Orders.TicketId
																AND menuitemid = t.menuitemid
																AND MenuItemName = t.menuitemname
																AND portionname = t.portionname
																AND Price = t.Price
																AND Orders.DecreaseInventory = 1	
																AND tick.[Date] between '{0}' and '{1}'
																{3}	
														),
														'Gift' = 
														(
																SELECT sum((PlainTotal+OrderTagPrice*Quantity)-Total) 
																FROM Orders, Tickets tick 
																WHERE  tick.Id = Orders.TicketId
																AND menuitemid = t.menuitemid
																AND MenuItemName = t.menuitemname
																AND portionname = t.portionname
																AND Price = t.Price
																AND Orders.DecreaseInventory = 1	
																AND tick.[Date] between '{0}' and '{1}'
																{3}		
														),
														SortOrder =
														case when UltimateAccount is null then 0
														else SortOrder
														end,
														'OrderTotal' =
														(
																SELECT sum(OrderTotal) FROM 
																(
																	SELECT menuitemid, MenuItemName, portionname, tick.Price,
																	max(OrderTotal)OrderTotal
																	FROM 
																	TicketItemview tick														
																	where tick.[ticketDate] between '{0}' and '{1}'
																	{3}		
																	group by menuitemid, MenuItemName, portionname, tick.Price
																)l
																WHERE l.menuitemid = t.menuitemid
																AND l.MenuItemName = t.menuitemname
																AND l.portionname = t.portionname
																AND l.price = t.Price
																GROUP BY menuitemid, MenuItemName, PortionName, Price																								
														),
														isnull(UltimateAccount, '_') UltimateAccount,
														SUM(DistinctAmount) DistinctAmount
														from TicketItemView t
														where T.TicketDate between '{0}' and '{1}'
														{2}
														--AND MenuItemName = 'Affogato'
														group by MenuItemID, menuitemname, portionname, UltimateAccount, SortOrder,price
			
													)G
													order by menuitemname,Portionname,SortOrder", StartDate, EndDate, sqlClause, sqlClause2);

                string sql2 = string.Format(@"select 
												  'SUMTotalCollection' =
												(
														SELECT isnull(sum(Collection),0) FROM 
														(
															SELECT o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
															CASE WHEN t.PlainSum = 0 THEN 0
															ELSE  o.Total/t.PlainSum*t.TotalAmount
															END Collection
															FROM 
															Orders o, tickets t
															WHERE O.ticketid = t.ID
															AND t.[Date]  between '{0}' and '{1}'
															AND o.DecreaseInventory = 1
															{2}															
														)l				
														--GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
												),
												'SUMPrice' = 
													(
															SELECT isnull(sum(Price),0) 
															FROM Orders, Tickets t 
															WHERE  t.Id = Orders.TicketId
															AND Orders.DecreaseInventory = 1	
															AND t.[Date] between '{0}' and '{1}'
															{2}
													),
												'SUMQuantity' = 
												(
														SELECT isnull(sum(quantity),0) 
														FROM Orders, Tickets t 
														WHERE  t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date]  between '{0}' and '{1}'
														{2}
												),
												'SUMGross' = 
												(
														SELECT isnull(sum(PlainTotal+OrderTagPrice*Quantity),0) 
														FROM Orders, Tickets t 
														Where t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												),
												'SUMGift' = 
												(
														SELECT isnull(sum((PlainTotal+OrderTagPrice*Quantity)-Total),0) 
														FROM Orders, Tickets t 
														WHERE t.Id = Orders.TicketId                
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												)", StartDate, EndDate, sqlClause);

                string sql = sql1 + ";" + sql2;
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                //AND tick.[Date] between '{0}' and '{1}'
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);

                ds.Tables[0].TableName = "ItemSalesAccountWise";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }


            return ds;
        }
        public static DataSet GetSumItemSalesAccountWiseGroup(int SyncOutletId, string DepartmentIds, DateTime StartDate, DateTime EndDate)
        {
            DataSet ds = new DataSet();
            string dbConnString = DBUtility.GetConnectionString();

            string sqlClause = string.Empty;
            string sqlClause2 = string.Empty;
            if (SyncOutletId > 0)
            {
                sqlClause = string.Format(@" and SyncOutletId = {0}", SyncOutletId);
                sqlClause2 = string.Format(@" and tick.SyncOutletId = {0}", SyncOutletId);
            }
            if (!string.IsNullOrEmpty(DepartmentIds))
            {
                sqlClause = sqlClause + string.Format(@" and t.DepartmentId in ({0})", DepartmentIds);
                sqlClause2 = sqlClause2 + string.Format(@" and tick.DepartmentId in ({0})", DepartmentIds);
            }

            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();

            string sql2 = string.Format(@"select 
												  'SUMTotalCollection' =
												   (
														SELECT isnull(sum(Collection),0) FROM 
														(
															SELECT o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
															CASE WHEN t.PlainSum = 0 THEN 0
															ELSE  o.Total/t.PlainSum*t.TotalAmount
															END Collection
															FROM 
															Orders o, tickets t
															WHERE O.ticketid = t.ID
															AND t.[Date]  between '{0}' and '{1}'
															AND o.DecreaseInventory = 1
															{2}															
														)l				
														--GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
												),
												'SUMPrice' = 
													(
															SELECT isnull(sum(Price),0) 
															FROM Orders, Tickets t 
															WHERE  t.Id = Orders.TicketId
															AND Orders.DecreaseInventory = 1	
															AND t.[Date] between '{0}' and '{1}'
															{2}
													),
												'SUMQuantity' = 
												(
														SELECT isnull(sum(quantity),0) 
														FROM Orders, Tickets t 
														WHERE  t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date]  between '{0}' and '{1}'
														{2}
												),
												'SUMGross' = 
												(
														SELECT isnull(sum(PlainTotal+OrderTagPrice*Quantity),0) 
														FROM Orders, Tickets t 
														Where t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												),
												'SUMGift' = 
												(
														SELECT isnull(sum((PlainTotal+OrderTagPrice*Quantity)-Total),0) 
														FROM Orders, Tickets t 
														WHERE t.Id = Orders.TicketId                
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												)", StartDate, EndDate, sqlClause);

            string sql = sql2;
            SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
            //AND tick.[Date] between '{0}' and '{1}'
            da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
            da.Fill(ds);
            ds.Tables[0].TableName = "SUMs";
            dbConn.Close();

            return ds;
        }
        public static DataSet GetItemSalesAccountWiseGroup(string StartDate, string EndDate, string DepartmentIds, int SyncOutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<Ticket> oTickets = GetTicketsFaster(SyncOutletId, DepartmentIds, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
                IEnumerable<MenuItem> oMenuItems = GetMenuItems();
                IEnumerable<TaxTemplate> oTaxTemplates = GetObjectTaxTemplates();
                IEnumerable<CalculationType> oCalculationTypes = GetObjectCalculationTypes();
                var incomeCalculator = GetIncomeCalculator(oTickets);
                List<string> sPaymentNames = GetPaymentNames();//select distict name from payments
                                                               //                
                var menGroupSalesInfo = MenuGroupBuilder.CalculateOrderSalesInfo(oTickets, oMenuItems).ToList();
                var menGroupsOrderCalculationsInfo = MenuGroupBuilder.CalculateMenuItemOrderGroupCalculationInfo(oTickets, oMenuItems, oCalculationTypes).ToList();
                var menGroupTaxesInfo = MenuGroupBuilder.CalculateMenuItemOrderWiseAccountsInfo(oTickets, oMenuItems, oTaxTemplates).ToList();
                //
                //                
                var menGroupsAccountsInfo = MenuGroupBuilder.CalculateMenuItemGroupAccountsInfo(oTickets, oMenuItems, oTaxTemplates).ToList();
                var menGroupsCalculationsInfo = MenuGroupBuilder.CalculateMenuItemGroupCalculationInfo(oTickets, oMenuItems, oCalculationTypes).ToList();
                //var menGroupsPaymentsInfo = MenuGroupBuilder.CalculatePaymentInfo(oTickets, oMenuItems, sPaymentNames).ToList();//sPaymentNames
                var menGroupsPaymentsInfo = MenuGroupBuilder.CalculateGroupCodePaymentInfo(oTickets, menGroupSalesInfo, menGroupsOrderCalculationsInfo, menGroupTaxesInfo, sPaymentNames, oTaxTemplates, oCalculationTypes).ToList();
                var menGroupsSalesInfo = MenuGroupBuilder.CalculateSalesInfo(oTickets, oMenuItems).ToList();
                var menGroupsGiftInfo = MenuGroupBuilder.CalculateGiftInfo(oTickets, oMenuItems).ToList();
                //var menIncludingVATInfo = MenuGroupBuilder.CalculateIncludingTaxInfo(oTickets, oMenuItems).ToList();
                //var menAllPaymentInfoInfo = MenuGroupBuilder.CalculatePaymentTotalInfo(oTickets, oMenuItems, sPaymentNames).ToList();//sPaymentNames
                var TempmenAllPaymentInfoInfo =
                from info in menGroupsPaymentsInfo
                group info by new { GroupCode = info.GroupName } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Total",
                    SortOrder = 3000,
                    Quantity = 0,
                    Amount = grp.Sum(y => y.Amount),
                    OrderTotal = 0,
                    GrossAmount = 0
                };
                var menAllPaymentInfoInfo = TempmenAllPaymentInfoInfo.ToList();
                //
                DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menGroupsAccountsInfo);
                DataSet dsMenGroupsCalculationsInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menGroupsCalculationsInfo);
                DataSet dsMenGroupsPaymentsInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menGroupsPaymentsInfo);
                DataSet dsMenGroupsSalesInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menGroupsSalesInfo);
                DataSet dsMenGroupsGiftInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menGroupsGiftInfo);
                //DataSet dsmenIncludingVATInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menIncludingVATInfo);
                DataSet dsmenAllPaymentInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menAllPaymentInfoInfo);
                //
                DataSet dsMenGroupItemSalesInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupSalesInfo);
                DataSet dsMenGroupItemCalculationInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupsOrderCalculationsInfo);
                DataSet dsMenGroupItemTaxInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupTaxesInfo);
                DataSet dsVoid = TicketManager.VoidOrdersGroupWise(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), "Void", SyncOutletId, DepartmentIds);
                DataSet dsCancel = TicketManager.VoidOrdersGroupWise(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), "Cancel", SyncOutletId, DepartmentIds);
                //

                DataTable masterTable = new DataTable("ItemSalesAccountWise");

                masterTable.Columns.AddRange(new DataColumn[] { new DataColumn("GroupCode"), new DataColumn("Gross", typeof(decimal)), new DataColumn("Quantity", typeof(decimal)),
                new DataColumn("Gift", typeof(decimal) ), new DataColumn("SortOrder", typeof(int)), new DataColumn("UltimateAccount"), new DataColumn("DistinctAmount", typeof(decimal)),
                new DataColumn("TotalCollection", typeof(decimal))});

                foreach (var groupAccountsInfo in menGroupsSalesInfo)
                {
                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, -1, "Quantity", groupAccountsInfo.Quantity });
                }
                foreach (var groupAccountsInfo in menGroupsAccountsInfo)
                {
                    if (groupAccountsInfo.Amount == 0)
                    {
                        continue;
                    }

                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                foreach (var groupAccountsInfo in menGroupsCalculationsInfo)
                {
                    if (groupAccountsInfo.Amount == 0)
                    {
                        continue;
                    }

                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                foreach (var groupAccountsInfo in menGroupsPaymentsInfo)
                {
                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, 1000, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                foreach (var groupAccountsInfo in menGroupsSalesInfo)
                {
                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, 1, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                //foreach (var groupAccountsInfo in menGroupsGiftInfo)
                //{
                //    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                //}
                //foreach (var groupAccountsInfo in menIncludingVATInfo)
                //{
                //    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, 75, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                //}
                foreach (var groupAccountsInfo in menAllPaymentInfoInfo)
                {
                    if (groupAccountsInfo.Amount == 0)
                    {
                        continue;
                    }

                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }

                foreach (DataRow dr in dsVoid.Tables[0].Rows)
                {
                    masterTable.Rows.Add(new object[] { dr["GroupName"].ToString(), Convert.ToDouble(dr["Amount"]).ToString("0.00"), Convert.ToDouble(dr["Amount"]).ToString("0.00"), 0, 5000, "Void Amount", Convert.ToDouble(dr["Amount"]).ToString("0.00") });
                }
                foreach (DataRow dr in dsCancel.Tables[0].Rows)
                {
                    masterTable.Rows.Add(new object[] { dr["GroupName"].ToString(), Convert.ToDouble(dr["Amount"]).ToString("0.00"), Convert.ToDouble(dr["Amount"]).ToString("0.00"), 0, 5000, "Cancel Amount", Convert.ToDouble(dr["Amount"]).ToString("0.00") });
                }
                DataView dv = masterTable.DefaultView;
                dv.Sort = "GroupCode asc, SortOrder asc";
                masterTable = dv.ToTable();
                string dbConnString = DBUtility.GetConnectionString();

                string sqlClause = string.Empty;
                string sqlClause2 = string.Empty;
                if (SyncOutletId > 0)
                {
                    sqlClause = string.Format(@" and SyncOutletId = {0}", SyncOutletId);
                    sqlClause2 = string.Format(@" and tick.SyncOutletId = {0}", SyncOutletId);
                }
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = sqlClause + string.Format(@" and t.DepartmentId in ({0})", DepartmentIds);
                    sqlClause2 = sqlClause2 + string.Format(@" and tick.DepartmentId in ({0})", DepartmentIds);
                }

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                //string sql1 = string.Format(@"
                //                            select 
                //                            GroupCode,
                //                                'TotalCollection' =
                //                            (
                //                                 SELECT sum(Collection) FROM 
                //                                 (
                //                               SELECT o.departmentid, m.GroupCode, menuitemid, MenuItemName, portionname, o.Price,
                //                               CASE WHEN tick.PlainSum = 0 THEN 0
                //                               ELSE  o.Total/tick.PlainSum*tick.TotalAmount
                //                               END Collection
                //                               FROM 
                //                               Orders o, tickets tick, MenuItems m
                //                               WHERE O.ticketid = tick.ID
                //                               AND m.Id = o.MenuItemId
                //                               AND tick.[Date] between '{0}' and '{1}'
                //                               AND o.DecreaseInventory = 1	
                //                                        {3}														
                //                              )l
                //                              WHERE l.GroupCode = t.GroupCode       
                //                              GROUP BY GroupCode
                //                            ),

                //                            'Quantity' = 
                //                            (
                //                                 SELECT sum(quantity) 
                //                                    FROM Orders, Tickets tick, MenuItems m
                //                                    WHERE 
                //                                        tick.Id = Orders.TicketId
                //                                    AND Orders.MenuItemId = m.Id
                //                                    AND m.GroupCode = t.groupcode
                //                                    AND Orders.DecreaseInventory = 1	
                //                                    AND tick.[Date] between '{0}' and '{1}'
                //                                    {3}
                //                            ),
                //                            'Gross' = 
                //                            (
                //                                    SELECT sum(PlainTotal+OrderTagPrice*Quantity) 
                //                                    FROM Orders, Tickets tick, MenuItems m 
                //                                    WHERE
                //                                        tick.Id = Orders.TicketId
                //                                    AND OrderS.MenuItemId = m.Id
                //                                    AND m.GroupCode = t.GroupCode
                //                                    AND Orders.DecreaseInventory = 1	
                //                                    AND tick.[Date] between '{0}' and '{1}'
                //                                    {3}
                //                            ),
                //                            'Gift' = 
                //                            (
                //                                    SELECT sum((PlainTotal+OrderTagPrice*Quantity)-Total) 
                //                                    FROM Orders, Tickets tick, MenuItems m
                //                                    WHERE 
                //                                        tick.Id = Orders.TicketId
                //                                    AND m.Id = orders.MenuItemId
                //                                    AND groupcode = t.groupcode
                //                                    AND Orders.DecreaseInventory = 1	
                //                                    AND tick.[Date] between '{0}' and '{1}'
                //                              {3}
                //                            ),
                //                            SortOrder =
                //                            case when UltimateAccount is null then 0
                //                            else SortOrder
                //                            end,
                //                            'OrderTotal' =
                //                            (
                //                                 SELECT sum(OrderTotal) FROM 
                //                                 (
                //                               SELECT  groupcode, menuitemid, MenuItemName, portionname, tick.Price,
                //                               max(OrderTotal)OrderTotal
                //                               FROM 
                //                               TicketItemview tick														
                //                               where tick.[ticketDate] between '{0}' and '{1}'
                //                                        {3}
                //                               group by  groupcode, menuitemid, MenuItemName, portionname, tick.Price

                //                              )l
                //                              WHERE l.groupcode = t.groupcode		

                //                              GROUP BY  groupcode																						
                //                            ),
                //                            isnull(UltimateAccount, '_') UltimateAccount,
                //                            SUM(DistinctAmount) DistinctAmount
                //                            from TicketItemView t
                //                            where T.TicketDate between '{0}' and '{1}'
                //                            {2}
                //                            --AND MenuItemName = 'Affogato'
                //                            group by  GroupCode, UltimateAccount, SortOrder
                //                            order by groupcode,SortOrder", StartDate, EndDate, sqlClause, sqlClause2);

                string sql2 = string.Format(@"select 
												  'SUMTotalCollection' =
												(
														SELECT isnull(sum(Collection),0) FROM 
														(
															SELECT o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
															CASE WHEN t.PlainSum = 0 THEN 0
															ELSE  o.Total/t.PlainSum*t.TotalAmount
															END Collection
															FROM 
															Orders o, tickets t
															WHERE O.ticketid = t.ID
															AND t.[Date]  between '{0}' and '{1}'
															AND o.DecreaseInventory = 1
															{2}															
														)l				
														--GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
												),
												'SUMPrice' = 
													(
															SELECT isnull(sum(Price),0) 
															FROM Orders, Tickets t 
															WHERE  t.Id = Orders.TicketId
															AND Orders.DecreaseInventory = 1	
															AND t.[Date] between '{0}' and '{1}'
															{2}
													),
												'SUMQuantity' = 
												(
														SELECT isnull(sum(quantity),0) 
														FROM Orders, Tickets t 
														WHERE  t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date]  between '{0}' and '{1}'
														{2}
												),
												'SUMGross' = 
												(
														SELECT isnull(sum(PlainTotal+OrderTagPrice*Quantity),0) 
														FROM Orders, Tickets t 
														Where t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												),
												'SUMGift' = 
												(
														SELECT isnull(sum((PlainTotal+OrderTagPrice*Quantity)-Total),0) 
														FROM Orders, Tickets t 
														WHERE t.Id = Orders.TicketId                
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												)", StartDate, EndDate, sqlClause);

                string sql = sql2;
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                //AND tick.[Date] between '{0}' and '{1}'
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);

                ds.Tables.Add(masterTable);
                ds.Tables[0].TableName = "SUMs";

                ds.Tables[1].TableName = "ItemSalesAccountWise";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetItemSalesAccountWise(string StartDate, string EndDate, string DepartmentIds, int SyncOutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<Ticket> oTickets = GetTicketsFaster(SyncOutletId, DepartmentIds, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
                IEnumerable<MenuItem> oMenuItems = GetMenuItems();
                IEnumerable<TaxTemplate> oTaxTemplates = GetObjectTaxTemplates();
                IEnumerable<CalculationType> oCalculationTypes = GetObjectCalculationTypes();
                var incomeCalculator = GetIncomeCalculator(oTickets);
                List<string> sPaymentNames = GetPaymentNames();//select distict name from payments
                                                               //                
                var menGroupSalesInfo = MenuGroupBuilder.CalculateOrderSalesInfo(oTickets, oMenuItems).ToList();
                var menGroupsOrderCalculationsInfo = MenuGroupBuilder.CalculateMenuItemOrderGroupCalculationInfo(oTickets, oMenuItems, oCalculationTypes).ToList();
                var menGroupTaxesInfo = MenuGroupBuilder.CalculateMenuItemOrderWiseAccountsInfo(oTickets, oMenuItems, oTaxTemplates).ToList();
                //
                //                
                var menGroupsAccountsInfo = MenuGroupBuilder.CalculateMenuItemAccountsInfo(oTickets, oMenuItems, oTaxTemplates).ToList();
                var menGroupsCalculationsInfo = MenuGroupBuilder.CalculateMenuItemCalculationInfo(oTickets, oMenuItems, oCalculationTypes).ToList();
                //var menGroupsPaymentsInfo = MenuGroupBuilder.CalculatePaymentInfo(oTickets, oMenuItems, sPaymentNames).ToList();//sPaymentNames
                var menGroupsPaymentsInfo = MenuGroupBuilder.CalculateItemPaymentInfo(oTickets, menGroupSalesInfo, menGroupsOrderCalculationsInfo, menGroupTaxesInfo, sPaymentNames, oTaxTemplates, oCalculationTypes).ToList();
                var menGroupsSalesInfo = MenuGroupBuilder.CalculateItemOrderSalesInfo(oTickets, oMenuItems).ToList();
                var menGroupsGiftInfo = MenuGroupBuilder.CalculateItemGiftInfo(oTickets, oMenuItems).ToList();
                //var menIncludingVATInfo = MenuGroupBuilder.CalculateIncludingTaxInfo(oTickets, oMenuItems).ToList();
                //var menAllPaymentInfoInfo = MenuGroupBuilder.CalculatePaymentTotalInfo(oTickets, oMenuItems, sPaymentNames).ToList();//sPaymentNames
                var TempmenAllPaymentInfoInfo =
                from info in menGroupsPaymentsInfo
                group info by new { MenuItemId = info.MenuItemId, MenuItemName = info.MenuItemName, info.PortionName, Price = info.Price, info.Quantity, GroupCode = info.GroupName } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Total",
                    SortOrder = 3000,
                    Quantity = grp.Key.Quantity,
                    Amount = grp.Sum(y => y.Amount),
                    OrderTotal = 0,
                    GrossAmount = 0
                };
                var menAllPaymentInfoInfo = TempmenAllPaymentInfoInfo.ToList();
                //
                DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupsAccountsInfo);
                DataSet dsMenGroupsCalculationsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupsCalculationsInfo);
                DataSet dsMenGroupsPaymentsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupsPaymentsInfo);
                DataSet dsMenGroupsSalesInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupsSalesInfo);
                DataSet dsMenGroupsGiftInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupsGiftInfo);
                //DataSet dsmenIncludingVATInfo = DatasetConverter.ToDataSet<MenuItemGroupAccountsInfo>(menIncludingVATInfo);
                DataSet dsmenAllPaymentInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menAllPaymentInfoInfo);
                //
                DataSet dsMenGroupItemSalesInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupSalesInfo);
                DataSet dsMenGroupItemCalculationInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupsOrderCalculationsInfo);
                DataSet dsMenGroupItemTaxInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menGroupTaxesInfo);
                DataSet dsVoid = TicketManager.VoidOrdersGroupWise(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), "Void", SyncOutletId, DepartmentIds);
                //

                DataTable masterTable = new DataTable("ItemSalesAccountWise");

                masterTable.Columns.AddRange(new DataColumn[] {new DataColumn("GroupCode"), new DataColumn("MenuItemName"),new DataColumn("PortionName"), new DataColumn("Price", typeof(decimal)), new DataColumn("Gross", typeof(decimal)), new DataColumn("Quantity", typeof(decimal)),
                new DataColumn("Gift", typeof(decimal) ), new DataColumn("SortOrder", typeof(int)), new DataColumn("UltimateAccount"), new DataColumn("DistinctAmount", typeof(decimal)),
                new DataColumn("TotalCollection", typeof(decimal))});

                //foreach (var groupAccountsInfo in menGroupsSalesInfo)
                //{
                //    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.MenuItemName, groupAccountsInfo.PortionName, groupAccountsInfo.Price.ToString("0.00"), groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, -1, "Quantity", groupAccountsInfo.Amount });
                //}
                foreach (var groupAccountsInfo in menGroupsAccountsInfo)
                {
                    if (groupAccountsInfo.Amount == 0)
                    {
                        continue;
                    }

                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.MenuItemName, groupAccountsInfo.PortionName, groupAccountsInfo.Price.ToString("0.00"), groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                foreach (var groupAccountsInfo in menGroupsCalculationsInfo)
                {
                    if (groupAccountsInfo.Amount == 0)
                    {
                        continue;
                    }

                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.MenuItemName, groupAccountsInfo.PortionName, groupAccountsInfo.Price.ToString("0.00"), groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                foreach (var groupAccountsInfo in menGroupsPaymentsInfo)
                {
                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.MenuItemName, groupAccountsInfo.PortionName, groupAccountsInfo.Price.ToString("0.00"), groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, 1000, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                foreach (var groupAccountsInfo in menGroupsSalesInfo)
                {
                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.MenuItemName, groupAccountsInfo.PortionName, groupAccountsInfo.Price.ToString("0.00"), groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, 1, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }
                //foreach (var groupAccountsInfo in menGroupsGiftInfo)
                //{
                //    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.MenuItemName, groupAccountsInfo.PortionName, groupAccountsInfo.Price.ToString("0.00"), groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                //}
                //foreach (var groupAccountsInfo in menIncludingVATInfo)
                //{
                //    masterTable.Rows.Add(new object[] { groupAccountsInfo.MenuItemName, groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, 75, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                //}
                foreach (var groupAccountsInfo in menAllPaymentInfoInfo)
                {
                    if (groupAccountsInfo.Amount == 0)
                    {
                        continue;
                    }

                    masterTable.Rows.Add(new object[] { groupAccountsInfo.GroupName, groupAccountsInfo.MenuItemName, groupAccountsInfo.PortionName, groupAccountsInfo.Price.ToString("0.00"), groupAccountsInfo.GrossAmount.ToString("0.00"), groupAccountsInfo.Quantity.ToString("0.00"), 0, groupAccountsInfo.SortOrder, groupAccountsInfo.AccountHead, groupAccountsInfo.Amount });
                }

                //foreach (DataRow dr in dsVoid.Tables[0].Rows)
                //{
                //    masterTable.Rows.Add(new object[] { dr["GroupName"].ToString(), Convert.ToDouble(dr["Amount"]).ToString("0.00"), Convert.ToDouble(dr["Amount"]).ToString("0.00"), 0, 5000, "Void Amount", Convert.ToDouble(dr["Amount"]).ToString("0.00") });
                //}
                DataView dv = masterTable.DefaultView;
                dv.Sort = "MenuItemName asc, SortOrder asc";
                masterTable = dv.ToTable();
                string dbConnString = DBUtility.GetConnectionString();

                string sqlClause = string.Empty;
                string sqlClause2 = string.Empty;
                if (SyncOutletId > 0)
                {
                    sqlClause = string.Format(@" and SyncOutletId = {0}", SyncOutletId);
                    sqlClause2 = string.Format(@" and tick.SyncOutletId = {0}", SyncOutletId);
                }
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = sqlClause + string.Format(@" and t.DepartmentId in ({0})", DepartmentIds);
                    sqlClause2 = sqlClause2 + string.Format(@" and tick.DepartmentId in ({0})", DepartmentIds);
                }

                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sql2 = string.Format(@"select 
												  'SUMTotalCollection' =
												(
														SELECT isnull(sum(Collection),0) FROM 
														(
															SELECT o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
															CASE WHEN t.PlainSum = 0 THEN 0
															ELSE  o.Total/t.PlainSum*t.TotalAmount
															END Collection
															FROM 
															Orders o, tickets t
															WHERE O.ticketid = t.ID
															AND t.[Date]  between '{0}' and '{1}'
															AND o.DecreaseInventory = 1
															{2}															
														)l				
														--GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
												),
												'SUMPrice' = 
													(
															SELECT isnull(sum(Price),0) 
															FROM Orders, Tickets t 
															WHERE  t.Id = Orders.TicketId
															AND Orders.DecreaseInventory = 1	
															AND t.[Date] between '{0}' and '{1}'
															{2}
													),
												'SUMQuantity' = 
												(
														SELECT isnull(sum(quantity),0) 
														FROM Orders, Tickets t 
														WHERE  t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date]  between '{0}' and '{1}'
														{2}
												),
												'SUMGross' = 
												(
														SELECT isnull(sum(PlainTotal+OrderTagPrice*Quantity),0) 
														FROM Orders, Tickets t 
														Where t.Id = Orders.TicketId
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												),
												'SUMGift' = 
												(
														SELECT isnull(sum((PlainTotal+OrderTagPrice*Quantity)-Total),0) 
														FROM Orders, Tickets t 
														WHERE t.Id = Orders.TicketId                
														AND Orders.DecreaseInventory = 1	
														AND t.[Date] between '{0}' and '{1}'
														{2}
												)", StartDate, EndDate, sqlClause);

                string sql = sql2;
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                //AND tick.[Date] between '{0}' and '{1}'
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);

                ds.Tables.Add(masterTable);
                ds.Tables[0].TableName = "SUMs";

                ds.Tables[1].TableName = "ItemSalesAccountWise";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetItemSalesEntityAccountWise(string StartDate, string EndDate, int EntityTypeId, int EntityId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql1 = string.Format(@"select TicketDate,T.ticketnumber,DepartmentName,MenuItemID,
											'GroupCode' =
											(

												SELECT max(GroupCode) from MenuItems
												where id = t.MenuItemID
											), 
											MenuItemName, PortionName,Price,
											  'TotalCollection' =
											(
													SELECT sum(Collection) FROM 
													(
														SELECT tick.ID ticketid, o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
														CASE WHEN tick.PlainSum = 0 THEN 0
														ELSE  o.Total/tick.PlainSum*tick.TotalAmount
														END Collection
														FROM 
														Orders o, tickets tick
														WHERE O.ticketid = tick.ID
														AND tick.[Date] between '{0}' and '{1}'
														AND o.DecreaseInventory = 1															
													)l
													WHERE l.menuitemid = t.menuitemid
													AND l.ticketid = t.ticketid
													AND l.MenuItemName = t.menuitemname
													AND l.portionname = t.portionname
													AND l.departmentid = t.DepartmentID
													AND l.price = t.Price
													--AND t.UltimateAccount = 'Sales'
													GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
											),
		
											'Quantity' = 
											(
													SELECT sum(quantity) 
													FROM Orders, Tickets tick 
													WHERE orders.DepartmentID = t.DepartmentID 
													AND tick.Id = Orders.TicketId
													AND tick.Id = t.ticketid
													AND menuitemid = t.menuitemid
													AND MenuItemName = t.menuitemname
													AND portionname = t.portionname
													AND Price = t.Price
													AND Orders.DecreaseInventory = 1	
													AND tick.[Date] between '{0}' and '{1}'
											),
											'Gross' = 
											(
													SELECT sum(PlainTotal) 
													FROM Orders, Tickets tick 
													WHERE orders.DepartmentID = t.DepartmentID 
													AND tick.Id = Orders.TicketId
													AND tick.Id = t.ticketid
													AND menuitemid = t.menuitemid
													AND MenuItemName = t.menuitemname
													AND portionname = t.portionname
													AND Price = t.Price
													AND Orders.DecreaseInventory = 1	
													AND tick.[Date] between '{0}' and '{1}'
											),
											'Gift' = 
											(
													SELECT sum(PlainTotal-Total) 
													FROM Orders, Tickets tick 
													WHERE orders.DepartmentID = t.DepartmentID 
													AND tick.Id = Orders.TicketId
													AND tick.Id = t.ticketid
													AND menuitemid = t.menuitemid
													AND MenuItemName = t.menuitemname
													AND portionname = t.portionname
													AND Price = t.Price
													AND Orders.DecreaseInventory = 1	
													AND tick.[Date] between '{0}' and '{1}'
											),
											SortOrder =
											case when UltimateAccount is null then 0
											else SortOrder
											end,
											'OrderTotal' =
											(
													SELECT sum(OrderTotal) FROM 
													(
														SELECT o.ticketid, o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
														max(OrderTotal)OrderTotal
														FROM 
														TicketItemview o														
														where o.[ticketDate] between '{0}' and '{1}'
														group by o.ticketid, o.departmentid, menuitemid, MenuItemName, portionname, o.Price
													)l
													WHERE l.menuitemid = t.menuitemid
													AND l.ticketId = t.ticketid
													AND l.MenuItemName = t.menuitemname
													AND l.portionname = t.portionname
													AND l.departmentid = t.DepartmentID
													AND l.price = t.Price
													GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
											),
											isnull(UltimateAccount, '_') UltimateAccount,
											SUM(DistinctAmount) DistinctAmount
											from TicketItemView t, TicketEntities te, Entities e
											where T.TicketDate  between '{0}' and '{1}'
											and T.ticketid= te.Ticket_Id
											and te.EntityId = e.Id
											and te.EntityTypeId = {2} and te.EntityId = {3}
											and T.payment is  null
											--AND MenuItemName = 'Affogato'
											group by TicketDate,T.ticketnumber,t.ticketid, DepartmentID, DepartmentName,Name, MenuItemID, menuitemname, portionname, UltimateAccount, SortOrder,price
											order by t.ticketnumber, menuitemname,DepartmentName,Portionname,SortOrder", StartDate, EndDate, EntityTypeId, EntityId);

                string sql2 = string.Format(@"select 
												'SUMTotalCollection' =
											(
													SELECT sum(Collection) FROM 
													(
														SELECT o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
														CASE WHEN tick.PlainSum = 0 THEN 0
														ELSE  o.Total/tick.PlainSum*tick.TotalAmount
														END Collection
														FROM 
														Orders o, tickets tick, TicketEntities te
														WHERE O.ticketid = tick.ID
														and tick.Id = te.Ticket_Id
														and te.EntityTypeId = {0} and te.EntityId = {1}
														AND tick.[Date]  between '{2}' and '{3}'
														AND o.DecreaseInventory = 1															
													)l				
													--GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
											),
											'SUMPrice' = 
												(
														SELECT sum(Price) 
														FROM Orders, Tickets tick, TicketEntities te 
														WHERE  tick.Id = Orders.TicketId
														and tick.Id = te.Ticket_Id
														and te.EntityTypeId = {0} and te.EntityId = {1}
														AND tick.[Date]  between '{2}' and '{3}'
														AND Orders.DecreaseInventory = 1	
												),
											'SUMQuantity' = 
											(
													SELECT sum(quantity) 
													FROM Orders, Tickets tick, TicketEntities te
													WHERE  tick.Id = Orders.TicketId
													and tick.Id = te.Ticket_Id
													and te.EntityTypeId = {0} and te.EntityId = {1}
													AND tick.[Date]  between '{2}' and '{3}'
													AND Orders.DecreaseInventory = 1	
			
											),
											'SUMGross' = 
											(
													SELECT sum(PlainTotal) 
													FROM Orders, Tickets tick, TicketEntities te 
													Where tick.Id = Orders.TicketId
													and tick.Id = te.Ticket_Id
													and te.EntityTypeId = {0} and te.EntityId = {1}
													AND tick.[Date]  between '{2}' and '{3}'
													AND Orders.DecreaseInventory = 1	
											),
											'SUMGift' = 
											(
													SELECT sum(PlainTotal-Total) 
													FROM Orders, Tickets tick, TicketEntities te 
													WHERE tick.Id = Orders.TicketId 
													and tick.Id = te.Ticket_Id
													and te.EntityTypeId = {0} and te.EntityId = {1}
													AND tick.[Date]  between '{2}' and '{3}'
													AND Orders.DecreaseInventory = 1	
											)", EntityTypeId, EntityId, StartDate, EndDate);

                string sql = sql1 + ";" + sql2;
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                //AND tick.[Date] between '{0}' and '{1}'
                da.Fill(ds);

                ds.Tables[0].TableName = "ItemSalesEntityAccountWise";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetItemSalesEntityAccountWiseForScreenMenu(string StartDate, string EndDate, int EntityTypeId, int ScreenMenuCategoryId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql1 = string.Format(@"select e.Name EntityName, TicketDate,T.ticketnumber,DepartmentName,t.MenuItemID,
											'GroupCode' =
											(
												SELECT max(GroupCode) from MenuItems
												where id = t.MenuItemID
											), 
											MenuItemName, PortionName,Price,
											  'TotalCollection' =
											(
													SELECT sum(Collection) FROM 
													(
														SELECT tick.ID ticketid, o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
														CASE WHEN tick.PlainSum = 0 THEN 0
														ELSE  o.Total/tick.PlainSum*tick.TotalAmount
														END Collection
														FROM 
														Orders o, tickets tick
														WHERE O.ticketid = tick.ID
														AND tick.[Date] between '{0}' and '{1}'
														AND o.DecreaseInventory = 1															
													)l
													WHERE l.menuitemid = t.menuitemid
													AND l.ticketid = t.ticketid
													AND l.MenuItemName = t.menuitemname
													AND l.portionname = t.portionname
													AND l.departmentid = t.DepartmentID
													AND l.price = t.Price
													--AND t.UltimateAccount = 'Sales'
													GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
											),
		
											'Quantity' = 
											(
													SELECT sum(quantity) 
													FROM Orders, Tickets tick 
													WHERE orders.DepartmentID = t.DepartmentID 
													AND tick.Id = Orders.TicketId
													AND tick.Id = t.ticketid
													AND menuitemid = t.menuitemid
													AND MenuItemName = t.menuitemname
													AND portionname = t.portionname
													AND Price = t.Price
													AND Orders.DecreaseInventory = 1	
													AND tick.[Date] between '{0}' and '{1}'
											),
											'Gross' = 
											(
													SELECT sum(PlainTotal) 
													FROM Orders, Tickets tick 
													WHERE orders.DepartmentID = t.DepartmentID 
													AND tick.Id = Orders.TicketId
													AND tick.Id = t.ticketid
													AND menuitemid = t.menuitemid
													AND MenuItemName = t.menuitemname
													AND portionname = t.portionname
													AND Price = t.Price
													AND Orders.DecreaseInventory = 1	
													AND tick.[Date] between '{0}' and '{1}'
											),
											'Gift' = 
											(
													SELECT sum(PlainTotal-Total) 
													FROM Orders, Tickets tick 
													WHERE orders.DepartmentID = t.DepartmentID 
													AND tick.Id = Orders.TicketId
													AND tick.Id = t.ticketid
													AND menuitemid = t.menuitemid
													AND MenuItemName = t.menuitemname
													AND portionname = t.portionname
													AND Price = t.Price
													AND Orders.DecreaseInventory = 1	
													AND tick.[Date] between '{0}' and '{1}'
											),
											SortOrder =
											case when UltimateAccount is null then 0
											else t.SortOrder
											end,
											'OrderTotal' =
											(
													SELECT sum(OrderTotal) FROM 
													(
														SELECT o.ticketid, o.departmentid, menuitemid, MenuItemName, portionname, o.Price,
														max(OrderTotal)OrderTotal
														FROM 
														TicketItemview o														
														where o.[ticketDate] between '{0}' and '{1}'
														group by o.ticketid, o.departmentid, menuitemid, MenuItemName, portionname, o.Price
													)l
													WHERE l.menuitemid = t.menuitemid
													AND l.ticketId = t.ticketid
													AND l.MenuItemName = t.menuitemname
													AND l.portionname = t.portionname
													AND l.departmentid = t.DepartmentID
													AND l.price = t.Price
													GROUP BY departmentid, menuitemid, MenuItemName, PortionName, Price																								
											),
											isnull(UltimateAccount, '_') UltimateAccount,
											SUM(DistinctAmount) DistinctAmount
											INTO #T
											from TicketItemView t, TicketEntities te, Entities e,ScreenMenuItems smi
											where T.TicketDate  between '{0}' and '{1}'
											and smi.ScreenMenuCategoryId = {3}
											and smi.MenuItemId = t.MenuItemID
											and T.ticketid= te.Ticket_Id
											and te.EntityId = e.Id
											and te.EntityTypeId = {2} 
											and T.payment is  null											
											group by TicketDate,T.ticketnumber,t.ticketid, DepartmentID, DepartmentName,e.Name, t.MenuItemID, menuitemname, portionname, UltimateAccount, t.SortOrder,price
											order by t.ticketnumber, menuitemname,DepartmentName,Portionname,t.SortOrder
											
											SELECT EntityName,
											SUM(TotalCollection) TotalCollection,
											SUM(Quantity) Quantity,
											SUM(Gross) Gross,
											SUM(Gift) Gift,
											SUM(OrderTotal) OrderTotal,
											UltimateAccount,
											SUM(DistinctAmount) DistinctAmount 
											INTO #F 
											from #T  
											GROUP BY EntityName,UltimateAccount Order By EntityName

											--SELECT * from #F ORder by EntityName

											SELECT EntityName,
											TotalCollection = (select MAX(TotalCollection) FROM #F where #F.EntityName = NewTable.EntityName),
											Quantity=(select MAX(Quantity)  FROM #F where #F.EntityName = NewTable.EntityName ),
											Gross=(select MAX(Gross)  FROM #F where #F.EntityName = NewTable.EntityName ),
											Gift=(select MAX(Gift) FROM #F where #F.EntityName = NewTable.EntityName),
											OrderTotal=(select MAX(OrderTotal)  FROM #F where #F.EntityName = NewTable.EntityName ),
											UltimateAccount, 
											DistinctAmount FROM #F NewTable Order By EntityName
											DROP TABLE #T
											DROP TABLE #F", StartDate, EndDate, EntityTypeId, ScreenMenuCategoryId);

                string sql = sql1;
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                ds.Tables[0].TableName = "Entity_SMC_Wise";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetTicketsAccountWise(string StartDate, string EndDate, double VATIncludedDenominator, int OutletId, string DepartmentIds)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = $@" and t.DepartmentId in ({DepartmentIds})";
                }

                if (OutletId > 0)
                {
                    sqlClause = sqlClause + $@" and t.SyncOutletID  = {OutletId}";
                }

                dbConn.Open();
                string sql1 = $@"Select DepartmentName, TicketNumber, TicketDate, SortOrder,UltimateAccount,Amount, TotalAmount,
												 CASE
													WHEN {VATIncludedDenominator} = 0 THEN 0
													ELSE 
														TotalAmount/{VATIncludedDenominator}
												END as	IncludedVAT
												from TicketView t
											 where TicketDate between '{StartDate}' and '{EndDate}'
											 {sqlClause}";


                string sql2 = $@"Select SUM(TotalAmount) SUMTotalAmount, SUM(PlainSum)+SUM(PostTaxDiscountServicesTotal) SUMNetSales,
												 CASE
													WHEN {VATIncludedDenominator} = 0 THEN 0
													ELSE 
														SUM(TotalAmount)/{VATIncludedDenominator}
												END as	SUMIncludedVAT
												from Tickets t
											 where Date  between '{StartDate}' and '{EndDate}'
											 {sqlClause}";

                string sql = sql1 + ";" + sql2;

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                //AND tick.[Date] between '{0}' and '{1}'
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                ds.Tables[0].TableName = "TicketsAccountWise";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetTicketsAccountWiseSalesTaxReport(string StartDate, string EndDate, double VATIncludedDenominator, int OutletId, string DepartmentIds)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = string.Format(@" and t.DepartmentId in ({0})", DepartmentIds);
                }

                if (OutletId > 0)
                {
                    sqlClause = sqlClause + string.Format(@" and t.SyncOutletID  = {0}", OutletId);
                }

                dbConn.Open();
                string sql1 = string.Format(@"Select TicketID , DepartmentName, TicketNumber, CAST(TicketDate as Date) TicketDate , SortOrder,UltimateAccount,Amount, TotalAmount,
												 CASE
													WHEN {2} = 0 THEN 0
													ELSE 
														TotalAmount/{2}
												END as	IncludedVAT
												INTO ##Table
												from TicketView t
											 where TicketDate between '{0}' and '{1}'
											 {3}
											 select DepartmentName='ALL' ,
											 SortOrder=2 , 
											 (CAST((SELECT TOP 1 TicketNumber from ##Table where TicketDate = T.TicketDate  Order By TicketID) as nvarchar(MAX)) +'-' +CAST((SELECT TOP 1 TicketNumber from ##Table where TicketDate = T.TicketDate Order By TicketID desc ) as nvarchar(MAX))) TicketNumber ,
											 TicketDate ,
											 UltimateAccount , 
											 SUM(amount) Amount , 
											 TotalAmount = (SELECT SUM(TotalAmount) FROM ##Table WHere TicketDate = T.TicketDate)  , 
											 SUM(IncludedVAT) IncludedVat 
											 from ##Table T 
											 where amount is not null
											 GROUP BY TicketDate,UltimateAccount 
											 ORDER BY LEN(UltimateAccount) desc ,TicketDate ASC 
											 Drop Table ##Table", StartDate, EndDate, VATIncludedDenominator, sqlClause);


                string sql2 = string.Format(@"Select SUM(TotalAmount) SUMTotalAmount, SUM(PlainSum)+SUM(PostTaxDiscountServicesTotal) SUMNetSales,
												 CASE
													WHEN {2} = 0 THEN 0
													ELSE 
														SUM(TotalAmount)/{2}
												END as	SUMIncludedVAT
												from Tickets t
											 where Date  between '{0}' and '{1}'
											 {3}", StartDate, EndDate, VATIncludedDenominator, sqlClause);

                string sql = sql1 + ";" + sql2;

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                //AND tick.[Date] between '{0}' and '{1}'
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                ds.Tables[0].TableName = "TicketsAccountWise";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetCashierAccountWise(string StartDate, string EndDate, double VATIncludedDenominator, int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sqlClause = string.Empty;
                if (OutletId > 0)
                {
                    sqlClause = string.Format(@" and t.SyncOutletID  = {0}", OutletId);
                }

                dbConn.Open();
                string sql1 = string.Format(@"Select * from 
												(
													Select ticketid, DepartmentName, TicketNumber, TicketDate, SortOrder,UltimateAccount,Amount, TotalAmount,  SettledBy
																									from TicketView t
																								 where TicketDate between '{0}' and '{1}'
																								{2}

													Union All

													select t.Id ticketid, d.Name DepartmentName, T.TicketNumber, T.Date TicketDate,  10000 SortOrder, 'Total Collection' UltimateAccount, TotalAmount Amount, TotalAmount, t.SettledBy
													from tickets t, Departments d
													where t.DepartmentId = d.Id
													and t.Date between '{0}' and '{1}'
													{2}
													Union All

													select t.Id ticketid, d.Name DepartmentName, T.TicketNumber, T.Date TicketDate, 41 SortOrder,'Net Amount' UltimateAccount,(t.PlainSum+sum(o.OrderCalculationDiscountAmount*o.Quantity)) NetAmount ,t.TotalAmount, t.SettledBy
													from tickets t, Departments d, Orders o
													where t.DepartmentId = d.Id
													and t.id = o.TicketId
													and t.Date between '{0}' and '{1}'
													{2}
													group by  t.Id , d.Name , T.TicketNumber, T.Date ,t.TotalAmount, t.SettledBy,  t.PlainSum
													Union All

													select t.Id ticketid, d.Name DepartmentName, T.TicketNumber, T.Date TicketDate, 51 SortOrder,'Total Amount' UltimateAccount,(t.PlainSum+sum(o.OrderCalculationDiscountAmount*o.Quantity)+sum(o.OrderCalculationTaxAmount*o.Quantity)) NetAmount ,t.TotalAmount, t.SettledBy
													from tickets t, Departments d, Orders o
													where t.DepartmentId = d.Id
													and t.id = o.TicketId
													and t.Date between '{0}' and '{1}'
													{2}
													group by  t.Id , d.Name , T.TicketNumber, T.Date ,t.TotalAmount, t.SettledBy,  t.PlainSum
												)Q
												Order by Q.ticketid, Q.SortOrder  
											", StartDate, EndDate, sqlClause);


                string sql2 = string.Format(@"Select SUM(TotalAmount) SUMTotalAmount, 
												 CASE
													WHEN {2} = 0 THEN 0
													ELSE 
														SUM(TotalAmount)/{2}
												END as	SUMIncludedVAT
												from Tickets t
											 where Date  between '{0}' and '{1}'
											 {3}", StartDate, EndDate, VATIncludedDenominator, sqlClause);

                string sql = sql1 + ";" + sql2;

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                //AND tick.[Date] between '{0}' and '{1}'
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                ds.Tables[0].TableName = "TicketsAccountWise";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static DataSet GetCashierAccountWiseSummary(string StartDate, string EndDate, double VATIncludedDenominator, int OutletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sqlClause = string.Empty;
                if (OutletId > 0)
                {
                    sqlClause = string.Format(@" and t.SyncOutletID  = {0}", OutletId);
                }

                dbConn.Open();
                string sql1 = string.Format(@"Select * from 
												(
													Select ticketid, DepartmentName, TicketNumber, TicketDate, SortOrder,UltimateAccount,Amount, TotalAmount,  SettledBy
																									from TicketView t
																								 where TicketDate between '{0}' and '{1}'
																								{2}

													Union All

													select t.Id ticketid, d.Name DepartmentName, T.TicketNumber, T.Date TicketDate,  10000 SortOrder, 'Total Collection' UltimateAccount, TotalAmount Amount, TotalAmount, t.SettledBy
													from tickets t, Departments d
													where t.DepartmentId = d.Id
													and t.Date between '{0}' and '{1}'
													{2}

													Union All

													select t.Id ticketid, d.Name DepartmentName, T.TicketNumber, T.Date TicketDate, 41 SortOrder,'Net Amount' UltimateAccount,(t.PlainSum+sum(o.OrderCalculationDiscountAmount*o.Quantity)) NetAmount ,t.TotalAmount, t.SettledBy
													from tickets t, Departments d, Orders o
													where t.DepartmentId = d.Id
													and t.id = o.TicketId
													and t.Date between '{0}' and '{1}'
													{2}
													group by  t.Id , d.Name , T.TicketNumber, T.Date ,t.TotalAmount, t.SettledBy,  t.PlainSum
													Union All

													select t.Id ticketid, d.Name DepartmentName, T.TicketNumber, T.Date TicketDate, 51 SortOrder,'Total Amount' UltimateAccount,(t.PlainSum+sum(o.OrderCalculationDiscountAmount*o.Quantity)+sum(o.OrderCalculationTaxAmount*o.Quantity)) NetAmount ,t.TotalAmount, t.SettledBy
													from tickets t, Departments d, Orders o
													where t.DepartmentId = d.Id
													and t.id = o.TicketId
													and t.Date between '{0}' and '{1}'
													{2}
													group by  t.Id , d.Name , T.TicketNumber, T.Date ,t.TotalAmount, t.SettledBy,  t.PlainSum
													 
													Union All
													select t.Id ticketid, d.Name DepartmentName, T.TicketNumber, T.Date TicketDate, -1000 SortOrder,'NoOfTickets' UltimateAccount,1 NetAmount ,t.TotalAmount, t.SettledBy
													from tickets t, Departments d, Orders o
													where t.DepartmentId = d.Id
													and t.id = o.TicketId
													and t.Date between '{0}' and '{1}'
													{2}
													group by  t.Id , d.Name , T.TicketNumber, T.Date ,t.TotalAmount, t.SettledBy,  t.PlainSum
													
												)Q
												Order by Q.ticketid, Q.SortOrder  
											", StartDate, EndDate, sqlClause);


                string sql2 = string.Format(@"Select SUM(TotalAmount) SUMTotalAmount, 
												 CASE
													WHEN {2} = 0 THEN 0
													ELSE 
														SUM(TotalAmount)/{2}
												END as	SUMIncludedVAT
												from Tickets t
											 where Date  between '{0}' and '{1}'
											 {3}", StartDate, EndDate, VATIncludedDenominator, sqlClause);

                string sql = sql1 + ";" + sql2;

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                //AND tick.[Date] between '{0}' and '{1}'
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                ds.Tables[0].TableName = "TicketsAccountWiseSummary";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetTicketsAccountDayWise(string StartDate, string EndDate, double VATIncludedDenominator, int OutletId, string DepartmentIds)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sqlClause = string.Empty;
                if (!string.IsNullOrEmpty(DepartmentIds))
                {
                    sqlClause = string.Format(@" and t.DepartmentId in ({0})", DepartmentIds);
                }

                if (OutletId > 0)
                {
                    sqlClause = sqlClause + string.Format(@" and t.SyncOutletID  = {0}", OutletId);
                }

                dbConn.Open();
                string sql1 = string.Format(@"  select 
												TotalAmount =(select sum(totalamount) from tickets t where [Date] between q.startdate and q.enddate {3}),
												NetSales =(select SUM(PlainSum)+SUM(PostTaxDiscountServicesTotal) from tickets t where [Date] between q.startdate and q.enddate {3}),
												IncludedVAT =(select sum(totalamount) from tickets t where [Date] between q.startdate and q.enddate {3})/{2},
												TicketCount = (select Count(Id) from tickets t where [Date] between q.startdate and q.enddate and TotalAmount > 0 {3}),
												WorkPeriodID, WorkPeriodDescription, UltimateAccount, sum(Amount)Amount, SortOrder
												from 
												(
													SELECT 'WorkPeriodID' = 
													(
														SELECT ID 
														FROM 
														(
															SELECT Id, StartDate, 
															CASE
																WHEN startdate = enddate THEN getdate()
																ELSE enddate
															END as	EndDate, StartDescription, EndDescription, Name
															FROM WorkPeriods
														)w
														WHERE t.ticketdate BETWEEN startdate AND enddate
													),
													'WorkPeriodDescription' = 
													(
														SELECT convert(varchar(20), startdate, 113)+ '--'+ convert(varchar(20), enddate, 113)
														FROM 
														(
															SELECT Id, StartDate, 
															CASE
																WHEN startdate = enddate THEN getdate()
																ELSE enddate
															END as	EndDate, StartDescription, EndDescription, Name
															FROM WorkPeriods
														)w
														WHERE t.ticketdate BETWEEN startdate AND enddate
													),
													'StartDate' = 
													(
														SELECT StartDate
														FROM 
														(
															SELECT Id, StartDate, 
															CASE
																WHEN startdate = enddate THEN getdate()
																ELSE enddate
															END as	EndDate, StartDescription, EndDescription, Name
															FROM WorkPeriods
														)w
														WHERE t.ticketdate BETWEEN startdate AND enddate
													),
													'EndDate' = 
													(
														SELECT EndDate
														FROM 
														(
															SELECT Id, StartDate, 
															CASE
																WHEN startdate = enddate THEN getdate()
																ELSE enddate
															END as	EndDate, StartDescription, EndDescription, Name
															FROM WorkPeriods
														)w
														WHERE t.ticketdate BETWEEN startdate AND enddate
													)
													, t.*

													FROM TicketView t
														WHERE t.ticketdate BETWEEN	'{0}' and '{1}'
														{3}
												  
												)Q

												group by WorkPeriodID, WorkPeriodDescription, startdate, enddate,SortOrder, UltimateAccount
												having UltimateAccount is not null --and WorkPeriodID is not null
												order by WorkPeriodID,SortOrder", StartDate, EndDate, VATIncludedDenominator, sqlClause);


                string sql2 = string.Format(@" Select SUM(TotalAmount) SUMTotalAmount, SUM(TotalAmount)/0.001 SUMIncludedVAT,Count(Id) SUMTicketCount, SUM(PlainSum)+SUM(PostTaxDiscountServicesTotal) SUMNetSales
											from Tickets t
											 where Date  between '{0}' and '{1}' and TotalAmount > 0
											 {3}", StartDate, EndDate, VATIncludedDenominator, sqlClause);

                string sql = sql1 + ";" + sql2;

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                da.Fill(ds);
                ds.Tables[0].TableName = "TicketsAccountWise";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        public static DataSet GetTicketsEntityWise(string StartDate, string EndDate, string departmentIds, int outletId)
        {
            DataSet ds = new DataSet();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                string sqlClause = string.Empty;
                dbConn.Open();
                //string sql1 = string.Format(@"SELECT t.DepartmentName,t.TicketId,t.TicketDate,et.Name EntityTypeName, te.EntityName,t.UltimateAccount,t.Amount,t.TotalAmount
                //                            FROM TicketView t, TicketEntities te, EntityTypes et
                //                            WHERE t.ticketid = te.Ticket_Id
                //                            AND te.EntityTypeId = et.Id
                //                            and t.TicketDate between '{0}' and '{1}'", StartDate, EndDate);

                //string sql1 = string.Format(@"SELECT t.DepartmentName,t.ticketnumber TicketId,t.TicketDate,isnull(t.UltimateAccount, 'N/A') UltimateAccount,isnull(t.Amount,0) Amount,t.TotalAmount,isnull(t.SortOrder,2000) SortOrder,isnull(et.EntityName, 'No Entity Type')EntityTypeName ,isnull(te.EntityName, 'No Entity Selected')EntityName
                //                                ,isnull(ti.NoOfGuests,0) NoOfGuests, isnull(ti.SettledBy, 'N/A') SettledBy, isnull(ti.Note, '') Note
                //                                FROM TicketView t
                //                                inner join Tickets ti
                //                                on t.ticketid = ti.Id
                //                                left outer join 
                //                                TicketEntities te
                //                                on T.ticketid = te.Ticket_Id
                //                                left outer join EntityTypes et
                //                                on te.EntityTypeId = et.Id
                //                                WHERE                                           
                //                                t.TicketDate between '{0}' and '{1}'
                //                                order by t.ticketid", StartDate, EndDate);
                if (!string.IsNullOrEmpty(departmentIds))
                {
                    sqlClause = string.Format(@" AND t.DepartmentId in ({0})", departmentIds);
                }
                if (outletId > 0)
                {
                    sqlClause += $@" AND t.SyncOutletId = {outletId}";
                }

                string sql1 = string.Format(@"Select * from (
											SELECT t.ticketnumber TicketId,t.TicketDate,isnull(t.UltimateAccount, 'N/A') UltimateAccount,isnull(t.Amount,0) Amount,t.TotalAmount,isnull(t.SortOrder,2000) SortOrder,
											isnull(et.EntityName, 'No Entity Type')EntityTypeName ,isnull(te.EntityName, 'No Entity Selected')EntityName
											,isnull(ti.NoOfGuests,0) NoOfGuests, isnull(ti.SettledBy, 'N/A') SettledBy, isnull(ti.Note, '') Note
											FROM TicketView t
											inner join Tickets ti
											on t.ticketid = ti.Id
											left outer join 
											TicketEntities te
											on T.ticketid = te.Ticket_Id
											left outer join EntityTypes et
											on te.EntityTypeId = et.Id
											WHERE                                           
											t.TicketDate between '{0}' and '{1}' {2}
										)T1
										union all
										(
											select t2.TicketNumber TicketId, t2.TicketDate,t2.UltimateAccount, t2.Amount, t2.TotalAmount,t2.SortOrder,
											isnull(et.EntityName, 'No Entity Type')EntityTypeName ,isnull(te.EntityName, 'No Entity Selected')EntityName, t2.NoOfGuests,t2.SettledBy, t2.Note  
											from (
												select d.Name UltimateAccount ,t.Id,t.TicketNumber,t.date TicketDate, sum(o.PlainTotal+o.OrderTagPrice*o.Quantity) Amount , t.TotalAmount,1 SortOrder, 
												isnull(t.NoOfGuests,0) NoOfGuests, isnull(t.SettledBy, 'N/A') SettledBy, isnull(cast(t.Note as nvarchar(4000)),'') Note
												from Orders o, tickets t, Departments d
												where o.TicketId = t.Id
												and o.DepartmentId = d.Id
												and o.DecreaseInventory =1 and o.CalculatePrice = 1
												and t.date between '{0}' and '{1}' {2}
												group by d.Name,t.TicketNumber,t.Id,t.Date, t.TotalAmount, SortOrder, t.NoOfGuests,  t.SettledBy,cast(t.Note as nvarchar(4000))
	
											)T2
											inner join Tickets ti
											on T2.Id = ti.Id
											left outer join 
											TicketEntities te
											on T2.Id = te.Ticket_Id
											left outer join EntityTypes et
											on te.EntityTypeId = et.Id
										)
										order by TicketId", StartDate, EndDate, sqlClause);


                string sql2 = string.Format(@"Select SUM(TotalAmount) SUMTotalAmount
											 from Tickets t
											 where t.Date  between '{0}' and '{1}' {2}", StartDate, EndDate, sqlClause);

                string sql = sql1 + ";" + sql2;
                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
                da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
                //AND tick.[Date] between '{0}' and '{1}'
                da.Fill(ds);
                ds.Tables[0].TableName = "TicketsEntityWise";
                ds.Tables[1].TableName = "SUMs";
                dbConn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
        public static List<int> numberOfPendingProcess(DateTime dtFromDate, DateTime dtToDate)
        {
            try
            {
                SqlConnection dbConn = new SqlConnection(DBUtility.GetConnectionString());
                return dbConn.Query<int>($@"SELECT O.Id PendingProcess FROM Orders O JOIN Tickets t ON t.Id = O.TicketId WHERE O.OrderCalculationTypes IS NULL AND t.Date BETWEEN '{dtFromDate.ToString("dd MMM yyyy")}' AND '{dtToDate.ToString("dd MMM yyyy")}';").ToList();
            }
            catch (SqlException ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex);
                return null;
            }


        }

        public static List<int> pendingProcessTicketId(DateTime dtFromDate, DateTime dtToDate)
        {
            try
            {
                SqlConnection dbConn = new SqlConnection(DBUtility.GetConnectionString());
                return dbConn.Query<int>($@"SELECT DISTINCT T.Id FROM Orders O JOIN Tickets t ON t.Id = O.TicketId WHERE O.OrderCalculationTypes IS NULL AND t.Date BETWEEN '{dtFromDate.ToString("dd MMM yyyy")}' AND '{dtToDate.ToString("dd MMM yyyy")}';").ToList();
            }
            catch (SqlException ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex);
                return new List<int>();
            }

        }
        public static int UpdateOrderByOrderCalculationType(int id, string data)
        {
            //int value = 0;
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);

                string query = string.Format(@"UPDATE Orders SET OrderCalculationTypes = '{0}' WHERE Id = {1}", data, id);
                dbConn.Open();
                SqlCommand cmd = new SqlCommand(query, dbConn);
                cmd.ExecuteReader();
                dbConn.Close();
                return 0;
            }
            catch (SqlException ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ex);
                return id;
            }
        }
        public static List<Ticket> GetIndividualTicket(int id)
        {
            SqlConnection dbConn = new SqlConnection(DBUtility.GetConnectionString());

            List<Ticket> tickets = dbConn.Query<Ticket>($@"SELECT * FROM Tickets Where Id = @ID", new { ID = id }).ToList();
            tickets.ForEach(ticket =>
            {
                ticket.Orders = GetAllOrders(dbConn, ticket.Id);
                ticket.Calculations = GetAllCalculations(dbConn, ticket.Id);
                ticket.Payments = GetAllPayments(dbConn, ticket.Id);
                ticket.TransactionDocument = GetTransactionDocument(dbConn, ticket.TransactionDocument_Id);
            });

            return tickets;
        }
        private static List<Order> GetAllOrders(SqlConnection dbConn, int ticketId)
        {
            return dbConn.Query<Order>($@"select * from Orders where Ticketid = @TicketId", new { TicketId = ticketId }).ToList();
        }
        private static List<Calculation> GetAllCalculations(SqlConnection dbConn, int ticketId)
        {
            /**/
            string sqlString = $@"select * from Calculations where Ticketid=@ticketId";
            //var reader = dbConn.ExecuteReader(sqlString, CommandBehavior.SequentialAccess);
            List<Calculation> objects = dbConn.Query<Calculation>(sqlString, new { ticketId = ticketId }).ToList();

            /**/
            return objects;
        }
        private static List<Payment> GetAllPayments(SqlConnection dbConn, int Id)
        {
            return dbConn.Query<Payment>($@"select * from Payments where Ticketid = @TicketId", new { TicketId = Id }).ToList();
        }
        public static List<MenuItemOrderAccountsInfo> GetMenuItemOrderAccountsInfoData(List<Ticket> oTickets, IEnumerable<CalculationType> oCalculationTypes)
        {
            IEnumerable<MenuItem> oMenuItems = GetMenuItems();

            List<MenuItemOrderAccountsInfo> result = MenuGroupBuilder.CalculateMenuItemOrderGroupCalculationInfo(oTickets, oMenuItems, oCalculationTypes).ToList();
            var ds = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(result.ToList());

            return result;
            //return 0;
        }

        internal static AmountCalculator GetRefundCalculator(IEnumerable<Ticket> oTicket)
        {
            var groups = oTicket
                .SelectMany(x => x.Payments)
                .Where(x => x.Amount < 0)
                .GroupBy(x => x.PaymentTypeId)
                .Select(x => new TenderedAmount
                { PaymentName = GetPaymentTypeName(x.Key), Amount = x.Sum(y => y.Amount) });
            return new AmountCalculator(groups);
        }

        private static string GetPaymentTypeName(int paymentTypeId)
        {
            DataSet dsPayment = GetPaymentType();
            var pt = dsPayment.Tables[0].Select(string.Format("Id={0}", paymentTypeId))[0]["Name"] == DBNull.Value
                ? ""
                : dsPayment.Tables[0].Select(string.Format("Id={0}", paymentTypeId))[0]["Name"].ToString();
            return pt != null ? pt.ToString() : "";
        }
        /*Work Period Report Purpose*/

        //public static DataSet GetReport(DateTime fromDate, DateTime toDate, int departmentId)
        //{
        //    DataSet ds = new DataSet();
        //    decimal nGrandTotal = 0;
        //    //var currentPeriod = ReportContext.CurrentWorkPeriod;
        //    //var report = new SimpleReport("8cm");
        //    //AddDefaultReportHeader(report, currentPeriod, Resources.WorkPeriodReport);
        //    //AddHeaderInfos(report, LocalSettings.CompanyName);
        //    //AddHeaderInfos(report, LocalSettings.CompanyAddress);
        //    //AddHeaderInfos(report, LocalSettings.VatRegistrationNumber);
        //    //AddHeaderInfos(report, "Report Time: " + DateTime.Now.ToString("F"));
        //    //report.AddLink(Resources.ExportSalesData);
        //    //HandleLink(Resources.ExportSalesData);
        //    List<Ticket> oTickets = GetTickets(fromDate, toDate, departmentId);
        //    DataTable table = new DataTable("Sales");
        //    table = CreateTicketTypeInfo(oTickets, oTickets.Where(x => x.TotalAmount >= 0), ref nGrandTotal);
        //    table.TableName = "Sales";
        //    ds.Tables.Add(table);
        //    var refundTickets = oTickets.Where(x => x.TotalAmount < 0).ToList();

        //    table = new DataTable("RefundTickets");
        //    if (refundTickets.Any())
        //    {
        //        table = CreateTicketTypeInfo(oTickets, refundTickets, ref nGrandTotal);
        //        table.TableName = "RefundTickets";
        //    }
        //    ds.Tables.Add(table);
        //    var incomeCalculator = GetIncomeCalculator(oTickets);

        //    //report.AddColumnLength("GelirlerTablosu", "45*", "Auto", "35*");
        //    //report.AddColumTextAlignment("GelirlerTablosu", TextAlignment.Left, TextAlignment.Right, TextAlignment.Right);
        //    //report.AddTable("GelirlerTablosu", Resources.Incomes, "", "");
        //    table = new DataTable("Income");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("Incomes"), new DataColumn("Perc"),
        //                new DataColumn("Amount")});

        //    foreach (var paymentName in incomeCalculator.PaymentNames)
        //    {
        //        //report.AddRow("GelirlerTablosu", paymentName, incomeCalculator.GetPercent(paymentName), incomeCalculator.GetAmount(paymentName).ToString(ReportContext.CurrencyFormat));
        //        table.Rows.Add(new object[] { paymentName, incomeCalculator.GetPercent(paymentName), incomeCalculator.GetAmount(paymentName).ToString("0.00") });
        //        //report.AddRow("GelirlerTablosu", Resources.TotalIncome.ToUpper(), "", incomeCalculator.TotalAmount.ToString(ReportContext.CurrencyFormat));
        //    }
        //    //report.AddRow("GelirlerTablosu", Resources.TotalIncome.ToUpper(), "", incomeCalculator.TotalAmount.ToString(ReportContext.CurrencyFormat));
        //    table.Rows.Add(new object[] { "Total Income", "", incomeCalculator.TotalAmount.ToString("0.00") });

        //    ds.Tables.Add(table);

        //    table = new DataTable("CustomerDue");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("CustomerDueAdvance"), new DataColumn("Value") });

        //    var CustomerAdvanceDueAmount = Convert.ToDecimal(GetAdvanceDue(fromDate, toDate).Tables[0].Rows[0]["CustomerCashPayment"]);
        //    table.Rows.Add(new object[] { "Customer Advance/Due Colleciton Cash", CustomerAdvanceDueAmount.ToString("0.00") });
        //    CustomerAdvanceDueAmount = Convert.ToDecimal(GetAdvanceDue(fromDate, toDate).Tables[0].Rows[0]["CustomerCreditPayment"]);
        //    table.Rows.Add(new object[] { "Customer Advance/Due Colleciton Credit Card", CustomerAdvanceDueAmount.ToString("0.00") });
        //    ds.Tables.Add(table);

        //    table = new DataTable("DepartmentWise");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("DepartmentName"), new DataColumn("Amount") });
        //    IEnumerable<Department> oDepartments = GetDepartmentCollection();
        //    var departmentWiseSales = MenuGroupBuilder.CalculateDepartmentWiseSales(oTickets, GetDepartmentCollection()).OrderByDescending(x => x.Name);
        //    IList<DepartmentItemInfo> DeprtmentSalesInfos = new List<DepartmentItemInfo>();
        //    foreach (var departmentwisesales in departmentWiseSales)
        //    {
        //        var s = oDepartments.Where(y => y.Id == departmentwisesales.ID).Select(y => new { y.Id, y.Name });

        //        DepartmentItemInfo q = new DepartmentItemInfo
        //        {
        //            Name = s.First().Name,
        //            Amount = departmentwisesales.Amount
        //        };
        //        DeprtmentSalesInfos.Add(q);
        //    }
        //    foreach (var departmentSalesInfo in DeprtmentSalesInfos.OrderBy(l => l.Name))
        //    {
        //        table.Rows.Add(new object[] { departmentSalesInfo.Name, departmentSalesInfo.Amount.ToString("0.00") });
        //        //report.AddRow("DepartmentWiseSales", departmentSalesInfo.Name, departmentSalesInfo.Amount.ToString(ReportContext.CurrencyFormat));
        //    }
        //    table.Rows.Add(new object[] { "Total", departmentWiseSales.Sum(x => x.Amount).ToString("0.00") });
        //    //report.AddRow("DepartmentWiseSales", Resources.Total, departmentWiseSales.Sum(x => x.Amount).ToString(ReportContext.CurrencyFormat));
        //    ds.Tables.Add(table);
        //    /**/
        //    table = new DataTable("Refund");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("RefundType"), new DataColumn("Amount") });
        //    var refundCalculator = GetRefundCalculator(oTickets);
        //    if (refundCalculator.TotalAmount != 0)
        //    {
        //        foreach (var paymentName in refundCalculator.PaymentNames)
        //        {
        //            table.Rows.Add(new object[] { paymentName, refundCalculator.GetAmount(paymentName).ToString("0.00") });
        //        }
        //        table.Rows.Add(new object[] { "TOTAL REFUND", refundCalculator.TotalAmount.ToString("0.00") });
        //    }
        //    ds.Tables.Add(table);
        //    //---------------
        //    table = new DataTable("GeneralInfo");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
        //    DataSet dtTicketType = GetTicketType();
        //    var ticketGropus = oTickets
        //        .GroupBy(x => new { x.TicketTypeId })
        //        .Select(x => new TicketTypeInfo
        //        {
        //            TicketTypeId = x.Key.TicketTypeId,
        //            TicketCount = x.Count(),
        //            Amount = x.Sum(y => y.Sum),
        //            Tax = x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)),
        //            Services = x.Sum(y => y.PostTaxServicesTotal),
        //            TicketTypeName = dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"] == DBNull.Value ? "" : dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"].ToString()
        //        }).ToList();

        //    var propertySum = oTickets
        //        .SelectMany(x => x.Orders)
        //        .Sum(x => x.GetOrderTagPrice() * x.Quantity);

        //    var discounts = Math.Abs(oTickets.Sum(x => x.PreTaxServicesTotal));

        //    table.Rows.Add(new object[] { "Extra Item Properties", propertySum.ToString("0.00") });
        //    table.Rows.Add(new object[] { "Pre-Tax Services Total", discounts.ToString("0.00") });
        //    ds.Tables.Add(table);
        //    //-------------------------
        //    table = new DataTable("Orders");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });

        //    if (ticketGropus.Count() > 1)
        //    {
        //        foreach (var TicketTypeInfo in ticketGropus)
        //        {
        //            table.Rows.Add(new object[] { TicketTypeInfo.TicketTypeName, TicketTypeInfo.TicketCount.ToString() });
        //            //report.AddRow("Bilgi", TicketTypeInfo.TicketTypeName, TicketTypeInfo.TicketCount.ToString());
        //        }
        //    }

        //    var orderStates = oTickets
        //                       .SelectMany(x => x.Orders)
        //                       .SelectMany(x => x.GetOrderStateValues()).Distinct().ToList();

        //    if (orderStates.Any())
        //    {
        //        table.Rows.Add(new object[] { "Orders", "" });

        //        foreach (var orderStateValue in orderStates)
        //        {
        //            var value = orderStateValue;
        //            var items =
        //                oTickets.SelectMany(x => x.Orders).Where(x => x.IsInState(value.StateName, value.State)).ToList();
        //            var amount = items.Sum(x => x.GetValue());
        //            var count = items.Count();
        //            table.Rows.Add(new object[] { string.Format("{0} ({1})", orderStateValue.State, count), amount.ToString("0.00") });
        //        }
        //    }



        //    #region Ovi asked to remove unpaid/paid portion 27 nov 2015
        //    //var ticketStates = ReportContext.Tickets
        //    //    .SelectMany(x => x.GetTicketStateValues()).Distinct().ToList();

        //    //if (ticketStates.Any())
        //    //{
        //    //    report.AddBoldRow("Bilgi", Resources.Tickets, "");
        //    //    foreach (var ticketStateValue in ticketStates)
        //    //    {
        //    //        TicketStateValue value = ticketStateValue;
        //    //        var items = ReportContext.Tickets.Where(x => x.IsInState(value.StateName, value.State)).ToList();
        //    //        var amount = items.Sum(x => x.GetSum());
        //    //        var count = items.Count();
        //    //        report.AddRow("Bilgi", string.Format("{0} ({1})", ticketStateValue.State, count), amount.ToString(ReportContext.CurrencyFormat));
        //    //    }
        //    //} 
        //    #endregion

        //    var ticketCount = ticketGropus.Sum(x => x.TicketCount);

        //    table.Rows.Add(new object[] { "Ticket Count", ticketCount.ToString() });
        //    table.Rows.Add(new object[] { "Number Of Guests", oTickets.Sum(x => x.NoOfGuests).ToString() });


        //    table.Rows.Add(new object[] {"Grand Sales/ Ticket", ticketCount > 0
        //        ? (ticketGropus.Sum(x => x.Amount) / ticketGropus.Sum(x => x.TicketCount)).ToString("0.00")
        //        : "0"});

        //    table.Rows.Add(new object[] { "Sales/ Ticket", ticketCount > 0 ? (nGrandTotal / ticketGropus.Sum(x => x.TicketCount)).ToString("0.00") : "0" });

        //    table.Rows.Add(new object[] {"Gross Sales/ Guest", oTickets.Sum(x => x.NoOfGuests) > 0
        //    ? (ticketGropus.Sum(x => x.Amount) / oTickets.Sum(x => x.NoOfGuests)).ToString("0.00") : "0"});
        //    table.Rows.Add(new object[] { "Sales/ Guest", oTickets.Sum(x => x.NoOfGuests) > 0 ? (nGrandTotal / oTickets.Sum(x => x.NoOfGuests)).ToString("0.00") : "0" });
        //    ds.Tables.Add(table);

        //    table = new DataTable("TicketGroupInfo");
        //    if (ticketGropus.Count() > 1)
        //    {
        //        table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value"), new DataColumn("Value1"), new DataColumn("Value2") });
        //        foreach (var ticketTypeInfo in ticketGropus)
        //        {
        //            var dinfo = ticketTypeInfo;

        //            var groups = oTickets
        //                .Where(x => x.TicketTypeId == dinfo.TicketTypeId)
        //                .SelectMany(x => x.Payments)
        //                .GroupBy(x => new { x.Name })
        //                .Select(x => new TenderedAmount { PaymentName = x.Key.Name, Amount = x.Sum(y => y.Amount) });

        //            var ticketTypeAmountCalculator = new AmountCalculator(groups);

        //            table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", string.Format("{0} Incomes", ticketTypeInfo.TicketTypeName), "", "" });

        //            foreach (var paymentName in ticketTypeAmountCalculator.PaymentNames)
        //            {
        //                table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", paymentName, ticketTypeAmountCalculator.GetPercent(paymentName), ticketTypeAmountCalculator.GetAmount(paymentName).ToString("0.00") });
        //            }

        //            table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", " Total Income", "", ticketTypeInfo.Amount.ToString("0.00") });

        //            var ddiscounts = oTickets
        //                .Where(x => x.TicketTypeId == dinfo.TicketTypeId)
        //                .Sum(x => x.PreTaxServicesTotal);

        //            ddiscounts = Math.Abs(ddiscounts);

        //            table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", "Discounts Total", "", ddiscounts.ToString("0.00") });
        //        }
        //        //ds.Tables.Add(table);
        //    }
        //    ds.Tables.Add(table);

        //    //--
        //    #region Ticket Tag for further development
        //    //if (oTickets.Select(x => x.GetTagData()).Where(x => !string.IsNullOrEmpty(x)).Distinct().Any())
        //    //{
        //    //    var dict = new Dictionary<string, List<Ticket>>();

        //    //    foreach (var ticket in oTickets.Where(x => x.IsTagged))
        //    //    {
        //    //        foreach (var tag in ticket.GetTicketTagValues().Select(x => x.TagName + ":" + x.TagValue))
        //    //        {
        //    //            if (!dict.ContainsKey(tag))
        //    //                dict.Add(tag, new List<Ticket>());
        //    //            dict[tag].Add(ticket);
        //    //        }
        //    //    }

        //    //    var tagGroups = dict.Select(x => new TicketTagInfo { Amount = x.Value.Sum(y => y.GetPlainSum()), TicketCount = x.Value.Count, TagName = x.Key }).OrderBy(x => x.TagName);

        //    //    var tagGrp = tagGroups.GroupBy(x => x.TagName.Split(':')[0]);


        //    //    table = new DataTable();
        //    //    table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
        //    //    table.Rows.Add(new object[] { "Ticket Tags", "Detail-1", "Detail-2" });

        //    //    foreach (var grp in tagGrp)
        //    //    {
        //    //        var grouping = grp;
        //    //        var tag = ReportContext.TicketTagGroups.FirstOrDefault(x => x.Name == grouping.Key);
        //    //        if (tag == null) continue;

        //    //        report.AddBoldRow("Etiket", grp.Key, "", "");

        //    //        if (tag.IsDecimal)
        //    //        {
        //    //            var tCount = grp.Sum(x => x.TicketCount);
        //    //            var tSum = grp.Sum(x => Convert.ToDecimal(x.TagName.Split(':')[1]) * x.TicketCount);
        //    //            var amnt = grp.Sum(x => x.Amount);
        //    //            var rate = tSum / amnt;
        //    //            report.AddRow("Etiket", string.Format(Resources.TotalAmount_f, tag.Name), "", tSum.ToString(ReportContext.CurrencyFormat));
        //    //            report.AddRow("Etiket", Resources.TicketCount, "", tCount.ToString());
        //    //            report.AddRow("Etiket", Resources.TicketTotal, "", amnt.ToString(ReportContext.CurrencyFormat));
        //    //            report.AddRow("Etiket", Resources.Rate, "", rate.ToString("%#0.##"));
        //    //            continue;
        //    //        }

        //    //        foreach (var ticketTagInfo in grp)
        //    //        {
        //    //            report.AddRow("Etiket",
        //    //                ticketTagInfo.TagName.Split(':')[1],
        //    //                ticketTagInfo.TicketCount.ToString(),
        //    //                ticketTagInfo.Amount.ToString(ReportContext.CurrencyFormat));
        //    //        }

        //    //        var totalAmount = grp.Sum(x => x.Amount);
        //    //        report.AddRow("Etiket", string.Format(Resources.TotalAmount_f, tag.Name), "", totalAmount.ToString(ReportContext.CurrencyFormat));

        //    //        var sum = 0m;

        //    //        if (tag.IsInteger)
        //    //        {
        //    //            try
        //    //            {
        //    //                sum = grp.Sum(x => Convert.ToDecimal(x.TagName.Split(':')[1]) * x.TicketCount);
        //    //                report.AddRow("Etiket", string.Format(Resources.TicketTotal_f, tag.Name), "", sum.ToString("#,##.##"));
        //    //            }
        //    //            catch (FormatException)
        //    //            {
        //    //                report.AddRow("Etiket", string.Format(Resources.TicketTotal_f, tag.Name), "", "#Hata!");
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            sum = grp.Sum(x => x.TicketCount);
        //    //        }

        //    //        if (sum > 0)
        //    //        {
        //    //            var average = totalAmount / sum;
        //    //            report.AddRow("Etiket", string.Format(Resources.TotalAmountDivTag_f, tag.Name), "", average.ToString(ReportContext.CurrencyFormat));
        //    //        }
        //    //    }
        //    //} 
        //    #endregion

        //    ////----

        //    var owners = oTickets.SelectMany(ticket => ticket.Orders.Where(x => !x.IncreaseInventory).Select(order => new { Ticket = ticket, Order = order }))
        //        .GroupBy(x => new { x.Order.CreatingUserName })
        //        .Select(x => new UserInfo { UserName = x.Key.CreatingUserName, Amount = x.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) });

        //    table = new DataTable("UserWiseOrders");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("UserWiseSalesOrders"), new DataColumn("Value"), new DataColumn("Perc") });

        //    foreach (var ownerInfo in owners)
        //    {
        //        table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00"), "%" + (ownerInfo.Amount * 100 / owners.Sum(x => x.Amount)).ToString("0.00") });
        //    }
        //    ds.Tables.Add(table);

        //    owners = oTickets.GroupBy(x => new { x.UserString })
        //        .Select(x => new UserInfo { UserName = x.Key.UserString, Amount = x.Sum(y => y.TotalAmount) });

        //    table = new DataTable("UserWiseTickets");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("UserWiseTicketsTotal"), new DataColumn("Value"), new DataColumn("Perc") });

        //    foreach (var ownerInfo in owners)
        //    {
        //        table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00"), "%" + (ownerInfo.Amount * 100 / owners.Sum(x => x.Amount)).ToString("0.00") });
        //    }
        //    ds.Tables.Add(table);

        //    var refundOwners = oTickets.SelectMany(ticket => ticket.Orders.Where(x => x.IncreaseInventory).Select(order => new { Ticket = ticket, Order = order }))
        //        .GroupBy(x => new { x.Order.CreatingUserName })
        //        .Select(x => new UserInfo { UserName = x.Key.CreatingUserName, Amount = x.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) }).ToList();

        //    table = new DataTable("RefundOwners");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("UserReturns"), new DataColumn("Value") });
        //    if (refundOwners.Any())
        //    {
        //        //table = new DataTable("RefundOwners");
        //        //table.Columns.AddRange(new DataColumn[] { new DataColumn("UserReturns"), new DataColumn("Value") });                    

        //        foreach (var ownerInfo in refundOwners)
        //        {
        //            table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00") });
        //        }
        //        //ds.Tables.Add(table);
        //    }
        //    ds.Tables.Add(table);
        //    IEnumerable<MenuItem> oMenuItems = GetMenuItems();
        //    var menuGroups = MenuGroupBuilder.CalculateMenuGroups(oTickets, oMenuItems).ToList();


        //    table = new DataTable("ItemSales");
        //    table.Columns.AddRange(new DataColumn[] { new DataColumn("ItemSales"), new DataColumn("Perc"), new DataColumn("Amount") });

        //    foreach (var menuItemInfo in menuGroups)
        //    {
        //        table.Rows.Add(new object[] { menuItemInfo.GroupName,
        //            string.Format("%{0:0.00}", menuItemInfo.Rate),
        //            menuItemInfo.Amount.ToString("0.00")});
        //    }

        //    table.Rows.Add(new object[] { "Total", "", menuGroups.Sum(x => x.Amount).ToString("0.00") });
        //    //return report.Document;
        //    ds.Tables.Add(table);
        //    return ds;
        //}
        public static DataSet GetReport(DateTime fromDate, DateTime toDate, string DepartmentIds, int OutletId, bool IsShort)
        {
            DataSet ds = new DataSet();
            decimal nGrandTotal = 0;
            //var currentPeriod = ReportContext.CurrentWorkPeriod;
            //var report = new SimpleReport("8cm");
            //AddDefaultReportHeader(report, currentPeriod, Resources.WorkPeriodReport);
            //AddHeaderInfos(report, LocalSettings.CompanyName);
            //AddHeaderInfos(report, LocalSettings.CompanyAddress);
            //AddHeaderInfos(report, LocalSettings.VatRegistrationNumber);
            //AddHeaderInfos(report, "Report Time: " + DateTime.Now.ToString("F"));
            //report.AddLink(Resources.ExportSalesData);
            //HandleLink(Resources.ExportSalesData);
            List<Ticket> oTickets = GetTicketsFaster(OutletId, DepartmentIds, fromDate, toDate);
            //oTickets = GetTickets(OutletId, DepartmentIds, fromDate, toDate);
            DataTable table = new DataTable("Sales");
            table = CreateTicketTypeInfo(oTickets, oTickets.Where(x => x.TotalAmount >= 0), ref nGrandTotal, OutletId);
            table.TableName = "Sales";
            ds.Tables.Add(table);
            var refundTickets = oTickets.Where(x => x.TotalAmount < 0).ToList();

            table = new DataTable("RefundTickets");
            if (refundTickets.Any())
            {
                table = CreateTicketTypeInfo(oTickets, refundTickets, ref nGrandTotal, OutletId);
                table.TableName = "RefundTickets";
            }
            ds.Tables.Add(table);
            var incomeCalculator = GetIncomeCalculator(oTickets);

            //report.AddColumnLength("GelirlerTablosu", "45*", "Auto", "35*");
            //report.AddColumTextAlignment("GelirlerTablosu", TextAlignment.Left, TextAlignment.Right, TextAlignment.Right);
            //report.AddTable("GelirlerTablosu", Resources.Incomes, "", "");
            table = new DataTable("Income");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Incomes"), new DataColumn("Perc"),
                        new DataColumn("Amount")});

            foreach (var paymentName in incomeCalculator.PaymentNames)
            {
                //report.AddRow("GelirlerTablosu", paymentName, incomeCalculator.GetPercent(paymentName), incomeCalculator.GetAmount(paymentName).ToString(ReportContext.CurrencyFormat));
                table.Rows.Add(new object[] { paymentName, incomeCalculator.GetPercent(paymentName), incomeCalculator.GetAmount(paymentName).ToString("0.00") });
                //report.AddRow("GelirlerTablosu", Resources.TotalIncome.ToUpper(), "", incomeCalculator.TotalAmount.ToString(ReportContext.CurrencyFormat));
            }
            //report.AddRow("GelirlerTablosu", Resources.TotalIncome.ToUpper(), "", incomeCalculator.TotalAmount.ToString(ReportContext.CurrencyFormat));
            table.Rows.Add(new object[] { "Total Income", "", incomeCalculator.TotalAmount.ToString("0.00") });

            ds.Tables.Add(table);

            table = new DataTable("CustomerDue");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("CustomerDueAdvance"), new DataColumn("Value") });

            var CustomerAdvanceDueAmount = Convert.ToDecimal(GetAdvanceDue(fromDate, toDate).Tables[0].Rows[0]["CustomerCashPayment"]);
            table.Rows.Add(new object[] { "Customer Advance/Due Colleciton Cash", CustomerAdvanceDueAmount.ToString("0.00") });
            CustomerAdvanceDueAmount = Convert.ToDecimal(GetAdvanceDue(fromDate, toDate).Tables[0].Rows[0]["CustomerCreditPayment"]);
            table.Rows.Add(new object[] { "Customer Advance/Due Colleciton Credit Card", CustomerAdvanceDueAmount.ToString("0.00") });
            ds.Tables.Add(table);

            if (IsShort)
            {
                ds.Tables.Add(new DataTable("DepartmentWise"));
                ds.Tables.Add(new DataTable("Refund"));
                ds.Tables.Add(new DataTable("GeneralInfo"));
                ds.Tables.Add(new DataTable("Orders"));
                ds.Tables.Add(new DataTable("TicketGroupInfo"));
                ds.Tables.Add(new DataTable("UserWiseOrders"));
                ds.Tables.Add(new DataTable("UserWiseTickets"));
                ds.Tables.Add(new DataTable("RefundOwners"));
                ds.Tables.Add(new DataTable("ItemSales"));
                return ds;
            }

            table = new DataTable("DepartmentWise");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("DepartmentName"), new DataColumn("Amount") });
            IEnumerable<Department> oDepartments = GetDepartmentCollection();
            var departmentWiseSales = MenuGroupBuilder.CalculateDepartmentWiseSales(oTickets, GetDepartmentCollection()).OrderByDescending(x => x.Name);
            IList<DepartmentItemInfo> DeprtmentSalesInfos = new List<DepartmentItemInfo>();
            foreach (var departmentwisesales in departmentWiseSales)
            {
                var s = oDepartments.Where(y => y.Id == departmentwisesales.ID).Select(y => new { y.Id, y.Name });

                DepartmentItemInfo q = new DepartmentItemInfo
                {
                    Name = s.First().Name,
                    Amount = departmentwisesales.Amount
                };
                DeprtmentSalesInfos.Add(q);
            }
            foreach (var departmentSalesInfo in DeprtmentSalesInfos.OrderBy(l => l.Name))
            {
                table.Rows.Add(new object[] { departmentSalesInfo.Name, departmentSalesInfo.Amount.ToString("0.00") });
                //report.AddRow("DepartmentWiseSales", departmentSalesInfo.Name, departmentSalesInfo.Amount.ToString(ReportContext.CurrencyFormat));
            }
            table.Rows.Add(new object[] { "Total", departmentWiseSales.Sum(x => x.Amount).ToString("0.00") });
            //report.AddRow("DepartmentWiseSales", Resources.Total, departmentWiseSales.Sum(x => x.Amount).ToString(ReportContext.CurrencyFormat));
            ds.Tables.Add(table);
            /**/
            table = new DataTable("Refund");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("RefundType"), new DataColumn("Amount") });
            var refundCalculator = GetRefundCalculator(oTickets);
            if (refundCalculator.TotalAmount != 0)
            {
                foreach (var paymentName in refundCalculator.PaymentNames)
                {
                    table.Rows.Add(new object[] { paymentName, refundCalculator.GetAmount(paymentName).ToString("0.00") });
                }
                table.Rows.Add(new object[] { "TOTAL REFUND", refundCalculator.TotalAmount.ToString("0.00") });
            }
            ds.Tables.Add(table);
            //---------------
            table = new DataTable("GeneralInfo");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
            DataSet dtTicketType = GetTicketType();
            var ticketGropus = oTickets
                .GroupBy(x => new { x.TicketTypeId })
                .Select(x => new TicketTypeInfo
                {
                    TicketTypeId = x.Key.TicketTypeId,
                    TicketCount = x.Count(),
                    Amount = x.Sum(y => y.Sum),
                    Tax = x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)),
                    Services = x.Sum(y => y.PostTaxServicesTotal),
                    TicketTypeName = dtTicketType.Tables[0].Select($"Id={x.Key.TicketTypeId}")[0]["Name"] == DBNull.Value ? "" : dtTicketType.Tables[0].Select($"Id={x.Key.TicketTypeId}")[0]["Name"].ToString()
                }).OrderBy(x => x.TicketTypeName).ToList();

            var propertySum = oTickets
                .SelectMany(x => x.Orders)
                .Sum(x => x.GetOrderTagPrice() * x.Quantity);

            var discounts = Math.Abs(oTickets.Sum(x => x.PreTaxServicesTotal));

            table.Rows.Add(new object[] { "Extra Item Properties", propertySum.ToString("0.00") });
            table.Rows.Add(new object[] { "Pre-Tax Services Total", discounts.ToString("0.00") });
            ds.Tables.Add(table);
            //-------------------------
            table = new DataTable("Orders");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });

            if (ticketGropus.Count() > 1)
            {
                foreach (var TicketTypeInfo in ticketGropus)
                {
                    table.Rows.Add(new object[] { TicketTypeInfo.TicketTypeName, TicketTypeInfo.TicketCount.ToString() });
                    //report.AddRow("Bilgi", TicketTypeInfo.TicketTypeName, TicketTypeInfo.TicketCount.ToString());
                }
            }

            var orderStates = oTickets
                               .SelectMany(x => x.Orders)
                               .SelectMany(x => x.GetOrderStateValues()).Distinct().ToList();

            if (orderStates.Any())
            {
                table.Rows.Add(new object[] { "Orders", "" });

                foreach (var orderStateValue in orderStates)
                {
                    if (orderStateValue.StateName.ToLower() == "gstatus")
                    {
                        var value = orderStateValue;
                        var items =
                            oTickets.SelectMany(x => x.Orders).Where(x => x.IsInState(value.StateName, value.State, value.StateValue)).ToList();
                        var amount = items.Sum(x => x.GetValue());
                        var count = items.Count();
                        table.Rows.Add(new object[] { $"{orderStateValue.State}:{orderStateValue.StateValue} ({count})", amount.ToString("0.00") });
                    }
                }
            }



            #region Ovi asked to remove unpaid/paid portion 27 nov 2015
            //var ticketStates = ReportContext.Tickets
            //    .SelectMany(x => x.GetTicketStateValues()).Distinct().ToList();

            //if (ticketStates.Any())
            //{
            //    report.AddBoldRow("Bilgi", Resources.Tickets, "");
            //    foreach (var ticketStateValue in ticketStates)
            //    {
            //        TicketStateValue value = ticketStateValue;
            //        var items = ReportContext.Tickets.Where(x => x.IsInState(value.StateName, value.State)).ToList();
            //        var amount = items.Sum(x => x.GetSum());
            //        var count = items.Count();
            //        report.AddRow("Bilgi", string.Format("{0} ({1})", ticketStateValue.State, count), amount.ToString(ReportContext.CurrencyFormat));
            //    }
            //} 
            #endregion

            var ticketCount = ticketGropus.Sum(x => x.TicketCount);

            table.Rows.Add(new object[] { "Ticket Count", ticketCount.ToString() });
            table.Rows.Add(new object[] { "Number Of Guests", oTickets.Sum(x => x.NoOfGuests).ToString() });


            table.Rows.Add(new object[] {"Grand Sales/ Ticket", ticketCount > 0
                ? (ticketGropus.Sum(x => x.Amount) / ticketGropus.Sum(x => x.TicketCount)).ToString("0.00")
                : "0"});

            table.Rows.Add(new object[] { "Sales/ Ticket", ticketCount > 0 ? (nGrandTotal / ticketGropus.Sum(x => x.TicketCount)).ToString("0.00") : "0" });

            table.Rows.Add(new object[] {"Gross Sales/ Guest", oTickets.Sum(x => x.NoOfGuests) > 0
            ? (ticketGropus.Sum(x => x.Amount) / oTickets.Sum(x => x.NoOfGuests)).ToString("0.00") : "0"});
            table.Rows.Add(new object[] { "Sales/ Guest", oTickets.Sum(x => x.NoOfGuests) > 0 ? (nGrandTotal / oTickets.Sum(x => x.NoOfGuests)).ToString("0.00") : "0" });
            ds.Tables.Add(table);

            table = new DataTable("TicketGroupInfo");
            if (ticketGropus.Count() > 1)
            {
                table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value"), new DataColumn("Value1"), new DataColumn("Value2") });
                foreach (var ticketTypeInfo in ticketGropus)
                {
                    var dinfo = ticketTypeInfo;

                    var groups = oTickets
                        .Where(x => x.TicketTypeId == dinfo.TicketTypeId)
                        .SelectMany(x => x.Payments)
                        .GroupBy(x => new { x.Name })
                        .Select(x => new TenderedAmount { PaymentName = x.Key.Name, Amount = x.Sum(y => y.Amount) });

                    var ticketTypeAmountCalculator = new AmountCalculator(groups);

                    table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", string.Format("{0} Incomes", ticketTypeInfo.TicketTypeName), "", "" });

                    foreach (var paymentName in ticketTypeAmountCalculator.PaymentNames)
                    {
                        table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", paymentName, ticketTypeAmountCalculator.GetPercent(paymentName), ticketTypeAmountCalculator.GetAmount(paymentName).ToString("0.00") });
                    }

                    table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", " Total Income", "", ticketTypeInfo.Amount.ToString("0.00") });

                    var ddiscounts = oTickets
                        .Where(x => x.TicketTypeId == dinfo.TicketTypeId)
                        .Sum(x => x.PreTaxServicesTotal);

                    ddiscounts = Math.Abs(ddiscounts);

                    table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", "Discounts Total", "", ddiscounts.ToString("0.00") });
                }
                //ds.Tables.Add(table);
            }
            ds.Tables.Add(table);

            //--
            #region Ticket Tag for further development
            //if (oTickets.Select(x => x.GetTagData()).Where(x => !string.IsNullOrEmpty(x)).Distinct().Any())
            //{
            //    var dict = new Dictionary<string, List<Ticket>>();

            //    foreach (var ticket in oTickets.Where(x => x.IsTagged))
            //    {
            //        foreach (var tag in ticket.GetTicketTagValues().Select(x => x.TagName + ":" + x.TagValue))
            //        {
            //            if (!dict.ContainsKey(tag))
            //                dict.Add(tag, new List<Ticket>());
            //            dict[tag].Add(ticket);
            //        }
            //    }

            //    var tagGroups = dict.Select(x => new TicketTagInfo { Amount = x.Value.Sum(y => y.GetPlainSum()), TicketCount = x.Value.Count, TagName = x.Key }).OrderBy(x => x.TagName);

            //    var tagGrp = tagGroups.GroupBy(x => x.TagName.Split(':')[0]);


            //    table = new DataTable();
            //    table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
            //    table.Rows.Add(new object[] { "Ticket Tags", "Detail-1", "Detail-2" });

            //    foreach (var grp in tagGrp)
            //    {
            //        var grouping = grp;
            //        var tag = ReportContext.TicketTagGroups.FirstOrDefault(x => x.Name == grouping.Key);
            //        if (tag == null) continue;

            //        report.AddBoldRow("Etiket", grp.Key, "", "");

            //        if (tag.IsDecimal)
            //        {
            //            var tCount = grp.Sum(x => x.TicketCount);
            //            var tSum = grp.Sum(x => Convert.ToDecimal(x.TagName.Split(':')[1]) * x.TicketCount);
            //            var amnt = grp.Sum(x => x.Amount);
            //            var rate = tSum / amnt;
            //            report.AddRow("Etiket", string.Format(Resources.TotalAmount_f, tag.Name), "", tSum.ToString(ReportContext.CurrencyFormat));
            //            report.AddRow("Etiket", Resources.TicketCount, "", tCount.ToString());
            //            report.AddRow("Etiket", Resources.TicketTotal, "", amnt.ToString(ReportContext.CurrencyFormat));
            //            report.AddRow("Etiket", Resources.Rate, "", rate.ToString("%#0.##"));
            //            continue;
            //        }

            //        foreach (var ticketTagInfo in grp)
            //        {
            //            report.AddRow("Etiket",
            //                ticketTagInfo.TagName.Split(':')[1],
            //                ticketTagInfo.TicketCount.ToString(),
            //                ticketTagInfo.Amount.ToString(ReportContext.CurrencyFormat));
            //        }

            //        var totalAmount = grp.Sum(x => x.Amount);
            //        report.AddRow("Etiket", string.Format(Resources.TotalAmount_f, tag.Name), "", totalAmount.ToString(ReportContext.CurrencyFormat));

            //        var sum = 0m;

            //        if (tag.IsInteger)
            //        {
            //            try
            //            {
            //                sum = grp.Sum(x => Convert.ToDecimal(x.TagName.Split(':')[1]) * x.TicketCount);
            //                report.AddRow("Etiket", string.Format(Resources.TicketTotal_f, tag.Name), "", sum.ToString("#,##.##"));
            //            }
            //            catch (FormatException)
            //            {
            //                report.AddRow("Etiket", string.Format(Resources.TicketTotal_f, tag.Name), "", "#Hata!");
            //            }
            //        }
            //        else
            //        {
            //            sum = grp.Sum(x => x.TicketCount);
            //        }

            //        if (sum > 0)
            //        {
            //            var average = totalAmount / sum;
            //            report.AddRow("Etiket", string.Format(Resources.TotalAmountDivTag_f, tag.Name), "", average.ToString(ReportContext.CurrencyFormat));
            //        }
            //    }
            //} 
            #endregion

            ////----

            var owners = oTickets.SelectMany(ticket => ticket.Orders.Where(x => !x.IncreaseInventory).Select(order => new { Ticket = ticket, Order = order }))
                .GroupBy(x => new { x.Order.CreatingUserName })
                .Select(x => new UserInfo { UserName = x.Key.CreatingUserName, Amount = x.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) });

            table = new DataTable("UserWiseOrders");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("UserWiseSalesOrders"), new DataColumn("Value"), new DataColumn("Perc") });

            foreach (var ownerInfo in owners.OrderBy(x => x.UserName))
            {
                table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00"), "%" + (ownerInfo.Amount * 100 / owners.Sum(x => x.Amount)).ToString("0.00") });
            }
            ds.Tables.Add(table);

            owners = oTickets.GroupBy(x => new { x.UserString })
                .Select(x => new UserInfo { UserName = x.Key.UserString, Amount = x.Sum(y => y.TotalAmount) });

            table = new DataTable("UserWiseTickets");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("UserWiseTicketsTotal"), new DataColumn("Value"), new DataColumn("Perc") });

            foreach (var ownerInfo in owners.OrderBy(x => x.UserName))
            {
                table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00"), "%" + (ownerInfo.Amount * 100 / owners.Sum(x => x.Amount)).ToString("0.00") });
            }
            ds.Tables.Add(table);

            var refundOwners = oTickets.SelectMany(ticket => ticket.Orders.Where(x => x.IncreaseInventory).Select(order => new { Ticket = ticket, Order = order }))
                .GroupBy(x => new { x.Order.CreatingUserName })
                .Select(x => new UserInfo { UserName = x.Key.CreatingUserName, Amount = x.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) }).ToList();

            table = new DataTable("RefundOwners");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("UserReturns"), new DataColumn("Value") });
            if (refundOwners.Any())
            {
                //table = new DataTable("RefundOwners");
                //table.Columns.AddRange(new DataColumn[] { new DataColumn("UserReturns"), new DataColumn("Value") });                    

                foreach (var ownerInfo in refundOwners)
                {
                    table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00") });
                }
                //ds.Tables.Add(table);
            }
            ds.Tables.Add(table);
            IEnumerable<MenuItem> oMenuItems = GetMenuItems();
            var menuGroups = MenuGroupBuilder.CalculateMenuGroups(oTickets, oMenuItems).ToList();


            table = new DataTable("ItemSales");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("ItemSales"), new DataColumn("Perc"), new DataColumn("Amount") });

            foreach (var menuItemInfo in menuGroups)
            {
                table.Rows.Add(new object[] { menuItemInfo.GroupName,
                    string.Format("%{0:0.00}", menuItemInfo.Rate),
                    menuItemInfo.AmountWithOrderTag.ToString("0.00")});
            }

            table.Rows.Add(new object[] { "Total", "", menuGroups.Sum(x => x.AmountWithOrderTag).ToString("0.00") });
            //return report.Document;
            ds.Tables.Add(table);
            return ds;
        }
        public static DataSet GetReportSP(DateTime fromDate, DateTime toDate, string DepartmentIds, int OutletId, bool IsShort)
        {
            DataSet ds = new DataSet();
            DataTable table = GetSalesCalculationTable(Convert.ToString(OutletId), DepartmentIds, fromDate, toDate);
            ds.Tables.Add(table);
            table = GetWorkperiodReprotRefunds(Convert.ToString(OutletId), DepartmentIds, fromDate, toDate);
            ds.Tables.Add(table);
            table = GetWorkperiodReprotPayments(Convert.ToString(OutletId), DepartmentIds, fromDate, toDate);
            ds.Tables.Add(table);
            table = GetWorkperiodReprotCustomerDueAdvance(Convert.ToString(OutletId), DepartmentIds, fromDate, toDate);
            ds.Tables.Add(table);
            ds.Tables.Add(new DataTable("DepartmentWise"));
            ds.Tables.Add(new DataTable("Refund"));
            ds.Tables.Add(new DataTable("GeneralInfo"));
            ds.Tables.Add(new DataTable("Orders"));
            ds.Tables.Add(new DataTable("TicketGroupInfo"));
            ds.Tables.Add(new DataTable("UserWiseOrders"));
            ds.Tables.Add(new DataTable("UserWiseTickets"));
            ds.Tables.Add(new DataTable("RefundOwners"));
            ds.Tables.Add(new DataTable("ItemSales"));
            return ds;

            #region Work Later
            //table = new DataTable("DepartmentWise");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("DepartmentName"), new DataColumn("Amount") });
            //IEnumerable<Department> oDepartments = GetDepartmentCollection();
            //var departmentWiseSales = MenuGroupBuilder.CalculateDepartmentWiseSales(oTickets, GetDepartmentCollection()).OrderByDescending(x => x.Name);
            //IList<DepartmentItemInfo> DeprtmentSalesInfos = new List<DepartmentItemInfo>();
            //foreach (var departmentwisesales in departmentWiseSales)
            //{
            //    var s = oDepartments.Where(y => y.Id == departmentwisesales.ID).Select(y => new { y.Id, y.Name });

            //    DepartmentItemInfo q = new DepartmentItemInfo
            //    {
            //        Name = s.First().Name,
            //        Amount = departmentwisesales.Amount
            //    };
            //    DeprtmentSalesInfos.Add(q);
            //}
            //foreach (var departmentSalesInfo in DeprtmentSalesInfos.OrderBy(l => l.Name))
            //{
            //    table.Rows.Add(new object[] { departmentSalesInfo.Name, departmentSalesInfo.Amount.ToString("0.00") });
            //    //report.AddRow("DepartmentWiseSales", departmentSalesInfo.Name, departmentSalesInfo.Amount.ToString(ReportContext.CurrencyFormat));
            //}
            //table.Rows.Add(new object[] { "Total", departmentWiseSales.Sum(x => x.Amount).ToString("0.00") });
            ////report.AddRow("DepartmentWiseSales", Resources.Total, departmentWiseSales.Sum(x => x.Amount).ToString(ReportContext.CurrencyFormat));
            //ds.Tables.Add(table);
            ///**/
            //table = new DataTable("Refund");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("RefundType"), new DataColumn("Amount") });
            //var refundCalculator = GetRefundCalculator(oTickets);
            //if (refundCalculator.TotalAmount != 0)
            //{
            //    foreach (var paymentName in refundCalculator.PaymentNames)
            //    {
            //        table.Rows.Add(new object[] { paymentName, refundCalculator.GetAmount(paymentName).ToString("0.00") });
            //    }
            //    table.Rows.Add(new object[] { "TOTAL REFUND", refundCalculator.TotalAmount.ToString("0.00") });
            //}
            //ds.Tables.Add(table);
            ////---------------
            //table = new DataTable("GeneralInfo");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
            //DataSet dtTicketType = GetTicketType();
            //var ticketGropus = oTickets
            //    .GroupBy(x => new { x.TicketTypeId })
            //    .Select(x => new TicketTypeInfo
            //    {
            //        TicketTypeId = x.Key.TicketTypeId,
            //        TicketCount = x.Count(),
            //        Amount = x.Sum(y => y.Sum),
            //        Tax = x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)),
            //        Services = x.Sum(y => y.PostTaxServicesTotal),
            //        TicketTypeName = dtTicketType.Tables[0].Select($"Id={x.Key.TicketTypeId}")[0]["Name"] == DBNull.Value ? "" : dtTicketType.Tables[0].Select($"Id={x.Key.TicketTypeId}")[0]["Name"].ToString()
            //    }).OrderBy(x => x.TicketTypeName).ToList();

            //var propertySum = oTickets
            //    .SelectMany(x => x.Orders)
            //    .Sum(x => x.GetOrderTagPrice() * x.Quantity);

            //var discounts = Math.Abs(oTickets.Sum(x => x.PreTaxServicesTotal));

            //table.Rows.Add(new object[] { "Extra Item Properties", propertySum.ToString("0.00") });
            //table.Rows.Add(new object[] { "Pre-Tax Services Total", discounts.ToString("0.00") });
            //ds.Tables.Add(table);
            ////-------------------------
            //table = new DataTable("Orders");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });

            //if (ticketGropus.Count() > 1)
            //{
            //    foreach (var TicketTypeInfo in ticketGropus)
            //    {
            //        table.Rows.Add(new object[] { TicketTypeInfo.TicketTypeName, TicketTypeInfo.TicketCount.ToString() });
            //        //report.AddRow("Bilgi", TicketTypeInfo.TicketTypeName, TicketTypeInfo.TicketCount.ToString());
            //    }
            //}

            //var orderStates = oTickets
            //                   .SelectMany(x => x.Orders)
            //                   .SelectMany(x => x.GetOrderStateValues()).Distinct().ToList();

            //if (orderStates.Any())
            //{
            //    table.Rows.Add(new object[] { "Orders", "" });

            //    foreach (var orderStateValue in orderStates)
            //    {
            //        if (orderStateValue.StateName.ToLower() == "gstatus")
            //        {
            //            var value = orderStateValue;
            //            var items =
            //                oTickets.SelectMany(x => x.Orders).Where(x => x.IsInState(value.StateName, value.State, value.StateValue)).ToList();
            //            var amount = items.Sum(x => x.GetValue());
            //            var count = items.Count();
            //            table.Rows.Add(new object[] { $"{orderStateValue.State}:{orderStateValue.StateValue} ({count})", amount.ToString("0.00") });
            //        }
            //    }
            //}



            //#region Ovi asked to remove unpaid/paid portion 27 nov 2015
            ////var ticketStates = ReportContext.Tickets
            ////    .SelectMany(x => x.GetTicketStateValues()).Distinct().ToList();

            ////if (ticketStates.Any())
            ////{
            ////    report.AddBoldRow("Bilgi", Resources.Tickets, "");
            ////    foreach (var ticketStateValue in ticketStates)
            ////    {
            ////        TicketStateValue value = ticketStateValue;
            ////        var items = ReportContext.Tickets.Where(x => x.IsInState(value.StateName, value.State)).ToList();
            ////        var amount = items.Sum(x => x.GetSum());
            ////        var count = items.Count();
            ////        report.AddRow("Bilgi", string.Format("{0} ({1})", ticketStateValue.State, count), amount.ToString(ReportContext.CurrencyFormat));
            ////    }
            ////} 
            //#endregion

            //var ticketCount = ticketGropus.Sum(x => x.TicketCount);

            //table.Rows.Add(new object[] { "Ticket Count", ticketCount.ToString() });
            //table.Rows.Add(new object[] { "Number Of Guests", oTickets.Sum(x => x.NoOfGuests).ToString() });


            //table.Rows.Add(new object[] {"Grand Sales/ Ticket", ticketCount > 0
            //    ? (ticketGropus.Sum(x => x.Amount) / ticketGropus.Sum(x => x.TicketCount)).ToString("0.00")
            //    : "0"});

            //table.Rows.Add(new object[] { "Sales/ Ticket", ticketCount > 0 ? (nGrandTotal / ticketGropus.Sum(x => x.TicketCount)).ToString("0.00") : "0" });

            //table.Rows.Add(new object[] {"Gross Sales/ Guest", oTickets.Sum(x => x.NoOfGuests) > 0
            //? (ticketGropus.Sum(x => x.Amount) / oTickets.Sum(x => x.NoOfGuests)).ToString("0.00") : "0"});
            //table.Rows.Add(new object[] { "Sales/ Guest", oTickets.Sum(x => x.NoOfGuests) > 0 ? (nGrandTotal / oTickets.Sum(x => x.NoOfGuests)).ToString("0.00") : "0" });
            //ds.Tables.Add(table);

            //table = new DataTable("TicketGroupInfo");
            //if (ticketGropus.Count() > 1)
            //{
            //    table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value"), new DataColumn("Value1"), new DataColumn("Value2") });
            //    foreach (var ticketTypeInfo in ticketGropus)
            //    {
            //        var dinfo = ticketTypeInfo;

            //        var groups = oTickets
            //            .Where(x => x.TicketTypeId == dinfo.TicketTypeId)
            //            .SelectMany(x => x.Payments)
            //            .GroupBy(x => new { x.Name })
            //            .Select(x => new TenderedAmount { PaymentName = x.Key.Name, Amount = x.Sum(y => y.Amount) });

            //        var ticketTypeAmountCalculator = new AmountCalculator(groups);

            //        table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", string.Format("{0} Incomes", ticketTypeInfo.TicketTypeName), "", "" });

            //        foreach (var paymentName in ticketTypeAmountCalculator.PaymentNames)
            //        {
            //            table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", paymentName, ticketTypeAmountCalculator.GetPercent(paymentName), ticketTypeAmountCalculator.GetAmount(paymentName).ToString("0.00") });
            //        }

            //        table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", " Total Income", "", ticketTypeInfo.Amount.ToString("0.00") });

            //        var ddiscounts = oTickets
            //            .Where(x => x.TicketTypeId == dinfo.TicketTypeId)
            //            .Sum(x => x.PreTaxServicesTotal);

            //        ddiscounts = Math.Abs(ddiscounts);

            //        table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName + " Incomes", "Discounts Total", "", ddiscounts.ToString("0.00") });
            //    }
            //    //ds.Tables.Add(table);
            //}
            //ds.Tables.Add(table);

            ////--
            //#region Ticket Tag for further development
            ////if (oTickets.Select(x => x.GetTagData()).Where(x => !string.IsNullOrEmpty(x)).Distinct().Any())
            ////{
            ////    var dict = new Dictionary<string, List<Ticket>>();

            ////    foreach (var ticket in oTickets.Where(x => x.IsTagged))
            ////    {
            ////        foreach (var tag in ticket.GetTicketTagValues().Select(x => x.TagName + ":" + x.TagValue))
            ////        {
            ////            if (!dict.ContainsKey(tag))
            ////                dict.Add(tag, new List<Ticket>());
            ////            dict[tag].Add(ticket);
            ////        }
            ////    }

            ////    var tagGroups = dict.Select(x => new TicketTagInfo { Amount = x.Value.Sum(y => y.GetPlainSum()), TicketCount = x.Value.Count, TagName = x.Key }).OrderBy(x => x.TagName);

            ////    var tagGrp = tagGroups.GroupBy(x => x.TagName.Split(':')[0]);


            ////    table = new DataTable();
            ////    table.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
            ////    table.Rows.Add(new object[] { "Ticket Tags", "Detail-1", "Detail-2" });

            ////    foreach (var grp in tagGrp)
            ////    {
            ////        var grouping = grp;
            ////        var tag = ReportContext.TicketTagGroups.FirstOrDefault(x => x.Name == grouping.Key);
            ////        if (tag == null) continue;

            ////        report.AddBoldRow("Etiket", grp.Key, "", "");

            ////        if (tag.IsDecimal)
            ////        {
            ////            var tCount = grp.Sum(x => x.TicketCount);
            ////            var tSum = grp.Sum(x => Convert.ToDecimal(x.TagName.Split(':')[1]) * x.TicketCount);
            ////            var amnt = grp.Sum(x => x.Amount);
            ////            var rate = tSum / amnt;
            ////            report.AddRow("Etiket", string.Format(Resources.TotalAmount_f, tag.Name), "", tSum.ToString(ReportContext.CurrencyFormat));
            ////            report.AddRow("Etiket", Resources.TicketCount, "", tCount.ToString());
            ////            report.AddRow("Etiket", Resources.TicketTotal, "", amnt.ToString(ReportContext.CurrencyFormat));
            ////            report.AddRow("Etiket", Resources.Rate, "", rate.ToString("%#0.##"));
            ////            continue;
            ////        }

            ////        foreach (var ticketTagInfo in grp)
            ////        {
            ////            report.AddRow("Etiket",
            ////                ticketTagInfo.TagName.Split(':')[1],
            ////                ticketTagInfo.TicketCount.ToString(),
            ////                ticketTagInfo.Amount.ToString(ReportContext.CurrencyFormat));
            ////        }

            ////        var totalAmount = grp.Sum(x => x.Amount);
            ////        report.AddRow("Etiket", string.Format(Resources.TotalAmount_f, tag.Name), "", totalAmount.ToString(ReportContext.CurrencyFormat));

            ////        var sum = 0m;

            ////        if (tag.IsInteger)
            ////        {
            ////            try
            ////            {
            ////                sum = grp.Sum(x => Convert.ToDecimal(x.TagName.Split(':')[1]) * x.TicketCount);
            ////                report.AddRow("Etiket", string.Format(Resources.TicketTotal_f, tag.Name), "", sum.ToString("#,##.##"));
            ////            }
            ////            catch (FormatException)
            ////            {
            ////                report.AddRow("Etiket", string.Format(Resources.TicketTotal_f, tag.Name), "", "#Hata!");
            ////            }
            ////        }
            ////        else
            ////        {
            ////            sum = grp.Sum(x => x.TicketCount);
            ////        }

            ////        if (sum > 0)
            ////        {
            ////            var average = totalAmount / sum;
            ////            report.AddRow("Etiket", string.Format(Resources.TotalAmountDivTag_f, tag.Name), "", average.ToString(ReportContext.CurrencyFormat));
            ////        }
            ////    }
            ////} 
            //#endregion

            //////----

            //var owners = oTickets.SelectMany(ticket => ticket.Orders.Where(x => !x.IncreaseInventory).Select(order => new { Ticket = ticket, Order = order }))
            //    .GroupBy(x => new { x.Order.CreatingUserName })
            //    .Select(x => new UserInfo { UserName = x.Key.CreatingUserName, Amount = x.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) });

            //table = new DataTable("UserWiseOrders");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("UserWiseSalesOrders"), new DataColumn("Value"), new DataColumn("Perc") });

            //foreach (var ownerInfo in owners.OrderBy(x => x.UserName))
            //{
            //    table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00"), "%" + (ownerInfo.Amount * 100 / owners.Sum(x => x.Amount)).ToString("0.00") });
            //}
            //ds.Tables.Add(table);

            //owners = oTickets.GroupBy(x => new { x.UserString })
            //    .Select(x => new UserInfo { UserName = x.Key.UserString, Amount = x.Sum(y => y.TotalAmount) });

            //table = new DataTable("UserWiseTickets");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("UserWiseTicketsTotal"), new DataColumn("Value"), new DataColumn("Perc") });

            //foreach (var ownerInfo in owners.OrderBy(x => x.UserName))
            //{
            //    table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00"), "%" + (ownerInfo.Amount * 100 / owners.Sum(x => x.Amount)).ToString("0.00") });
            //}
            //ds.Tables.Add(table);

            //var refundOwners = oTickets.SelectMany(ticket => ticket.Orders.Where(x => x.IncreaseInventory).Select(order => new { Ticket = ticket, Order = order }))
            //    .GroupBy(x => new { x.Order.CreatingUserName })
            //    .Select(x => new UserInfo { UserName = x.Key.CreatingUserName, Amount = x.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) }).ToList();

            //table = new DataTable("RefundOwners");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("UserReturns"), new DataColumn("Value") });
            //if (refundOwners.Any())
            //{
            //    //table = new DataTable("RefundOwners");
            //    //table.Columns.AddRange(new DataColumn[] { new DataColumn("UserReturns"), new DataColumn("Value") });                    

            //    foreach (var ownerInfo in refundOwners)
            //    {
            //        table.Rows.Add(new object[] { ownerInfo.UserName, ownerInfo.Amount.ToString("0.00") });
            //    }
            //    //ds.Tables.Add(table);
            //}
            //ds.Tables.Add(table);
            //IEnumerable<MenuItem> oMenuItems = GetMenuItems();
            //var menuGroups = MenuGroupBuilder.CalculateMenuGroups(oTickets, oMenuItems).ToList();


            //table = new DataTable("ItemSales");
            //table.Columns.AddRange(new DataColumn[] { new DataColumn("ItemSales"), new DataColumn("Perc"), new DataColumn("Amount") });

            //foreach (var menuItemInfo in menuGroups)
            //{
            //    table.Rows.Add(new object[] { menuItemInfo.GroupName,
            //        string.Format("%{0:0.00}", menuItemInfo.Rate),
            //        menuItemInfo.AmountWithOrderTag.ToString("0.00")});
            //}

            //table.Rows.Add(new object[] { "Total", "", menuGroups.Sum(x => x.AmountWithOrderTag).ToString("0.00") });
            ////return report.Document;
            //ds.Tables.Add(table);
            //return ds; 
            #endregion
        }
        public static string GetDataFetchFrom()
        {
            return DBUtility.GetDataFetchFrom();
        }

        //ok
        public static DataTable GetSalesCalculationTable(string OutletId, string DepartmentIds, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetWorkperiodReport_Sales_Calculations]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    SqlParameter paramDepartmentIds = new SqlParameter();
                    paramDepartmentIds.ParameterName = "@Departments";
                    paramDepartmentIds.SqlDbType = SqlDbType.NVarChar;
                    paramDepartmentIds.Value = DepartmentIds;
                    cmd.Parameters.Add(paramDepartmentIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "Sales";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        //ok
        public static DataSet GetMenuGroupAccountDetails(string OutletId, string DepartmentIds, DateTime fromDate, DateTime toDate)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {

                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetMenuGroupAccountDetailsForLegacyReportingTool]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    SqlParameter paramDepartmentIds = new SqlParameter();
                    paramDepartmentIds.ParameterName = "@Departments";
                    paramDepartmentIds.SqlDbType = SqlDbType.NVarChar;
                    paramDepartmentIds.Value = DepartmentIds;
                    cmd.Parameters.Add(paramDepartmentIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "Sales";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            ds = GetSumItemSalesAccountWiseGroup(Convert.ToInt32(OutletId), DepartmentIds, fromDate, toDate);
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "SUMs";
            ds.Tables[1].TableName = "ItemSalesAccountWise";

            return ds;
        }
        //ok
        public static DataSet GetMenuItemsAccountDetails(string OutletId, string DepartmentIds, DateTime fromDate, DateTime toDate)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {

                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetMenuItemAccountDetailsForLegacyReportingTool]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    SqlParameter paramDepartmentIds = new SqlParameter();
                    paramDepartmentIds.ParameterName = "@Departments";
                    paramDepartmentIds.SqlDbType = SqlDbType.NVarChar;
                    paramDepartmentIds.Value = DepartmentIds;
                    cmd.Parameters.Add(paramDepartmentIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "Sales";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            ds = GetSumItemSalesAccountWiseGroup(Convert.ToInt32(OutletId), DepartmentIds, fromDate, toDate);
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "SUMs";
            ds.Tables[1].TableName = "ItemSalesAccountWise";

            return ds;
        }
        //ok
        public static DataTable GetWorkperiodReprotRefunds(string OutletId, string DepartmentIds, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetWorkperiodReprotRefunds]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    SqlParameter paramDepartmentIds = new SqlParameter();
                    paramDepartmentIds.ParameterName = "@Departments";
                    paramDepartmentIds.SqlDbType = SqlDbType.NVarChar;
                    paramDepartmentIds.Value = DepartmentIds;
                    cmd.Parameters.Add(paramDepartmentIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "RefundTickets";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataTable GetWorkperiodReprotPayments(string OutletId, string DepartmentIds, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetWorkperiodReprotPayments]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    SqlParameter paramDepartmentIds = new SqlParameter();
                    paramDepartmentIds.ParameterName = "@Departments";
                    paramDepartmentIds.SqlDbType = SqlDbType.NVarChar;
                    paramDepartmentIds.Value = DepartmentIds;
                    cmd.Parameters.Add(paramDepartmentIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "Income";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        public static DataTable GetWorkperiodReprotCustomerDueAdvance(string OutletId, string DepartmentIds, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetWorkperiodReprotCustomerDueAdvance]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    SqlParameter paramDepartmentIds = new SqlParameter();
                    paramDepartmentIds.ParameterName = "@Departments";
                    paramDepartmentIds.SqlDbType = SqlDbType.NVarChar;
                    paramDepartmentIds.Value = DepartmentIds;
                    cmd.Parameters.Add(paramDepartmentIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "CustomerDue";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        //ok
        public static DataSet GetSalesSummaryReport(DateTime fromDate, DateTime toDate, string DepartmentIds, int OutletId)
        {
            DataSet _ds = new DataSet();
            List<Ticket> oTickets = GetTicketsFaster(OutletId, DepartmentIds, fromDate, toDate);
            IEnumerable<MenuItem> oMenuItems = GetMenuItems();
            IEnumerable<TaxTemplate> oTaxTemplates = GetObjectTaxTemplates();
            IEnumerable<CalculationType> oCalculationTypes = GetObjectCalculationTypes();
            IEnumerable<AccountTransactionType> oAccountTransactionTypes = GetAccountTransactionTypes();

            DataTable dtSalesSummary = new DataTable("SpecialSalesSummary");
            dtSalesSummary.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value"), new DataColumn("Value1"),
                new DataColumn("Value2"), new DataColumn("Value3"), new DataColumn("Value4"), new DataColumn("Value5"), new DataColumn("Value6") });
            DataTable dtDueAdvance = new DataTable("DueAdvance");
            dtDueAdvance.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
            DataTable dtIncomes = new DataTable("Incomes");
            dtIncomes.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value"), new DataColumn("Value1") });
            DataTable dtDiscounts = new DataTable("Discounts");
            dtDiscounts.Columns.AddRange(new DataColumn[] { new DataColumn("Info"), new DataColumn("Value") });
            //Tax Summary
            dtSalesSummary.Rows.Add(new object[] { "Item Name", "Quantity", "Net Sales", "SC", "SD", "VAT", "Sales Total", "%" });
            #region ForQuery
            //Taxes

            var menGroupsAccountsInfo = MenuGroupBuilder.CalculateMenuItemGroupAccountsInfo(oTickets, oMenuItems, oTaxTemplates).ToList();
            var menGroupsCalculationsInfo = MenuGroupBuilder.CalculateMenuItemGroupCalculationInfo(oTickets, oMenuItems, oCalculationTypes).ToList();
            var menGroupsSalesInfo = MenuGroupBuilder.CalculateSalesInfo(oTickets, oMenuItems);
            decimal CollectionSalesPurpose = 0;
            decimal nTotalOrderTotal = 0;
            decimal nSDTotal = 0;
            decimal nVATTotal = 0;
            decimal nGranTotal = 0;
            decimal nServiceChargeTotal = 0;

            menGroupsSalesInfo.ToList().ForEach(
                x =>
                {

                    var DiscountCalculations = menGroupsCalculationsInfo.Where(y => y.IsDiscount && y.GroupName == x.GroupName).ToList();
                    var nServiceCharge = menGroupsCalculationsInfo.Where(y => y.IsServiceCharge && y.GroupName == x.GroupName).Sum(z => z.Amount);
                    var nTotalDiscountGiven = menGroupsCalculationsInfo.Where(y => y.IsDiscount && y.GroupName == x.GroupName).Sum(z => z.Amount);
                    var nTotalTax = menGroupsAccountsInfo.Where(y => y.GroupName == x.GroupName).Sum(z => z.Amount) + menGroupsCalculationsInfo.Where(y => y.IsTax && !y.IsSD && y.GroupName == x.GroupName).Sum(z => z.Amount);
                    var nSD = menGroupsCalculationsInfo.Where(y => y.IsSD && y.GroupName == x.GroupName).Sum(z => z.Amount);
                    var nOrderTotal = menGroupsCalculationsInfo.Where(y => y.IsSD && y.GroupName == x.GroupName).Sum(z => z.Amount);
                    dtSalesSummary.Rows.Add(new object[] {
                          x.GroupName
                        , x.Quantity.ToString("0")
                        , (x.Amount + nTotalDiscountGiven).ToString("N2")
                        , nServiceCharge.ToString("N2")
                        , nSD.ToString("N2")
                        , nTotalTax.ToString("N2")
                        , (x.Amount+ nTotalDiscountGiven + nServiceCharge + nSD + nTotalTax).ToString("N2")
                        , (x.Amount/ menGroupsSalesInfo.Sum(q => q.Amount)*100).ToString("N2") + "%"
                    });

                    CollectionSalesPurpose += x.Amount + nTotalDiscountGiven + nTotalTax;
                    nTotalOrderTotal += x.Amount + nTotalDiscountGiven;
                    nSDTotal += nSD;
                    nServiceChargeTotal += nServiceCharge;
                    nVATTotal += nTotalTax;
                    nGranTotal += x.Amount + nTotalDiscountGiven + nServiceCharge + nSD + nTotalTax;
                });

            dtSalesSummary.Rows.Add(new object[] {
                    "Total"
                  , oTickets.Where(x => x.TotalAmount > 0).Count().ToString("0")
                  , nTotalOrderTotal.ToString("N2")
                  , nServiceChargeTotal.ToString("N2")
                  , nSDTotal.ToString("N2")
                  , nVATTotal.ToString("N2")
                  , nGranTotal.ToString("N2")
                  , "100%"
            });
            #endregion

            var customeradvancedulePayments = MenuGroupBuilder.CalculateCustomerAdvanceDues(oAccountTransactionTypes, oTickets.Select(x => x.TransactionDocument));
            decimal nCollectionSummary = CollectionSalesPurpose;
            dtDueAdvance.Rows.Add(new object[] { "Total Sales", CollectionSalesPurpose.ToString("N2") });
            foreach (var CashCardTran in customeradvancedulePayments)
            {
                nCollectionSummary += CashCardTran.Amount;
                dtDueAdvance.Rows.Add(new object[] { CashCardTran.Name, CashCardTran.Amount.ToString("N2") });
            }
            dtDueAdvance.Rows.Add(new object[] { "Total", nCollectionSummary.ToString("N2") });

            var incomeCalculator = GetIncomeCalculator(oTickets);

            //report.AddColumnLength("GelirlerTablosu", "45*", "Auto", "35*");
            //report.AddColumTextAlignment("GelirlerTablosu", TextAlignment.Left, TextAlignment.Right, TextAlignment.Right);
            //report.AddTable("GelirlerTablosu", Resources.Incomes, "", "");
            //dtIncomes = new DataTable("Income");
            //dtIncomes.Columns.AddRange(new DataColumn[] { new DataColumn("Incomes"), new DataColumn("Perc"),
            //            new DataColumn("Amount")});

            foreach (var paymentName in incomeCalculator.PaymentNames)
            {
                //report.AddRow("GelirlerTablosu", paymentName, incomeCalculator.GetPercent(paymentName), incomeCalculator.GetAmount(paymentName).ToString(ReportContext.CurrencyFormat));
                dtIncomes.Rows.Add(new object[] { paymentName, incomeCalculator.GetAmount(paymentName).ToString("N2"), incomeCalculator.GetPercent(paymentName) });
                //report.AddRow("GelirlerTablosu", Resources.TotalIncome.ToUpper(), "", incomeCalculator.TotalAmount.ToString(ReportContext.CurrencyFormat));
            }
            //report.AddRow("GelirlerTablosu", Resources.TotalIncome.ToUpper(), "", incomeCalculator.TotalAmount.ToString(ReportContext.CurrencyFormat));
            dtIncomes.Rows.Add(new object[] { "Total Income", incomeCalculator.TotalAmount.ToString("N2"), "" });


            var menuItemInfoGroups =
                from c in menGroupsCalculationsInfo
                group c by new { AccountHead = c.AccountHead, c.IsDiscount, c.IsTax } into grp
                select new MenuItemGroupAccountsInfo
                {
                    AccountHead = grp.Key.AccountHead,
                    IsDiscount = grp.Key.IsDiscount,
                    IsTax = grp.Key.IsTax,
                    Amount = grp.Sum(y => y.Amount)
                };
            var DiscountList = menuItemInfoGroups.Where(y => y.IsDiscount).ToList();
            foreach (var discount in DiscountList)
            {
                dtDiscounts.Rows.Add(new object[] { discount.AccountHead, (-1 * discount.Amount).ToString("N2") });
            }
            //
            var GiftOrders = oTickets.SelectMany(x => x.Orders).Where(x => x.DecreaseInventory && !x.CalculatePrice).ToList();
            var orderStates = GiftOrders.SelectMany(x => x.GetOrderStateValues()).Distinct().ToList();
            var giftOrderStates = orderStates.Where(x => x.State.Contains("Gift")).ToList();
            decimal nGiftValue = 0;
            if (orderStates.Any())
            {
                foreach (var orderStateValue in giftOrderStates)
                {
                    var value = orderStateValue;
                    var items =
                        GiftOrders.Where(x => value.State.Contains("Gift")
                                      && x.IsInState(value.StateName, value.State, value.StateValue)
                                  ).ToList();
                    var amount = items.Sum(x => x.GetValue());
                    nGiftValue += amount;
                    var count = items.Count();
                    if (amount > 0)
                    {
                        dtDiscounts.Rows.Add(new object[] { string.Format("{0} ({1}) ({2})", orderStateValue.State, count, value.StateValue), amount.ToString("N2") });
                    }
                }
            }
            //
            dtDiscounts.Rows.Add(new object[] { "Total", (nGiftValue + menGroupsCalculationsInfo.Where(x => x.IsDiscount).Sum(x => -1 * x.Amount)).ToString("N2") });

            _ds.Clear();
            _ds.Tables.Clear();
            _ds.Tables.Add(dtSalesSummary.Copy());
            _ds.Tables.Add(dtDueAdvance.Copy());
            _ds.Tables.Add(dtIncomes.Copy());
            _ds.Tables.Add(dtDiscounts.Copy());
            return _ds;
        }
        //ok
        public static DataSet GetSalesSummaryTaxReport(DateTime fromDate, DateTime toDate, string DepartmentIds, int OutletId)
        {
            DataSet _ds = new DataSet();
            List<Ticket> oTickets = GetTicketsFaster(OutletId, DepartmentIds, fromDate, toDate);
            IEnumerable<MenuItem> oMenuItems = GetMenuItems();
            IEnumerable<TaxTemplate> oTaxTemplates = GetObjectTaxTemplates();
            IEnumerable<CalculationType> oCalculationTypes = GetObjectCalculationTypes();
            IEnumerable<AccountTransactionType> oAccountTransactionTypes = GetAccountTransactionTypes();

            DataTable dtTaxSummary = new DataTable("SpecialSalesSummaryTAX");
            dtTaxSummary.Columns.AddRange(new DataColumn[] {
                  new DataColumn("Info")
                , new DataColumn("Value")
                , new DataColumn("Value1")
                , new DataColumn("Value2")
                , new DataColumn("Value3")
                , new DataColumn("Value4")
                , new DataColumn("Value5")
            });
            //Tax Summary
            if (oTickets.SelectMany(x => x.Calculations).Where(x => x.IsTax && !x.IsSD && x.CalculationAmount != 0).Count() > 0)
            {
                /*SD Inclusion*/
                var menGroupsCalculationsInfo = MenuGroupBuilder.CalculateTaxCalculationInfo(oTickets, oMenuItems, oCalculationTypes).ToList();
                dtTaxSummary.Rows.Add(new object[] {
                    "Particulars"
                    , "Tickets"
                    , "Net Sales"
                    , "SC"
                    , "SD"
                    , "VAT"
                    , "Sales Total"
                });
                decimal nTotalOrderTotal = 0;
                decimal nSDTotal = 0;
                decimal nServiceChargeTotal = 0;
                decimal nVATTotal = 0;
                decimal nGranTotal = 0;
                oTickets.SelectMany(x => x.Calculations).Where(x => x.IsTax && !x.IsSD && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                    x =>
                    {
                        var template = oCalculationTypes.FirstOrDefault(y => y.Id == x.Key);
                        var SDAmount = menGroupsCalculationsInfo.FirstOrDefault(y => y.AccountHead == template.Name).SDAmount;
                        var ServiceChargeAmount = menGroupsCalculationsInfo.FirstOrDefault(y => y.AccountHead == template.Name).ServiceChargeAmount;
                        var OrderAmount = menGroupsCalculationsInfo.FirstOrDefault(y => y.AccountHead == template.Name).OrderTotal;
                        var title = template != null ? template.Name : "[Undefined]";
                        dtTaxSummary.Rows.Add(new object[] {
                            title
                          , x.Count().ToString("0")
                          , (OrderAmount -ServiceChargeAmount - SDAmount).ToString("N2")
                          , ServiceChargeAmount.ToString("N2")
                          , SDAmount.ToString("N2")
                          , x.Sum(y => y.CalculationAmount).ToString("N2")
                          , (OrderAmount + x.Sum(y => y.CalculationAmount)).ToString("N2")
                        });
                        nTotalOrderTotal += OrderAmount - SDAmount - ServiceChargeAmount;
                        nSDTotal += SDAmount;
                        nServiceChargeTotal += ServiceChargeAmount;
                        nVATTotal += x.Sum(y => y.CalculationAmount);
                        nGranTotal += OrderAmount + x.Sum(y => y.CalculationAmount);
                    });
                dtTaxSummary.Rows.Add(new object[] {
                      "Total"
                    , oTickets.Where(x => x.TotalAmount > 0).Count().ToString("0")
                    , nTotalOrderTotal.ToString("N2")
                    , nServiceChargeTotal.ToString("N2")
                    , nSDTotal.ToString("N2")
                    , nVATTotal.ToString("N2")
                    , nGranTotal.ToString("N2") });
            }

            _ds.Clear();
            _ds.Tables.Clear();
            _ds.Tables.Add(dtTaxSummary.Copy());

            return _ds;
        }
        //ok
        public static DataSet GetItemSalesReportShort(DateTime fromDate, DateTime toDate, string DepartmentIds, int OutletId)
        {
            DataSet ds = new DataSet();
            List<Ticket> oTickets = GetTicketsFaster(OutletId, DepartmentIds, fromDate, toDate);
            IEnumerable<MenuItem> oMenuItems = GetMenuItems();
            var menuGroups = MenuGroupBuilder.CalculateMenuGroups(oTickets, oMenuItems).ToList();

            DataTable table = new DataTable("ItemSales");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("ItemSales"), new DataColumn("Perc"), new DataColumn("Amount") });

            foreach (var menuItemInfo in menuGroups)
            {
                table.Rows.Add(new object[] { menuItemInfo.GroupName,
                    string.Format("%{0:0.00}", menuItemInfo.Rate),
                    menuItemInfo.AmountWithOrderTag.ToString("0.00")});
            }

            table.Rows.Add(new object[] { "Total", "", menuGroups.Sum(x => x.AmountWithOrderTag).ToString("0.00") });
            //return report.Document;
            ds.Tables.Add(table);

            table = new DataTable("ItemSales_Quantity");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("ItemSales"), new DataColumn("Perc"), new DataColumn("Amount") });
            foreach (var menuItemInfo in menuGroups)
            {
                table.Rows.Add(new object[] { menuItemInfo.GroupName,
                    string.Format("%{0:0.00}", menuItemInfo.QuantityRate),
                    menuItemInfo.Quantity.ToString("0.00")});
            }

            table.Rows.Add(new object[] { "Total", "", menuGroups.Sum(x => x.Quantity).ToString("0.00") });

            ds.Tables.Add(table);

            var menuItems = MenuGroupBuilder.CalculateMenuItemsWithTimer(oTickets, oMenuItems).OrderByDescending(x => x.Quantity);

            table = new DataTable("ItemSales_Product");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("MenuItemId"), new DataColumn("GroupCode"), new DataColumn("MenuItemName"), new DataColumn("PortionName"), new DataColumn("UnitPrice"), new DataColumn("Quantity"), new DataColumn("Total"), new DataColumn("OutletName") });

            IList<MenuGroupItemInfo> Products = new List<MenuGroupItemInfo>();
            foreach (var menuItemInfo in menuItems)
            {
                /*
				public string GroupName { get; set; }
				public string ItemName { get; set; }
				public decimal Quantity { get; set; }
				public decimal Amount
				*/
                var s = oMenuItems.Where(y => y.Id == menuItemInfo.ID).Select(y => new { y.GroupCode, y.Name });

                MenuGroupItemInfo q = new MenuGroupItemInfo
                {
                    Quantity = menuItemInfo.Quantity,
                    GroupName = s.First().GroupCode,
                    ItemName = s.First().Name,
                    Portion = menuItemInfo.Portion,
                    Amount = menuItemInfo.Amount,
                    AmountWithOrderTag = menuItemInfo.AmountWithOrderTag,
                    TimeMins = menuItemInfo.TimeSpent
                };
                Products.Add(q);
            }
            foreach (var menuItemInfo in Products.OrderByDescending(l => l.Quantity))
            {
                table.Rows.Add(new object[] { "0", menuItemInfo.GroupName, menuItemInfo.ItemName, menuItemInfo.Portion, (menuItemInfo.AmountWithOrderTag / menuItemInfo.Quantity).ToString("0.00"), menuItemInfo.Quantity.ToString("0.00"), menuItemInfo.AmountWithOrderTag.ToString("0.00"), "" });
            }
            table.Rows.Add(new object[] { "Total", "", "", "", menuItems.Sum(x => x.AmountWithOrderTag).ToString("0.00") });
            ds.Tables.Add(table);
            var properties = oTickets
                .SelectMany(x => x.Orders.Where(y => !string.IsNullOrEmpty(y.OrderTags)))
                .SelectMany(x => x.GetOrderTagValues(y => y.MenuItemId == 0).Select(y => new { Name = y.TagValue, x.Quantity }))
                .GroupBy(x => new { x.Name })
                .Select(x => new { x.Key.Name, Quantity = x.Sum(y => y.Quantity) }).ToList();

            table = new DataTable("Properties");
            if (properties.Any())
            {

                table = new DataTable("Properties");
                table.Columns.AddRange(new DataColumn[] { new DataColumn("MenuItemId"), new DataColumn("GroupCode"), new DataColumn("MenuItemName"), new DataColumn("PortionName"), new DataColumn("UnitPrice"), new DataColumn("Quantity"), new DataColumn("Total"), new DataColumn("OutletName") });
                foreach (var property in properties.OrderByDescending(x => x.Quantity))
                {
                    table.Rows.Add(new object[] { "0", "", property.Name, "", "", property.Quantity.ToString("0.00"), "", "" });
                }

            }
            ds.Tables.Add(table);
            return ds;
        }
        public static DataTable CreateTicketTypeInfo(List<Ticket> actualTickets, IEnumerable<Ticket> tickets, ref decimal nGrandTotal, int OutletId)
        {
            DataSet dtTicketType = GetTicketType();
            DataSet dtCalculationType = GetCalculationTypes();
            DataSet dtTaxTemplate = GetTaxTemplates();
            double dVATDenominator = DBUtility.GetVATIncludingDenominator(OutletId);
            DataTable table = new DataTable();
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Sales"), new DataColumn("Net"), new DataColumn("Gross") });
            var ticketGropus = tickets
               .GroupBy(x => new { x.TicketTypeId })
               .Select(x => new TicketTypeInfo
               {
                   TicketTypeId = x.Key.TicketTypeId,
                   TicketCount = x.Count(),
                   AmountExcludingDiscount = x.Sum(y => y.GetSum()) - x.Sum(y => y.CalculateTax(y.GetPlainSum(), y.GetPreTaxServicesTotal())) + x.Sum(y => y.GetPostTaxDiscountServicesTotal()) + x.Sum(y => y.GetPreTaxDiscountServicesTotal()) - x.Sum(y => y.GetPostTaxServicesTotal()) - x.Sum(y => y.GetPreTaxServicesTotal()),
                   Amount = x.Sum(y => y.Sum) - x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)) - x.Sum(y => y.PostTaxServicesTotal) - x.Sum(y => y.PreTaxServicesTotal),
                   GrossAmount = x.Sum(y => y.PlainSumForEndOfPeriod),
                   Tax = x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)),
                   Services = x.Sum(y => y.PostTaxServicesTotal),
                   PreServices = x.Sum(y => y.PreTaxServicesTotal),
                   TicketTypeName = dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"] == DBNull.Value ? "" : dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"].ToString()
               }).OrderBy(x => x.TicketTypeName).ToList();


            /*{
				TicketTypeId = x.Key.TicketTypeId,
				TicketCount = x.Count(),
				Amount = x.Sum(y => y.GetSum()) - x.Sum(y => y.CalculateTax(y.GetPlainSum(), y.GetPreTaxServicesTotal())) - x.Sum(y => y.GetPostTaxServicesTotal()),
				Tax = x.Sum(y => y.CalculateTax(y.GetPlainSum(), y.GetPreTaxServicesTotal())),
				Services = x.Sum(y => y.GetPostTaxServicesTotal())
			  }*/
            //SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
            var nCalculateTax = tickets.Sum(x => x.CalculateTax(x.PlainSum, x.PreTaxServicesTotal));
            IEnumerable<Order> oTempOrders = actualTickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory));
            decimal TotalOfTocketGroups = ticketGropus.Sum(x => x.Amount);

            if (ticketGropus.Count() > 1)
            {
                foreach (var ticketTypeInfo in ticketGropus)
                {
                    table.Rows.Add(new object[] { ticketTypeInfo.TicketTypeName, ticketTypeInfo.Amount.ToString("0.00"), "%" + (ticketTypeInfo.Amount * 100 / TotalOfTocketGroups).ToString("0.00") });
                    //report.AddRow(rpKey, ticketTypeInfo.TicketTypeName,
                    //ticketTypeInfo.Amount.ToString(ReportContext.CurrencyFormat), "%" + (ticketTypeInfo.Amount * 100 / TotalOfTocketGroups).ToString("0.00"));
                }
            }
            //GrossAmount
            table.Rows.Add(new object[] { "Total", ticketGropus.Sum(x => x.Amount).ToString("0.00"), "%" + 100.ToString("0.00") });
            //report.AddRow(rpKey, string.Format(Resources.Total_f, header.ToUpper()).ToUpper(),
            //              ticketGropus.Sum(x => x.Amount).ToString(ReportContext.CurrencyFormat), oTempOrders.Sum(x => x.GetPlainTotal()).ToString(ReportContext.CurrencyFormat));

            var taxSum = ticketGropus.Sum(x => x.Tax);
            var serviceSum = ticketGropus.Sum(x => x.Services);
            var preServiceSum = ticketGropus.Sum(x => x.PreServices);
            bool hasDiscount = false;
            double SDTotal = 0;
            double DiscountTotal = 0;
            //if (taxSum == 0 && dVATDenominator > 0)
            //{
            //    var taxSumIncluding = (double)TotalOfTocketGroups / dVATDenominator;// ((double)TotalOfTicketGroups - (double)TotalOfTicketGroups / 1.15);
            //    table.Rows.Add(new object[] { "VAT TOTAL(VAT INCLUDED IN SALES)", taxSumIncluding.ToString("0.00").ToString(), "" });
            //}

            if (taxSum != 0 || serviceSum != 0 || preServiceSum != 0)
            {
                if (serviceSum != 0 && tickets.SelectMany(x => x.Calculations).Where(x => x.IncludeTax && x.IsDiscount).Count() > 0)
                {
                    hasDiscount = true;
                    tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                            var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                            table.Rows.Add(new object[] { title + " " + "(" + x.Count() + ")", x.Sum(y => y.CalculationAmount).ToString("0.00"), "" });
                            DiscountTotal = DiscountTotal + (double)x.Sum(y => y.CalculationAmount);
                        });
                }
                if (preServiceSum != 0 && tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && !x.IncludeTax).Count() > 0)
                {
                    hasDiscount = true;
                    tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && !x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                      x =>
                      {
                          var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                          var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                          table.Rows.Add(new object[] { title + " " + "(" + x.Count() + ")", x.Sum(y => y.CalculationAmount).ToString("0.00"), "" });

                      });
                }
                if (hasDiscount)
                {
                    table.Rows.Add(new object[] { "NET TOTAL", ticketGropus.Sum(x => x.AmountExcludingDiscount).ToString("0.00"), "" });
                }
                if (serviceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                            var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                            table.Rows.Add(new object[] { title + " " + "(" + x.Count() + ")", x.Sum(y => y.CalculationAmount).ToString("0.00"), "" });
                        });
                }

                if (serviceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            if (Convert.ToInt32(dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["IsSD"]) != 0)
                            {
                                var template = dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"].ToString();
                                var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"].ToString() : "[Undefined]";
                                SDTotal = SDTotal + Convert.ToDouble(x.Sum(y => y.CalculationAmount));
                            }
                        });
                }

                if (preServiceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && !x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                      x =>
                        {
                            var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                            var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                            table.Rows.Add(new object[] { title + " " + "(" + x.Count() + ")", x.Sum(y => y.CalculationAmount).ToString("0.00"), "" });
                        });
                }
                if (taxSum != 0)
                {
                    table.Rows.Add(new object[] { "SUB TOTAL", ticketGropus.Sum(x => x.Amount + x.Services + x.PreServices).ToString("0.00"), "" });

                    if (dtTaxTemplate.Tables[0].Rows.Count > 1)
                    {
                        foreach (DataRow taxTemplate in dtTaxTemplate.Tables[0].Rows)
                        {
                            if (taxTemplate["AccountTransactionType_Id"] != DBNull.Value || Convert.ToInt32(taxTemplate["AccountTransactionType_Id"]) != 0)
                            {
                                int AccountTransactionTypeID = Convert.ToInt32(taxTemplate["AccountTransactionType_Id"]);
                                string sTaxTemplateName = Convert.ToString(taxTemplate["Name"]);
                                var tax = tickets.Sum(x => x.GetTaxTotal(AccountTransactionTypeID, x.PreTaxServicesTotal, x.PlainSum));
                                table.Rows.Add(new object[] { sTaxTemplateName, tax.ToString("0.00"), "" });
                                //report.AddRow(rpKey, taxTemplate.Name, tax.ToString(ReportContext.CurrencyFormat));
                            }
                        }
                    }
                    table.Rows.Add(new object[] { "TAX TOTAL", taxSum.ToString("0.00"), "" });
                    //report.AddRow(rpKey, Resources.TaxTotal.ToUpper(), taxSum.ToString(ReportContext.CurrencyFormat));
                }

            }
            if (taxSum == 0 && dVATDenominator > 0)
            {
                var taxSumIncluding = ((double)TotalOfTocketGroups + DiscountTotal + SDTotal) / dVATDenominator;// ((double)TotalOfTicketGroups - (double)TotalOfTicketGroups / 1.15);
                table.Rows.Add(new object[] { "VAT TOTAL(VAT INCLUDED IN SALES)", taxSumIncluding.ToString("0.00").ToString(), "" });
            }
            if (taxSum != 0 && dVATDenominator > 0)
            {
                var nVATIncludingTAX = tickets.Sum(x => x.CalculateIncludingTax(dVATDenominator));
                //report.AddRow(rpKey, Resources.IncludedVATTotal.ToUpper(),
                //          nVATIncludingTAX.ToString(ReportContext.CurrencyFormat));
                if (nVATIncludingTAX > 0)
                {
                    table.Rows.Add(new object[] { "VAT TOTAL(VAT INCLUDED IN SALES): ", nVATIncludingTAX.ToString("0.00"), "" });
                }
            }
            table.Rows.Add(new object[] { "GRAND TOTAL", ticketGropus.Sum(x => x.Amount + x.Tax + x.Services + x.PreServices).ToString("0.00"), "" });
            //report.AddRow(rpKey, Resources.GrandTotal.ToUpper(),
            //              ticketGropus.Sum(x => x.Amount + x.Tax + x.Services).ToString(ReportContext.CurrencyFormat));
            nGrandTotal = ticketGropus.Sum(x => x.Amount);
            return table;
        }
        public static DataTable CreateTicketTypeInfoShort(List<Ticket> actualTickets, IEnumerable<Ticket> tickets, IEnumerable<Department> oDepartments, ref decimal nGrandTotal, ref decimal nGrossTotal, int OutletId)
        {
            DataSet dtTicketType = GetTicketType();
            DataSet dtCalculationType = GetCalculationTypes();
            DataSet dtTaxTemplate = GetTaxTemplates();
            double dVATDenominator = DBUtility.GetVATIncludingDenominator(OutletId);
            DataTable table = new DataTable();
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Sales"), new DataColumn("Net", typeof(decimal)),
                        new DataColumn("Gross"), new DataColumn("Sorting", typeof(int)) });
            var ticketGropus = tickets
               .GroupBy(x => new { x.TicketTypeId })
               .Select(x => new TicketTypeInfo
               {
                   TicketTypeId = x.Key.TicketTypeId,
                   TicketCount = x.Count(),
                   AmountExcludingDiscount = x.Sum(y => y.GetSum()) - x.Sum(y => y.CalculateTax(y.GetPlainSum(), y.GetPreTaxServicesTotal())) + x.Sum(y => y.GetPostTaxDiscountServicesTotal()) + x.Sum(y => y.GetPreTaxDiscountServicesTotal()) - x.Sum(y => y.GetPostTaxServicesTotal()) - x.Sum(y => y.GetPreTaxServicesTotal()),
                   Amount = x.Sum(y => y.Sum) - x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)) - x.Sum(y => y.PostTaxServicesTotal) - x.Sum(y => y.PreTaxServicesTotal),
                   GrossAmount = x.Sum(y => y.PlainSumForEndOfPeriod),
                   Tax = x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)),
                   Services = x.Sum(y => y.PostTaxServicesTotal),
                   PreServices = x.Sum(y => y.PreTaxServicesTotal),
                   TicketTypeName = dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"] == DBNull.Value ? "" : dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"].ToString()
               }).OrderBy(x => x.TicketTypeName).ToList();

            var ticketDepartmentWiseSales = from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                                            join department in oDepartments on c.Order.DepartmentId equals department.Id
                                            group c by new { department.Id, department.Name, department.Tag } into grp
                                            select new DepartmentWiseSellInfo { ID = grp.Key.Id, Name = grp.Key.Tag, Amount = grp.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) };
            /*{
				TicketTypeId = x.Key.TicketTypeId,
				TicketCount = x.Count(),
				Amount = x.Sum(y => y.GetSum()) - x.Sum(y => y.CalculateTax(y.GetPlainSum(), y.GetPreTaxServicesTotal())) - x.Sum(y => y.GetPostTaxServicesTotal()),
				Tax = x.Sum(y => y.CalculateTax(y.GetPlainSum(), y.GetPreTaxServicesTotal())),
				Services = x.Sum(y => y.GetPostTaxServicesTotal())
			  }*/
            //SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
            var nCalculateTax = tickets.Sum(x => x.CalculateTax(x.PlainSum, x.PreTaxServicesTotal));
            IEnumerable<Order> oTempOrders = actualTickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory));
            decimal TotalOfTocketGroups = ticketGropus.Sum(x => x.Amount);

            if (ticketDepartmentWiseSales.Count() > 1)
            {
                foreach (var ticketTypeInfo in ticketDepartmentWiseSales)
                {
                    table.Rows.Add(new object[] { ticketTypeInfo.Name, ticketTypeInfo.Amount.ToString("0.00"), "%" + (ticketTypeInfo.Amount * 100 / TotalOfTocketGroups).ToString("0.00"), 10 });
                    //report.AddRow(rpKey, ticketTypeInfo.TicketTypeName,
                    //ticketTypeInfo.Amount.ToString(ReportContext.CurrencyFormat), "%" + (ticketTypeInfo.Amount * 100 / TotalOfTocketGroups).ToString("0.00"));
                }
            }
            //GrossAmount
            table.Rows.Add(new object[] { "Sales Total", ticketGropus.Sum(x => x.Amount).ToString("0.00"), "", 20 });
            var SalesTotal = ticketGropus.Sum(x => x.Amount);
            //report.AddRow(rpKey, string.Format(Resources.Total_f, header.ToUpper()).ToUpper(),
            //              ticketGropus.Sum(x => x.Amount).ToString(ReportContext.CurrencyFormat), oTempOrders.Sum(x => x.GetPlainTotal()).ToString(ReportContext.CurrencyFormat));
            //nGrandTotal = Convert.ToDouble(ticketGropus.Sum(x => x.Amount));
            var taxSum = ticketGropus.Sum(x => x.Tax);
            var serviceSum = ticketGropus.Sum(x => x.Services);
            var preServiceSum = ticketGropus.Sum(x => x.PreServices);
            double SDTotal = 0;
            double DiscountTotal = 0;

            if (taxSum != 0 || serviceSum != 0 || preServiceSum != 0)
            {
                if (serviceSum != 0 && tickets.SelectMany(x => x.Calculations).Where(x => x.IncludeTax && x.IsDiscount).Count() > 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                            var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                            table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 30 });
                            DiscountTotal = DiscountTotal + (double)x.Sum(y => y.CalculationAmount);
                        });
                }
                if (preServiceSum != 0 && tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && !x.IncludeTax).Count() > 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && !x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                      x =>
                      {
                          var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                          var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                          table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 30 });
                      });
                }
                //if (hasDiscount)
                //table.Rows.Add(new object[] { "NET TOTAL", ticketGropus.Sum(x => x.AmountExcludingDiscount).ToString("0.00"), "" });
                if (serviceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                            var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                            table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 40 });
                        });
                }
                if (serviceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            if (Convert.ToInt32(dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["IsSD"]) != 0)
                            {
                                var template = dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"].ToString();
                                var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"].ToString() : "[Undefined]";
                                SDTotal = SDTotal + Convert.ToDouble(x.Sum(y => y.CalculationAmount));
                            }
                        });
                }

                if (preServiceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && !x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                      x =>
                      {
                          var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                          var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                          //table.Rows.Add(new object[] { title + " " + "(" + x.Count() + ")", x.Sum(y => y.CalculationAmount).ToString("0.00"), "" });
                          table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 50 });
                      });
                }
                if (taxSum != 0)
                {
                    //table.Rows.Add(new object[] { "SUB TOTAL", ticketGropus.Sum(x => x.Amount + x.Services + x.PreServices).ToString("0.00"), "" });

                    if (dtTaxTemplate.Tables[0].Rows.Count > 1)
                    {
                        foreach (DataRow taxTemplate in dtTaxTemplate.Tables[0].Rows)
                        {
                            if (taxTemplate["AccountTransactionType_Id"] != DBNull.Value || Convert.ToInt32(taxTemplate["AccountTransactionType_Id"]) != 0)
                            {
                                int AccountTransactionTypeID = Convert.ToInt32(taxTemplate["AccountTransactionType_Id"]);
                                string sTaxTemplateName = Convert.ToString(taxTemplate["Name"]);
                                var tax = tickets.Sum(x => x.GetTaxTotal(AccountTransactionTypeID, x.PreTaxServicesTotal, x.PlainSum));
                                table.Rows.Add(new object[] { sTaxTemplateName, tax.ToString("0.00"), "", 60 });
                                //report.AddRow(rpKey, taxTemplate.Name, tax.ToString(ReportContext.CurrencyFormat));
                            }
                        }
                    }
                    //table.Rows.Add(new object[] { "TAX TOTAL", taxSum.ToString("0.00"), "" });
                    //report.AddRow(rpKey, Resources.TaxTotal.ToUpper(), taxSum.ToString(ReportContext.CurrencyFormat));
                }
                //table.Rows.Add(new object[] { "Gross TOTAL", ticketGropus.Sum(x => x.Amount + x.Tax + x.Services + x.PreServices).ToString("0.00"), "" , 1});
                //report.AddRow(rpKey, Resources.GrandTotal.ToUpper(),
                //              ticketGropus.Sum(x => x.Amount + x.Tax + x.Services).ToString(ReportContext.CurrencyFormat));
                //nGrandTotal = ticketGropus.Sum(x => x.Amount);

                //if (SDTotal > 0)
                //{
                //    table.Rows.Add(new object[] { "SD", SDTotal.ToString("0.00"), SDTotal.ToString("0.00") });
                //}
                //double taxSumIncluding = 0;
                //if (dVATDenominator > 0)
                //{
                //    taxSumIncluding = ((double)TotalOfTocketGroups-DiscountTotal+SDTotal) / dVATDenominator;// ((double)TotalOfTicketGroups - (double)TotalOfTicketGroups / 1.15);
                //    table.Rows.Add(new object[] { "VAT TOTAL(VAT INCLUDED IN SALES)", taxSumIncluding.ToString("0.00").ToString(), "", 3 });
                //}
                //table.Rows.Add(new object[] { "Sales (Net)", (TotalOfTocketGroups -(decimal)DiscountTotal - (decimal)taxSumIncluding).ToString("0.00"), oTempOrders.Sum(x => x.PlainTotal).ToString("0.00"), 4 });
                //nGrandTotal = (TotalOfTocketGroups - (decimal)DiscountTotal - (decimal)taxSumIncluding);
                //if (taxSum != 0 && dVATDenominator > 0)
                //{
                //    var nVATIncludingTAX = tickets.Sum(x => x.CalculateIncludingTax(dVATDenominator));
                //    //report.AddRow(rpKey, Resources.IncludedVATTotal.ToUpper(),
                //    //          nVATIncludingTAX.ToString(ReportContext.CurrencyFormat));
                //    if (nVATIncludingTAX > 0)
                //        table.Rows.Add(new object[] { string.Format("VAT TOTAL(VAT INCLUDED IN SALES):"), nVATIncludingTAX.ToString("0.00"), "", "", 3 });
                //}

            }

            double taxSumIncluding = 0;
            if (dVATDenominator > 0)
            {
                taxSumIncluding = ((double)TotalOfTocketGroups + DiscountTotal + SDTotal) / dVATDenominator;// ((double)TotalOfTicketGroups - (double)TotalOfTicketGroups / 1.15);
                table.Rows.Add(new object[] { "VAT TOTAL(VAT INCLUDED IN SALES)", taxSumIncluding.ToString("0.00").ToString(), "", 70 });
            }

            nGrandTotal = TotalOfTocketGroups + (decimal)DiscountTotal - (decimal)taxSumIncluding;
            nGrossTotal = ticketGropus.Sum(x => x.Amount + x.Tax + x.Services + x.PreServices);
            table.Rows.Add(new object[] { "Net Total", (TotalOfTocketGroups - (decimal)DiscountTotal - (decimal)taxSumIncluding).ToString("0.00"), oTempOrders.Sum(x => x.PlainTotal).ToString("0.00"), 90 });
            table.Rows.Add(new object[] { "Gross Total", nGrossTotal.ToString("0.00"), "", 80 });

            return table;
        }
        public static DataTable CreateTicketTypeInfoShortOptimized(DataSet dtCalculationType, DataSet dtTicketType, DataSet dtTaxTemplate, List<Ticket> actualTickets, IEnumerable<Ticket> tickets, IEnumerable<Department> oDepartments, ref decimal nGrandTotal, ref decimal nGrossTotal, int OutletId)
        {
            double dVATDenominator = DBUtility.GetVATIncludingDenominator(OutletId);
            DataTable table = new DataTable();
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Sales"), new DataColumn("Net", typeof(decimal)),
                        new DataColumn("Gross"), new DataColumn("Sorting", typeof(int)) });
            var ticketGropus = tickets
               .GroupBy(x => new { x.TicketTypeId })
               .Select(x => new TicketTypeInfo
               {
                   TicketTypeId = x.Key.TicketTypeId,
                   TicketCount = x.Count(),
                   AmountExcludingDiscount = x.Sum(y => y.GetSum()) - x.Sum(y => y.CalculateTax(y.GetPlainSum(), y.GetPreTaxServicesTotal())) + x.Sum(y => y.GetPostTaxDiscountServicesTotal()) + x.Sum(y => y.GetPreTaxDiscountServicesTotal()) - x.Sum(y => y.GetPostTaxServicesTotal()) - x.Sum(y => y.GetPreTaxServicesTotal()),
                   Amount = x.Sum(y => y.Sum) - x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)) - x.Sum(y => y.PostTaxServicesTotal) - x.Sum(y => y.PreTaxServicesTotal),
                   GrossAmount = x.Sum(y => y.PlainSumForEndOfPeriod),
                   Tax = x.Sum(y => y.CalculateTax(y.PlainSum, y.PreTaxServicesTotal)),
                   Services = x.Sum(y => y.PostTaxServicesTotal),
                   PreServices = x.Sum(y => y.PreTaxServicesTotal),
                   TicketTypeName = dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"] == DBNull.Value ? "" : dtTicketType.Tables[0].Select(string.Format("Id={0}", x.Key.TicketTypeId))[0]["Name"].ToString()
               }).OrderBy(x => x.TicketTypeName).ToList();

            var ticketDepartmentWiseSales = from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                                            join department in oDepartments on c.Order.DepartmentId equals department.Id
                                            group c by new { department.Id, department.Name, department.Tag } into grp
                                            select new DepartmentWiseSellInfo { ID = grp.Key.Id, Name = grp.Key.Tag, Amount = grp.Sum(y => MenuGroupBuilder.CalculateOrderTotal(y.Ticket, y.Order)) };
            var nCalculateTax = tickets.Sum(x => x.CalculateTax(x.PlainSum, x.PreTaxServicesTotal));
            IEnumerable<Order> oTempOrders = actualTickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory));
            decimal TotalOfTocketGroups = ticketGropus.Sum(x => x.Amount);

            if (ticketDepartmentWiseSales.Count() > 1)
            {
                foreach (var ticketTypeInfo in ticketDepartmentWiseSales)
                {
                    table.Rows.Add(new object[] { ticketTypeInfo.Name, ticketTypeInfo.Amount.ToString("0.00"), "%" + (ticketTypeInfo.Amount * 100 / TotalOfTocketGroups).ToString("0.00"), 10 });
                }
            }
            table.Rows.Add(new object[] { "Sales Total", ticketGropus.Sum(x => x.Amount).ToString("0.00"), "", 20 });
            var SalesTotal = ticketGropus.Sum(x => x.Amount);
            var taxSum = ticketGropus.Sum(x => x.Tax);
            var serviceSum = ticketGropus.Sum(x => x.Services);
            var preServiceSum = ticketGropus.Sum(x => x.PreServices);
            double SDTotal = 0;
            double DiscountTotal = 0;

            if (taxSum != 0 || serviceSum != 0 || preServiceSum != 0)
            {
                if (serviceSum != 0 && tickets.SelectMany(x => x.Calculations).Where(x => x.IncludeTax && x.IsDiscount).Count() > 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                            var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                            table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 30 });
                            DiscountTotal = DiscountTotal + (double)x.Sum(y => y.CalculationAmount);
                        });
                }
                if (preServiceSum != 0 && tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && !x.IncludeTax).Count() > 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => x.IsDiscount && !x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                      x =>
                      {
                          var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                          var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                          table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 30 });
                      });
                }

                if (serviceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                            var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                            table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 40 });
                        });
                }
                if (serviceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                        x =>
                        {
                            if (Convert.ToInt32(dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["IsSD"]) != 0)
                            {
                                var template = dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"].ToString();
                                var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0} and IsSD=1", x.Key))[0]["Name"].ToString() : "[Undefined]";
                                SDTotal = SDTotal + Convert.ToDouble(x.Sum(y => y.CalculationAmount));
                            }
                        });
                }

                if (preServiceSum != 0)
                {
                    tickets.SelectMany(x => x.Calculations).Where(x => !x.IsDiscount && !x.IncludeTax && x.CalculationAmount != 0).GroupBy(x => x.CalculationTypeId).ToList().ForEach(
                      x =>
                      {
                          var template = dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString();
                          var title = template != "" ? dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"] == DBNull.Value ? "" : dtCalculationType.Tables[0].Select(string.Format("Id={0}", x.Key))[0]["Name"].ToString() : "[Undefined]";
                          table.Rows.Add(new object[] { title, x.Sum(y => y.CalculationAmount).ToString("0.00"), "", 50 });
                      });
                }
                if (taxSum != 0)
                {
                    if (dtTaxTemplate.Tables[0].Rows.Count > 1)
                    {
                        foreach (DataRow taxTemplate in dtTaxTemplate.Tables[0].Rows)
                        {
                            if (taxTemplate["AccountTransactionType_Id"] != DBNull.Value || Convert.ToInt32(taxTemplate["AccountTransactionType_Id"]) != 0)
                            {
                                int AccountTransactionTypeID = Convert.ToInt32(taxTemplate["AccountTransactionType_Id"]);
                                string sTaxTemplateName = Convert.ToString(taxTemplate["Name"]);
                                var tax = tickets.Sum(x => x.GetTaxTotal(AccountTransactionTypeID, x.PreTaxServicesTotal, x.PlainSum));
                                table.Rows.Add(new object[] { sTaxTemplateName, tax.ToString("0.00"), "", 60 });
                            }
                        }
                    }
                }
            }

            double taxSumIncluding = 0;
            if (dVATDenominator > 0)
            {
                taxSumIncluding = ((double)TotalOfTocketGroups + DiscountTotal + SDTotal) / dVATDenominator;// ((double)TotalOfTicketGroups - (double)TotalOfTicketGroups / 1.15);
                table.Rows.Add(new object[] { "VAT TOTAL(VAT INCLUDED IN SALES)", taxSumIncluding.ToString("0.00").ToString(), "", 70 });
            }

            nGrandTotal = TotalOfTocketGroups + (decimal)DiscountTotal - (decimal)taxSumIncluding;
            nGrossTotal = ticketGropus.Sum(x => x.Amount + x.Tax + x.Services + x.PreServices);
            table.Rows.Add(new object[] { "Net Total", (TotalOfTocketGroups - (decimal)DiscountTotal - (decimal)taxSumIncluding).ToString("0.00"), oTempOrders.Sum(x => x.PlainTotal).ToString("0.00"), 90 });
            table.Rows.Add(new object[] { "Gross Total", nGrossTotal.ToString("0.00"), "", 80 });

            return table;
        }
        public static DataTable GetIncomes(List<Ticket> Tickets)
        {
            DataTable table = new DataTable("Incomes");
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Sales"),
                new DataColumn("Net", typeof(decimal)),
                new DataColumn("Gross"),
                new DataColumn("Sorting", typeof(int)) });
            var incomeCalculator = GetIncomeCalculator(Tickets);
            foreach (var paymentName in incomeCalculator.PaymentNames)
            {
                table.Rows.Add(new object[] { paymentName, incomeCalculator.GetAmount(paymentName).ToString("0.00"), "", 100 });
            }
            table.Rows.Add(new object[] { "Total Income", incomeCalculator.TotalAmount.ToString("0.00"), "", 110 });
            return table;
        }
        internal static AmountCalculator GetIncomeCalculator(List<Ticket> oTickets)
        {
            var groups = oTickets
                .SelectMany(x => x.Payments)
                //.Where(x => x.Amount >= 0) // Edited by Badhon : for Refund ke income theke baad diye grand total er sathe mialay deya hoise
                .GroupBy(x => x.PaymentTypeId)
                .Select(x => new TenderedAmount { PaymentName = GetPaymentTypeName(x.Key), Amount = x.Sum(y => y.Amount) });
            return new AmountCalculator(groups);
        }
        /*Work Period Report Purpose*/
        public static DataTable GetTicketsWithIncludingVAT(IEnumerable<Ticket> tickets, DataTable preTable, int OutletId)
        {
            DataSet dtTicketType = GetTicketType();
            DataSet dtCalculationType = GetCalculationTypes();
            DataSet dtTaxTemplate = GetTaxTemplates();
            double dVATDenominator = DBUtility.GetVATIncludingDenominator(OutletId);

            DataTable table = preTable.Copy();

            if (dVATDenominator > 0)
            {
                var ticketGropus = tickets
                       .GroupBy(x => new { x.Id, x.TicketNumber })
                       .Select(x => new TicketTypeInfo
                       {
                           TicketTypeId = x.Key.Id,
                           Tax = x.Sum(y => y.CalculateIncludingTax(dVATDenominator)),
                           TicketTypeName = x.Key.TicketNumber
                       }).OrderBy(x => x.TicketTypeName).ToList();

                foreach (var ticket in ticketGropus)
                {
                    DataTable ticketEntities = GetTicketEntities(ticket.TicketTypeId);
                    if (ticketEntities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in ticketEntities.Rows)
                        {
                            string entityTypeName = Convert.ToString(dr["EntityType"]);
                            string entityName = Convert.ToString(dr["EntityName"]);

                            DataRow[] row = preTable.Select(string.Format("TicketId = '{0}'", ticket.TicketTypeName));
                            table.Rows.Add(new object[] {  ticket.TicketTypeName,  Convert.ToDateTime(row[0]["TicketDate"]).ToString(), "Included VAT", Math.Round(ticket.Tax,4), Convert.ToDouble(row[0]["TotalAmount"]).ToString("N4"),
                        85, entityTypeName, entityName, Convert.ToInt32(row[0]["NoOfGuests"]),row[0]["SettledBy"].ToString(), row[0]["Note"].ToString() });
                        }
                    }
                    else
                    {
                        DataRow[] row = preTable.Select(string.Format("TicketId = '{0}'", ticket.TicketTypeName));
                        table.Rows.Add(new object[] { ticket.TicketTypeName,  Convert.ToDateTime(row[0]["TicketDate"]).ToString(), "Included VAT", Math.Round(ticket.Tax,2), Convert.ToDouble(row[0]["TotalAmount"]).ToString("N2"),
                    85,"No Entity Type", "No Entity Selected", row[0]["NoOfGuests"].ToString(),row[0]["SettledBy"].ToString(), row[0]["Note"].ToString() });
                    }
                }
            }
            return table;
        }

        public static DataTable GetOrders(Ticket ticket)
        {
            rptDataset.OrdersDataTable dTable = new rptDataset.OrdersDataTable();
            if (ticket != null)
            {
                string menuItem, orderStates;

                try
                {
                    foreach (Order ordr in ticket.Orders)
                    {
                        menuItem = string.Empty;
                        orderStates = string.Empty;

                        DataRow Rowbody = dTable.NewRow();
                        Rowbody["Quantity"] = ordr.Quantity;

                        menuItem = ordr.MenuItemName + System.Environment.NewLine;
                        foreach (OrderTagValue ordrTagValue in ordr.OrderTagValues)
                        {
                            menuItem += "," + ordrTagValue.TagValue;
                        }
                        Rowbody["MenuItem"] = menuItem;

                        foreach (OrderStateValue ordrStateValue in ordr.OrderStateValues)
                        {
                            if (string.IsNullOrEmpty(orderStates))
                                orderStates = ordrStateValue.State;
                            else
                                orderStates += "," + ordrStateValue.State;
                        }

                        Rowbody["CreatingUserName"] = ordr.CreatingUserName;
                        Rowbody["CreatedDateTime"] = ordr.CreatedDateTime;
                        Rowbody["OrderStates"] = orderStates;
                        Rowbody["UnitPrice"] = ordr.GetVisiblePrice();
                        Rowbody["Price"] = ordr.CalculatePrice ? ordr.GetVisiblePrice() * ordr.Quantity : 0;
                        Rowbody["UnitProductionCost"] = ordr.CalculatePrice ? ordr.UnitProductionCost : 0;
                        DataTable dtProductionCostFixed = MenuItemManager.ProductionCostFixed(ordr.MenuItemId, ordr.PortionName);
                        Rowbody["FixedProductionCost"] = dtProductionCostFixed.Rows.Count > 0 && ordr.CalculatePrice ? Convert.ToDecimal(dtProductionCostFixed.Rows[0]["FixedProductionCost"]) : 0;
                        Rowbody["Time"] = GetTime(ordr);

                        dTable.Rows.Add(Rowbody);
                    }
                }
                catch (Exception ex)
                { throw new Exception(ex.Message); }
            }
            return dTable;
        }

        private static string GetTime(Order ordr)
        {
            string time = string.Empty;
            if (ordr.ProductTimerValue != null)
            {
                string TimethresholdHour = ConfigurationManager.AppSettings["TimethresholdHour"];
                string TimethresholdMinute = ConfigurationManager.AppSettings["TimethresholdMinute"];
                int hoursToAdd = 0;
                int MinutesToAdd = 0;
                int.TryParse(TimethresholdHour, out hoursToAdd);
                int.TryParse(TimethresholdMinute, out MinutesToAdd);

                DateTime end = ordr.ProductTimerValue.End != ordr.ProductTimerValue.Start ? ordr.ProductTimerValue.End : DateTime.Now.AddHours(hoursToAdd).AddMinutes(MinutesToAdd);
                time = ordr.ProductTimerValue.Start.ToString("hh:mm tt") + " -" + end.ToString("hh:mm tt") + " (" + ordr.ProductTimerValue.GetTime() + ")";
            }
            return time;
        }

        public static DataTable GetCalculations(Ticket ticket)
        {
            rptDataset.CalculationsDataTable dTable = new rptDataset.CalculationsDataTable();
            if (ticket != null)
            {
                foreach (Calculation obj in ticket.Calculations)
                {
                    if (obj.CalculationAmount == 0)
                        continue;
                    DataRow Rowbody = dTable.NewRow();

                    Rowbody["Name"] = obj.Name;
                    Rowbody["CalculationAmount"] = obj.CalculationAmount;

                    dTable.Rows.Add(Rowbody);
                }
            }
            return dTable;
        }

        public static DataTable GetPayments(Ticket ticket)
        {
            rptDataset.PaymentsDataTable dTable = new rptDataset.PaymentsDataTable();
            if (ticket != null)
            {
                foreach (Payment obj in ticket.Payments)
                {
                    DataRow Rowbody = dTable.NewRow();

                    Rowbody["Name"] = obj.Name;
                    Rowbody["Date"] = obj.Date;
                    Rowbody["Amount"] = obj.Amount;
                    dTable.Rows.Add(Rowbody);
                }
            }
            return dTable;
        }

        public static dynamic GetOrderTagService(List<Ticket> tickets)
        {
            var properties = tickets
                .SelectMany(x => x.Orders.Where(y => !string.IsNullOrEmpty(y.OrderTags)))
                .SelectMany(x => x.GetOrderTagValues(y => y.MenuItemId == 0).Select(y => new { Name = y.TagValue, x.MenuItemName, x.Quantity, x.Value }))
                .GroupBy(x => new { x.Name, x.MenuItemName })
                .Select(x => new { x.Key.Name, x.Key.MenuItemName, Quantity = x.Sum(y => y.Quantity), Value = x.Sum(y => y.Value) }).ToList();
            return properties;
        }
        public static DataSet GetMenuItemOutletWiseSaleSummarySP(DateTime fromDate, DateTime toDate, string DepartmentIds, int OutletId)
        {
            DataSet ds = new DataSet();
            DataTable table = GetMenuItemOutletWiseSaleSummary(Convert.ToString(OutletId), DepartmentIds, fromDate, toDate);
            ds.Tables.Add(table);
            return ds;
        }
        public static DataTable GetMenuItemOutletWiseSaleSummary(string OutletId, string DepartmentIds, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = string.Empty;

                string dbConnString = DBUtility.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(dbConnString))
                {
                    string spName = @"dbo.[GetMenuItemWiseOutletWiseSalesSummary]";
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    SqlParameter paramExactTime = new SqlParameter();
                    paramExactTime.ParameterName = "@IsExactTime";
                    paramExactTime.SqlDbType = SqlDbType.Int;
                    paramExactTime.Value = 1;
                    cmd.Parameters.Add(paramExactTime);

                    SqlParameter paramStartDate = new SqlParameter();
                    paramStartDate.ParameterName = "@StartDate";
                    paramStartDate.SqlDbType = SqlDbType.DateTime;
                    paramStartDate.Value = fromDate;
                    cmd.Parameters.Add(paramStartDate);

                    SqlParameter paramEndDate = new SqlParameter();
                    paramEndDate.ParameterName = "@EndDate";
                    paramEndDate.SqlDbType = SqlDbType.DateTime;
                    paramEndDate.Value = toDate;
                    cmd.Parameters.Add(paramEndDate);

                    SqlParameter paramOutletIds = new SqlParameter();
                    paramOutletIds.ParameterName = "@Outlets";
                    paramOutletIds.SqlDbType = SqlDbType.NVarChar;
                    paramOutletIds.Value = OutletId;
                    cmd.Parameters.Add(paramOutletIds);

                    SqlParameter paramDepartmentIds = new SqlParameter();
                    paramDepartmentIds.ParameterName = "@Departments";
                    paramDepartmentIds.SqlDbType = SqlDbType.NVarChar;
                    paramDepartmentIds.Value = DepartmentIds;
                    cmd.Parameters.Add(paramDepartmentIds);

                    conn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    var dataTable = new DataTable();
                    dt.Load(dr);
                    dt.TableName = "MenuItemOutletWiseSalesSummary";
                    dr.Close();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
    }

    internal class TicketTypeInfo
    {
        public int TicketTypeId { get; set; }
        public decimal AmountExcludingDiscount { get; set; }
        public decimal Amount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Services { get; set; }
        public decimal PreServices { get; set; }
        public int TicketCount { get; set; }
        public string TicketTypeName { get; set; }
        //{
        //get
        //{
        //    TicketManager.GetTicketType();
        //    var d = ReportContext.TicketTypes.FirstOrDefault(x => x.Id == TicketTypeId);
        //    return d != null ? d.Name : Localization.Properties.Resources.UndefinedWithBrackets;
        //}
        //}
    }

    internal class AmountCalculator
    {
        private readonly IEnumerable<TenderedAmount> _amounts;
        public AmountCalculator(IEnumerable<TenderedAmount> amounts)
        {
            _amounts = amounts;
            //
        }

        internal decimal GetAmount(string paymentName)
        {
            var r = _amounts.FirstOrDefault(x => x.PaymentName == paymentName);
            return r != null ? r.Amount : 0;
        }

        internal string GetPercent(string paymentName)
        {
            return TotalAmount > 0 ? string.Format("%{0:0.00}", GetAmount(paymentName) * 100 / TotalAmount) : "%0";
        }

        public IEnumerable<string> PaymentNames { get { return _amounts.Select(x => x.PaymentName).Distinct(); } }
        public decimal TotalAmount { get { return _amounts.Sum(x => x.Amount); } }
    }

    internal class TenderedAmount
    {
        public string PaymentName { get; set; }
        public decimal Amount { get; set; }
    }
}
