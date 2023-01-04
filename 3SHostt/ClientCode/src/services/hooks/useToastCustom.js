//@ts-check
import { useColorModeValue, useToast, useToken } from '@chakra-ui/react';
import { useCallback } from 'react';

const DEFAULT_DURATION = 3000;
const DEFAULT_CONFIG = {
    title: 'Success',
    description: 'The action was successful',
    status: 'success',
    duration: DEFAULT_DURATION,
    isClosable: true,
    position: 'bottom-right'
};

const useToastCustom = () => {
    const toast = useToast();
    const [brandColorValue] = useToken('colors', ["green.900"]);
    const [errorColorValue] = useToken('colors', ["red.900"]);
    const [warningColorValue] = useToken('colors', ["yellow.800"]);
    const [infoColorValue] = useToken('colors', ["blue.700"]);
    const textColor = "white";

    const successToast = useCallback(
        (config = DEFAULT_CONFIG) => {
            toast({
                ...DEFAULT_CONFIG,
                title: 'Success',
                containerStyle: {
                    backgroundColor: brandColorValue,
                    color: textColor,
                    borderRadius: 10
                },
                description: 'The action was successful',
                variant:'subtle',
                status: 'success',
                ...config
            });
        },
        [toast, brandColorValue]
    );

    const infoToast = useCallback(
        (config = DEFAULT_CONFIG) => {
            toast({
                ...DEFAULT_CONFIG,
                title: 'Message',
                containerStyle: {
                    backgroundColor: infoColorValue,
                    color: textColor,
                    borderRadius: 10
                },
                description: '',
                status: 'info',
                ...config
            });
        },
        [toast]
    );

    const warningToast = useCallback(
        (config = DEFAULT_CONFIG) => {
            toast({
                ...DEFAULT_CONFIG,
                title: 'Warning',
                containerStyle: {
                    backgroundColor: warningColorValue,
                    color: textColor,
                    borderRadius: 10
                },
                description: 'Success with warning',
                status: 'warning',
                ...config
            });
        },
        [toast]
    );

    const errorToast = useCallback(
        (config = DEFAULT_CONFIG) => {
            toast({
                ...DEFAULT_CONFIG,
                title: 'Error',
                containerStyle: {
                    backgroundColor: errorColorValue,
                    color: textColor,
                    borderRadius: 10
                },
                description: 'Something went wrong',
                status: 'error',
                ...config
            });
        },
        [toast]
    );

    return { toast, successToast, warningToast, infoToast, errorToast };
};

export default useToastCustom;
