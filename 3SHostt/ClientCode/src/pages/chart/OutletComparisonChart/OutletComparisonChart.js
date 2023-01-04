//@ts-check
import React, { useState, useEffect } from 'react';
import {
    Box,
    Flex,
    Heading,
    Table,
    Tbody,
    Td,
    Text,
    Th,
    Thead,
    Tr,
    useBreakpointValue,
    useColorModeValue
} from '@chakra-ui/react';
import ReactApexChart from 'react-apexcharts';
import {
    onOutletWiseSales,
    onOutletAndPaymentMethodWiseSales,
    onPaymentType
} from '../../../services/dashboard-service';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import { commaFormat, getTableTotal } from '../../../services/_utils/index';
import Loading from '../../shared/Loading';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import { exportTable } from '../../shared/dataExport/ExportStyle';
import FilterOption from '../../shared/FilterOption';

const OutletComparisonChart = () => {
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'gray.200');
    const tableCaptionBg = useColorModeValue('whitesmoke', 'whiteAlpha.50');
    // filter options
    const [isExact, setIsExact] = useState(true);
    const [fromDate, setFromDate] = useState(null);
    const [toDate, setToDate] = useState(null);
    const [outletIds, setOutletIds] = useState([0]);
    const [departmentIds, setDepartmentIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    // data
    const [loading, setLoading] = useState(true);
    const [outletWiseSalesData, setOutletWiseSalesData] = useState([]);
    const [paymentMethodWiseSalesData, setPaymentMethodWiseSalesData] = useState([]);
    const [paymentTypes, setPaymentTypes] = useState([]);
    // customise data
    const [outletNames, setOutletNames] = useState([]);
    const [outletGross, setOutletGross] = useState([]);

    const isLargeScreen = useBreakpointValue({
        lg: true,
        '2xl': true,
        md: false,
        sm: false,
        base: false
    });

    const onLoadData = () => {
        setLoading(true);

        onOutletWiseSales(isExact, fromDate, toDate, outletIds, departmentIds)
            .then((res) => {
                setOutletWiseSalesData(res.data.result);
                setFromDate(res.data.fromDate.slice(0, 16));
                setToDate(res.data.toDate.slice(0, 16));
                setLoading(false);
                setIsSearching(false);
            })
            .catch((err) => {
                console.log(err);
                setLoading(false);
                setIsSearching(false);
            });

        onOutletAndPaymentMethodWiseSales(isExact, fromDate, toDate, outletIds, departmentIds)
            .then((res) => {
                setPaymentMethodWiseSalesData(res.data.result);
                setIsSearching(false);
                setLoading(false);
            })
            .catch((err) => {
                console.log(err);
                setLoading(false);
                setIsSearching(false);
            });

        onPaymentType()
            .then((res) => {
                setPaymentTypes(res.data);
            })
            .catch((err) => {
                console.log(err);
            });
    };

    useEffect(() => {
        onLoadData();
    }, []);

    useEffect(() => {
        isSearching && onLoadData();
    }, [isSearching]);

    useEffect(() => {
        setOutletNames(outletWiseSalesData.map((item) => item.OutletName));
        setOutletGross(outletWiseSalesData.map((item) => Math.round(item.GrossAmount)));
    }, [outletWiseSalesData, paymentMethodWiseSalesData]);

    const series = outletGross;
    const options = {
        colors: [
            '#00355f',
            '#FA7242',
            '#94B49F',
            '#513252',
            '#a42102',
            '#da7701',
            '#a8a8a8',
            '#F79489',
            '#05445E',
            '#116530',
            '#613659',
            '#D67BA8'
        ],
        chart: {
            type: 'donut',
            foreColor: textColor
        },
        plotOptions: {
            pie: {
                donut: {
                    labels: {
                        show: true
                    }
                }
            }
        },
        legend: {
            show: isLargeScreen ? true : false,
            markers: {
                radius: 0
            }
        },
        labels: outletNames,
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return parseInt(val) + '%';
            },
            dropShadow: {
                enabled: false
            }
        },
        responsive: [
            {
                breakpoint: 480,
                options: {
                    chart: {
                        width: isLargeScreen ? '200' : '100%'
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        ]
    };

    return (
        <Layout>
            <Flex direction="column" bg={bgColor} p="30px" boxShadow="sm" borderRadius="xl">
                <Heading size="md" mb="5">
                    Outlet Wise Comparison
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
                    setOutletIds={setOutletIds}
                    setDepartmentIds={setDepartmentIds}
                />

                <Text fontSize="14px" mb="5">
                    Work period considered from <strong>{getFormattedDateTime(fromDate)}</strong> to{' '}
                    <strong>{getFormattedDateTime(toDate)}</strong>
                </Text>

                {!loading ? (
                    <Flex mb="5">
                        <ReactApexChart
                            options={options}
                            series={series}
                            type="donut"
                            width="500px"
                        />
                    </Flex>
                ) : (
                    <Box w="100%">
                        <Loading line={8} mb="10" />
                    </Box>
                )}

                {!loading ? (
                    <>
                        <ExportOptions
                            title="Outlet Comparison"
                            fromDate={fromDate}
                            toDate={toDate}
                            colSpan={outletNames.length <= 3 ? 3 : outletNames.length + 1}
                            scale={0.6}
                            margin="20pt">
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th} colSpan="3">
                                    Sales Information
                                </th>
                            </tr>
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}>Outlet</th>
                                <th style={exportTable.th}>Net</th>
                                <th style={exportTable.th}>Gross</th>
                            </tr>
                            {outletWiseSalesData.map((item, index) => (
                                <tr style={exportTable.tr} key={index}>
                                    <td style={exportTable.td}>{item?.OutletName}</td>
                                    <td style={exportTable.td}>{commaFormat(item?.NetAmount)}</td>
                                    <td style={exportTable.td}>{commaFormat(item?.GrossAmount)}</td>
                                </tr>
                            ))}
                            <tr style={exportTable.tr}>
                                <td style={exportTable.td}>Total</td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(outletWiseSalesData, 'NetAmount'))}
                                </td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(outletWiseSalesData, 'GrossAmount'))}
                                </td>
                            </tr>

                            {/* Income Information */}
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th} colSpan={outletNames.length + 1}>
                                    Income Information
                                </th>
                            </tr>
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}>Payment</th>
                                {outletNames.map((item, index) => (
                                    <th style={exportTable.th} key={index}>
                                        {item}
                                    </th>
                                ))}
                            </tr>
                            {paymentTypes.map((payment, index) => {
                                return (
                                    <tr style={exportTable.tr} key={index}>
                                        <td style={exportTable.td}>{payment.Name}</td>

                                        {outletNames.map((outlet, index) => {
                                            var targetRow = paymentMethodWiseSalesData.find(
                                                (x) =>
                                                    x.Name === outlet &&
                                                    x.PaymentMethod === payment.Name
                                            );
                                            return (
                                                <td style={exportTable.td} key={index}>
                                                    {commaFormat(targetRow?.Amount || 0)}
                                                </td>
                                            );
                                        })}
                                    </tr>
                                );
                            })}
                            <tr style={exportTable.tr}>
                                <td style={exportTable.td}>Total</td>
                                {outletNames.map((outlet, index) => {
                                    const outletsData = paymentMethodWiseSalesData.filter(
                                        (item) => item.Name === outlet
                                    );
                                    let outletTotal = 0;
                                    outletsData.map((data) => (outletTotal += data.Amount));
                                    return (
                                        <td style={exportTable.td} key={index}>
                                            {commaFormat(outletTotal)}
                                        </td>
                                    );
                                })}
                            </tr>
                        </ExportOptions>

                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead bg={tableCaptionBg}>
                                <Tr>
                                    <Th p="2" color={brandBgText} colSpan="3" textAlign="center">
                                        Sales Information
                                    </Th>
                                </Tr>
                            </Thead>

                            <Thead bg="black">
                                <Tr>
                                    <Th p="2" bg="primary.900" color="white">
                                        Outlet
                                    </Th>
                                    <Th p="2" bg="primary.900" color="white">
                                        Net
                                    </Th>
                                    <Th p="2" bg="primary.900" color="white">
                                        Gross
                                    </Th>
                                </Tr>
                            </Thead>

                            <Tbody>
                                {outletWiseSalesData.map((item, index) => (
                                    <Tr key={index}>
                                        <Td px="2">{item?.OutletName}</Td>
                                        <Td px="2">{commaFormat(item?.NetAmount)}</Td>
                                        <Td px="2">{commaFormat(item?.GrossAmount)}</Td>
                                    </Tr>
                                ))}

                                <Tr>
                                    <Td px="2" fontWeight="semibold">
                                        Total
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(
                                            getTableTotal(outletWiseSalesData, 'NetAmount')
                                        )}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(
                                            getTableTotal(outletWiseSalesData, 'GrossAmount')
                                        )}
                                    </Td>
                                </Tr>
                            </Tbody>
                        </Table>
                    </>
                ) : (
                    <Loading line={6} mb="10" />
                )}

                {!loading ? (
                    <Table variant="simple" w="100%" size="sm" className="tableBorder">
                        <Thead bg={tableCaptionBg}>
                            <Tr>
                                <Th
                                    p="2"
                                    color={brandBgText}
                                    colSpan={paymentTypes.length + 1}
                                    textAlign="center">
                                    Income Information
                                </Th>
                            </Tr>
                        </Thead>

                        <Thead bg="black">
                            <Tr>
                                <Th p="2" bg="primary.900" color="white">
                                    Payment
                                </Th>
                                {outletNames.map((item, index) => (
                                    <Th p="2" bg="primary.900" color="white" key={index}>
                                        {item}
                                    </Th>
                                ))}
                            </Tr>
                        </Thead>

                        <Tbody>
                            {paymentTypes.map((payment, index) => {
                                return (
                                    <Tr key={index}>
                                        <Td px="2">{payment.Name}</Td>

                                        {outletNames.map((outlet, index) => {
                                            var targetRow = paymentMethodWiseSalesData.find(
                                                (x) =>
                                                    x.Name === outlet &&
                                                    x.PaymentMethod === payment.Name
                                            );
                                            return (
                                                <Td px="2" key={index}>
                                                    {commaFormat(targetRow?.Amount || 0)}
                                                </Td>
                                            );
                                        })}
                                    </Tr>
                                );
                            })}

                            <Tr>
                                <Td px="2" fontWeight="semibold">
                                    Total
                                </Td>
                                {outletNames.map((outlet, index) => {
                                    const outletsData = paymentMethodWiseSalesData.filter(
                                        (item) => item.Name === outlet
                                    );

                                    let outletTotal = 0;
                                    outletsData.map((data) => (outletTotal += data.Amount));
                                    return (
                                        <Td px="2" fontWeight="semibold" key={index}>
                                            {commaFormat(outletTotal)}
                                        </Td>
                                    );
                                })}
                            </Tr>
                        </Tbody>
                    </Table>
                ) : (
                    <Loading line={6} />
                )}
            </Flex>
        </Layout>
    );
};

export default OutletComparisonChart;
