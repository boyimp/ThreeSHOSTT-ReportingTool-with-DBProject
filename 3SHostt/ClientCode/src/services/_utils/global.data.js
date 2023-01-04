//In the name of Allah

export default class GloblaConstant {
    static CURRENT_USER = 'current_user';
    static BASE_URL = '.';
    static ROUTES_IS_VERIFIED_USER = '/api/Authentication/Login';
    static ROUTES_LOG_OUT = '/api/Authentication/LogOut';
    static ROUTES_IS_SESSION_ACTIVE = '/api/Authentication/GetIsSessionActive';

    // static ROUTES_GET_DAILY_SALES = '/api/dashboard2/GetdailySales/';
    // static ROUTES_TOP_SALES = '/api/dashboard2/GetTop10SellingItems/';

    static ROUTES_GET_OUTLETS = '/api/dashboard2/GetOutlets/';
    static ROUTES_GET_DEPARTMENTS = '/api/ticket/GetDepartments';
    static ROUTES_GET_ALL_CATEGORIES_AND_MENU_ITEMS =
        '/api/dashboard2/GetAllCategoriesAndMenuItems';

    static ROUTES_DASHBOARD_CARD_VALUE = '/api/dashboard2/GetDashboardCardValue/';
    static ROUTES_DISCOUNT_INFO = '/api/dashboard2/GetDiscountInfo/';
    static ROUTES_PRODUCT_WISE_SALES_INFO = '/api/dashboard2/GetProductWiseSalesInfo/';
    static ROUTES_PAYMENT_METHOD_WISE_SALES_INFO = '/api/dashboard2/GetPaymentMethodWiseSalesInfo/';
    static ROUTES_PRODUCT_CATEGORY_WISE_SALES_COUNT_INFO =
        '/api/dashboard2/GetProductCagegoryWiseSalesCountInfo/';
    static ROUTES_DEPARTMENT_WISE_SALES_INFO = '/api/dashboard2/GetDepartmentWiseSalesInfo/';

    static ROUTES_MONTHLY_TOTAL_SALE = '/api/dashboard2/GetMonthlySalesForDashBoard/';
    static ROUTES_MONTHLY_NET_SALE = '/api/dashboard2/GetMonthlyNetSales/';

    static ROUTES_YEARLY_SALES = '/api/dashboard2/GetSalesYearly/';
    static ROUTES_DAILY_SALES = '/api/dashboard2/GetDailySalesParameterized/';

    static ROUTES_TIME_WISE_SALES = '/api/chart/GetTimeWiseSalesMultiParameterized';
    static ROUTES_PAYMENT_TYPE = '/api/ticket/GetPaymentType';

    static ROUTES_OUTLET_WISE_SALES_INFO = '/api/chart/GetOutletWiseSalesInfo';
    static ROUTES_OUTLET_AND_PAYMENT_METHOD_WISE_SALES =
        '/api/chart/GetOutletWiseAndPaymentMethodWiseSalesInfo';

    static ROUTES_MENU_ITEM_CATEGORIES_NAMES_AND_IDS = '/api/menu/GetMenuItemCategoriesNamesAndIds';
    static ROUTES_MENU_ITEM_NAMES_AND_IDS = '/api/menu/GetMenuItemNamesAndIds';
    static ROUTES_DAY_AND_TIME_WISE_SALES = '/api/chart/GetDayTimeWiseSalesMultiParameterized';

    // sales report
    static ROUTES_WORK_PERIOD_REPORT = '/api/sales/GetWorkPeriodReport';
    static ROUTES_INVOICE_WISE_SALE = '/api/sales/GetInvoiceWiseSales';
    static ROUTES_TICKET_DETAILS_BY_TICKET_ID = '/api/sales/GetTicketDetailsByTicketId';

    // item wise sales
    static ROUTES_ITEM_SALES_REPORT = '/api/itemSales/GetItemSalesReport';
    static ROUTES_ITEM_SALES_REPORT_BY_ID = '/api/itemSales/GetProductionCostDrill';
    static ROUTES_ITEM_SALES_PROFIT_LOSS_RECIPE =
        '/api/itemSales/GetItemSalesProfitLossRecipeReport';
    static ROUTES_ITEM_SALES_PROFIT_ANALYSIS = '/api/itemSales/GetItemSalesProfitAnalysisReport';

    // markdown reports
    static ROUTES_GIFT_DETAILS_REPORT = '/api/markdown/GetGiftDetailsReport';
} //clas
