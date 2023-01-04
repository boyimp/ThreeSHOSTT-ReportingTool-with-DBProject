//@ts-check
import { Avatar, Box, useColorModeValue, useToken } from '@chakra-ui/react';
import React, { useEffect, useRef, useState } from 'react';
import Select, { components } from 'react-select';
import CreatableSelect from 'react-select/creatable';
import AsyncCreatableSelect from 'react-select/async-creatable';
import AsyncSelect from 'react-select/async';

export const CUSTOM_SELECT_TYPE = {
    REGULAR: 'regular',
    CREATABLE: 'creatable',
    ASYNC_REGULAR: 'aysnc-regular',
    ASYNC_CREATABLE: 'async-creatable'
};

const CustomTagSelect = ({
    options,
    selectedOptions,
    placeholder,
    onChange,
    onChangeInstant = (values) => {},
    onSearch = async (searchText) => {},
    isMulti = true,
    hasAvatar = false,
    hasSelectedAvatar = true,
    avatarFieldName = 'PhotoThumbUrl',
    avaterBorderRadius = 'full',
    isInvalid = false,
    isClearable = true,
    isDisabled = false,
    createLabelText = 'Create',
    optionListMaxHeight = 300,
    type = CUSTOM_SELECT_TYPE.REGULAR
}) => {
    const fontColor = useColorModeValue(`brandGray.800`, `brandGray.50`);
    const [fontColorValue] = useToken('colors', [fontColor]);
    const [invalidBorderColor] = useToken('colors', ['red.300']);
    const bgMenuColor = useColorModeValue('brandGray.100', 'brandGray.700');
    const borderColor = useColorModeValue(`brandGray.800`, `white`);
    const borderHover = useColorModeValue(`gray.400`, 'whiteAlpha.400');
    const selectBg = useColorModeValue(`brandGray.50`, 'brandGray.600');
    const selectedBg = useColorModeValue(`blue.50`, 'blue.900');
    const selectFontColor = useColorModeValue(`brandGray.800`, 'brandGray.50');
    const [selectedValues, setSelectedValues] = useState(null);

    const selectRef = useRef(null);

    useEffect(() => {
        setSelectedValues(selectedOptions);
    }, [selectedOptions]);

    const SingleValue = ({ children, ...props }) => {
        return (
            <components.SingleValue {...props}>
                {hasAvatar && hasSelectedAvatar && children && (
                    <Avatar
                        size={'sm'}
                        borderRadius={avaterBorderRadius}
                        mr={2}
                        src={props?.data[avatarFieldName] || ''}
                    />
                )}
                {children}
            </components.SingleValue>
        );
    };

    const MultiValue = ({ children, ...props }) => {
        return (
            <components.MultiValue {...props}>
                {hasAvatar && hasSelectedAvatar && children && (
                    <Avatar
                        size={'sm'}
                        borderRadius={avaterBorderRadius}
                        mr={2}
                        src={props?.data[avatarFieldName] || ''}
                    />
                )}
                {children}
            </components.MultiValue>
        );
    };

    const Option = ({ children, ...props }) => {
        return (
            <components.Option {...props}>
                {hasAvatar && (
                    <Avatar
                        size={'sm'}
                        borderRadius={avaterBorderRadius}
                        mr={2}
                        src={props?.data[avatarFieldName] || ''}
                    />
                )}
                {children}
            </components.Option>
        );
    };

    const props = {
        styles: {
            control: (provided, state) => {
                return {
                    ...provided,
                    background: 'transparent'
                };
            }
        },
        classNamePrefix: 'mb-tag-field',
        isMulti: isMulti,
        onChange: (values) => {
            var updated = isMulti ? values || [] : values ? [values] : values;
            onChangeInstant(updated);
        },
        onBlur: (event) => {
            var values = selectRef.current.getValue();
            onChange(values);
        },
        placeholder: placeholder,
        options: options,
        // value: selectedValues,
        defaultValue: selectedValues,
        isClearable: isClearable,
        isDisabled: isDisabled,
        'aria-invalid': isInvalid,
        components: { SingleValue, Option, MultiValue }
    };

    const loadOptions = (searchText) => {
        return new Promise((resolve) => {
            setTimeout(async () => {
                resolve(await onSearch(searchText));
            }, 300);
        });
    };

    const SelectComponent = ({ type, ...props }) => {
        switch (type) {
            case CUSTOM_SELECT_TYPE.REGULAR:
                return <Select ref={selectRef} {...props} />;
            case CUSTOM_SELECT_TYPE.ASYNC_REGULAR:
                return (
                    <AsyncSelect
                        ref={selectRef}
                        cacheOptions
                        {...{ ...props, options: null }}
                        defaultOptions={options}
                        loadOptions={loadOptions}
                    />
                );
            case CUSTOM_SELECT_TYPE.CREATABLE:
                return (
                    <CreatableSelect
                        ref={selectRef}
                        {...props}
                        formatCreateLabel={(value) => `${createLabelText} "${value}"`}
                    />
                );
            case CUSTOM_SELECT_TYPE.ASYNC_CREATABLE:
                return (
                    <AsyncCreatableSelect
                        ref={selectRef}
                        cacheOptions
                        {...{ ...props, options: null }}
                        defaultOptions={options}
                        loadOptions={loadOptions}
                    />
                );
            default:
                return (
                    <Select
                        ref={selectRef}
                        {...props}
                        components={{
                            DropdownIndicator: () => null,
                            IndicatorSeparator: () => null
                        }}
                    />
                );
        }
    };

    return (
        <Box
            width={'full'}
            cursor={isDisabled ? 'not-allowed' : 'auto'}
            opacity={isDisabled ? 0.4 : 1}
            sx={{
                '.mb-tag-field__menu': {
                    background: bgMenuColor,
                    color: fontColor
                },
                '.mb-tag-field__menu-list': {
                    maxH: optionListMaxHeight
                },
                '.mb-tag-field__control': {
                    padding: '4px 0px',
                    borderColor: isInvalid ? invalidBorderColor : 'inherit',
                    boxShadow: isInvalid ? `0 0 0 1px ${invalidBorderColor}` : 'inherit',
                    fontSize: 'md',
                    borderRadius: 'md'
                },
                '.mb-tag-field__control:hover': {
                    borderColor: borderHover
                },
                '.mb-tag-field__control--is-focused': {
                    borderColor: borderColor
                },
                '.mb-tag-field__multi-value': {
                    padding: '4px 8px',
                    borderRadius: '4px',
                    background: selectBg
                },
                '.mb-tag-field__multi-value__label': {
                    display: 'flex',
                    color: selectFontColor,
                    alignItems: 'center'
                },
                '.mb-tag-field__multi-value__remove>svg': {
                    height: 18,
                    width: 18,
                    color: selectFontColor
                },
                '.mb-tag-field__single-value,.mb-tag-field__input,.mb-tag-field__input:focus ': {
                    color: `${fontColorValue} !important`,
                    background: 'transparent !important',
                    boxShadow: 'none',
                    display: 'flex',
                    alignItems: 'center'
                },
                '.mb-tag-field__option--is-focused': {
                    background: selectBg,
                    color: selectFontColor
                },
                '.mb-tag-field__option--is-selected': {
                    background: selectedBg,
                    color: selectFontColor
                }
            }}>
            <SelectComponent {...props} type={type} />
        </Box>
    );
};
export default CustomTagSelect;
