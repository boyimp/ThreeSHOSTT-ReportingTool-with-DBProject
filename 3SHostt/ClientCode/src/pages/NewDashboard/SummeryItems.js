import { Grid } from '@chakra-ui/react';
import React from 'react';
import TopSalesByProduct from './TopSalesByProduct';
import TopSalesCategories from './TopSalesCategories';
import TenderTypes from './TenderTypes';
import DepartmentWiseSales from './DepartmentWiseSales';
import DiscountInfo from './DiscountInfo';

const SummeryItems = () => {
    return (
        <>
            <Grid
                templateColumns={{
                    base: 'repeat(1, 1fr)',
                    md: 'repeat(2, 1fr)',
                    lg: 'repeat(3, 1fr)'
                }}
                gap={{ base: '3', lg: '4' }}
                pb="3">
                <TopSalesCategories />
                <TopSalesByProduct />
                <TenderTypes />
                <DepartmentWiseSales />
                <DiscountInfo />
            </Grid>
        </>
    );
};

export default SummeryItems;
