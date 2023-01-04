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

const TicketWisePaymentsReport2 = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const theadBg = useColorModeValue('brand.400', '#1A202C');
    const theadText = useColorModeValue('white', 'brand.400');

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
                h="fit-content"
                bg={bgColor}
                p="20px 20px"
                boxShadow="sm"
                borderRadius="xl"
                overflowX="scroll">
                <Heading size="md" textAlign="left" m="0 0 20px 0" color={brandBgText}>
                    Ticket Wise Payments Report 2
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg={theadBg}>
                        <Tr>
                            <Th p="2" color={theadText}>
                                Ticket No
                            </Th>
                            <Th p="2" color={theadText} minW="89px">
                                Date
                            </Th>
                            <Th p="2" color={theadText}>
                                Department
                            </Th>
                            <Th p="2" color={theadText}>
                                User
                            </Th>
                            <Th p="2" color={theadText}>
                                Customer
                            </Th>
                            <Th p="2" color={theadText}>
                                Table
                            </Th>
                            <Th p="2" color={theadText}>
                                Waiter
                            </Th>
                            <Th p="2" color={theadText}>
                                Ticket Note
                            </Th>
                            <Th p="2" color={theadText}>
                                Duration
                            </Th>
                            <Th p="2" color={theadText}>
                                Status
                            </Th>
                            <Th p="2" color={theadText}>
                                Guest
                            </Th>
                            <Th p="2" color={theadText}>
                                Total
                            </Th>
                            <Th p="2" color={theadText}>
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

export default TicketWisePaymentsReport2;
