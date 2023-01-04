//@ts-check
import { Flex, Heading, useColorModeValue } from '@chakra-ui/react';
import React from 'react';
import { useDashboardFilterContext } from '../../context/DashboardFilterContext';
import Loading from '../shared/Loading';
import ReactApexChart from 'react-apexcharts';

const TopSalesCategoriesChart = () => {
    // color
    const chartHeadText = useColorModeValue('gray.600', 'lightgray');
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'lightgray');
    const gridColor = useColorModeValue('lightgray', 'gray');
    // get global data from- DashboardFilter
    const { categoriesLoading, grossAmount, itemName } = useDashboardFilterContext();

    const series = [
        {
            name: 'Total',
            data: grossAmount
        }
    ];
    const options = {
        colors: ['#FA7242'],
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
            showForSingleSeries: false,
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
            categories: itemName,
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
        <>
            <Flex
                width={{ base: '100%', lg: '50%' }}
                direction="column"
                bg={bgColor}
                boxShadow="sm"
                p="30px 20px"
                rounded="xl">
                <Heading fontSize="18px" mb="10px" color={chartHeadText}>
                    Top Sales Categories
                </Heading>

                {!categoriesLoading ? (
                    <ReactApexChart options={options} series={series} type="bar" height="250" />
                ) : (
                    <Loading line={8} />
                )}
            </Flex>
        </>
    );
};

export default TopSalesCategoriesChart;
