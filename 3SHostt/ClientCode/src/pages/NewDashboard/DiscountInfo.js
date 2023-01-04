import {
    GridItem,
    Heading,
    Table,
    Tbody,
    Td,
    Th,
    Thead,
    Tr,
    useColorModeValue
} from '@chakra-ui/react';
import React, { useEffect, useState } from 'react';
import { useDashboardFilterContext } from '../../context/DashboardFilterContext';
import { onDiscountInfo } from '../../services/dashboard-service';
import { commaFormat, getTableTotal, roundCommaFormat } from '../../services/_utils/index';
import Loading from '../shared/Loading';

const DiscountInfo = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');

    const {
        isExact,
        fromDate,
        toDate,
        outletIds,
        departmentIds,
        setFromDate,
        setToDate,
        isSearching,
        setIsSearching
    } = useDashboardFilterContext();
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState([]);

    useEffect(() => {
        onLoadData();
    }, []);

    useEffect(() => {
        isSearching && onLoadData();
    }, [isSearching]);

    const onLoadData = async () => {
        try {
            setLoading(true);
            const { data } = await onDiscountInfo(
                false,
                fromDate,
                toDate,
                outletIds,
                departmentIds
            );
            setData(data.result);
            // setFromDate(data.fromDate);
            // setToDate(data.toDate);
            setLoading(false);
            setIsSearching(false);
        } catch (error) {
            setLoading(true);
            console.log(error);
            setLoading(false);
        }
    };

    return (
        <GridItem
            rounded="xl"
            w="100%"
            minH="300px"
            h="auto"
            bg={bgColor}
            boxShadow="sm"
            p="30px 20px 40px 20px">
            <Heading fontSize="15px" align="center" bg="brand.400" color="white" p="8px 5px">
                Discount Info
            </Heading>

            {!loading ? (
                <Table variant="simple" w="100%" size="sm" className="">
                    <Thead>
                        <Tr>
                            <Th p="2" color="primary.900Text">
                                Discount
                            </Th>
                            <Th p="2" color="primary.900Text">
                                Quantity
                            </Th>
                            <Th p="2" color="primary.900Text">
                                Amount
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        {data.map((item, index) => (
                            <Tr key={index}>
                                <Td p="2">{item.Name}</Td>
                                <Td p="2">{roundCommaFormat(item.Count)}</Td>
                                <Td p="2">{commaFormat(item.Amount)}</Td>
                            </Tr>
                        ))}

                        {data.length > 0 && (
                            <Tr>
                                <Td p="2" fontWeight="semibold">
                                    Total
                                </Td>
                                <Td p="2" fontWeight="semibold">
                                    {roundCommaFormat(getTableTotal(data, 'Count'))}
                                </Td>
                                <Td p="2" fontWeight="semibold">
                                    {commaFormat(getTableTotal(data, 'Amount'))}
                                </Td>
                            </Tr>
                        )}
                    </Tbody>
                </Table>
            ) : (
                <Loading line={8} />
            )}
        </GridItem>
    );
};

export default DiscountInfo;
