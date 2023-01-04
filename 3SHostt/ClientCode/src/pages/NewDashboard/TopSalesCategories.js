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
import React from 'react';
import { useDashboardFilterContext } from '../../context/DashboardFilterContext';
import Loading from '../shared/Loading';
import { commaFormat } from '../../services/_utils/index';

const TopSalesCategories = () => {
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    // get global data from- DashboardFilter
    const { categoriesLoading, categoriesData } = useDashboardFilterContext();

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
                Top Sales Categories
            </Heading>

            {!categoriesLoading ? (
                <Table variant="simple" w="100%" size="sm" className="">
                    <Thead>
                        <Tr>
                            <Th p="2" color="primary.900Text">
                                Item
                            </Th>
                            <Th p="2" color="primary.900Text">
                                Total
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        {categoriesData.map((item, index) => (
                            <Tr key={index}>
                                <Td p="2">{item.ProductCategory}</Td>
                                <Td p="2">{commaFormat(item.GrossAmount)}</Td>
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

export default TopSalesCategories;
