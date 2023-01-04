//@ts-check
import React, { useState, useEffect } from 'react';
import {
    Flex,
    Heading,
    Table,
    Tbody,
    Td,
    Text,
    Th,
    Thead,
    Tr,
    useColorModeValue
} from '@chakra-ui/react';
import ReactApexChart from 'react-apexcharts';
import { onDailySales } from '../../../services/dashboard-service';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import { commaFormat, getTableTotal } from '../../../services/_utils/index';
import Loading from '../../shared/Loading';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import { exportTable } from '../../shared/dataExport/ExportStyle';
import FilterOption from '../../shared/FilterOption';

const DailySalesChart = () => {
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'lightgray');
    const gridColor = useColorModeValue('lightgray', 'gray');
    // default days
    const today = new Date();
    const todayLocal = today.toISOString().substring(0, 16);
    const thirtyDaysAgo = new Date(today);
    thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
    const thirtyDaysAgoLocal = thirtyDaysAgo.toISOString().substring(0, 16);
    // filter options
    const [isExact, setIsExact] = useState(true);
    const [fromDate, setFromDate] = useState(thirtyDaysAgoLocal);
    const [toDate, setToDate] = useState(todayLocal);
    const [departmentIds, setDepartmentIds] = useState([0]);
    const [outletIds, setOutletIds] = useState([0]);
    const [categoriesIds, setCategoriesIds] = useState(['']);
    const [menuItemIds, setMenuItemIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    // data
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState([]);
    const [categories, setCategories] = useState([]);
    const [netSales, setNetSales] = useState([]);
    const [totalSales, setTotalSales] = useState([]);

    const onLoadData = async () => {
        try {
            setLoading(true);
            const { data } = await onDailySales(
                isExact,
                fromDate,
                toDate,
                outletIds,
                departmentIds,
                categoriesIds,
                menuItemIds
            );
            setData(data.result);
            setFromDate(data.fromDate.slice(0, 16));
            setToDate(data.toDate.slice(0, 16));
            setLoading(false);
            setIsSearching(false);
        } catch (error) {
            setLoading(true);
            console.log(error);
            setLoading(false);
            setIsSearching(false);
        }
    };

    useEffect(() => {
        isSearching && onLoadData();
    }, [isSearching]);

    useEffect(() => {
        onLoadData();
    }, []);

    useEffect(() => {
        setCategories(data.map((item) => item.StartDate));
        setNetSales(data.map((item) => item.Sales));
        setTotalSales(data.map((item) => item.TicketTotalAmount));
    }, [data]);

    const series = [
        {
            name: 'Net Sales',
            data: netSales
        },
        {
            name: 'Total Sales',
            data: totalSales
        }
    ];
    const options = {
        colors: ['#00355f', '#FA7242'],
        chart: {
            height: 350,
            type: 'area',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        legend: {
            markers: {
                radius: 0
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth'
        },
        xaxis: {
            // type: 'datetime',
            categories: categories
        },
        tooltip: {
            x: {
                format: 'dd/MM/yy HH:mm'
            }
        },

        grid: {
            borderColor: gridColor,
            strokeDashArray: 5,
            xaxis: {
                lines: {
                    show: true
                }
            },
            yaxis: {
                lines: {
                    show: true
                }
            }
        }
    };

    return (
        <Layout>
            <Flex direction="column" bg={bgColor} p="30px" boxShadow="sm" borderRadius="xl">
                <Heading size="md" mb="5">
                    Daily Sales Chart
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
                />

                <Text fontSize="14px">
                    Work period considered from <strong>{getFormattedDateTime(fromDate)}</strong> to{' '}
                    <strong>{getFormattedDateTime(toDate)}</strong>
                </Text>

                {!loading ? (
                    <ReactApexChart options={options} series={series} type="area" height="350" />
                ) : (
                    <Loading line={8} mb="10" />
                )}

                {!loading ? (
                    <>
                        <ExportOptions
                            title="Day Wise Sales"
                            fromDate={fromDate}
                            toDate={toDate}
                            colSpan="3"
                            scale={0.6}
                            margin="2cm">
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}>Date</th>
                                <th style={exportTable.th}>Net Sales</th>
                                <th style={exportTable.th}>Total Sales</th>
                            </tr>

                            {data.map((item, index) => (
                                <tr key={index} style={exportTable.tr}>
                                    <td style={exportTable.td}>{item.StartDate}</td>
                                    <td style={exportTable.td}>{commaFormat(item.Sales)}</td>
                                    <td style={exportTable.td}>
                                        {commaFormat(item.TicketTotalAmount)}
                                    </td>
                                </tr>
                            ))}

                            <tr style={exportTable.tr}>
                                <td style={exportTable.td}>Total</td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(data, 'Sales'))}
                                </td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(data, 'TicketTotalAmount'))}
                                </td>
                            </tr>
                        </ExportOptions>

                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead bg="primary.900">
                                <Tr>
                                    <Th p="2" color="white">
                                        Date
                                    </Th>
                                    <Th p="2" color="white">
                                        Net Sales
                                    </Th>
                                    <Th p="2" color="white">
                                        Total Sales
                                    </Th>
                                </Tr>
                            </Thead>
                            <Tbody>
                                {data.map((item, i) => (
                                    <Tr key={i}>
                                        <Td px="2">{item.StartDate}</Td>
                                        <Td px="2">{commaFormat(item.Sales)}</Td>
                                        <Td px="2">{commaFormat(item.TicketTotalAmount)}</Td>
                                    </Tr>
                                ))}
                                <Tr>
                                    <Td px="2" fontWeight="semibold">
                                        Total
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(getTableTotal(data, 'Sales'))}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(getTableTotal(data, 'TicketTotalAmount'))}
                                    </Td>
                                </Tr>
                            </Tbody>
                        </Table>
                    </>
                ) : (
                    <Loading line={8} />
                )}
            </Flex>
        </Layout>
    );
};

export default DailySalesChart;
