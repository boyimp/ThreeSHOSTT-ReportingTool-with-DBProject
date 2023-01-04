import axios from 'axios';
import GlobalConstant from './_utils/global.data';

export const onWorkPeriodReport = async (isExact, fromDate, toDate, outletIds, departmentIds) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_WORK_PERIOD_REPORT,
        data: {
            isExact: isExact,
            from: fromDate,
            to: toDate,
            outletIds: outletIds,
            departmentIds: departmentIds
        }
    };
    return await axios.request(options);
};
export const onInvoiceWiseSale = async (
    isExact,
    isTicketOpen,
    fromDate,
    toDate,
    outletIds,
    departmentIds
) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_INVOICE_WISE_SALE,
        data: {
            isExact: isExact,
            onlyOpenTickets: isTicketOpen,
            fromDate: fromDate,
            toDate: toDate,
            outletId: outletIds,
            departmentId: departmentIds
        }
    };
    return await axios.request(options);
};
export const onTicketDetailsByTicketId = async (id) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_TICKET_DETAILS_BY_TICKET_ID,
        data: { Id: id }
    };
    return await axios.request(options);
};
