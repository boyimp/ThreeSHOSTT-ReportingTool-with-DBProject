import { format } from 'date-fns';

export const getFormattedDateTime = (dateTime) => {
    return format(new Date(dateTime), 'dd LLL yyyy, hh:mm aa');
};
