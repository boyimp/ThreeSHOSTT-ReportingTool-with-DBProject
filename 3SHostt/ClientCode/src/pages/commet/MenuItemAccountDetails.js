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
    useColorModeValue,
    Button
} from '@chakra-ui/react';
import Layout from '../shared/Layout';

const MenuItemAccountDetails = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'white');
    const theadBg = useColorModeValue('brand.400', '#1A202C');
    const theadText = useColorModeValue('white', 'brand.400');

    const TdLine = () => {
        return (
            <Tr>
                <Td px="2">Cafe Drinks</Td>
                <Td px="2">247,179.00</Td>
                <Td px="2">45,781,094.07</Td>
                <Td px="2">-149,55232</Td>
                <Td px="2">-537.00</Td>
                <Td px="2">-3,090.20 </Td>
                <Td px="2">0.00</Td>
                <Td px="2">0.00 </Td>
                <Td px="2">0.00</Td>
                <Td px="2">2,046,514.45</Td>
                <Td px="2">469,762.48</Td>
                <Td px="2">0.00</Td>
                <Td px="2">88.00</Td>
                <Td px="2">-457.00</Td>
                <Td px="2">21,654,119.20</Td>
                <Td px="2">3,993,071.00</Td>
                <Td px="2">1,344,426.98</Td>
                <Td px="2">7,921,401.00</Td>
                <Td px="2">2,155,926.43</Td>
                <Td px="2">7,256,640.00</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
                <Td px="2">214,199.08</Td>
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
                    Menu Item Account Details
                </Heading>

                <Table variant="simple" w="100%" size="sm" className="tableBorder">
                    <Thead bg="brand.400">
                        <Tr>
                            <Th p="2" color="white">
                                Menu Group
                            </Th>
                            <Th p="2" color="white">
                                Quantity
                            </Th>
                            <Th p="2" color="white">
                                Sales
                            </Th>
                            <Th p="2" color="white">
                                Discount
                            </Th>
                            <Th p="2" color="white">
                                Special Discount
                            </Th>
                            <Th p="2" color="white">
                                Item Discount
                            </Th>
                            <Th p="2" color="white">
                                SD (10%)
                            </Th>
                            <Th p="2" color="white">
                                Vat (Mer 15%)
                            </Th>
                            <Th p="2" color="white">
                                Vat (Mer 7.5%)
                            </Th>
                            <Th p="2" color="white">
                                Vat (5%)
                            </Th>
                            <Th p="2" color="white">
                                Vat (10%)
                            </Th>
                            <Th p="2" color="white">
                                Vat (15%)
                            </Th>
                            <Th p="2" color="white">
                                Auto Round+
                            </Th>
                            <Th p="2" color="white">
                                Auto Round-
                            </Th>
                            <Th p="2" color="white">
                                Cash
                            </Th>
                            <Th p="2" color="white">
                                CBL
                            </Th>
                            <Th p="2" color="white">
                                DBBL
                            </Th>
                            <Th p="2" color="white">
                                UCBL
                            </Th>
                            <Th p="2" color="white">
                                EBL
                            </Th>
                            <Th p="2" color="white">
                                Food Panda
                            </Th>
                            <Th p="2" color="white">
                                Pathao Cash
                            </Th>
                            <Th p="2" color="white">
                                Pathao bKash
                            </Th>
                            <Th p="2" color="white">
                                HungrayNaki
                            </Th>
                            <Th p="2" color="white">
                                Room Service
                            </Th>
                            <Th p="2" color="white">
                                Management
                            </Th>
                            <Th p="2" color="white">
                                bKash
                            </Th>
                            <Th p="2" color="white">
                                Nagad
                            </Th>
                            <Th p="2" color="white">
                                bkash Total
                            </Th>
                            <Th p="2" color="white">
                                Total
                            </Th>
                            <Th p="2" color="white">
                                Void
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
                        <TdLine />
                        <TdLine />
                        <TdLine />

                        <Tr>
                            <Th px="2">Grand Total</Th>
                            <Th px="2">247,179.00</Th>
                            <Th px="2">45,781,094.07</Th>
                            <Th px="2">-149,55232</Th>
                            <Th px="2">-537.00</Th>
                            <Th px="2">-3,090.20 </Th>
                            <Th px="2">0.00</Th>
                            <Th px="2">0.00 </Th>
                            <Th px="2">0.00</Th>
                            <Th px="2">2,046,514.45</Th>
                            <Th px="2">469,762.48</Th>
                            <Th px="2">0.00</Th>
                            <Th px="2">88.00</Th>
                            <Th px="2">-457.00</Th>
                            <Th px="2">21,654,119.20</Th>
                            <Th px="2">3,993,071.00</Th>
                            <Th px="2">1,344,426.98</Th>
                            <Th px="2">7,921,401.00</Th>
                            <Th px="2">2,155,926.43</Th>
                            <Th px="2">7,256,640.00</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                            <Th px="2">214,199.08</Th>
                        </Tr>
                    </Tbody>
                </Table>
            </Flex>
        </Layout>
    );
};

export default MenuItemAccountDetails;
