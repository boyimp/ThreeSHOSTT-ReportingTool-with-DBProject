//@ts-check
import React, { useState, useEffect } from 'react';
import {
    Flex,
    Heading,
    Stack,
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
import { onMonthlyNetSales, onMonthlyTotalSales } from '../../../services/dashboard-service';
import { commaFormat, getTableTotal } from '../../../services/_utils/index';
import Loading from '../../shared/Loading';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import { exportTable } from '../../shared/dataExport/ExportStyle';
import FilterOption from '../../shared/FilterOption';

const MonthlySales = () => {
    // chakra
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'lightgray');
    const gridColor = useColorModeValue('lightgray', 'gray');
    // filter
    const [outletIds, setOutletIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    // data
    const [loading, setLoading] = useState(true);
    const [monthlyTotalSales, setMonthlyTotalSales] = useState([]);
    const [monthlyNetSales, setMonthlyNetSales] = useState([]);
    // formate data
    const [totalAmounts, setTotalAmounts] = useState([]);
    const [netAmounts, setNetAmounts] = useState([]);
    const [monthYear, setMonthYear] = useState([]);

    const onLoadData = () => {
        setLoading(true);

        onMonthlyTotalSales(outletIds)
            .then((res) => {
                setMonthlyTotalSales(res.data);
                setLoading(false);
                setIsSearching(false);
            })
            .catch((err) => {
                console.log(err);
                setLoading(false);
                setIsSearching(false);
            });

        onMonthlyNetSales(outletIds)
            .then((res) => {
                setMonthlyNetSales(res.data);
            })
            .catch((err) => {
                console.log(err);
            });
    };

    useEffect(() => {
        setMonthYear(monthlyTotalSales.map((item) => item.MonthYear));
        setTotalAmounts(monthlyTotalSales.map((item) => Math.round(item.TotalAmount)));
        setNetAmounts(monthlyNetSales.map((item) => Math.round(item.Amount)));
    }, [monthlyTotalSales, monthlyNetSales]);

    useEffect(() => {
        isSearching && onLoadData();
    }, [isSearching]);

    useEffect(() => {
        onLoadData();
    }, []);

    const series = [
        {
            name: 'Net Sales',
            data: netAmounts
        },
        {
            name: 'Total Sales',
            data: totalAmounts
        }
    ];
    const options = {
        colors: ['#00355f', '#FA7242'],
        chart: {
            height: 350,
            type: 'bar',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        // plotOptions: {
        //     bar: {
        //         borderRadius: 5,
        //         dataLabels: {
        //             position: 'top' // top, center, bottom
        //         }
        //     }
        // },
        legend: {
            markers: {
                radius: 0
            }
        },
        dataLabels: {
            enabled: false,
            formatter: function (val) {
                return val + '%';
            },
            offsetY: -20,
            style: {
                fontSize: '12px',
                colors: ['#304758']
            }
        },
        xaxis: {
            categories: monthYear,
            position: 'bottom',
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            crosshairs: {
                fill: {
                    type: 'gradient',
                    gradient: {
                        colorFrom: '#D8E3F0',
                        colorTo: '#BED1E6',
                        stops: [0, 100],
                        opacityFrom: 0.4,
                        opacityTo: 0.5
                    }
                }
            },
            tooltip: {
                enabled: true
            }
        },
        yaxis: {
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            labels: {
                show: true
            }
        },
        grid: {
            borderColor: gridColor,
            strokeDashArray: 5
        }
    };

    return (
        <Layout>
            <Flex direction="column" bg={bgColor} p="30px" boxShadow="sm" borderRadius="xl">
                <Heading size="md" mb="5">
                    Last 12 Month Sales Chart
                </Heading>

                <FilterOption
                    isApiCalling={loading}
                    setIsSearching={setIsSearching}
                    setOutletIds={setOutletIds}
                />

                {!loading ? (
                    <ReactApexChart options={options} series={series} type="bar" height="350" />
                ) : (
                    <Loading line={8} mb="10" />
                )}

                {!loading ? (
                    <>
                        <ExportOptions
                            title="Month Wise Sales"
                            colSpan="3"
                            scale={0.6}
                            margin="2cm">
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}>Month</th>
                                <th style={exportTable.th}>Net Sales</th>
                                <th style={exportTable.th}>Total Sales</th>
                            </tr>

                            {monthlyTotalSales.map((item, i) => {
                                const targetNetSalesRow = monthlyNetSales.find(
                                    (target) => target.MonthYear === item.MonthYear
                                );
                                return (
                                    <tr key={i} style={exportTable.tr}>
                                        <td style={exportTable.td}>{item?.MonthYear}</td>
                                        <td style={exportTable.td}>
                                            {commaFormat(targetNetSalesRow?.Amount || 0)}
                                        </td>
                                        <td style={exportTable.td}>
                                            {commaFormat(item?.TotalAmount || 0)}
                                        </td>
                                    </tr>
                                );
                            })}

                            <tr style={exportTable.tr}>
                                <td style={exportTable.td}>Total</td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(monthlyNetSales, 'Amount'))}
                                </td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(monthlyTotalSales, 'TotalAmount'))}
                                </td>
                            </tr>
                        </ExportOptions>

                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead bg="primary.900">
                                <Tr>
                                    <Th p="2" color="white">
                                        Month
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
                                {monthlyTotalSales.map((item, i) => {
                                    const targetNetSalesRow = monthlyNetSales.find(
                                        (target) => target.MonthYear === item.MonthYear
                                    );
                                    return (
                                        <Tr key={i}>
                                            <Td px="2">{item?.MonthYear}</Td>
                                            <Td px="2">
                                                {commaFormat(targetNetSalesRow?.Amount || 0)}
                                            </Td>
                                            <Td px="2">{commaFormat(item?.TotalAmount || 0)}</Td>
                                        </Tr>
                                    );
                                })}

                                <Tr>
                                    <Td px="2" fontWeight="semibold">
                                        Total
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(getTableTotal(monthlyNetSales, 'Amount'))}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(
                                            getTableTotal(monthlyTotalSales, 'TotalAmount')
                                        )}
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

export default MonthlySales;
