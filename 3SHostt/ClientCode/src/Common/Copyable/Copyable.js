//@ts-check
import { HStack, Text, IconButton, useClipboard as useNativeClipboard } from '@chakra-ui/react';
import React from 'react';
import { IoCopyOutline } from 'react-icons/io5';
import useToastCustom from '../../../services/hooks/useToastCustom';

const Copyable = ({
    children,
    value,
    onCopy = (value) => {},
    useClipboard = true,
    containerProps = { minH: 10 },
    copyButtonProps = {},
    hideCopyButton = false
}) => {
    const { onCopy: onClipboardCopy } = useNativeClipboard(value);
    const toast = useToastCustom();

    return (
        <HStack className={'copy-text-area'} {...containerProps}>
            {children}
            {!hideCopyButton && (
                <IconButton
                    sx={{
                        '.copy-text-area:hover &': {
                            visibility: 'visible',
                            cursor: 'pointer',
                            // marginTop: 1
                        }
                    }}
                    h={'auto'}
                    marginTop={2}
                    visibility={'hidden'}
                    cursor={'default'}
                    variant="ghost"
                    aria-label="copy"
                    onClick={() => {
                        useClipboard && onClipboardCopy();
                        onCopy(value);
                        toast.successToast({title: "Clipboard", description: "Value copied to clipboard"})
                    }}
                    icon={<IoCopyOutline />}
                    {...copyButtonProps}
                />
            )}
        </HStack>
    );
};

export default Copyable;
