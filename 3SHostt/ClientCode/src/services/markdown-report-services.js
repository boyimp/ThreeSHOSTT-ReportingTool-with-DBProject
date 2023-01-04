import axios from 'axios';
import GlobalConstant from './_utils/global.data';

export const onGiftDetails = async (fromDate, toDate) => {
    const options = {
        method: 'POST',
        url: GlobalConstant.BASE_URL + GlobalConstant.ROUTES_GIFT_DETAILS_REPORT,
        data: { from: fromDate, to: toDate }
    };
    return await axios.request(options);
};
