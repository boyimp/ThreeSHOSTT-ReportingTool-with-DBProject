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
import { onProductWiseSalesInfo } from '../../services/dashboard-service';
import Loading from '../shared/Loading';
import { commaFormat, roundCommaFormat } from '../../services/_utils/index';

const TopSalesByProduct = () => {
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
            const { data } = await onProductWiseSalesInfo(
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
                Top Sales By Product
            </Heading>

            {!loading ? (
                <Table variant="simple" w="100%" size="sm" className="">
                    <Thead>
                        <Tr>
                            <Th p="2" color="primary.900Text">
                                Product
                            </Th>
                            <Th p="2" color="primary.900Text">
                                Quantity
                            </Th>
                            <Th p="2" color="primary.900Text">
                                %
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        {data.map((item, index) => (
                            <Tr key={index}>
                                <Td p="2">{item.Product}</Td>
                                <Td p="2">{roundCommaFormat(item.Quantity)}</Td>
                                <Td p="2">{commaFormat(item['Gross(%)'])}</Td>
                            </Tr>
                        ))}
                    </Tbody>
                </Table>
            ) : (
                <Loading line={8} />
            )}
        </GridItem>
    );
};

export default TopSalesByProduct;
