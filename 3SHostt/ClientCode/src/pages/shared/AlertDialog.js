import {
    AlertDialog,
    AlertDialogBody,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogContent,
    AlertDialogOverlay,
    Button
} from '@chakra-ui/react';
import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import { logout } from '../Login/login.action';

const LogoutModal = ({ isOpen, cancelRef, onClose, headerText, bodyText }) => {
    const history = useHistory();
    const [response, setResoponse] = useState(false);

    return (
        <>
            <AlertDialog isOpen={isOpen} leastDestructiveRef={cancelRef} onClose={onClose}>
                <AlertDialogOverlay>
                    <AlertDialogContent>
                        <AlertDialogHeader fontSize="xl" fontWeight="bold">
                            Log Out
                        </AlertDialogHeader>

                        <AlertDialogBody>Are you sure you want to logout?</AlertDialogBody>

                        <AlertDialogFooter>
                            <Button
                                ref={cancelRef}
                                onClick={onClose}
                                bg="primary.900"
                                color="white"
                                _active={{ bg: 'primary.900' }}
                                _hover={{ bg: 'primary.800' }}>
                                No
                            </Button>
                            <Button
                                colorScheme="red"
                                ml={3}
                                onClick={async () => {
                                    setResoponse(true);
                                    var response = await logout();
                                    if (response === true) {
                                        history.replace('/');
                                    }
                                    setResoponse(false);
                                }}
                                disabled={response}>
                                Yes
                            </Button>
                        </AlertDialogFooter>
                    </AlertDialogContent>
                </AlertDialogOverlay>
            </AlertDialog>
        </>
    );
};

export default LogoutModal;
