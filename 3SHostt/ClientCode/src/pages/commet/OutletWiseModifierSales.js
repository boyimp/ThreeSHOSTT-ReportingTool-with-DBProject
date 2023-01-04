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

const OutletWiseModifierSales = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const brandBgText = useColorModeValue('primary.900', 'white');
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
                    Outlet Wise Modifier Sales
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg={tableCaptionBg}>
                        <Tr>
                            <Th p="2" color={brandBgText} textAlign="center" colSpan="3"></Th>
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
                                Modifier Group
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Modifier Name
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Unit Price
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Tag Qty
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Tag Price
                            </Th>

                            <Th p="2" bg="primary.900" color="white">
                                Tag Qty
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Tag Price
                            </Th>

                            <Th p="2" bg="brand.400" color="white">
                                Tag Qty
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Tag Price
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        <Tr>
                            <Td px="2" rowSpan={3}>
                                Modifier Cafe & Frozen drinks
                            </Td>
                            <Td px="2">Blended</Td>
                            <Td px="2">17.73 </Td>
                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Butter</Td>
                            <Td px="2">22.73 </Td>
                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Butter</Td>
                            <Td px="2">21.55 </Td>
                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>
                        </Tr>

                        <Tr>
                            <Td px="2" rowSpan={3}>
                                Modifier Food
                            </Td>
                            <Td px="2">Chili Cream Cheese</Td>
                            <Td px="2">24.73 </Td>
                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Chili Cream Cheese</Td>
                            <Td px="2">25.73 </Td>
                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Extra Sauce</Td>
                            <Td px="2">17.12 </Td>
                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>

                            <Td px="2">4</Td>
                            <Td px="2">114.29 </Td>
                        </Tr>
                    </Tbody>
                </Table>
            </Flex>
        </Layout>
    );
};

export default OutletWiseModifierSales;
