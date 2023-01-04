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

const OutletWiseItemSales = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const tableCaptionBg = useColorModeValue('whitesmoke', 'whiteAlpha.50');
    const tableCaptionText = useColorModeValue('gray', 'gray');

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
                    Outlet Wise Item Sales
                </Heading>

                {/* Sales & Quantity By Item Groups */}
                <Table variant="simple" w="100%" size="sm" className="tableBorder" mb="10">
                    <Thead bg={tableCaptionBg}>
                        <Tr>
                            <Th p="3" color={tableCaptionText} textAlign="center" colSpan="13">
                                Sales & Quantity By Item Groups
                            </Th>
                        </Tr>
                    </Thead>

                    <Thead bg={tableCaptionBg}>
                        <Tr>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="1"></Th>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="4">
                                LK Tower-2
                            </Th>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="4">
                                City Scape
                            </Th>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="4">
                                Grand Total
                            </Th>
                        </Tr>
                    </Thead>

                    <Thead bg="black">
                        <Tr>
                            <Th p="2" bg="primary.900" color="white" minW="150px">
                                Item Group Names
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Quantity
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Sales
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Quantity%
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Sales%
                            </Th>

                            <Th p="2" bg="primary.900" color="white">
                                Quantity
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Sales
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Quantity%
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Sales%
                            </Th>

                            <Th p="2" bg="brand.400" color="white">
                                Quantity
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Sales
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Quantity%
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Sales%
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        <Tr>
                            <Td px="2">Café Drinks</Td>
                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Coffee Beans</Td>
                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Coffee Equipment</Td>
                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>
                        </Tr>

                        <Tr>
                            <Td px="2">Coffee Events</Td>
                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Coupons</Td>
                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Food</Td>
                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>

                            <Td px="2">17058.00</Td>
                            <Td px="2">3016884.77</Td>
                            <Td px="2">41.56</Td>
                            <Td px="2">38.00 </Td>
                        </Tr>
                    </Tbody>
                </Table>

                {/* Sales By Item Wise */}
                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg={tableCaptionBg}>
                        <Tr>
                            <Th p="3" color={tableCaptionText} textAlign="center" colSpan="13">
                                Sales By Item Wise
                            </Th>
                        </Tr>
                    </Thead>

                    <Thead bg={tableCaptionBg}>
                        <Tr>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="4"></Th>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="2">
                                LK Tower-2
                            </Th>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="2">
                                City Scape
                            </Th>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="2">
                                Grand Total
                            </Th>
                        </Tr>
                    </Thead>

                    <Thead bg="black">
                        <Tr>
                            <Th p="2" bg="primary.900" color="white">
                                Item Group Names
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
                            <Td px="2" rowSpan="6">
                                Café Drinks
                            </Td>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>
                        </Tr>

                        <Tr>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">263.64</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>

                            <Td px="2">26.00</Td>
                            <Td px="2">6763.63</Td>
                        </Tr>
                    </Tbody>
                </Table>
            </Flex>
        </Layout>
    );
};

export default OutletWiseItemSales;
