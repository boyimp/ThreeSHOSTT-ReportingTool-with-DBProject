import React from 'react';
import { Heading, VStack } from '@chakra-ui/react';
import Layout from '../shared/Layout';

const PageNotFound = () => {
    return (
        <Layout>
            <VStack h="85vh" justify="center">
                <Heading fontSize="60px" color="brand.400" mt="10">
                    404
                </Heading>
                <Heading fontSize="18px">Page Not Found</Heading>
            </VStack>
        </Layout>
    );
};

export default PageNotFound;
