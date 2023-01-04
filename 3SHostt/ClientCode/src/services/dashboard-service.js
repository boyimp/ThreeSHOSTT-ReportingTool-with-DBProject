// @ts-nocheck
// import { format } from 'date-fns';
import axios from 'axios';
import GlobalConstant from './_utils/global.data';

export const onTopSale = async (start, end, dept, outletId) => {
    let today = new Date();
    let fromDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7);
    let toDate = today;
    if (start && end) {
        fromDate = start;
        toDate = end;
    }
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_TOP_SALES,
        data: {
            fromDate: fromDate,
            toDate: toDate,
            departmentId: dept,
            outletId: outletId
        }
    };
    return await axios.request(options);
};

export const onOutletNames = async () => {
    return await axios.get(GlobalConstant.BASE_URL + GlobalConstant.ROUTES_GET_OUTLETS);
};
export const onDepartmentNames = async () => {
    return await axios.get(GlobalConstant.BASE_URL + GlobalConstant.ROUTES_GET_DEPARTMENTS);
};
export const onAllCategoriesAndMenuItems = async () => {
    return await axios.get(
        GlobalConstant.BASE_URL + GlobalConstant.ROUTES_GET_ALL_CATEGORIES_AND_MENU_ITEMS
    );
};

// Daily Sales
export const onDailySales = async (
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
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_DAILY_SALES,
        data: {
            isExact: isExact,
            from: fromDate,
            to: toDate,
            outletIds: outletIds,
            departmentIds: departmentIds,
            menuItemIds: menuItemIds,
            menuItemCategoryNames: categoriesIds
        }
    };
    return await axios.request(options);
};

// Monthly Sales
export const onMonthlyNetSales = (outletIds) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_MONTHLY_NET_SALE,
        data: { outletIds: outletIds }
    };
    return axios.request(options);
};
export const onMonthlyTotalSales = (outletIds) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_MONTHLY_TOTAL_SALE,
        data: { outletIds: outletIds }
    };
    return axios.request(options);
};

// Dashboard Items
export const onDashboardCardValue = async (
    isExact,
    startDate,
    endDate,
    outletIds,
    departmentIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_DASHBOARD_CARD_VALUE,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return await axios.request(options);
};
export const onProductWiseSalesInfo = async (
    isExact,
    startDate,
    endDate,
    outletIds,
    departmentIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_PRODUCT_WISE_SALES_INFO,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return await axios.request(options);
};
export const onPaymentMethodWiseSalesInfo = async (
    isExact,
    startDate,
    endDate,
    outletIds,
    departmentIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_PAYMENT_METHOD_WISE_SALES_INFO,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return await axios.request(options);
};
export const onProductCategoryWiseSalesCountInfo = async (
    isExact,
    startDate,
    endDate,
    outletIds,
    departmentIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_PRODUCT_CATEGORY_WISE_SALES_COUNT_INFO,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return await axios.request(options);
};
export const onDepartmentWiseSalesInfo = async (
    isExact,
    startDate,
    endDate,
    outletIds,
    departmentIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_DEPARTMENT_WISE_SALES_INFO,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return await axios.request(options);
};
export const onDiscountInfo = async (isExact, startDate, endDate, outletIds, departmentIds) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_DISCOUNT_INFO,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return await axios.request(options);
};

// Yearly comparison
export const onYearlySales = async (outletIds, departmentIds, numberOfYears) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_YEARLY_SALES,
        data: {
            outletIds: outletIds,
            departmentIds: departmentIds,
            NumberOfYears: numberOfYears
        }
    };
    return await axios.request(options);
};

export const onTimeWiseSales = async (
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
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_TIME_WISE_SALES,
        data: {
            isExact: isExact,
            from: fromDate,
            to: toDate,
            outletIds: outletIds,
            departmentIds: departmentIds,
            menuItemIds: menuItemIds,
            menuItemCategoryNames: categoriesIds
        }
    };
    return await axios.request(options);
};

// outlet comparison
export const onOutletWiseSales = (isExact, startDate, endDate, outletIds, departmentIds) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_OUTLET_WISE_SALES_INFO,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return axios.request(options);
};
export const onOutletAndPaymentMethodWiseSales = (
    isExact,
    startDate,
    endDate,
    outletIds,
    departmentIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_OUTLET_AND_PAYMENT_METHOD_WISE_SALES,
        data: {
            isExact: isExact,
            from: startDate,
            to: endDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return axios.request(options);
};
export const onPaymentType = () => {
    return axios.get(GlobalConstant.BASE_URL + GlobalConstant.ROUTES_PAYMENT_TYPE);
};

// Day & Time wise sales chart
export const onDayAndTimeWiseSales = async (
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
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_DAY_AND_TIME_WISE_SALES,
        data: {
            isExact: isExact,
            from: fromDate,
            to: toDate,
            outletIds: outletIds,
            departmentIds: departmentIds,
            menuItemCategoryNames: categoriesIds,
            menuItemIds: menuItemIds
        }
    };
    return await axios.request(options);
};
