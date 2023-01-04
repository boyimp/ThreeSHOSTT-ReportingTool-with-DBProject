import React, { useState } from 'react';
import {
    Heading,
    Flex,
    useColorModeValue,
    Table,
    Thead,
    Tr,
    Th,
    Tbody,
    Td
} from '@chakra-ui/react';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import FilterOption from '../../shared/FilterOption';
import ReportConsideredTime from '../../shared/ReportConsideredTime';
import { useQuery } from 'react-query';
import Loading from '../../shared/Loading';
import { commaFormat, getTableTotal } from '../../../services/_utils';
import { onGiftDetails } from '../../../services/markdown-report-services';
import NoDataFound from '../../shared/NoDataFound';

const GiftDetails = () => {
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    // filter options
    const [isExact, setIsExact] = useState(true);
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [isSearching, setIsSearching] = useState(false);
    // data query
    const onSuccess = (data) => {
        setFromDate(data?.data?.fromDate?.slice(0, 16) || '');
        setToDate(data?.data?.toDate?.slice(0, 16) || '');
        setIsSearching(false);
    };
    const onError = (err) => {
        console.log(err);
        setIsSearching(false);
    };
    const { data, isLoading, refetch } = useQuery(
        'giftDetails',
        () => onGiftDetails(fromDate, toDate),
        { onSuccess, onError }
    );
    const loading = isLoading || isSearching;
    const result = data?.data?.VoidOrders || [];

    return (
        <Layout>
            <Flex direction="column" bg={bgColor} p="30px" boxShadow="sm" borderRadius="xl">
                <Heading size="md" mb="5">
                    Gift Details Report
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
                    refetch={refetch}
                />

                <ReportConsideredTime fromDate={fromDate} toDate={toDate} />

                <ExportOptions
                    title="Gift Details"
                    fromDate={fromDate}
                    toDate={toDate}
                    colSpan={3}
                    scale={0.6}
                    margin="20pt">
                    {/*  */}
                </ExportOptions>

                <Flex overflow="auto" h="600px" direction="column">
                    {loading ? (
                        <Loading line={18} width="100%" />
                    ) : result.length === 0 ? (
                        <NoDataFound />
                    ) : (
                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead>
                                <Tr bg="primary.900">
                                    {Object.keys(result[0])?.map((item, i) => (
                                        <Th p="2" color="white" key={i}>
                                            {item}
                                        </Th>
                                    ))}
                                </Tr>
                            </Thead>

                            <Tbody>
                                {result?.map((item, i) => (
                                    <Tr key={i}>
                                        {Object.keys(result[0])?.map((property, i) => (
                                            <Td px="2" key={i}>
                                                {item[property]}
                                            </Td>
                                        ))}
                                    </Tr>
                                ))}
                            </Tbody>
                        </Table>
                    )}
                </Flex>
            </Flex>
        </Layout>
    );
};

export default GiftDetails;
