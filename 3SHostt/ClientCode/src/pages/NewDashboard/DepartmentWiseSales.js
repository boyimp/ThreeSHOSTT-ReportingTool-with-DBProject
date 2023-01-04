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
import { commaFormat, getTableTotal, roundCommaFormat } from '../../services/_utils/index';
import Loading from '../shared/Loading';

const DepartmentWiseSales = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');

    const { departmentsLoading, departmentsData } = useDashboardFilterContext();

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
                Department Wise Sales
            </Heading>

            {!departmentsLoading ? (
                <Table variant="simple" w="100%" size="sm" className="">
                    <Thead>
                        <Tr>
                            <Th p="2" color="primary.900Text">
                                Deprt.
                            </Th>
                            <Th p="2" color="primary.900Text">
                                Ticket
                            </Th>
                            <Th p="2" color="primary.900Text">
                                Amount
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        {departmentsData.map((item, index) => (
                            <Tr key={index}>
                                <Td p="2">{item.Department}</Td>
                                <Td p="2">{roundCommaFormat(item.Quantity)}</Td>
                                <Td p="2">{commaFormat(item.Amount)}</Td>
                            </Tr>
                        ))}

                        {departmentsData.length > 0 && (
                            <Tr>
                                <Td p="2" fontWeight="semibold">
                                    Total
                                </Td>
                                <Td p="2" fontWeight="semibold">
                                    {roundCommaFormat(getTableTotal(departmentsData, 'Quantity'))}
                                </Td>
                                <Td p="2" fontWeight="semibold">
                                    {commaFormat(getTableTotal(departmentsData, 'Amount'))}
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

export default DepartmentWiseSales;
