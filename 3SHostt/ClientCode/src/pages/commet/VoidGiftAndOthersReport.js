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

const VoidGiftAndOthersReport = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const theadBg = useColorModeValue('brand.400', '#1A202C');
    const theadText = useColorModeValue('white', 'brand.400');

    const TdLine = () => {
        return (
            <Tr>
                <Td px="2">46940</Td>
                <Td px="2">Ukhiya</Td>
                <Td px="2"></Td>
                <Td px="2"></Td>
                <Td px="2"></Td>
                <Td px="2">24 Aug 2022</Td>
                <Td px="2">6:29 PM</Td>
                <Td px="2">6:29PM</Td>
                <Td px="2"></Td>
                <Td px="2">Paid, Running Order</Td>
                <Td px="2">6372199</Td>
                <Td px="2">Ukhia</Td>
                <Td px="2">24 Aug 22 - 10:34 PM</Td>
                <Td px="2">Cancel Order</Td>
                <Td px="2"></Td>
                <Td px="2">24 Aug 22 - 10:34 PM</Td>
                <Td px="2"></Td>
                <Td px="2">Bottle Water</Td>
                <Td px="2">2.00</Td>
                <Td px="2">14.29</Td>
                <Td px="2">28.57</Td>
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
                overflowX="scroll">
                <Heading size="md" textAlign="left" m="0 0 20px 0" color={brandBgText}>
                    Void, Gift & Others Report
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg="brand.400">
                        <Tr>
                            <Th p="2" color="white">
                                Ticket No
                            </Th>
                            <Th p="2" color="white">
                                Outlet
                            </Th>
                            <Th p="2" color="white">
                                Customers
                            </Th>
                            <Th p="2" color="white">
                                Tables
                            </Th>
                            <Th p="2" color="white">
                                Not In Use
                            </Th>
                            <Th p="2" color="white">
                                Date
                            </Th>
                            <Th p="2" color="white">
                                Opening
                            </Th>
                            <Th p="2" color="white">
                                Closing
                            </Th>
                            <Th p="2" color="white">
                                Note
                            </Th>
                            <Th p="2" color="white">
                                Ticket States
                            </Th>
                            <Th p="2" color="white">
                                Order Id
                            </Th>
                            <Th p="2" color="white">
                                Entry By
                            </Th>
                            <Th p="2" color="white">
                                Order Time
                            </Th>
                            <Th p="2" color="white">
                                Action Type
                            </Th>
                            <Th p="2" color="white">
                                Action By
                            </Th>
                            <Th p="2" color="white">
                                Action Time
                            </Th>
                            <Th p="2" color="white">
                                Reason
                            </Th>
                            <Th p="2" color="white">
                                Menu Item
                            </Th>
                            <Th p="2" color="white">
                                Quantity
                            </Th>
                            <Th p="2" color="white">
                                Price
                            </Th>
                            <Th p="2" color="white">
                                Total
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
                        <TdLine />
                    </Tbody>
                </Table>
            </Flex>
        </Layout>
    );
};

export default VoidGiftAndOthersReport;
