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

const OutletWiseItemSales_Modified = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const theadBg = useColorModeValue('brand.400', '#1A202C');
    const outletHeadBg = useColorModeValue('whitesmoke', '');
    const theadText = useColorModeValue('white', 'brand.400');
    const tableCaptionBg = useColorModeValue('whitesmoke', 'whiteAlpha.50');

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
                    Outlet Wise Item Sales Modified
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg={tableCaptionBg}>
                        <Tr>
                            <Th p="2" color={brandBgText} colSpan="4"></Th>
                            <Th p="2" color={brandBgText} colSpan="2" textAlign="center">
                                Ascott Palace
                            </Th>
                            <Th p="2" color={brandBgText} colSpan="2" textAlign="center">
                                Borak Mehnur
                            </Th>
                            <Th p="2" color={brandBgText} colSpan="2" textAlign="center">
                                Grand Total
                            </Th>
                        </Tr>
                    </Thead>

                    <Thead bg="black">
                        <Tr>
                            <Th p="2" bg="primary.900" color="white">
                                Menu Group Names
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Menu Items
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Potion Name
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Unit Price
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Quantity
                            </Th>

                            <Th p="2" bg="brand.400" color="white">
                                Amount
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Quantity
                            </Th>

                            <Th p="2" bg="primary.900" color="white">
                                Amount
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Quantity
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Amount
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        <Tr>
                            <Td px="2" rowSpan={6}>
                                Café Drinks
                            </Td>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">52.00</Td>
                            <Td px="2">19763.63 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Americano</Td>
                            <Td px="2">Regular</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">52.00</Td>
                            <Td px="2">19763.63 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Bitter Lemon</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">52.00</Td>
                            <Td px="2">19763.63 </Td>
                        </Tr>

                        <Tr>
                            <Td px="2">Bitter Lemon</Td>
                            <Td px="2">Regular</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">52.00</Td>
                            <Td px="2">19763.63 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Caramel Chocolate</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">52.00</Td>
                            <Td px="2">19763.63 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Caramel Chocolate</Td>
                            <Td px="2">Regular</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63 </Td>

                            <Td px="2">52.00</Td>
                            <Td px="2">19763.63 </Td>
                        </Tr>
                    </Tbody>
                </Table>
            </Flex>
        </Layout>
    );
};

export default OutletWiseItemSales_Modified;
