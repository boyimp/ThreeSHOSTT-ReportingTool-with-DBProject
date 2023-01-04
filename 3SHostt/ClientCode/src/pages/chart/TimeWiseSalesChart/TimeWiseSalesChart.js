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
    useColorModeValue
} from '@chakra-ui/react';
import ReactApexChart from 'react-apexcharts';
import { onTimeWiseSales } from '../../../services/dashboard-service';
import { commaFormat, getTableTotal, roundCommaFormat } from '../../../services/_utils/index';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import Loading from '../../shared/Loading';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import { exportTable } from '../../shared/dataExport/ExportStyle';
import FilterOption from '../../shared/FilterOption';

const TimeWiseSalesChart = () => {
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'lightgray');
    const gridColor = useColorModeValue('lightgray', 'gray');
    const chartHeadText = useColorModeValue('gray.600', 'lightgray');
    // default days
    const today = new Date();
    const todayLocal = today.toISOString().substring(0, 16);
    const sevenDaysAgo = new Date(today);
    sevenDaysAgo.setDate(sevenDaysAgo.getDate() - 7);
    const sevenDaysAgoLocal = sevenDaysAgo.toISOString().substring(0, 16);
    // filter options
    const [isExact, setIsExact] = useState(true);
    const [fromDate, setFromDate] = useState(sevenDaysAgoLocal);
    const [toDate, setToDate] = useState(todayLocal);
    const [departmentIds, setDepartmentIds] = useState([0]);
    const [categoriesIds, setCategoriesIds] = useState(['']);
    const [menuItemIds, setMenuItemIds] = useState([0]);
    const [outletIds, setOutletIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    // data
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState([]);
    const [timeRange, setTimeRange] = useState([]);
    const [netSales, setNetSales] = useState([]);
    const [totalSales, setTotalSales] = useState([]);
    const [guestTotal, setGuestTotal] = useState([]);
    const [ticketTotal, setTicketTotal] = useState([]);
    const [formattedTime, setFormattedTime] = useState([]);

    const formatTimeRange = (times) => {
        const formatted = times.map((time) => {
            let duration = time;
            let times = duration.split('-');
            let fromTimeString = '2022-01-01' + 'T' + times[0];
            let toTimeString = '2022-01-01' + 'T' + times[1];

            let from = new Date(fromTimeString);
            let to = new Date(toTimeString);

            let fromTimeAsHumanReadable = from.toLocaleString('en-US', {
                hour: 'numeric',
                hour12: true
            });
            let toTimeAsHumanReadable = to.toLocaleString('en-US', {
                hour: 'numeric',
                hour12: true
            });
            let from_to = fromTimeAsHumanReadable + ' - ' + toTimeAsHumanReadable;
            return from_to;
        });
        setFormattedTime(formatted);
    };

    const onLoadData = async () => {
        try {
            setLoading(true);
            const { data } = await onTimeWiseSales(
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
        let timeRange = data.map((item) => item.TimeRange);
        setTimeRange(timeRange);
        formatTimeRange(timeRange);

        setNetSales(data.map((item) => item.Sales));
        setTotalSales(data.map((item) => item.GrandTotal));
        setGuestTotal(data.map((item) => item.NumberOfGuests));
        setTicketTotal(data.map((item) => item.NumberOfTickets));
    }, [data]);

    // chart series and options
    const series1 = [
        {
            name: 'Net Sales',
            data: netSales
        }
    ];
    const series2 = [
        {
            name: 'Total Sales',
            data: totalSales
        }
    ];
    const series3 = [
        {
            name: 'Number of Guest',
            data: guestTotal
        }
    ];
    const series4 = [
        {
            name: 'Number of Ticket',
            data: ticketTotal
        }
    ];
    const options1 = {
        colors: ['#FA7242', '#00355f', '#94B49F', '#513252'],
        chart: {
            height: 350,
            type: 'bar',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        plotOptions: {
            bar: {
                borderRadius: 5,
                dataLabels: {
                    position: 'top' // top, center, bottom
                }
            }
        },
        legend: {
            showForSingleSeries: true,
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
            categories: formattedTime,
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
    const options2 = {
        colors: ['#00355f', '#00355f', '#94B49F', '#513252'],
        chart: {
            height: 350,
            type: 'bar',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        plotOptions: {
            bar: {
                borderRadius: 5,
                dataLabels: {
                    position: 'top' // top, center, bottom
                }
            }
        },
        legend: {
            showForSingleSeries: true,
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
            categories: formattedTime,
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
    const options3 = {
        colors: ['#94B49F', '#00355f', '#94B49F', '#513252'],
        chart: {
            height: 350,
            type: 'bar',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        plotOptions: {
            bar: {
                borderRadius: 5,
                dataLabels: {
                    position: 'top' // top, center, bottom
                }
            }
        },
        legend: {
            showForSingleSeries: true,
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
            categories: formattedTime,
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
    const options4 = {
        colors: ['#513252', '#00355f', '#94B49F', '#513252'],
        chart: {
            height: 350,
            type: 'bar',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        plotOptions: {
            bar: {
                borderRadius: 5,
                dataLabels: {
                    position: 'top' // top, center, bottom
                }
            }
        },
        legend: {
            showForSingleSeries: true,
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
            categories: formattedTime,
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
                    Time Wise Sales Details
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
            </Flex>

            <Flex direction={{ base: 'column', lg: 'row' }} gap="15px" pb="10px" pt="5px">
                <Flex
                    width={{ base: '100%', lg: '50%' }}
                    direction="column"
                    bg={bgColor}
                    boxShadow="sm"
                    p="30px 20px"
                    rounded="xl">
                    <Heading fontSize="18px" mb="10px" ml="10px" color={chartHeadText}>
                        Net Sales
                    </Heading>
                    {!loading ? (
                        <ReactApexChart
                            options={options1}
                            series={series1}
                            type="bar"
                            height="350"
                        />
                    ) : (
                        <Loading line={10} />
                    )}
                </Flex>

                <Flex
                    width={{ base: '100%', lg: '50%' }}
                    direction="column"
                    bg={bgColor}
                    boxShadow="sm"
                    p="30px 20px"
                    rounded="xl">
                    <Heading fontSize="18px" mb="10px" ml="10px" color={chartHeadText}>
                        Total Sales
                    </Heading>

                    {!loading ? (
                        <ReactApexChart
                            options={options2}
                            series={series2}
                            type="bar"
                            height="350"
                        />
                    ) : (
                        <Loading line={10} />
                    )}
                </Flex>
            </Flex>

            <Flex direction={{ base: 'column', lg: 'row' }} gap="15px" pb="10px">
                <Flex
                    width={{ base: '100%', lg: '50%' }}
                    direction="column"
                    bg={bgColor}
                    boxShadow="sm"
                    p="30px 20px"
                    rounded="xl">
                    <Heading fontSize="18px" mb="10px" ml="10px" color={chartHeadText}>
                        Number of Guest
                    </Heading>

                    {!loading ? (
                        <ReactApexChart
                            options={options3}
                            series={series3}
                            type="bar"
                            height="350"
                        />
                    ) : (
                        <Loading line={10} />
                    )}
                </Flex>

                <Flex
                    width={{ base: '100%', lg: '50%' }}
                    direction="column"
                    bg={bgColor}
                    boxShadow="sm"
                    p="30px 20px"
                    rounded="xl">
                    <Heading fontSize="18px" mb="10px" ml="10px" color={chartHeadText}>
                        Number of Ticket
                    </Heading>

                    {!loading ? (
                        <ReactApexChart
                            options={options4}
                            series={series4}
                            type="bar"
                            height="350"
                        />
                    ) : (
                        <Loading line={10} />
                    )}
                </Flex>
            </Flex>

            <Flex
                bg={bgColor}
                boxShadow="sm"
                p="20px 30px"
                rounded="xl"
                overflowX="scroll"
                direction="column">
                {!loading ? (
                    <>
                        <ExportOptions
                            title="Time Wise Sales"
                            fromDate={fromDate}
                            toDate={toDate}
                            colSpan="5"
                            scale={0.6}
                            margin="2cm">
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}>Time Range</th>
                                <th style={exportTable.th}>Net Sales</th>
                                <th style={exportTable.th}>Total Sales</th>
                                <th style={exportTable.th}>Number of Ticket</th>
                                <th style={exportTable.th}>Number of Guest</th>
                            </tr>

                            {data.map((item, index) => (
                                <tr key={index} style={exportTable.tr}>
                                    <td style={exportTable.td}>{formattedTime[index]}</td>
                                    <td style={exportTable.td}>{commaFormat(item.Sales)}</td>
                                    <td style={exportTable.td}>{commaFormat(item.GrandTotal)}</td>
                                    <td style={exportTable.td}>
                                        {roundCommaFormat(item.NumberOfTickets)}
                                    </td>
                                    <td style={exportTable.td}>
                                        {roundCommaFormat(item.NumberOfGuests)}
                                    </td>
                                </tr>
                            ))}

                            <tr style={exportTable.tr}>
                                <td style={exportTable.td}>Total</td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(data, 'Sales'))}
                                </td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(data, 'GrandTotal'))}
                                </td>
                                <td style={exportTable.td}>
                                    {roundCommaFormat(getTableTotal(data, 'NumberOfTickets'))}
                                </td>
                                <td style={exportTable.td}>
                                    {roundCommaFormat(getTableTotal(data, 'NumberOfGuests'))}
                                </td>
                            </tr>
                        </ExportOptions>

                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead bg="primary.900">
                                <Tr>
                                    <Th p="2" color="white">
                                        Time Range
                                    </Th>
                                    <Th p="2" color="white">
                                        Net Sales
                                    </Th>
                                    <Th p="2" color="white">
                                        Total Sales
                                    </Th>
                                    <Th p="2" color="white">
                                        Number of Ticket
                                    </Th>
                                    <Th p="2" color="white">
                                        Number of Guest
                                    </Th>
                                </Tr>
                            </Thead>

                            <Tbody>
                                {data.map((item, index) => (
                                    <Tr key={index}>
                                        <Td px="2">{formattedTime[index]}</Td>
                                        <Td px="2">{commaFormat(item.Sales)}</Td>
                                        <Td px="2">{commaFormat(item.GrandTotal)}</Td>
                                        <Td px="2">{roundCommaFormat(item.NumberOfTickets)}</Td>
                                        <Td px="2">{roundCommaFormat(item.NumberOfGuests)}</Td>
                                    </Tr>
                                ))}

                                <Tr key={1}>
                                    <Td px="2" fontWeight="semibold">
                                        Total
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(getTableTotal(data, 'Sales'))}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(getTableTotal(data, 'GrandTotal'))}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {roundCommaFormat(getTableTotal(data, 'NumberOfTickets'))}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {roundCommaFormat(getTableTotal(data, 'NumberOfGuests'))}
                                    </Td>
                                </Tr>
                            </Tbody>
                        </Table>
                    </>
                ) : (
                    <Box w="100%">
                        <Loading />
                    </Box>
                )}
            </Flex>
        </Layout>
    );
};

export default TimeWiseSalesChart;
