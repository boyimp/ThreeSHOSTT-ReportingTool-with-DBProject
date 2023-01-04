import { Heading } from '@chakra-ui/react';
import React from 'react';

const NoDataFound = () => {
    return (
        <Heading
            color="brand.400"
            py="10"
            fontWeight="bold"
            letterSpacing="1px"
            textAlign="center"
            fontSize="18px">
            No Data Found !!
        </Heading>
    );
};

export default NoDataFound;
