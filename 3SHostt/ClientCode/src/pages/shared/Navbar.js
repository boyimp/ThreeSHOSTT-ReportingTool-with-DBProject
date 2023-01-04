//@ts-check
import { Box, HStack, useColorMode, useDisclosure } from '@chakra-ui/react';
import React, { useEffect } from 'react';
import { HiMenuAlt1 } from 'react-icons/hi';
import AlertDialog from './AlertDialog';
import { IoLogOutSharp } from 'react-icons/io5';
import { MdLightMode } from 'react-icons/md';
import { IoMdMoon } from 'react-icons/io';
import { isSessionActive } from '../Login/login.action';
import { useHistory } from 'react-router-dom';

const Navbar = ({ sidebarOpen, setSidebarOpen }) => {
    const { isOpen, onOpen, onClose } = useDisclosure();
    const cancelRef = React.useRef();
    const { toggleColorMode, colorMode } = useColorMode();
    const history = useHistory();

    useEffect(() => {
        async function fetchSession() {
            var session = await isSessionActive();
            if (!session) {
                history.replace('/login');
            }
        }
        fetchSession();
    }, []);

    return (
        <HStack justify="space-between" mb="4">
            <HStack spacing="8">
                <Box
                    fontSize="26"
                    color="white"
                    cursor="pointer"
                    transition="0.3s ease"
                    onClick={() => setSidebarOpen(!sidebarOpen)}>
                    <HiMenuAlt1 />
                </Box>

                {/* <HStack d={{ base: 'none', lg: 'flex' }}>
                    <Breadcrumb spacing="8px" separator={<MdOutlineChevronRight />}>
                        <BreadcrumbItem color="white">
                            <BreadcrumbLink href="#">Dashboard</BreadcrumbLink>
                        </BreadcrumbItem>

                        <BreadcrumbItem color="white">
                            <BreadcrumbLink href="#">Test Page</BreadcrumbLink>
                        </BreadcrumbItem>

                        <BreadcrumbItem isCurrentPage color="white">
                            <BreadcrumbLink href="#">One</BreadcrumbLink>
                        </BreadcrumbItem>
                    </Breadcrumb>
                </HStack> */}
            </HStack>

            <HStack spacing="3">
                <Box
                    title="Switch dark/light mode"
                    cursor="pointer"
                    fontSize="24"
                    color="white"
                    onClick={toggleColorMode}>
                    {colorMode === 'light' ? <IoMdMoon /> : <MdLightMode />}
                </Box>

                <Box title="Signout" cursor="pointer" fontSize="24" color="white" onClick={onOpen}>
                    <IoLogOutSharp />
                </Box>

                <AlertDialog
                    isOpen={isOpen}
                    // onOpen={onOpen}
                    onClose={onClose}
                    cancelRef={cancelRef}
                    headerText="Log Out"
                    bodyText="Are you sure you want to logout?"
                />
            </HStack>
        </HStack>
    );
};

export default Navbar;
