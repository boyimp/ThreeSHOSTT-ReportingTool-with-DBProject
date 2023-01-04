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

const MonthWiseItemSales = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const theadBg = useColorModeValue('brand.400', '#1A202C');
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
                    Month Wise Item Sales
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg={tableCaptionBg}>
                        <Tr>
                            <Th p="2" color={brandBgText} colSpan="3"></Th>
                            <Th p="2" color={brandBgText} colSpan="2" textAlign="center">
                                February-2022
                            </Th>
                            <Th p="2" color={brandBgText} colSpan="2" textAlign="center">
                                March-2022
                            </Th>
                            <Th p="2" color={brandBgText} colSpan="2" textAlign="center">
                                Grand Total
                            </Th>
                        </Tr>
                    </Thead>

                    <Thead bg="black">
                        <Tr>
                            <Th p="2" bg="primary.900" color="white">
                                Category
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Menu Item Name
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Portion Name
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Quantity
                            </Th>

                            <Th p="2" bg="brand.400" color="white">
                                Value
                            </Th>
                            <Th p="2" bg="primary.900" color="white">
                                Quantity
                            </Th>

                            <Th p="2" bg="primary.900" color="white">
                                Value
                            </Th>
                            <Th p="2" bg="brand.400" color="white">
                                Quantity
                            </Th>

                            <Th p="2" bg="brand.400" color="white">
                                Value
                            </Th>
                        </Tr>
                    </Thead>

                    <Tbody>
                        <Tr>
                            <Td px="2" rowSpan={3}>
                                Café Drinks
                            </Td>
                            <Td px="2">Americano</Td>
                            <Td px="2">Large</Td>
                            <Td px="2">7.00</Td>
                            <Td px="2">22,790.70</Td>

                            <Td px="2">9.00</Td>
                            <Td px="2">29,302.33</Td>

                            <Td px="2">16.00</Td>
                            <Td px="2">60,302.33</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Americano</Td>
                            <Td px="2">Regular</Td>
                            <Td px="2">7.00</Td>
                            <Td px="2">22,790.70</Td>

                            <Td px="2">9.00</Td>
                            <Td px="2">29,302.33</Td>

                            <Td px="2">16.00</Td>
                            <Td px="2">60,302.33</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Bitter Lemon</Td>
                            <Td px="2">Normal</Td>
                            <Td px="2">7.00</Td>
                            <Td px="2">22,790.70</Td>

                            <Td px="2">9.00</Td>
                            <Td px="2">29,302.33</Td>

                            <Td px="2">16.00</Td>
                            <Td px="2">60,302.33</Td>
                        </Tr>

                        <Tr>
                            <Td px="2" rowSpan={3}>
                                Coffee Equipment
                            </Td>
                            <Td px="2">Large French press</Td>
                            <Td px="2">Normal</Td>
                            <Td px="2">7.00</Td>
                            <Td px="2">22,790.70</Td>

                            <Td px="2">9.00</Td>
                            <Td px="2">29,302.33</Td>

                            <Td px="2">16.00</Td>
                            <Td px="2">60,302.33</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Shot glass</Td>
                            <Td px="2">Normal</Td>
                            <Td px="2">7.00</Td>
                            <Td px="2">22,790.70</Td>

                            <Td px="2">9.00</Td>
                            <Td px="2">29,302.33</Td>

                            <Td px="2">16.00</Td>
                            <Td px="2">60,302.33</Td>
                        </Tr>
                        <Tr>
                            <Td px="2">Small French press</Td>
                            <Td px="2">Normal</Td>
                            <Td px="2">7.00</Td>
                            <Td px="2">22,790.70</Td>

                            <Td px="2">9.00</Td>
                            <Td px="2">29,302.33</Td>

                            <Td px="2">16.00</Td>
                            <Td px="2">60,302.33</Td>
                        </Tr>
                    </Tbody>
                </Table>
            </Flex>
        </Layout>
    );
};

export default MonthWiseItemSales;
