//@ts-check
import React, { useState } from 'react';
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
import Layout from '../../shared/Layout';
import ReactApexChart from 'react-apexcharts';
import FilterOption from '../../shared/FilterOption';
import { onDayAndTimeWiseSales } from '../../../services/dashboard-service';
import { useEffect } from 'react';
import Loading from '../../shared/Loading';
import { commaFormat, getTableTotal, roundCommaFormat } from '../../../services/_utils/index';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import { exportTable } from '../../shared/dataExport/ExportStyle';

const DayAndTimeWiseSalesChart = () => {
    // color mode value
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'lightgray');
    const gridColor = useColorModeValue('lightgray', 'gray');
    const brandBgText = useColorModeValue('primary.900', 'gray.200');
    const tableCaptionBg = useColorModeValue('whitesmoke', 'whiteAlpha.50');
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
    const [outletIds, setOutletIds] = useState([0]);
    const [categoriesIds, setCategoriesIds] = useState(['']);
    const [menuItemIds, setMenuItemIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    // data
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState([]);
    const [daysList, setDaysList] = useState([]);
    const [hoursList, setHoursList] = useState([]);

    const onLoadData = async () => {
        try {
            setLoading(true);
            const { data } = await onDayAndTimeWiseSales(
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
        onLoadData();
    }, []);

    useEffect(() => {
        isSearching && onLoadData();
    }, [isSearching]);

    useEffect(() => {
        const days = data.map((item) => item.WeekDay);
        setDaysList([...new Set(days)]);

        const hours = data.map((item) => item.Hour);
        const sortHours = hours.sort((a, b) => {
            return a - b;
        });
        setHoursList([...new Set(sortHours)]);
    }, [data]);

    const series1 = [
        {
            name: 'Order Count',
            data: daysList.map((day) => {
                const dayWiseData = data.filter((item) => item.WeekDay === day);
                let quantity = 0;
                dayWiseData.forEach((item) => (quantity = quantity + item.OrderQuantity));
                return quantity;
            })
        }
    ];
    const series2 = [
        {
            name: 'Order Count',
            data: hoursList.map((hour) => {
                const hourWiseData = data.filter((item) => item.Hour === hour);
                let quantity = 0;
                hourWiseData.forEach((item) => (quantity = quantity + item.OrderQuantity));
                return quantity;
            })
        }
    ];
    const series3 = [
        {
            name: 'Ticket Count',
            data: daysList.map((day) => {
                const dayWiseData = data.filter((item) => item.WeekDay === day);
                let quantity = 0;
                dayWiseData.forEach((item) => (quantity = quantity + item.TicketQuantity));
                return quantity;
            })
        }
    ];
    const series4 = [
        {
            name: 'Ticket Count',
            data: hoursList.map((hour) => {
                const hourWiseData = data.filter((item) => item.Hour === hour);
                let quantity = 0;
                hourWiseData.forEach((item) => (quantity = quantity + item.TicketQuantity));
                return quantity;
            })
        }
    ];
    const series5 = [
        {
            name: 'Total Amount',
            data: daysList.map((day) => {
                const dayWiseData = data.filter((item) => item.WeekDay === day);
                let amount = 0;
                dayWiseData.forEach((item) => (amount = amount + item.Price));
                return Math.round(amount);
            })
        }
    ];
    const series6 = [
        {
            name: 'Total Amount',
            data: hoursList.map((hour) => {
                const hourWiseData = data.filter((item) => item.Hour === hour);
                let amount = 0;
                hourWiseData.forEach((item) => (amount = amount + item.Price));
                return Math.round(amount);
            })
        }
    ];

    const options1 = {
        colors: ['#513252'],
        chart: {
            height: 250,
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
            categories: daysList,
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
        colors: ['#513252'],
        chart: {
            height: 250,
            type: 'area',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        legend: {
            showForSingleSeries: true,
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
            categories: hoursList
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
    const options3 = {
        colors: ['#94B49F'],
        chart: {
            height: 250,
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
            categories: daysList,
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
        colors: ['#94B49F'],
        chart: {
            height: 250,
            type: 'area',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        legend: {
            showForSingleSeries: true,
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
            categories: hoursList
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
    const options5 = {
        colors: ['#00355f'],
        chart: {
            height: 250,
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
            categories: daysList,
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
    const options6 = {
        colors: ['#00355f'],
        chart: {
            height: 250,
            type: 'area',
            toolbar: {
                show: false
            },
            foreColor: textColor
        },
        legend: {
            showForSingleSeries: true,
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
            categories: hoursList
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
            <Flex direction="column" bg={bgColor} boxShadow="sm" p="30px" rounded="xl">
                <Heading size="md" textAlign="left" mb="5">
                    Day And Time Wise Sales
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
                        Week Day Order Count
                    </Heading>
                    {!loading ? (
                        <ReactApexChart
                            options={options1}
                            series={series1}
                            type="bar"
                            height="250"
                        />
                    ) : (
                        <Loading line={8} />
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
                        Hourly Order Count
                    </Heading>
                    {!loading ? (
                        <ReactApexChart
                            options={options2}
                            series={series2}
                            type="area"
                            height="250"
                        />
                    ) : (
                        <Loading line={8} />
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
                        Week Day Tickets Count
                    </Heading>
                    {!loading ? (
                        <ReactApexChart
                            options={options3}
                            series={series3}
                            type="bar"
                            height="250"
                        />
                    ) : (
                        <Loading line={8} />
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
                        Hourly Tickets Count
                    </Heading>
                    {!loading ? (
                        <ReactApexChart
                            options={options4}
                            series={series4}
                            type="area"
                            height="250"
                        />
                    ) : (
                        <Loading line={8} />
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
                        Week Day Total Amount
                    </Heading>
                    {!loading ? (
                        <ReactApexChart
                            options={options5}
                            series={series5}
                            type="bar"
                            height="250"
                        />
                    ) : (
                        <Loading line={8} />
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
                        Hourly Total Amount
                    </Heading>
                    {!loading ? (
                        <ReactApexChart
                            options={options6}
                            series={series6}
                            type="area"
                            height="250"
                        />
                    ) : (
                        <Loading line={8} />
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
                            title="Day & Time Wise Sales"
                            fromDate={fromDate}
                            toDate={toDate}
                            colSpan="25"
                            scale={0.4}
                            margin="30pt"
                            landscape={true}>
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}></th>
                                {daysList.map((day, index) => (
                                    <th style={exportTable.th} colSpan="3" key={index}>
                                        {day}
                                    </th>
                                ))}
                                <th style={exportTable.th} colSpan="3">
                                    Total
                                </th>
                            </tr>

                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}>Hour</th>

                                {daysList.map((day, index) => (
                                    <>
                                        <th style={exportTable.th} key={index}>
                                            Ticket
                                        </th>
                                        <th style={exportTable.th}>Order</th>
                                        <th style={exportTable.th}>Price</th>
                                    </>
                                ))}

                                <th style={exportTable.th}>Ticket</th>
                                <th style={exportTable.th}>Order</th>
                                <th style={exportTable.th}>Price</th>
                            </tr>
                            {hoursList.map((hour, index) => (
                                <tr key={index} style={exportTable.tr}>
                                    <td style={exportTable.td}>{hour}</td>

                                    {daysList.map((day, index) => {
                                        const dayData = data.find(
                                            (item) => item.WeekDay === day && item.Hour === hour
                                        );
                                        return (
                                            <>
                                                <td style={exportTable.td} key={index}>
                                                    {roundCommaFormat(dayData?.TicketQuantity || 0)}
                                                </td>
                                                <td style={exportTable.td}>
                                                    {roundCommaFormat(dayData?.OrderQuantity || 0)}
                                                </td>
                                                <td style={exportTable.td}>
                                                    {commaFormat(dayData?.Price || 0)}
                                                </td>
                                            </>
                                        );
                                    })}

                                    <td style={exportTable.td}>
                                        {roundCommaFormat(
                                            getTableTotal(
                                                data.filter((item) => item.Hour === hour),
                                                'TicketQuantity'
                                            )
                                        )}
                                    </td>
                                    <td style={exportTable.td}>
                                        {roundCommaFormat(
                                            getTableTotal(
                                                data.filter((item) => item.Hour === hour),
                                                'OrderQuantity'
                                            )
                                        )}
                                    </td>
                                    <td style={exportTable.td}>
                                        {commaFormat(
                                            getTableTotal(
                                                data.filter((item) => item.Hour === hour),
                                                'Price'
                                            )
                                        )}
                                    </td>
                                </tr>
                            ))}

                            <tr style={exportTable.tr}>
                                <td style={exportTable.td}>Total</td>
                                {daysList.map((day, index) => {
                                    const dayData = data.filter((item) => item.WeekDay === day);
                                    return (
                                        <>
                                            <td style={exportTable.td}>
                                                {roundCommaFormat(
                                                    getTableTotal(dayData, 'TicketQuantity')
                                                )}
                                            </td>
                                            <td style={exportTable.td}>
                                                {roundCommaFormat(
                                                    getTableTotal(dayData, 'OrderQuantity')
                                                )}
                                            </td>
                                            <td style={exportTable.td}>
                                                {commaFormat(getTableTotal(dayData, 'Price'))}
                                            </td>
                                        </>
                                    );
                                })}

                                <td style={exportTable.td}>
                                    {roundCommaFormat(getTableTotal(data, 'TicketQuantity'))}
                                </td>
                                <td style={exportTable.td}>
                                    {roundCommaFormat(getTableTotal(data, 'OrderQuantity'))}
                                </td>
                                <td style={exportTable.td}>
                                    {commaFormat(getTableTotal(data, 'Price'))}
                                </td>
                            </tr>
                        </ExportOptions>

                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead bg={tableCaptionBg}>
                                <Tr>
                                    <Th p="2" color={brandBgText} textAlign="center"></Th>
                                    {daysList.map((day, index) => (
                                        <Th
                                            p="2"
                                            color={brandBgText}
                                            colSpan="3"
                                            textAlign="center"
                                            key={index}>
                                            {day}
                                        </Th>
                                    ))}
                                    <Th p="2" color={brandBgText} textAlign="center" colSpan="3">
                                        Total
                                    </Th>
                                </Tr>
                            </Thead>

                            <Thead bg="black">
                                <Tr>
                                    <Th p="2" bg="primary.900" color="white">
                                        Hour
                                    </Th>

                                    {daysList.map((day, index) => (
                                        <>
                                            <Th
                                                p="2"
                                                bg={index % 2 === 0 ? 'brand.400' : 'primary.900'}
                                                color="white"
                                                key={index}>
                                                Ticket
                                            </Th>
                                            <Th
                                                p="2"
                                                bg={index % 2 === 0 ? 'brand.400' : 'primary.900'}
                                                color="white">
                                                Order
                                            </Th>
                                            <Th
                                                p="2"
                                                bg={index % 2 === 0 ? 'brand.400' : 'primary.900'}
                                                color="white">
                                                Price
                                            </Th>
                                        </>
                                    ))}

                                    <Th p="2" bg="primary.900" color="white">
                                        Ticket
                                    </Th>
                                    <Th p="2" bg="primary.900" color="white">
                                        Order
                                    </Th>
                                    <Th p="2" bg="primary.900" color="white">
                                        Price
                                    </Th>
                                </Tr>
                            </Thead>

                            <Tbody>
                                {hoursList.map((hour, index) => (
                                    <Tr key={index}>
                                        <Td px="2">{hour}</Td>

                                        {daysList.map((day, index) => {
                                            const dayData = data.find(
                                                (item) => item.WeekDay === day && item.Hour === hour
                                            );
                                            return (
                                                <>
                                                    <Td px="2" key={index}>
                                                        {roundCommaFormat(
                                                            dayData?.TicketQuantity || 0
                                                        )}
                                                    </Td>
                                                    <Td px="2">
                                                        {roundCommaFormat(
                                                            dayData?.OrderQuantity || 0
                                                        )}
                                                    </Td>
                                                    <Td px="2">
                                                        {commaFormat(dayData?.Price || 0)}
                                                    </Td>
                                                </>
                                            );
                                        })}

                                        <Td px="2">
                                            {roundCommaFormat(
                                                getTableTotal(
                                                    data.filter((item) => item.Hour === hour),
                                                    'TicketQuantity'
                                                )
                                            )}
                                        </Td>
                                        <Td px="2">
                                            {roundCommaFormat(
                                                getTableTotal(
                                                    data.filter((item) => item.Hour === hour),
                                                    'OrderQuantity'
                                                )
                                            )}
                                        </Td>
                                        <Td px="2">
                                            {commaFormat(
                                                getTableTotal(
                                                    data.filter((item) => item.Hour === hour),
                                                    'Price'
                                                )
                                            )}
                                        </Td>
                                    </Tr>
                                ))}

                                <Tr>
                                    <Td fontWeight="semibold" px="2">
                                        Total
                                    </Td>
                                    {daysList.map((day, index) => {
                                        const dayData = data.filter((item) => item.WeekDay === day);
                                        return (
                                            <>
                                                <Td fontWeight="semibold" px="2" key={index}>
                                                    {roundCommaFormat(
                                                        getTableTotal(dayData, 'TicketQuantity')
                                                    )}
                                                </Td>
                                                <Td fontWeight="semibold" px="2">
                                                    {roundCommaFormat(
                                                        getTableTotal(dayData, 'OrderQuantity')
                                                    )}
                                                </Td>
                                                <Td fontWeight="semibold" px="2">
                                                    {commaFormat(getTableTotal(dayData, 'Price'))}
                                                </Td>
                                            </>
                                        );
                                    })}

                                    <Td px="2" fontWeight="semibold">
                                        {roundCommaFormat(getTableTotal(data, 'TicketQuantity'))}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {roundCommaFormat(getTableTotal(data, 'OrderQuantity'))}
                                    </Td>
                                    <Td px="2" fontWeight="semibold">
                                        {commaFormat(getTableTotal(data, 'Price'))}
                                    </Td>
                                </Tr>
                            </Tbody>
                        </Table>
                    </>
                ) : (
                    <Box w="100%">
                        <Loading line={8} />
                    </Box>
                )}
            </Flex>
        </Layout>
    );
};

export default DayAndTimeWiseSalesChart;
