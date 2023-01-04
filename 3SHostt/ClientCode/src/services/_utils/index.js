export const getUniqueValue = (data, property) => {
    const list = data.map((item) => item[property]);
    const unique = [...new Set(list)];
    return unique;
};

export const getSortNumbers = (arr) => {
    return arr.sort(function (a, b) {
        return a - b;
    });
};

export const getTableTotal = (data, fieldName) => {
    const values = data?.map((item) => item[fieldName] || 0);
    const total = values?.reduce((prev, curr) => Number(prev) + Number(curr), 0);
    return total;
};

export const commaFormat = (num) => {
    const amount = Number(num)?.toLocaleString('en-IN', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    return amount;
};
export const roundCommaFormat = (number) => {
    return Math.round(number).toLocaleString('en-IN');
};
