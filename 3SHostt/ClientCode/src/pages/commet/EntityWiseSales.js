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

const EntityWiseSales = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const theadBg = useColorModeValue('brand.400', '#1A202C');
    const theadText = useColorModeValue('white', 'brand.400');

    const TdLine = () => {
        return (
            <Tr>
                <Td px="2">24 Aug 22 - 10:34 PM</Td>
                <Td px="2">411410</Td>
                <Td px="2">LK Tower-2</Td>
                <Td px="2">Staff Benefits </Td>
                <Td px="2">Free drinks</Td>
                <Td px="2">Abdul Ahad Rabbi </Td>
                <Td px="2">Latte</Td>
                <Td px="2">Regular</Td>
                <Td px="2">1</Td>
                <Td px="2">LKT-2 Café</Td>
                <Td px="2">Café</Td>
                <Td px="2">3035</Td>
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
                    Entity Wise Sales
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg="brand.400">
                        <Tr>
                            <Th p="2" color="white">
                                Date
                            </Th>
                            <Th p="2" color="white">
                                Ticket No
                            </Th>
                            <Th p="2" color="white">
                                Outlet
                            </Th>
                            <Th p="2" color="white">
                                Category
                            </Th>
                            <Th p="2" color="white">
                                Order Type
                            </Th>
                            <Th p="2" color="white">
                                Name
                            </Th>
                            <Th p="2" color="white">
                                Menu
                            </Th>
                            <Th p="2" color="white">
                                Portion
                            </Th>
                            <Th p="2" color="white">
                                Qty
                            </Th>
                            <Th p="2" color="white">
                                Department/Unit
                            </Th>
                            <Th p="2" color="white">
                                Group
                            </Th>
                            <Th p="2" color="white">
                                Id
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

export default EntityWiseSales;
