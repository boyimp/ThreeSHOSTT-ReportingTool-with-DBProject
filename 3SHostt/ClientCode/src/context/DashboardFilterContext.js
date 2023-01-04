import React, { createContext, useContext, useState, useEffect } from 'react';
import {
    onDepartmentWiseSalesInfo,
    onProductCategoryWiseSalesCountInfo
} from '../services/dashboard-service';
const DashboardFilterContext = createContext();

const DashboardFilterContextProvider = ({ children }) => {
    const [isExact, setIsExact] = useState(true);
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [outletIds, setOutletIds] = useState([0]);
    const [departmentIds, setDepartmentIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    // data
    const [categoriesLoading, setCategoriesLoading] = useState(false);
    const [categoriesData, setCategoriesData] = useState([]);
    // customise data
    const [grossAmount, setGrossAmount] = useState([]);
    const [itemName, setItemName] = useState([]);
    // data
    const [departmentsLoading, setDepartmentsLoading] = useState(false);
    const [departmentsData, setDepartmentsData] = useState([]);
    const [departments, setDepartments] = useState();
    const [amounts, setAmounts] = useState([]);

    const onCategoriesData = async () => {
        try {
            setCategoriesLoading(true);
            const { data } = await onProductCategoryWiseSalesCountInfo(
                isExact,
                fromDate,
                toDate,
                outletIds,
                departmentIds
            );
            setCategoriesData(data.result);
            setCategoriesLoading(false);
            setIsSearching(false);
        } catch (error) {
            setCategoriesLoading(true);
            console.log(error);
            setCategoriesLoading(false);
            setIsSearching(false);
        }
    };

    const onDepartmentsData = async () => {
        try {
            setDepartmentsLoading(true);
            const { data } = await onDepartmentWiseSalesInfo(
                isExact,
                fromDate,
                toDate,
                outletIds,
                departmentIds
            );
            setDepartmentsData(data.result);
            setDepartmentsLoading(false);
            setIsSearching(false);
        } catch (error) {
            setDepartmentsLoading(true);
            console.log(error);
            setDepartmentsLoading(false);
        }
    };

    useEffect(() => {
        onCategoriesData();
        onDepartmentsData();
    }, []);

    useEffect(() => {
        isSearching && onCategoriesData();
        isSearching && onDepartmentsData();
    }, [isSearching]);

    useEffect(() => {
        setGrossAmount(categoriesData.map((item) => Math.round(item.GrossAmount)));
        setItemName(categoriesData.map((item) => item.ProductCategory));
    }, [categoriesData]);

    useEffect(() => {
        setDepartments(departmentsData.map((item) => item.Department));
        setAmounts(departmentsData.map((item) => Math.round(item.Amount)));
    }, [departmentsData]);

    return (
        <DashboardFilterContext.Provider
            value={{
                fromDate,
                setFromDate,
                toDate,
                setToDate,
                outletIds,
                setOutletIds,
                departmentIds,
                setDepartmentIds,
                isExact,
                setIsExact,
                setIsSearching,
                isSearching,
                categoriesLoading,
                categoriesData,
                grossAmount,
                itemName,
                departmentsLoading,
                departmentsData,
                departments,
                amounts
            }}>
            {children}
        </DashboardFilterContext.Provider>
    );
};

const useDashboardFilterContext = () => {
    return useContext(DashboardFilterContext);
};

export { DashboardFilterContextProvider, useDashboardFilterContext };
