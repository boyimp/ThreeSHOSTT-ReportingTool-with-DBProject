//@ts-check
import { Stack, Flex, Text } from '@chakra-ui/react';
import React, { useState } from 'react';
import Layout from '../shared/Layout';
import TopSalesCategoriesChart from './TopSalesCategoriesChart';
import DepartmentWiseSalesChart from './DepartmentWiseSalesChart';
import SummeryItems from './SummeryItems';
import DashboardFilter from './DashboardFilter';
import CardItems from './CardItems';

const NewDashboard = () => {
    const [isTicketOpen, setIsTicketOpen] = useState(true);

    return (
        <Layout>
            <Stack>
                <DashboardFilter />

                {!isTicketOpen && (
                    <Text
                        color="brand.400"
                        pb="5"
                        pl="2"
                        fontWeight="bold"
                        letterSpacing="1px"
                        textAlign="center"
                        fontSize="18px">
                        No Tickets Yet !!
                    </Text>
                )}

                <CardItems setIsTicketOpen={setIsTicketOpen} />

                <Flex direction={{ base: 'column', lg: 'row' }} gap="15px" pb="10px">
                    <TopSalesCategoriesChart />
                    <DepartmentWiseSalesChart />
                </Flex>

                <SummeryItems />
            </Stack>
        </Layout>
    );
};

export default NewDashboard;
