import { Flex, Heading, useBreakpointValue, useColorModeValue } from '@chakra-ui/react';
import React from 'react';
import { useDashboardFilterContext } from '../../context/DashboardFilterContext';
import Loading from '../shared/Loading';
import ReactApexChart from 'react-apexcharts';

const DepartmentWiseSalesChart = () => {
    const chartHeadText = useColorModeValue('gray.600', 'lightgray');
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.800', 'lightgray');

    const { departmentsLoading, departments, amounts } = useDashboardFilterContext();

    const isLargeScreen = useBreakpointValue({
        lg: true,
        '2xl': true,
        md: false,
        sm: false,
        base: false
    });

    const series = amounts;
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
                        show: isLargeScreen ? false : true
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
        labels: departments,
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
        <>
            <Flex
                width={{ base: '100%', lg: '50%' }}
                direction="column"
                bg={bgColor}
                boxShadow="sm"
                p="30px 20px"
                rounded="xl">
                <Heading fontSize="18px" mb="10px" color={chartHeadText}>
                    Department Wise Sales
                </Heading>

                {!departmentsLoading ? (
                    <ReactApexChart
                        options={options}
                        series={series}
                        type="donut"
                        height="250"
                        w="100%"
                    />
                ) : (
                    <Loading line={8} />
                )}
            </Flex>
        </>
    );
};

export default DepartmentWiseSalesChart;
