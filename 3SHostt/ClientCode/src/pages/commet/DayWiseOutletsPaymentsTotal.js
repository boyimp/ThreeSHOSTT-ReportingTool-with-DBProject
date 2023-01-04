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

const DayWiseOutletsPaymentsTotal = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const theadBg = useColorModeValue('brand.400', '#1A202C');
    const theadText = useColorModeValue('white', 'brand.400');

    const TdLine = () => {
        return (
            <Tr>
                <Td px="2">24 Aug 22 - 10:34 PM</Td>
                <Td px="2">Main Cafe</Td>
                <Td px="2">bKash</Td>
                <Td px="2">6,000.00</Td>
            </Tr>
        );
    };

    return (
        <Layout>
            <Flex
                direction="column"
                w="100%"
                // h="calc(100vh - 100px)"
                bg={bgColor}
                p="20px 20px"
                boxShadow="sm"
                borderRadius="xl"
                overflowX="scroll">
                <Heading size="md" textAlign="left" m="0 0 20px 0" color={brandBgText}>
                    Day Wise Outlets Payments
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg="brand.400">
                        <Tr>
                            <Th p="2" color="white">
                                Work Period Date Time
                            </Th>
                            <Th p="2" color="white">
                                Outlets
                            </Th>
                            <Th p="2" color="white">
                                Payments Name
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

export default DayWiseOutletsPaymentsTotal;
