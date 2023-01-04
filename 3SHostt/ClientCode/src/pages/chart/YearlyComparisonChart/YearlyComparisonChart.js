//@ts-check
import {
    Box,
    Button,
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
import React, { useEffect, useState } from 'react';
import ReactApexChart from 'react-apexcharts';
import { onYearlySales } from '../../../services/dashboard-service';
import { commaFormat, getTableTotal } from '../../../services/_utils/index';
import { getSortNumbers, getUniqueValue, roundCommaFormat } from '../../../services/_utils/index';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import { IoMdArrowDropdown, IoMdArrowDropup } from 'react-icons/io';
import Loading from '../../shared/Loading';
import Layout from '../../shared/Layout';
import ExportOptions from '../../shared/dataExport/ExportOptions';
import { exportTable } from '../../shared/dataExport/ExportStyle';
import FilterOption from '../../shared/FilterOption';

const YearlyComparisonChart = () => {
    // colors
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'lightgray');
    const gridColor = useColorModeValue('lightgray', 'gray');
    const theadText = useColorModeValue('white', 'brand.400');
    // filter options
    const [fromDate, setFromDate] = useState(new Date());
    const [toDate, setToDate] = useState(new Date());
    const [departmentIds, setDepartmentIds] = useState([0]);
    const [outletIds, setOutletIds] = useState([0]);
    const [isSearching, setIsSearching] = useState(false);
    const [salesTypeIsNet, setSalesTypeIsNet] = useState(false);
    const [numberOfYears, setNumberOfYears] = useState(2);
    // data
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [haveDataYears, setHaveDataYears] = useState([]);
    // formatted data
    const [yearList, setYearList] = useState();
    const [seriesList, setSeriesList] = useState([]);
    const [tableNetTotals, setTableNetTotals] = useState([]);

    const Months = [
        'January',
        'February',
        'March',
        'April',
        'May',
        'June',
        'July',
        'August',
        'September',
        'October',
        'November',
        'December'
    ];

    const getHaveDataYears = (haveYears) => {
        const years = [];
        for (let i = haveYears; i >= 2; i--) {
            years.push(i);
        }
        const sortYears = getSortNumbers(years);
        const list = sortYears.map((year) => {
            return { year: year, label: `Last ${year} Years` };
        });
        setHaveDataYears(list);
    };

    const getTableNetTotal = () => {
        const totals = [];
        seriesList.forEach((item) => {
            const intNetAmount = getTableTotal(item.data, 'NetAmount');
            totals.push(intNetAmount);
        });
        setTableNetTotals(totals);
    };
    const getSeriesList = () => {
        let series = [];
        yearList.forEach((year) => {
            const obj = { year: year, data: data.filter((item) => item.YEAR === year) };
            series.push(obj);
        });
        setSeriesList(series);
    };
    const getYears = (data) => {
        const uniqueYears = getUniqueValue(data, 'YEAR');
        const sortYears = getSortNumbers(uniqueYears);
        setYearList(sortYears);
    };

    const onLoadData = async () => {
        try {
            setLoading(true);
            const { data } = await onYearlySales(outletIds, departmentIds, numberOfYears);
            setData(data.result);
            getYears(data.result);
            setFromDate(data.fromDate);
            setToDate(data.toDate);
            getHaveDataYears(data.TotalYears);
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
        seriesList && getTableNetTotal();
    }, [seriesList]);

    useEffect(() => {
        yearList && getSeriesList();
    }, [yearList]);

    useEffect(() => {
        isSearching && onLoadData();
    }, [isSearching]);

    useEffect(() => {
        onLoadData();
    }, []);

    const series = seriesList.map((series) => {
        return {
            name: series.year,
            data: Months.map((month) => {
                const targetData = series.data.find((data) => data.Month === month);
                return salesTypeIsNet ? targetData?.NetAmount || 0 : targetData?.GrossAmount || 0;
            })
        };
    });

    const options = {
        colors: ['#FA7242', '#00355f', '#94B49F', '#513252'],
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
            categories: Months,
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
                enabled: true,
                theme: 'light'
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
                <Heading size="md" textAlign="left" mb="5">
                    Yearly Comparison
                </Heading>

                <FilterOption
                    isApiCalling={loading}
                    setIsSearching={setIsSearching}
                    setDepartmentIds={setDepartmentIds}
                    setOutletIds={setOutletIds}
                    setNumberOfYears={setNumberOfYears}
                    haveDataYears={haveDataYears}
                />

                <Text fontSize="14px" mb="5">
                    Work period considered from <strong>{getFormattedDateTime(fromDate)}</strong> to{' '}
                    <strong>{getFormattedDateTime(toDate)}</strong>
                </Text>

                {!loading ? (
                    <>
                        <Flex justify="center">
                            <Button
                                size="sm"
                                mr="2"
                                variant={salesTypeIsNet ? 'solid' : 'ghost'}
                                background={salesTypeIsNet ? 'primary.900' : ''}
                                _hover={salesTypeIsNet ? 'primary.900' : ''}
                                color={salesTypeIsNet ? 'white' : ''}
                                onClick={() => setSalesTypeIsNet(true)}>
                                Net
                            </Button>
                            <Button
                                size="sm"
                                variant={salesTypeIsNet ? 'ghost' : 'solid'}
                                background={salesTypeIsNet ? '' : 'primary.900'}
                                _hover={salesTypeIsNet ? '' : 'primary.900'}
                                color={salesTypeIsNet ? '' : 'white'}
                                onClick={() => setSalesTypeIsNet(false)}>
                                Gross
                            </Button>
                        </Flex>

                        <ReactApexChart options={options} series={series} type="bar" height="350" />
                    </>
                ) : (
                    <Loading line={8} mb="10" />
                )}

                {!loading ? (
                    <Flex mt="5" overflowX="scroll" direction="column">
                        <ExportOptions
                            title={`Yearly Comparison ${salesTypeIsNet ? '(Net)' : '(Gross)'}`}
                            fromDate={fromDate}
                            toDate={toDate}
                            colSpan={yearList?.length + 1}
                            scale={0.6}
                            margin="30pt">
                            <tr style={exportTable.tr}>
                                <th style={exportTable.th}>Month</th>

                                {yearList?.map((year, i) => (
                                    <th style={exportTable.th} key={i}>
                                        {year}
                                    </th>
                                ))}
                            </tr>
                            {Months.map((month, index) => {
                                return (
                                    <tr style={exportTable.tr} key={index}>
                                        <td style={exportTable.td}>{month}</td>

                                        {seriesList.map((item, i) => {
                                            const target = item.data.find(
                                                (element) => element.Month === month
                                            );

                                            return (
                                                <td style={exportTable.td} key={i}>
                                                    {salesTypeIsNet
                                                        ? commaFormat(target?.NetAmount || 0)
                                                        : commaFormat(target?.GrossAmount || 0)}
                                                </td>
                                            );
                                        })}
                                    </tr>
                                );
                            })}
                            <tr style={exportTable.tr}>
                                <td style={exportTable.td}>Total</td>

                                {seriesList.map((item, i) => {
                                    return (
                                        <td style={exportTable.td} key={i}>
                                            {salesTypeIsNet
                                                ? commaFormat(getTableTotal(item.data, 'NetAmount'))
                                                : commaFormat(
                                                      getTableTotal(item.data, 'GrossAmount')
                                                  )}
                                        </td>
                                    );
                                })}
                            </tr>
                        </ExportOptions>

                        <Table variant="simple" w="100%" size="sm" className="tableBorder">
                            <Thead bg="primary.900">
                                <Tr>
                                    <Th p="2" color="white">
                                        Month
                                    </Th>

                                    {yearList?.map((year, i) => (
                                        <Th p="2" color="white" key={i}>
                                            {year}
                                        </Th>
                                    ))}
                                </Tr>
                            </Thead>

                            <Tbody>
                                {Months.map((month, index) => {
                                    let amounts = [];

                                    return (
                                        <Tr key={index}>
                                            <Td px="2">{month}</Td>

                                            {seriesList.map((item, i) => {
                                                const target = item.data.find(
                                                    (element) => element.Month === month
                                                );
                                                amounts.push(target?.NetAmount || 0);

                                                let prevAmount = amounts[amounts.length - 2];
                                                let lastAmount = amounts[amounts.length - 1];

                                                return (
                                                    <Td px="2" key={i}>
                                                        <Flex>
                                                            <Box minW="120px">
                                                                {salesTypeIsNet
                                                                    ? commaFormat(
                                                                          target?.NetAmount || 0
                                                                      )
                                                                    : commaFormat(
                                                                          target?.GrossAmount || 0
                                                                      )}
                                                            </Box>

                                                            {prevAmount < lastAmount && (
                                                                <Box
                                                                    color="green.500"
                                                                    fontSize="25">
                                                                    <IoMdArrowDropup />
                                                                </Box>
                                                            )}

                                                            {prevAmount > lastAmount && (
                                                                <Box color="red.500" fontSize="25">
                                                                    <IoMdArrowDropdown />
                                                                </Box>
                                                            )}
                                                        </Flex>
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

                                    {seriesList.map((item, i) => {
                                        const currAmount = tableNetTotals[i];
                                        const prevAmount = tableNetTotals[i - 1];

                                        return (
                                            <Td px="2" fontWeight="semibold" key={i}>
                                                <Flex>
                                                    <Box minW="120px">
                                                        {salesTypeIsNet
                                                            ? commaFormat(
                                                                  getTableTotal(
                                                                      item.data,
                                                                      'NetAmount'
                                                                  )
                                                              )
                                                            : commaFormat(
                                                                  getTableTotal(
                                                                      item.data,
                                                                      'GrossAmount'
                                                                  )
                                                              )}
                                                    </Box>

                                                    {prevAmount < currAmount && (
                                                        <Box color="green.500" fontSize="25">
                                                            <IoMdArrowDropup />
                                                        </Box>
                                                    )}

                                                    {prevAmount > currAmount && (
                                                        <Box color="red.500" fontSize="25">
                                                            <IoMdArrowDropdown />
                                                        </Box>
                                                    )}
                                                </Flex>
                                            </Td>
                                        );
                                    })}
                                </Tr>
                            </Tbody>
                        </Table>
                    </Flex>
                ) : (
                    <Loading line={8} />
                )}
            </Flex>
        </Layout>
    );
};

export default YearlyComparisonChart;
