import React, { useState, useEffect } from 'react';
import { Heading, Flex, useColorModeValue, Grid } from '@chakra-ui/react';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import FilterOption from '../../shared/FilterOption';
import ExportWorkPeriod from './ExportWorkPeriod';
import ReportConsideredTime from '../../shared/ReportConsideredTime';
import { onWorkPeriodReport } from '../../../services/sales-service';
import { useQuery } from 'react-query';
import SalesInformation from './SalesInformation';
import IncomeInformation from './IncomeInformation';
import CustomerDueAdvance from './CustomerDueAdvance';
import DepartmentWiseSales from './DepartmentWiseSales';
import GeneralInformation from './GeneralInformation';
import UserWiseSales from './UserWiseSales';
import UserWiseTicketsTotal from './UserWiseTicketsTotal';
import UserReturns from './UserReturns';
import Refunds from './Refunds';
import TicketTypeWiseIncomes from './TicketTypeWiseIncomes';
import ItemSalesComponent from './ItemSales';

const WorkPeriodDetails = () => {
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');

    // filter options
    const [isExact, setIsExact] = useState(true);
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [departmentIds, setDepartmentIds] = useState([0]);
    const [outletIds, setOutletIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    // data query
    const onSuccess = (data) => {
        setFromDate(data.data.fromDate.slice(0, 16));
        setToDate(data.data.toDate.slice(0, 16));
        setIsSearching(false);
    };
    const onError = (err) => {
        console.log(err);
        setIsSearching(false);
    };
    const { data, isLoading, refetch } = useQuery(
        'workPeriodReport',
        () => onWorkPeriodReport(isExact, fromDate, toDate, outletIds, departmentIds),
        { onSuccess, onError }
    );
    const loading = isLoading || isSearching;

    const {
        Sales,
        Income,
        CustomerDue,
        DepartmentWise,
        GeneralInfo,
        UserWiseOrders,
        UserWiseTickets,
        ItemSales,
        TicketGroupInfo,
        Refund,
        RefundOwners,
        RefundTickets,
        Orders
    } = data?.data?.result || {};

    return (
        <Layout>
            <Flex direction="column" bg={bgColor} p="30px" boxShadow="sm" borderRadius="xl">
                <Heading size="md" mb="5">
                    Work Period Details
                </Heading>

                <FilterOption
                    isApiCalling={loading}
                    setIsSearching={setIsSearching}
                    isExact={isExact}
                    setIsExact={setIsExact}
                    fromDate={fromDate}
                    setFromDate={setFromDate}
                    toDate={toDate}
                    setToDate={setToDate}
                    setDepartmentIds={setDepartmentIds}
                    setOutletIds={setOutletIds}
                    refetch={refetch}
                />

                <ReportConsideredTime fromDate={fromDate} toDate={toDate} />

                <ExportOptions
                    title="Work Period Details"
                    fromDate={fromDate}
                    toDate={toDate}
                    colSpan={3}
                    scale={0.6}
                    margin="20pt">
                    <ExportWorkPeriod />
                </ExportOptions>

                <Grid templateColumns={{ base: 'repeat(1, 1fr)', lg: 'repeat(2, 1fr)' }} gap={7}>
                    <SalesInformation data={Sales} loading={loading} />
                    <IncomeInformation data={Income} loading={loading} />
                    <CustomerDueAdvance data={CustomerDue} loading={loading} />
                    <GeneralInformation data={GeneralInfo} loading={loading} />
                    <ItemSalesComponent loading={loading} data={ItemSales} />
                    <DepartmentWiseSales data={DepartmentWise} loading={loading} />
                    <UserWiseSales data={UserWiseOrders} loading={loading} />
                    <UserWiseTicketsTotal data={UserWiseTickets} loading={loading} />
                    <UserReturns data={RefundOwners} loading={loading} />
                    <Refunds data={Refund} loading={loading} />
                    <TicketTypeWiseIncomes loading={loading} data={TicketGroupInfo} />
                </Grid>
            </Flex>
        </Layout>
    );
};

export default WorkPeriodDetails;
