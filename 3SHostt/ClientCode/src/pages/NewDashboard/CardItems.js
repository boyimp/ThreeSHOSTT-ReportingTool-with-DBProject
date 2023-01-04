import React, { useState, useEffect } from 'react';
import { Box, Heading, Flex, Text, Grid, GridItem, useColorModeValue } from '@chakra-ui/react';
import { onDashboardCardValue } from '../../services/dashboard-service';
import Loading from '../shared/Loading';
import { useDashboardFilterContext } from '../../context/DashboardFilterContext';
import { commaFormat } from '../../services/_utils';

const CardItems = ({ setIsTicketOpen }) => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.700', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'blue.100');

    const {
        isExact,
        fromDate,
        toDate,
        outletIds,
        departmentIds,
        setFromDate,
        setToDate,
        isSearching,
        setIsSearching
    } = useDashboardFilterContext();
    const [loading, setLoading] = useState(true);
    const [data, setData] = useState([]);

    const {
        TotalGrossSale,
        TotalNetSale,
        DiscountAmount,
        DiscountCount,
        GiftsAmount,
        GiftsCount,
        NoOfGuest,
        PerGuestSpend,
        PerTicketSpend,
        TicketsCount,
        VoidsAmount,
        VoidsCount,
        stockValue
    } = data;

    useEffect(() => {
        onLoadData();
    }, []);

    useEffect(() => {
        isSearching && onLoadData();
    }, [isSearching]);

    const onLoadData = async () => {
        try {
            setLoading(true);
            const { data } = await onDashboardCardValue(
                false,
                fromDate,
                toDate,
                outletIds,
                departmentIds
            );
            setData(data.result);
            setFromDate(data.fromDate.slice(0, 16));
            setToDate(data.toDate.slice(0, 16));
            setLoading(false);
            setIsSearching(false);
            setIsTicketOpen(data.result.TotalNetSale > 0 ? true : false);
        } catch (error) {
            setLoading(true);
            console.log(error);
            setLoading(false);
        }
    };

    return (
        <>
            <Grid
                templateColumns={{
                    base: 'repeat(1, 1fr)',
                    sm: 'repeat(2, 1fr)',
                    lg: 'repeat(4, 1fr)'
                }}
                gap={{ base: '3', lg: '4' }}
                pb="10px">
                <GridItem
                    w="100%"
                    minH="130px"
                    h="auto"
                    bg={bgColor}
                    boxShadow="sm"
                    p="20px 30px 30px 30px"
                    rounded="xl">
                    {!loading ? (
                        <>
                            <Text fontWeight="bold" fontSize="16">
                                Total Sales
                            </Text>
                            <Flex
                                alignItems="center"
                                justifyContent="space-between"
                                w="100%"
                                h="100%"
                                gap="10px">
                                <Box>
                                    <Text fontWeight="bold" color="gray" fontSize="15">
                                        Net
                                    </Text>
                                    <Heading
                                        fontSize={{ base: '24', sm: '20' }}
                                        color={brandBgText}>
                                        {commaFormat(TotalNetSale || 0)}
                                    </Heading>
                                </Box>
                                <Box>
                                    <Text fontWeight="bold" color="gray" fontSize="15">
                                        Gross
                                    </Text>
                                    <Heading
                                        fontSize={{ base: '24', sm: '20' }}
                                        color={brandBgText}>
                                        {commaFormat(TotalGrossSale || 0)}
                                    </Heading>
                                </Box>
                            </Flex>
                        </>
                    ) : (
                        <Loading line={4} />
                    )}
                </GridItem>

                <GridItem
                    w="100%"
                    minH="130px"
                    h="auto"
                    bg={bgColor}
                    boxShadow="sm"
                    p="15px 30px"
                    rounded="xl">
                    {!loading ? (
                        <>
                            <Flex direction="column" justify="center" w="100%" h="100%" gap="5px">
                                <a
                                    href={`${window.location.origin}${
                                        window.location.pathname
                                    }${'UI/VoidOrders.aspx'}`}>
                                    <Flex
                                        gap="10px"
                                        align="center"
                                        justify="space-between"
                                        cursor="pointer"
                                        _hover={{ textDecoration: 'underline' }}>
                                        <Text
                                            fontWeight="bold"
                                            color={textColor}
                                            marginRight="10px">
                                            Void ({VoidsCount || 0})
                                        </Text>
                                        <Heading
                                            fontSize={{ base: '20', sm: '18' }}
                                            color={brandBgText}>
                                            {commaFormat(VoidsAmount || 0)}
                                        </Heading>
                                    </Flex>
                                </a>

                                <a
                                    href={`${window.location.origin}${
                                        window.location.pathname
                                    }${'UI/GiftOrders.aspx'}`}>
                                    <Flex
                                        gap="10px"
                                        align="center"
                                        justify="space-between"
                                        cursor="pointer"
                                        _hover={{ textDecoration: 'underline' }}>
                                        <Text
                                            fontWeight="bold"
                                            color={textColor}
                                            marginRight="10px">
                                            Gift ({GiftsCount || 0})
                                        </Text>
                                        <Heading
                                            fontSize={{ base: '20', sm: '18' }}
                                            color={brandBgText}>
                                            {commaFormat(GiftsAmount || 0)}
                                        </Heading>
                                    </Flex>
                                </a>

                                <Flex gap="10px" align="center" justify="space-between">
                                    <Text fontWeight="bold" color={textColor} marginRight="10px">
                                        Discount ({DiscountCount || 0})
                                    </Text>
                                    <Heading
                                        fontSize={{ base: '20', sm: '18' }}
                                        color={brandBgText}>
                                        {commaFormat(DiscountAmount || 0)}
                                    </Heading>
                                </Flex>
                            </Flex>
                        </>
                    ) : (
                        <Loading line={4} />
                    )}
                </GridItem>

                <GridItem
                    w="100%"
                    minH="130px"
                    h="auto"
                    bg={bgColor}
                    boxShadow="sm"
                    p="20px 30px"
                    rounded="xl">
                    {!loading ? (
                        <>
                            {' '}
                            <Text fontSize="16" fontWeight="bold" color={brandBgText} mb="15px">
                                Average Spend
                            </Text>
                            <Flex direction="column" w="100%" h="100%">
                                <Flex gap="10px" align="center" justify="space-between">
                                    <Text
                                        fontSize={{ base: '16', sm: '15' }}
                                        fontWeight="bold"
                                        color={textColor}>
                                        Per Ticket ({TicketsCount || 0})
                                    </Text>
                                    <Heading
                                        fontSize={{ base: '20', sm: '18' }}
                                        color={brandBgText}>
                                        {commaFormat(PerTicketSpend || 0)}
                                    </Heading>
                                </Flex>

                                <Flex gap="10px" align="center" justify="space-between">
                                    <Text
                                        fontSize={{ base: '16', sm: '15' }}
                                        fontWeight="bold"
                                        color={textColor}>
                                        Per Guest ({NoOfGuest || 0})
                                    </Text>
                                    <Heading
                                        fontSize={{ base: '20', sm: '18' }}
                                        color={brandBgText}>
                                        {commaFormat(PerGuestSpend || 0)}
                                    </Heading>
                                </Flex>
                            </Flex>
                        </>
                    ) : (
                        <Loading line={4} />
                    )}
                </GridItem>

                <GridItem
                    w="100%"
                    minH="130px"
                    h="auto"
                    bg={bgColor}
                    boxShadow="sm"
                    p="20px 30px"
                    rounded="xl">
                    {!loading ? (
                        <>
                            <Flex
                                direction="column"
                                align="center"
                                justify="center"
                                w="100%"
                                h="100%">
                                <Text fontWeight="bold" color={textColor}>
                                    Stock Value
                                </Text>

                                <Heading fontSize="30" color={brandBgText}>
                                    {commaFormat(stockValue || 0)}
                                </Heading>
                            </Flex>
                        </>
                    ) : (
                        <Loading line={4} />
                    )}
                </GridItem>
            </Grid>
        </>
    );
};

export default CardItems;
