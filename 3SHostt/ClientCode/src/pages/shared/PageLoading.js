import { Box, Flex, SkeletonText, useColorModeValue } from '@chakra-ui/react';
import React from 'react';

const PageLoading = () => {
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');

    return (
        <Flex d="flex" align="center" justify="center" h="100vh" w="100%">
            <Box bg={bgColor} rounded="xl" w="600px">
                <SkeletonText noOfLines={20} spacing="4" />
            </Box>
        </Flex>
    );
};

export default PageLoading;
