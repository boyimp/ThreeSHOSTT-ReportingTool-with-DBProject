import {
    AccordionButton,
    AccordionItem,
    AccordionPanel,
    Box,
    HStack,
    Text,
    useColorMode,
    useColorModeValue
} from '@chakra-ui/react';
import React from 'react';
import { MdKeyboardArrowDown } from 'react-icons/md';
import { FaAngleRight } from 'react-icons/fa';
import { GoDash } from 'react-icons/go';
import { NavLink, useHistory, useLocation } from 'react-router-dom';
import { useSidebarContext } from '../../context/SidebarContext';
import { setSidebarActiveAccordion } from '../../services/localStorage/sidebarActiveAccordion';

const SidebarItem = ({ item }) => {
    const textColor = useColorModeValue('gray.600', 'whiteAlpha.800');
    const subTextColor = useColorModeValue('gray.500', 'whiteAlpha.600');
    const { colorMode } = useColorMode();
    const history = useHistory();
    const { isLgDevice, sidebarOpen, setSidebarOpen } = useSidebarContext();
    const { pathname } = useLocation();

    const getActiveClass = (isActive, type) => {
        if (colorMode === 'light' && isActive && type === 'navLink') {
            return 'activeLinkLight';
        }
        if (colorMode === 'dark' && isActive && type === 'navLink') {
            return 'activeLinkDark';
        }
        if (colorMode === 'light' && isActive && type === 'subNavLink') {
            if (pathname === '/loading') return;
            return 'activeSubLinkLight';
        }
        if (colorMode === 'dark' && isActive && type === 'subNavLink') {
            if (pathname === '/loading') return;
            return 'activeSubLinkDark';
        }
    };

    const clickLinkedMenu = (item) => {
        !isLgDevice && sidebarOpen && setSidebarOpen(false);
        setSidebarActiveAccordion(item.id - 1);
    };

    const goToLink = (legacy, link, item) => {
        if (legacy) {
            // const location = `${window.location.origin}${window.location.pathname}${link}`;
            // window.location.replace(location);
        } else {
            history.push(link);
        }

        !isLgDevice && sidebarOpen && setSidebarOpen(false);
        setSidebarActiveAccordion(item.id - 1);
    };

    return (
        <AccordionItem ml="4" mr="3" my="2">
            {item.link ? (
                <NavLink
                    onClick={() => clickLinkedMenu(item)}
                    to={item.link}
                    className={(isActive) => getActiveClass(isActive, 'navLink')}>
                    <AccordionButton px="0">
                        <HStack px="4" py="1">
                            <HStack spacing="5">
                                <Box fontSize="18" color="brand.400" fontWeight="bold">
                                    {<item.icon />}
                                </Box>
                                <Text fontSize="15px" fontWeight="normal" color={textColor}>
                                    {item.itemName}
                                </Text>
                            </HStack>

                            <Box color={textColor} fontSize="xl">
                                {item.subItems.length > 0 && <MdKeyboardArrowDown />}
                            </Box>
                        </HStack>
                    </AccordionButton>
                </NavLink>
            ) : (
                <AccordionButton px="0" onClick={() => setSidebarActiveAccordion(item.id - 1)}>
                    <HStack justify="space-between" w="100%" px="4" py="1">
                        <HStack spacing="5">
                            <Box fontSize="18" color="brand.400" fontWeight="bold">
                                {<item.icon />}
                            </Box>
                            <Text fontSize="15px" fontWeight="normal" color={textColor}>
                                {item.itemName}
                            </Text>
                        </HStack>

                        <Box color={textColor} fontSize="xl">
                            {item.subItems.length > 0 && <MdKeyboardArrowDown />}
                        </Box>
                    </HStack>
                </AccordionButton>
            )}

            {/* subitems  */}
            {item.subItems.length > 0 && (
                <AccordionPanel pb="1">
                    {item.subItems.map((sub) =>
                        sub.link && !sub.legacy ? (
                            <NavLink
                                onClick={() => goToLink(sub.legacy, sub.link, item)}
                                // to={sub.legacy ? '/loading' : sub.link}
                                to={sub.link}
                                key={sub.id}
                                className={(isActive) => getActiveClass(isActive, 'subNavLink')}>
                                <HStack
                                    pl="6"
                                    color={subTextColor}
                                    fontWeight="semibold"
                                    mb="3"
                                    cursor="pointer"
                                    transition=".2s ease"
                                    className="subNavLink"
                                    _hover={{ color: 'brand.400' }}>
                                    <Box>
                                        <GoDash />
                                    </Box>
                                    <Box fontSize="15px" fontWeight="normal">
                                        {sub.subName}
                                    </Box>
                                </HStack>
                            </NavLink>
                        ) : sub.link && sub.legacy ? (
                            <a
                                onClick={() => goToLink(sub.legacy, sub.link, item)}
                                href={`${window.location.origin}${window.location.pathname}${sub.link}`}
                                key={sub.id}
                                className={(isActive) => getActiveClass(isActive, 'subNavLink')}>
                                <HStack
                                    pl="6"
                                    color={subTextColor}
                                    fontWeight="semibold"
                                    mb="3"
                                    cursor="pointer"
                                    transition=".2s ease"
                                    className="subNavLink"
                                    _hover={{ color: 'brand.400' }}>
                                    <Box>
                                        <GoDash />
                                    </Box>
                                    <Box fontSize="15px" fontWeight="normal">
                                        {sub.subName}
                                    </Box>
                                </HStack>
                            </a>
                        ) : (
                            <HStack
                                key={sub.id}
                                pl="6"
                                color={subTextColor}
                                mb="3"
                                cursor="pointer"
                                transition=".2s ease"
                                className="subNavLink"
                                _hover={{ color: textColor }}>
                                <Box>
                                    <GoDash />
                                </Box>
                                <Box fontSize="15px" fontWeight="normal">
                                    {sub.subName}
                                </Box>
                            </HStack>
                        )
                    )}
                </AccordionPanel>
            )}
        </AccordionItem>
    );
};

export default SidebarItem;
