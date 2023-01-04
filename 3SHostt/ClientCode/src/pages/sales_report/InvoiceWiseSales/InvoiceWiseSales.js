import React, { useEffect, useState } from 'react';
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
    useDisclosure,
    Checkbox
} from '@chakra-ui/react';
import Loading from '../../shared/Loading';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import FilterOption from '../../shared/FilterOption';
import ReportConsideredTime from '../../shared/ReportConsideredTime';
import { BiMessageSquareDetail, BiDetail } from 'react-icons/bi';
import DetailsModal from './DetailsModal';
import InvoiceModal from './InvoiceModal';
import { onInvoiceWiseSale } from '../../../services/sales-service';
import { useQuery } from 'react-query';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import NoDataFound from '../../shared/NoDataFound';

const InvoiceWiseSales = () => {
    // chakra
    const {
        isOpen: isOpenInvoice,
        onOpen: onOpenInvoice,
        onClose: onCloseInvoice
    } = useDisclosure();
    const {
        isOpen: isOpenDetails,
        onOpen: onOpenDetails,
        onClose: onCloseDetails
    } = useDisclosure();
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    // filter options
    const [isExact, setIsExact] = useState(true);
    const [isTicketOpen, setIsTicketOpen] = useState(false);
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [departmentIds, setDepartmentIds] = useState(0);
    const [outletIds, setOutletIds] = useState(0);
    const [isSearching, setIsSearching] = useState(false);
    const [ticketId, setTicketId] = useState();
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
        'invoiceWiseSale',
        () => onInvoiceWiseSale(isExact, isTicketOpen, fromDate, toDate, outletIds, departmentIds),
        { onSuccess, onError }
    );
    const loading = isLoading || isSearching;
    const tickets = data?.data?.result?.Tickets;

    return (
        <Layout>
            <Flex direction="column" bg={bgColor} p="30px" boxShadow="sm" borderRadius="xl">
                <Heading size="md" mb="5">
                    Invoice Wise Sales
                </Heading>

                <FilterOption
                    isApiCalling={loading}
                    setIsSearching={setIsSearching}
                    isExact={isExact}
                    isTicketOpen={isTicketOpen}
                    setIsTicketOpen={setIsTicketOpen}
                    setIsExact={setIsExact}
                    fromDate={fromDate}
                    setFromDate={setFromDate}
                    toDate={toDate}
                    setToDate={setToDate}
                    setDepartmentIds={setDepartmentIds}
                    setOutletIds={setOutletIds}
                    refetch={refetch}
                    isMultiOutlet={false}
                    isMultiDept={false}
                />

                <ReportConsideredTime fromDate={fromDate} toDate={toDate} />

                <ExportOptions
                    title="Invoice Wise Sales"
                    fromDate={fromDate}
                    toDate={toDate}
                    colSpan={3}
                    scale={0.6}
                    margin="20pt">
                    <h2>Hi</h2>
                </ExportOptions>

                <Flex overflow="auto" h="600px" direction="column">
                    {isOpenDetails && (
                        <DetailsModal
                            isOpen={isOpenDetails}
                            onClose={onCloseDetails}
                            ticketId={ticketId}
                        />
                    )}
                    {isOpenInvoice && (
                        <InvoiceModal
                            isOpen={isOpenInvoice}
                            onClose={onCloseInvoice}
                            ticketId={ticketId}
                        />
                    )}

                    {loading ? (
                        <Loading line={18} width="100%" />
                    ) : tickets.length === 0 ? (
                        <NoDataFound />
                    ) : (
                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead>
                                <Tr bg="primary.900">
                                    <Th p="2" color="white">
                                        Details
                                    </Th>
                                    <Th p="2" color="white">
                                        Invoice
                                    </Th>
                                    <Th p="2" color="white">
                                        Ticket Number
                                    </Th>
                                    <Th p="2" color="white">
                                        Department
                                    </Th>
                                    <Th p="2" color="white">
                                        Settled By
                                    </Th>
                                    <Th p="2" color="white">
                                        Customers
                                    </Th>
                                    <Th p="2" color="white">
                                        Tables
                                    </Th>
                                    <Th p="2" color="white">
                                        Waiter
                                    </Th>
                                    <Th p="2" color="white">
                                        Administrator
                                    </Th>
                                    <Th p="2" color="white">
                                        Not Used
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
                                        Minutes Spend
                                    </Th>
                                    <Th p="2" color="white">
                                        No Of Guest
                                    </Th>
                                    <Th p="2" color="white">
                                        Total Amount
                                    </Th>
                                    <Th p="2" color="white">
                                        Note
                                    </Th>
                                    <Th p="2" color="white">
                                        Is Closed
                                    </Th>
                                </Tr>
                            </Thead>

                            <Tbody>
                                {tickets.map((item, i) => (
                                    <Tr key={i}>
                                        <Td px="2">
                                            <BiDetail
                                                fontSize="19px"
                                                cursor="pointer"
                                                onClick={() => {
                                                    onOpenDetails();
                                                    setTicketId(item.id);
                                                }}
                                            />
                                        </Td>
                                        <Td px="2">
                                            <BiMessageSquareDetail
                                                fontSize="19px"
                                                cursor="pointer"
                                                onClick={() => {
                                                    onOpenInvoice();
                                                    setTicketId(item.id);
                                                }}
                                            />
                                        </Td>
                                        <Td px="2">{item.TicketNumber || '-'}</Td>
                                        <Td px="2">{item.Department_Name || '-'}</Td>
                                        <Td px="2">{item.SettledBy || '-'}</Td>
                                        <Td px="2">{item.Customers || '-'}</Td>
                                        <Td px="2">{item.Tables || '-'}</Td>
                                        <Td px="2">{item.Waiter || '-'}</Td>
                                        <Td px="2">{item.Administrators || '-'}</Td>
                                        <Td px="2">{item['Not Used'] || '-'}</Td>
                                        <Td px="2" minW="110px">
                                            {getFormattedDateTime(item.Date) || '-'}
                                        </Td>
                                        <Td px="2">{item.Opening || '-'}</Td>
                                        <Td px="2">{item.Closing || '-'}</Td>
                                        <Td px="2">{item.MinutesSpend || '-'}</Td>
                                        <Td px="2">{item.No_Of_Guests || '-'}</Td>
                                        <Td px="2">{item.TotalAmount || '-'}</Td>
                                        <Td px="2">{item.Note || '-'}</Td>
                                        <Td px="2">
                                            <Checkbox
                                                isChecked={item.Isclosed}
                                                isDisabled></Checkbox>
                                        </Td>
                                    </Tr>
                                ))}
                            </Tbody>
                        </Table>
                    )}
                </Flex>
            </Flex>
        </Layout>
    );
};

export default InvoiceWiseSales;
