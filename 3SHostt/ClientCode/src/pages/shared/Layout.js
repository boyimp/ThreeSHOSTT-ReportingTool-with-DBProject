import { Flex, Stack, Box, useColorModeValue } from '@chakra-ui/react';
import React from 'react';
import Navbar from './Navbar';
import TopBg from './TopBg';
import { useSidebarContext } from '../../context/SidebarContext';

const Layout = ({ children }) => {
    const { isLgDevice, sidebarOpen, setSidebarOpen } = useSidebarContext();
    const bgColor = useColorModeValue('gray.100', '');
    // const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');

    return (
        <>
            <Flex position="relative" m="auto" w="100%" overflow="hidden" bg={bgColor}>
                <TopBg />

                <Stack
                    w="100%"
                    minH="100vh"
                    padding={{
                        base: '30px 20px',
                        lg: sidebarOpen ? '30px 20px 20px 308px' : '30px 20px 20px 20px'
                    }}
                    zIndex="1"
                    transition="0.4s ease">
                    <Navbar setSidebarOpen={setSidebarOpen} sidebarOpen={sidebarOpen} />
                    {children}
                </Stack>

                {!isLgDevice && sidebarOpen && (
                    <Box
                        bgGradient="linear(to-r, #4F4F4F99, #000000a1)"
                        zIndex="99"
                        h="100vh"
                        w="100vw"
                        position="fixed"
                        top="0"
                        left="0"
                        onClick={() => setSidebarOpen(false)}></Box>
                )}
            </Flex>
        </>
    );
};

export default Layout;
