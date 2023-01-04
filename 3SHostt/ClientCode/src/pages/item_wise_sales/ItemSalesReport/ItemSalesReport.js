import React, { useState } from 'react';
import {
    Heading,
    Flex,
    useColorModeValue,
    Table,
    Thead,
    Tr,
    Th,
    Tbody,
    Td,
    useDisclosure
} from '@chakra-ui/react';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import FilterOption from '../../shared/FilterOption';
import ReportConsideredTime from '../../shared/ReportConsideredTime';
import { onItemSalesReport } from '../../../services/item-wise-sales-services';
import { useQuery } from 'react-query';
import Loading from '../../shared/Loading';
import { commaFormat, getTableTotal } from '../../../services/_utils';
import { BiDetail } from 'react-icons/bi';
import NoDataFound from '../../shared/NoDataFound';
import DetailsModal from './DetailsModal';

const ItemSalesReport = () => {
    // chakra
    const { isOpen, onOpen, onClose } = useDisclosure();
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    // filter options
    const [isExact, setIsExact] = useState(true);
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [departmentIds, setDepartmentIds] = useState(0);
    const [outletIds, setOutletIds] = useState(0);
    const [categoriesIds, setCategoriesIds] = useState(['']);
    const [menuItemIds, setMenuItemIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    const [selectedItem, setSelectedItem] = useState();
    // data query
    const onSuccess = (data) => {
        setFromDate(data.data.fromDate.slice(0, 16));
        setToDate(data.data.toDate.slice(0, 16));
        setIsSearching(false);
    };
    const onError = (err) => {
        console.log(err);
        setIsSearching(false);
    };
    const { data, isLoading, refetch } = useQuery(
        'itemSalesReport',
        () =>
            onItemSalesReport(
                isExact,
                fromDate,
                toDate,
                outletIds,
                departmentIds,
                categoriesIds,
                menuItemIds
            ),
        { onSuccess, onError }
    );
    const loading = isLoading || isSearching;
    const result = data?.data?.result;

    return (
        <Layout>
            <Flex direction="column" bg={bgColor} p="30px" boxShadow="sm" borderRadius="xl">
                <Heading size="md" mb="5">
                    Item Sales Report
                </Heading>

                <FilterOption
                    isApiCalling={loading}
                    setIsSearching={setIsSearching}
                    isExact={isExact}
                    setIsExact={setIsExact}
                    fromDate={fromDate}
                    setFromDate={setFromDate}
                    toDate={toDate}
                    setToDate={setToDate}
                    setDepartmentIds={setDepartmentIds}
                    setOutletIds={setOutletIds}
                    setCategoriesIds={setCategoriesIds}
                    setMenuItemIds={setMenuItemIds}
                    refetch={refetch}
                    isMultiOutlet={false}
                    isMultiDept={false}
                />

                <ReportConsideredTime fromDate={fromDate} toDate={toDate} />

                <ExportOptions
                    title="Work Period Details"
                    fromDate={fromDate}
                    toDate={toDate}
                    colSpan={3}
                    scale={0.6}
                    margin="20pt">
                    {/* <ExportWorkPeriod /> */}
                </ExportOptions>

                <Flex overflow="auto" h="600px" direction="column">
                    {isOpen && (
                        <DetailsModal isOpen={isOpen} onClose={onClose} item={selectedItem} />
                    )}

                    {loading ? (
                        <Loading line={18} width="100%" />
                    ) : result.length === 0 ? (
                        <NoDataFound />
                    ) : (
                        <Table
                            variant="simple"
                            w="100%"
                            size="sm"
                            className="tableBorder"
                            overflowX="auto">
                            <Thead>
                                <Tr bg="primary.900">
                                    <Th py="2" color="white">
                                        Details
                                    </Th>
                                    <Th p="2" color="white">
                                        Department Name
                                    </Th>
                                    <Th p="2" color="white">
                                        Group Name
                                    </Th>
                                    <Th p="2" color="white">
                                        Item Name
                                    </Th>
                                    <Th p="2" color="white">
                                        Portion Name
                                    </Th>
                                    <Th p="2" color="white">
                                        Price
                                    </Th>
                                    <Th p="2" color="white">
                                        Quantity
                                    </Th>
                                    <Th p="2" color="white">
                                        Net Amount
                                    </Th>
                                    <Th p="2" color="white">
                                        Line Item Value
                                    </Th>
                                    <Th p="2" color="white">
                                        Gross
                                    </Th>
                                </Tr>
                            </Thead>

                            <Tbody>
                                {result?.map((item) => (
                                    <Tr key={item.ItemId}>
                                        <Td px="2">
                                            <BiDetail
                                                fontSize="19px"
                                                cursor="pointer"
                                                onClick={() => {
                                                    onOpen();
                                                    setSelectedItem(item);
                                                }}
                                            />
                                        </Td>
                                        <Td px="2">{item.DepartmentName || '-'}</Td>
                                        <Td px="2">{item.GroupName || '-'}</Td>
                                        <Td px="2">{item.ItemName || '-'}</Td>
                                        <Td px="2">{item.PortionName || '-'}</Td>
                                        <Td px="2">{commaFormat(item.Price) || '-'}</Td>
                                        <Td px="2">{item.Quantity || '-'}</Td>
                                        <Td px="2">{commaFormat(item.NetAmount) || '-'}</Td>
                                        <Td px="2">{commaFormat(item.LineItemValue) || '-'}</Td>
                                        <Td px="2">{commaFormat(item.Gross) || '-'}</Td>
                                    </Tr>
                                ))}
                                <Tr>
                                    <Td px="2"></Td>
                                    <Td px="2"></Td>
                                    <Td px="2"></Td>
                                    <Td px="2"></Td>
                                    <Td px="2"></Td>
                                    <Td px="2">Totals</Td>
                                    <Td px="2">{commaFormat(getTableTotal(result, 'Quantity'))}</Td>
                                    <Td px="2">
                                        {commaFormat(getTableTotal(result, 'NetAmount'))}
                                    </Td>
                                    <Td px="2">
                                        {commaFormat(getTableTotal(result, 'LineItemValue'))}
                                    </Td>
                                    <Td px="2">{commaFormat(getTableTotal(result, 'Gross'))}</Td>
                                </Tr>
                            </Tbody>
                        </Table>
                    )}
                </Flex>
            </Flex>
        </Layout>
    );
};

export default ItemSalesReport;
