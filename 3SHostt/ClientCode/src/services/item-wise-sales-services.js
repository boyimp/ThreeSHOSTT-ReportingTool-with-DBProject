import axios from 'axios';
import GlobalConstant from './_utils/global.data';

export const onItemSalesReport = async (
    isExact,
    fromDate,
    toDate,
    outletIds,
    departmentIds,
    categoriesIds,
    menuItemIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_ITEM_SALES_REPORT,
        data: {
            isExact: isExact,
            from: fromDate,
            to: toDate,
            outletIds: outletIds,
            departmentId: departmentIds,
            groupCodes: categoriesIds,
            menuItemIds: menuItemIds
        }
    };
    return await axios.request(options);
};

export const onItemSalesReportById = async (id, portion) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_ITEM_SALES_REPORT_BY_ID,
        data: {
            menuItemId: id,
            portionName: portion
        }
    };
    return await axios.request(options);
};

export const onItemSalesProfitLossRecipe = async (
    isExact,
    fromDate,
    toDate,
    outletIds,
    departmentIds,
    categoriesIds,
    menuItemIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_ITEM_SALES_PROFIT_LOSS_RECIPE,
        data: {
            isExact: isExact,
            from: fromDate,
            to: toDate,
            outletId: outletIds,
            departmentId: departmentIds,
            groupCode: categoriesIds,
            menuItemId: menuItemIds
        }
    };
    return await axios.request(options);
};

export const onItemSalesProfitAnalysis = async (
    isExact,
    fromDate,
    toDate,
    outletIds,
    departmentIds,
    categoriesIds,
    menuItemIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_ITEM_SALES_PROFIT_ANALYSIS,
        data: {
            isExact: isExact,
            from: fromDate,
            to: toDate,
            outletId: outletIds,
            departmentId: departmentIds,
            groupCode: categoriesIds,
            menuItemId: menuItemIds
        }
    };
    return await axios.request(options);
};
