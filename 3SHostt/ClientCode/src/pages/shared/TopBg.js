import { Box, useColorModeValue } from '@chakra-ui/react';
import React from 'react';
import { useLocation } from 'react-router-dom';

const TopBg = () => {
    const bgColor = useColorModeValue('primary.900', '');
    // const textColor = useColorModeValue('gray.600', 'whiteAlpha.600');
    const { pathname } = useLocation();

    return (
        <Box
            h={{
                base: pathname === '/dashboard' ? '942px' : '315px',
                sm: pathname === '/dashboard' ? '655px' : '315px',
                md: pathname === '/dashboard' ? '509px' : '315px',
                lg: pathname === '/dashboard' ? '315px ' : '315px'
            }}
            w="100%"
            bg={bgColor}
            position="absolute"
            top="0"></Box>
    );
};

export default TopBg;
