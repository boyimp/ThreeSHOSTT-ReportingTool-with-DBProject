//@ts-check
import './CustomTreeSelect.css';
import React, { useEffect } from 'react';
import TreeSelect from 'rc-tree-select';
import { Box, useColorModeValue, useToken } from '@chakra-ui/react';

const CustomTreeSelect = ({
    treeData,
    selectedValue,
    onChange,
    onLoadNextData,
    treeDefaultExpandedKeys = [],
    isInvalid= false,
    isDisabled = false
}) => {
    const fontColor = useColorModeValue(`brandGray.800`, `brandGray.50`);
    const accentColor = useColorModeValue(`gray.200`, `brandGray.50`);
    const bgMenuColor = useColorModeValue('brandGray.50', 'brandGray.700');
    const controlFontColor = useColorModeValue('brandGray.700', 'brandGray.50');
    const borderColor = useColorModeValue(`brandGray.800`, `white`);

    const [invalidBorderColor] = useToken('colors', ['red.300']);

    useEffect(() => {
        document
            .getElementsByClassName('rc-tree-select-selection-search-input')[0]
            .removeAttribute('readonly');
    }, []);

    var validatedTreedata =
        treeData && treeData.length > 0
            ? treeData
            : [];

    const loadData = (node) => {
        return new Promise((resolve) => {
            if (!node.isLeaf && node.children && node.children.length > 0) {
                resolve();
                return;
            }
            setTimeout(() => {
                onLoadNextData(node).then((values) => {
                    resolve();
                });
            }, 500);
        });
    };
    return (
        <Box
            sx={{
                '.rc-tree-select-single:not(.rc-tree-select-customize-input) .rc-tree-select-selector':
                    {
                        height: 10,
                        borderRadius: 'md',
                        color: fontColor,
                        borderColor: isInvalid ? invalidBorderColor : 'inherit',
                        boxShadow: isInvalid ? `0 0 0 1px ${invalidBorderColor}` : 'inherit',
                    },
                '.rc-tree-select-clear-icon': {
                    fontSize: 'xl',
                    color: accentColor,
                    cursor: 'pointer'
                },
                '.rc-tree-select-focused .rc-tree-select-selector': {
                    borderColor: borderColor
                }
            }}>
            <TreeSelect
                className="tree-select"
                style={{
                    width: '100%'
                    // border: '1px solid var(--control-border-color)'
                }}
                dropdownRender={(menu) => {
                    return (
                        <Box bg={bgMenuColor} color={controlFontColor}>
                            {menu}
                        </Box>
                    );
                }}
                dropdownStyle={{
                    backgroundColor: bgMenuColor,
                    color: controlFontColor
                }}
                treeData={validatedTreedata}
                // labelInValue
                defaultValue={{
                    key: '0',
                    value: '0',
                    title: 'No data',
                    label: 'No Data',
                    disabled: false
                }}
                value={selectedValue}
                onChange={onChange}
                showTreeIcon={false}
                treeLine
                allowClear
                animation={'slide-up'}
                placeholder={<span>Please Select</span>}
                disabled={isDisabled}
                loadData={loadData}
                showArrow={false}
                treeDefaultExpandedKeys={treeDefaultExpandedKeys}
            />
        </Box>
    );
};

export default CustomTreeSelect;
