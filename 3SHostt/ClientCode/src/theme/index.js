//@ts-check
import { extendTheme } from '@chakra-ui/react';
import foundations from './foundations';
// import { mode } from '@chakra-ui/theme-tools';

const direction = 'ltr';

const config = {
    useSystemColorMode: false,
    initialColorMode: 'dark',
    cssVarPrefix: 'ckds'
};

export const theme = {
    // components: {
    //     Button: {
    //         baseStyle: (props) => ({
    //             _focus: {
    //                 boxShadow: 'none'
    //             }
    //         }),
    //         variants: {
    //             ghost: (props) => ({
    //                 color: mode(`brandGray.700`, `brandGray.400`)(props),
    //                 _hover: {
    //                     color: mode(`brandGray.400`, `white`)(props),
    //                     bg: 'transparent'
    //                 }
    //             }),
    //             solid: (props) => ({
    //                 bg: mode(`brandGray.500`, `brandGray.600`)(props),
    //                 color: mode(`white`, `white`)(props),
    //                 _hover: {
    //                     color: mode(`white`, `white`)(props),
    //                     bg: mode(`brandGray.600`, `brandGray.500`)(props)
    //                 }
    //             })
    //         }
    //     },
    //     Text: {
    //         baseStyle: (props) => ({
    //             color: mode(`brandGray.800`, `brandGray.50`)(props)
    //         }),
    //         variants: {
    //             label: (props) => ({
    //                 fontSize: 'sm',
    //                 color: mode(`brandGray.200`, `brandGray.400`)(props)
    //             }),
    //             'break-line': {
    //                 wordWrap: 'break-word',
    //                 wordBreak: 'break-all'
    //             }
    //         }
    //     },
    //     Link: {
    //         variants: {
    //             'break-line': {
    //                 wordWrap: 'break-word',
    //                 wordBreak: 'break-all'
    //             }
    //         }
    //     },
    //     Input: {
    //         baseStyle: (theme) => ({
    //             field: {
    //                 color: mode(`brandGray.800`, `brandGray.50`)(theme),
    //                 width: 'auto',
    //                 boxSizing: 'border-box',
    //                 _readOnly: {
    //                     bg: 'transparent'
    //                 }
    //             }
    //         }),
    //         variants: {
    //             outline: (props) => ({
    //                 field: {
    //                     _focus: {
    //                         borderColor: mode(`brandGray.800`, `white`)(props),
    //                         boxShadow: 'none'
    //                     }
    //                 }
    //             })
    //         }
    //     },
    //     Textarea: {
    //         baseStyle: (props) => ({
    //             color: mode(`brandGray.800`, `brandGray.50`)(props),
    //             boxSizing: 'border-box',
    //             _readOnly: {
    //                 bg: 'transparent'
    //             }
    //         }),
    //         variants: {
    //             outline: (props) => {
    //                 return {
    //                     _focus: {
    //                         borderColor: `${mode(`brandGray.800`, `white`)(props)}`,
    //                         boxShadow: 'none',
    //                         background: 'transparent !important'
    //                     }
    //                 };
    //             }
    //         }
    //     },
    //     Checkbox: {
    //         baseStyle: (props) => ({
    //             control: {
    //                 borderRadius: 'md',
    //                 _checked: {
    //                     bg: mode(`brandGray.400`, `brandGray.400`)(props),
    //                     borderColor: mode(`brandGray.400`, `brandGray.400`)(props),
    //                     color: mode(`brandGray.50`, `brandGray.50`)(props)
    //                 },
    //                 _readOnly: {
    //                     bg: mode(`brandGray.400`, `brandGray.400`)(props)
    //                 }
    //             }
    //         })
    //     },
    //     NumberInput: {
    //         baseStyle: (theme) => ({
    //             field: {
    //                 color: mode(`brandGray.800`, `brandGray.50`)(theme),
    //                 boxSizing: 'border-box',
    //                 bg: 'transparent',
    //                 height: 40
    //             }
    //         }),
    //         variants: {
    //             outline: (props) => ({
    //                 field: {
    //                     _focus: {
    //                         borderColor: mode(`brandGray.800`, `white`)(props),
    //                         boxShadow: 'none'
    //                     },
    //                     _readOnly: {
    //                         bg: 'transparent'
    //                     }
    //                 }
    //             })
    //         }
    //     }
    // },
    styles: {
        global: (props) => ({
            body: {
                // WebkitTextSizeAdjust: '100%',
                // WebkitFontSmoothing: 'antialiased',
                // color: '#333333',
                // overflowX: 'hidden',
                // padding: '0',
                // lineHeight: '18px',
                // margin: '0',
                // fontSize: '13px',
                // fontFamily: 'Inter, "open_sansregular", sans-serif',
                // fontWeight: 'normal',
                // backgroundImage: 'none',
                // paddingBottom: '30px',
                // backgroundColor: '#FFFFFF'
            },
            img: { display: 'inline' },
            '*, *::before, *::after': {
                borderWidth: 'initial',
                borderStyle: 'initial',
                boxSizing: 'initial'
            },
            '::-webkit-scrollbar': {
                WebkitApprearance: 'none',
                width: '10px'
            },
            '::-webkit-scrollbar-thumb': {
                borderRadius: '4px',
                backgroundColor: 'rgba(0, 0, 0, 0.5)',
                WebKitBoxShadow: '0 0 1px rgba(255, 255, 255, 0.5)'
            }
        })
    },
    direction,
    ...foundations,
    config
};

export default extendTheme(theme);
