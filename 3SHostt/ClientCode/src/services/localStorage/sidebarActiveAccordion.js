export const getSidebarActiveAccordion = () => {
    const index = localStorage.getItem('sidebarActiveAccordion');
    return parseInt(index);
};

export const setSidebarActiveAccordion = (index) => {
    return localStorage.setItem('sidebarActiveAccordion', index);
};
