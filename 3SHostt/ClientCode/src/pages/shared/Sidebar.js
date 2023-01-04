//@ts-check
import React, { useState } from 'react';
import { Accordion, Box, Flex, Image, Stack, Text, useColorModeValue } from '@chakra-ui/react';
import { sidebarData } from '../../data';
import SidebarItem from './SidebarItem';
import { useSidebarContext } from '../../context/SidebarContext';
import { useLocation } from 'react-router-dom';
import { useEffect } from 'react';
import { getSidebarActiveAccordion } from '../../services/localStorage/sidebarActiveAccordion';

const Sidebar = () => {
    const [activeAccordionIndex, setActiveAccordionIndex] = useState(1);
    const [sidebarItemRender, setSidebarItemRender] = useState(true);
    const { sidebarOpen } = useSidebarContext();
    const bgColor = useColorModeValue('white', '#232934');
    const textColor = useColorModeValue('gray.800', 'whiteAlpha.800');
    const bgGradient = useColorModeValue(
        'linear-gradient(to right, #ffffff24, #a999997d, #ffffff24)',
        'linear-gradient(to right, #ffffff00, #a99999ab, #ffffff00)'
    );
    const { pathname } = useLocation();

    const getAccordionIndex = async () => {
        setSidebarItemRender(false);
        const index = await getSidebarActiveAccordion();
        setActiveAccordionIndex(index);
        setSidebarItemRender(true);
    };

    useEffect(() => {
        getAccordionIndex();
    }, []);

    return (
        <Stack
            w="305px"
            h="100vh"
            position="fixed"
            style={{ left: sidebarOpen ? '0px' : '-400px' }}
            transition="0.4s ease"
            zIndex="999"
            d={pathname === '/login' ? 'none' : 'flex'}>
            <Stack
                bg={bgColor}
                h="100vh"
                w="92%"
                m="auto"
                mt="3"
                position="relative"
                borderRadius="30px 30px 0 0"
                overflowY="scroll"
                className="sidebarScroll"
                boxShadow={{ base: 'xs', lg: 'xl' }}>
                <Flex mt="40px" direction="column" align="center">
                    <Flex justify="center" align="center">
                        <Image w="70px" src="./Images/3SLogo1.png" />
                        <Box h="20px" w="1px" bg={textColor} mx="15px"></Box>
                        <Image w="80px" src="./Images/3S_Hostt.png" />
                    </Flex>

                    <Text fontWeight="bold" mt="10px" color={textColor} fontSize="13px" mb="25px">
                        Taking Technology Forward
                    </Text>

                    <Box w="220px" h="1px" bgGradient={bgGradient}></Box>
                </Flex>

                <Stack w="100%" pt="20px">
                    {sidebarItemRender && (
                        <Accordion defaultIndex={[activeAccordionIndex]} allowMultiple>
                            {sidebarData.map((item) => (
                                <SidebarItem key={item.id} item={item} />
                            ))}
                        </Accordion>
                    )}
                </Stack>
            </Stack>
        </Stack>
    );
};

export default Sidebar;
