//@ts-check
import React from 'react';
import {
    Heading,
    Table,
    Flex,
    Thead,
    Tbody,
    Tr,
    Th,
    Td,
    useColorModeValue
} from '@chakra-ui/react';
import Layout from '../shared/Layout';
import FilterOption from '../shared/FilterOption';

const TicketsReport = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('black', 'white');
    const theadBg = useColorModeValue('lightgray', 'gray');
    const brandBgText = useColorModeValue('primary.900', 'white');

    const TdLine = () => {
        return (
            <Tr>
                <Td px="2">441902</Td>
                <Td px="2">24 Aug 22 - 10:34 PM</Td>
                <Td px="2">LK Tower-2</Td>
                <Td px="2">LK2</Td>
                <Td px="2">Abc </Td>
                <Td px="2">Def </Td>
                <Td px="2">Ghi</Td>
                <Td px="2">Jkl </Td>
                <Td px="2">1 Min</Td>
                <Td px="2">Closed</Td>
                <Td px="2">0</Td>
                <Td px="2">130.00</Td>
                <Td px="2">bKash</Td>
            </Tr>
        );
    };

    return (
        <Layout>
            <Flex
                direction="column"
                w="100%"
                h="calc(100vh - 100px)"
                bg={bgColor}
                p="20px 20px"
                boxShadow="sm"
                borderRadius="xl"
                overflowX="scroll"
                position="relative">
                <Heading size="md" textAlign="left" m="0 0 20px 0" color={brandBgText}>
                    Tickets Report
                </Heading>

                <FilterOption />

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg={theadBg}>
                        <Tr>
                            <Th p="2" color={textColor}>
                                Ticket No
                            </Th>
                            <Th p="2" color={textColor}>
                                Date
                            </Th>
                            <Th p="2" color={textColor}>
                                Department
                            </Th>
                            <Th p="2" color={textColor}>
                                User
                            </Th>
                            <Th p="2" color={textColor}>
                                Customer
                            </Th>
                            <Th p="2" color={textColor}>
                                Table
                            </Th>
                            <Th p="2" color={textColor}>
                                Waiter
                            </Th>
                            <Th p="2" color={textColor}>
                                Ticket Note
                            </Th>
                            <Th p="2" color={textColor}>
                                Duration
                            </Th>
                            <Th p="2" color={textColor}>
                                Status
                            </Th>
                            <Th p="2" color={textColor}>
                                Guest
                            </Th>
                            <Th p="2" color={textColor}>
                                Total
                            </Th>
                            <Th p="2" color={textColor}>
                                Payment
                            </Th>
                        </Tr>
                    </Thead>
                    <Tbody>
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                        <TdLine />
                    </Tbody>
                </Table>
            </Flex>
        </Layout>
    );
};

export default TicketsReport;
