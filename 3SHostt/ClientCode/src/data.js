import { FaHome, FaChartBar } from 'react-icons/fa';
import { TbReportSearch, TbReport, TbBuildingWarehouse } from 'react-icons/tb';
import { TiDocumentText } from 'react-icons/ti';
import { IoDocumentTextOutline, IoDocuments } from 'react-icons/io5';

export const sidebarData = [
    {
        id: 1,
        itemName: 'Dashboard',
        link: '/dashboard',
        icon: FaHome,
        subItems: []
    },
    {
        id: 2,
        itemName: 'Chart',
        link: '',
        icon: FaChartBar,
        subItems: [
            { id: 2, subName: 'Time Wise Sales', link: '/time_wise_sales_chart' },
            { id: 1, subName: 'Day Wise Sales', link: '/day_wise_sales_chart' },
            { id: 6, subName: 'Month Wise Sales', link: '/month_wise_sales_chart' },
            { id: 3, subName: 'Day & Time Wise Sales', link: '/day_and_time_wise_sales_chart' },
            { id: 4, subName: 'Outlet Comparison', link: '/outlet_comparison_chart' },
            { id: 5, subName: 'Yearly Comparison', link: '/yearly_comparison_chart' }
        ]
    },
    {
        id: 3,
        itemName: 'Sales Report',
        link: '',
        icon: TiDocumentText,
        subItems: [
            {
                id: 5,
                subName: 'Work Period',
                link: 'UI/WorkPeriodReport.aspx?ReportType=Short',
                legacy: true
            },
            {
                id: 1,
                subName: 'Work Period Details',
                link: 'UI/WorkPeriodReport.aspx?ReportType=Full',
                legacy: true
            },
            { id: 2, subName: 'Invoice Wise Sales', link: 'UI/Tickets.aspx', legacy: true },
            {
                id: 3,
                subName: 'Sales Breakdown',
                link: 'UI/TicketsAccountWise.aspx?ReportType=Normal',
                legacy: true
            },
            {
                id: 4,
                subName: 'Sales Statement',
                link: 'UI/SalesTaxReport.aspx?ReportType=Normal',
                legacy: true
            }
        ]
    },
    {
        id: 25,
        itemName: 'Sales Report (new)',
        link: '',
        icon: TiDocumentText,
        subItems: [
            {
                id: 1,
                subName: 'Work Period Details',
                link: '/work_period_details'
            },
            {
                id: 2,
                subName: 'Invoice Wise Sales',
                link: '/invoice_wise_sales'
            }
        ]
    },
    {
        id: 4,
        itemName: 'Item Wise Sales',
        link: '',
        icon: TbReport,
        subItems: [
            { id: 1, subName: 'Item Sales', link: 'UI/ItemSalesReport.aspx', legacy: true },
            {
                id: 5,
                subName: 'Menu Mix Report',
                link: 'UI/ItemWiseMenuMixReport.aspx',
                legacy: true
            },
            {
                id: 6,
                subName: 'Menu Profit Analysis',
                link: 'UI/ItemWiseMenuMixReport2.aspx',
                legacy: true
            },
            {
                id: 2,
                subName: 'Outlet Wise Item Sales',
                link: 'UI/ItemSalesOutletWise.aspx',
                legacy: true
            },
            {
                id: 3,
                subName: 'Item Sales Profit Loss Recipe',
                link: 'UI/ItemSalesProfitLossRecipe.aspx',
                legacy: true
            },
            {
                id: 4,
                subName: 'Sales Profit Analysis',
                link: 'UI/ItemSalesProfitAnalysis.aspx',
                legacy: true
            },

            {
                id: 7,
                subName: 'Order Tag/Services',
                link: 'UI/OrderServiceTagReport.aspx',
                legacy: true
            },
            {
                id: 8,
                subName: 'Production Cost Analysis',
                link: 'UI/ProductionCost.aspx',
                legacy: true
            },
            { id: 9, subName: 'Sales Summary', link: 'UI/SalesSummary.aspx', legacy: true },
            {
                id: 10,
                subName: 'Process Item Wise Sales Tax',
                link: 'UI/ItemWiseSaleTax.aspx',
                legacy: true
            }
        ]
    },
    {
        id: 26,
        itemName: 'Item Wise.. (new)',
        link: '',
        icon: TiDocumentText,
        subItems: [
            {
                id: 1,
                subName: 'Item Sales',
                link: '/item_sales_report'
            },
            {
                id: 2,
                subName: 'Item Sales Profit Loss Recipe',
                link: '/item_sales_profit_loss_recipe'
            },
            {
                id: 3,
                subName: 'Sales Profit Analysis',
                link: '/sales_profit_analysis'
            }
        ]
    },
    {
        id: 5,
        itemName: 'Accounts Details',
        link: '',
        icon: TbReportSearch,
        subItems: [
            {
                id: 1,
                subName: 'Current Balance',
                link: 'UI/CurrentBalanceOnAChead.aspx',
                legacy: true
            },
            { id: 2, subName: 'Income Statement', link: 'UI/IncomeStatement.aspx', legacy: true },
            {
                id: 3,
                subName: 'Account Type Wise Report',
                link: 'UI/AccountTypeWise.aspx',
                legacy: true
            },
            { id: 4, subName: 'Account Wise Report', link: 'UI/AccountWise.aspx', legacy: true },
            { id: 5, subName: 'Entity & Accounts', link: 'UI/Entities.aspx', legacy: true }
        ]
    },
    {
        id: 6,
        itemName: 'Inventory Reports',
        link: '',
        icon: TbBuildingWarehouse,
        subItems: [
            {
                id: 1,
                subName: 'Inventory Item Wise Potential Report',
                link: 'UI/InventoryItemWisePotential.aspx',
                legacy: true
            },
            {
                id: 2,
                subName: 'Inventory Potential Revenue',
                link: 'UI/InventoryPotentialRevenue.aspx?ReportType=All',
                legacy: true
            },
            {
                id: 3,
                subName: 'Consumption/Theoritical Usage Report',
                link: 'UI/InventoryPotentialRevenue.aspx?ReportType=TheoUsage',
                legacy: true
            },
            {
                id: 4,
                subName: 'Wastage Report',
                link: 'UI/InventoryPotentialRevenue.aspx?ReportType=Wastage',
                legacy: true
            },
            {
                id: 5,
                subName: 'Count Variance Report',
                link: 'UI/InventoryPotentialRevenue.aspx?ReportType=CountVariance',
                legacy: true
            },
            {
                id: 6,
                subName: 'Stock Take Report',
                link: 'UI/InventoryPotentialRevenue.aspx?ReportType=StockTake',
                legacy: true
            },
            {
                id: 7,
                subName: 'Prep Items Report',
                link: 'UI/InventoryPotentialRevenueProduction.aspx?ReportType=All',
                legacy: true
            },
            {
                id: 8,
                subName: 'Cost of Sales Summary',
                link: 'UI/InventoryCostOfSalesTotal.aspx',
                legacy: true
            },
            { id: 9, subName: 'Stock Report', link: 'UI/CurrentStock.aspx', legacy: true },
            {
                id: 10,
                subName: 'Inventory Register',
                link: 'UI/InvntoryRegister.aspx',
                legacy: true
            },
            {
                id: 11,
                subName: 'Work Period wise Inventory Register',
                link: 'UI/WorkPeriodWiseInvntoryRegister.aspx',
                legacy: true
            },
            {
                id: 12,
                subName: 'Work Period End Report',
                link: 'UI/WorkPeriodEndReport.aspx',
                legacy: true
            },
            {
                id: 13,
                subName: 'Special Inventory Register',
                link: 'UI/SpecialInventoryRegister.aspx',
                legacy: true
            },
            {
                id: 14,
                subName: 'Inventory Transactions',
                link: 'UI/InventoryTransactionDateWise.aspx',
                legacy: true
            }
        ]
    },
    {
        id: 7,
        itemName: 'Markdown Reports',
        link: '',
        icon: IoDocumentTextOutline,
        subItems: [
            { id: 1, subName: 'Gift Details', link: 'UI/GiftOrders.aspx', legacy: true },
            { id: 2, subName: 'Void Details', link: 'UI/VoidOrders.aspx', legacy: true },
            // { id: 3, subName: 'Discount Details', link: '' },
            { id: 4, subName: 'Entity Wise Sales', link: 'UI/EntityReport.aspx', legacy: true },
            {
                id: 5,
                subName: 'Cashier Wise Sales',
                link: 'UI/CashierWiseAccountReport.aspx?ReportType=Normal',
                legacy: true
            }
        ]
    },
    {
        id: 28,
        itemName: 'Mrdwn Rprt.. (new)',
        link: '',
        icon: IoDocumentTextOutline,
        subItems: [{ id: 1, subName: 'Gift Details', link: '/gift_details_report' }]
    },
    {
        id: 8,
        itemName: 'Special Reports',
        link: '',
        icon: IoDocuments,
        subItems: [
            {
                id: 1,
                subName: 'Inventory Item List',
                link: 'UI/ReportViewerAlter.aspx',
                legacy: true
            },
            { id: 2, subName: 'Menu Item List', link: 'UI/MenuItemList.aspx', legacy: true },
            {
                id: 3,
                subName: 'Inventory Vs Recipes',
                link: 'UI/InventoryVsRecipes.aspx',
                legacy: true
            },
            { id: 4, subName: 'Recipe', link: 'UI/Recipe.aspx', legacy: true },
            {
                id: 5,
                subName: 'Production Cost Analysis',
                link: 'UI/ProductionCost.aspx',
                legacy: true
            },
            { id: 6, subName: 'Income Statement', link: 'UI/IncomeStatement.aspx', legacy: true },
            {
                id: 7,
                subName: 'Inventory Cost of Sales Detail',
                link: 'UI/InventoryCostOfSalesDetail.aspx',
                legacy: true
            },
            { id: 8, subName: 'Sales Summary', link: 'UI/SalesSummary.aspx', legacy: true },
            { id: 9, subName: 'Outlet Status', link: 'UI/OutletStatus.aspx', legacy: true },
            {
                id: 10,
                subName: 'Notification Preference',
                link: 'UI/PushNotification.aspx',
                legacy: true
            },
            {
                id: 11,
                subName: 'Process Item Wise Sale Tax',
                link: 'UI/ItemWiseSaleTax.aspx',
                legacy: true
            }
        ]
    }
    // {
    //     id: 9,
    //     itemName: 'Comet (Pending)',
    //     link: '',
    //     icon: TbReportSearch,
    //     subItems: [
    //         { id: 1, subName: 'Tickets Report', link: '/tickets_report' },
    //         {
    //             id: 2,
    //             subName: 'Day Wise Outlets Payments Total',
    //             link: '/day_wise_outlets_payments_total'
    //         },
    //         { id: 3, subName: 'Menu Item Account Details', link: '/menu_item_account_details' },
    //         { id: 4, subName: 'Void, Gift & Others Report', link: '/void_gift_and_others_report' },
    //         { id: 9, subName: 'Entity Wise Sales', link: '/entity_wise_sales' },
    //         { id: 6, subName: 'Outlet Wise Modifier Sales', link: '/outlet_wise_modifier_sales' },
    //         {
    //             id: 7,
    //             subName: 'Outlet Wise Item Sales_Modified',
    //             link: '/outlet_wise_item_sales_modified'
    //         },
    //         { id: 8, subName: 'Month Wise Item Sales', link: '/month_wise_item_sales' },
    //         { id: 5, subName: 'Outlet Wise Item Sales', link: '/outlet_wise_item_sales' },
    //         {
    //             id: 10,
    //             subName: 'Ticket Wise Payments Report 2',
    //             link: '/ticket_wise_payments_report2'
    //         }
    //     ]
    // }
];
