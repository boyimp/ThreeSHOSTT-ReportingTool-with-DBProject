import {
    Button,
    Flex,
    HStack,
    Input,
    Text,
    useColorMode,
    useColorModeValue,
    useMediaQuery
} from '@chakra-ui/react';
import React, { useState, useEffect } from 'react';
import { useDashboardFilterContext } from '../../context/DashboardFilterContext';
import { onDepartmentNames, onOutletNames } from '../../services/dashboard-service';
import Select from 'react-select';
import { useQuery } from 'react-query';

const DashboardFilter = () => {
    // chakra
    const [isLargerThanSm] = useMediaQuery('(min-width: 30em)');
    // color
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray.700', 'whiteAlpha.800');
    const brandBgText = useColorModeValue('primary.900', 'blue.100');
    const { colorMode } = useColorMode();
    const selectInputBg = useColorModeValue('white', '#232934');
    // use query data
    const { data: outletsData, isLoading: outletLoading } = useQuery('outlets', () =>
        onOutletNames()
    );
    const { data: departmentsData, isLoading: deptLoading } = useQuery('departments', () =>
        onDepartmentNames()
    );
    const outletList = outletsData?.data || [];
    const departmentsList = departmentsData?.data || [];
    // data
    const [selectedOutlets, setSelectedOutlets] = useState([]);
    const [selectedDepartments, setSelectedDepartments] = useState([]);

    const {
        fromDate,
        setFromDate,
        toDate,
        setToDate,
        setIsSearching,
        setOutletIds,
        setDepartmentIds
    } = useDashboardFilterContext();

    const setWorkPeriod = () => {
        setFromDate(null);
        setToDate(null);
        setOutletIds([0]);
        setDepartmentIds([0]);
        setSelectedOutlets([]);
        setSelectedDepartments([]);
        setIsSearching(true);
    };

    useEffect(() => {
        selectedOutlets.length > 0
            ? setOutletIds(selectedOutlets.map((item) => item.value))
            : setOutletIds([0]);

        selectedDepartments.length > 0
            ? setDepartmentIds(selectedDepartments?.map((item) => item.value))
            : setDepartmentIds([0]);
    }, [selectedOutlets, selectedDepartments]);

    // select options
    const outletOptions = outletList.map((item) => ({
        value: item.id,
        label: item.Name
    }));
    const departmentOptions = departmentsList.map((item) => ({
        value: item.id,
        label: item.Name
    }));
    // custom style
    const customStyles = {
        control: (provided, state) => ({
            ...provided,
            background: selectInputBg,
            maxHeight: '60px',
            minWidth: '215px',
            width: isLargerThanSm ? '215px' : '100%',
            overflowY: 'scroll',
            fontSize: '15px',
            marginLeft: 'auto',
            marginRight: '4px',
            borderRadius: '5px',
            border: colorMode === 'light' ? '1px solid lightgray' : '1px solid gray'
        }),
        valueContainer: (provided, state) => ({
            ...provided,
            padding: '0 6px'
        }),
        menuList: (provided, state) => ({
            ...provided,
            minWidth: '215px',
            width: isLargerThanSm ? '215px' : '100%',
            background: selectInputBg,
            color: textColor,
            overflowY: 'auto'
        }),
        placeholder: (provided, state) => ({
            ...provided,
            color: textColor
        }),
        option: (provided, state) => ({
            ...provided,
            minHeight: '34px',
            height: '34px',
            fontSize: '15px',
            minWidth: '215px',
            width: isLargerThanSm ? '215px' : '100%',
            color: state.isFocused ? 'black' : textColor
        }),
        input: (provided, state) => ({
            ...provided,
            margin: '0px',
            height: '34px',
            color: textColor
        }),
        indicatorSeparator: (state) => ({
            display: 'none'
        }),
        indicatorsContainer: (provided, state) => ({
            ...provided,
            height: '34px'
        }),
        singleValue: (provided, state) => ({
            ...provided,
            color: textColor
        })
    };

    return (
        <HStack gap="10px" pb="5" flexWrap="wrap" align="flex-end">
            <Flex
                direction="column"
                w={isLargerThanSm ? '215px' : '100%'}
                sx={{ margin: '0px!important' }}>
                <Text color="white" fontSize="15px">
                    From Date
                </Text>
                <Input
                    type="datetime-local"
                    value={fromDate}
                    onChange={(e) => setFromDate(e.target.value)}
                    bg={bgColor}
                    h="38px"
                    color={textColor}
                    padding="5px"
                    fontSize="15px"
                    border={colorMode === 'light' ? '1px solid lightgray' : '1px solid gray'}
                />
            </Flex>

            <Flex
                direction="column"
                w={isLargerThanSm ? '215px' : '100%'}
                sx={{ margin: '0px!important' }}>
                <Text color="white" fontSize="15px">
                    To Date
                </Text>
                <Input
                    type="datetime-local"
                    value={toDate}
                    onChange={(e) => setToDate(e.target.value)}
                    bg={bgColor}
                    h="38px"
                    color={textColor}
                    padding="5px"
                    fontSize="15px"
                    border={colorMode === 'light' ? '1px solid lightgray' : '1px solid gray'}
                />
            </Flex>

            <Flex
                direction="column"
                align="flex-start"
                w={isLargerThanSm ? '215px' : '100%'}
                sx={{ margin: '0px!important' }}>
                <Text color="white" fontSize="15px">
                    Select Outlets
                </Text>
                <Select
                    styles={customStyles}
                    closeMenuOnSelect={false}
                    isMulti
                    value={selectedOutlets}
                    onChange={setSelectedOutlets}
                    options={outletList.length > 0 ? outletOptions : null}
                    className="reactSelectFilterInput"
                    placeholder={
                        !outletLoading && outletList.length > 0
                            ? 'All Outlets'
                            : !outletLoading && outletList.length === 0
                            ? 'All'
                            : 'Loading..'
                    }
                />
            </Flex>

            <Flex
                direction="column"
                align="flex-start"
                w={isLargerThanSm ? '215px' : '100%'}
                sx={{ margin: '0px!important' }}>
                <Text color="white" fontSize="15px">
                    Select Department
                </Text>
                <Select
                    styles={customStyles}
                    closeMenuOnSelect={false}
                    isMulti
                    value={selectedDepartments}
                    onChange={setSelectedDepartments}
                    options={departmentsList.length > 0 ? departmentOptions : null}
                    className="reactSelectFilterInput"
                    placeholder={
                        !deptLoading && departmentsList.length > 0
                            ? 'All Departments'
                            : !deptLoading && departmentsList.length === 0
                            ? 'All'
                            : 'Loading..'
                    }
                />
            </Flex>

            {/* <Flex
                direction="column"
                w={isLargerThanSm ? 'auto' : '100%'}
                sx={{ margin: '0px!important' }}>
                <Checkbox
                    size="md"
                    isChecked={isExact}
                    onChange={(e) => setIsExact(e.target.checked)}
                    color="gray.300"
                    borderColor="gray.300">
                    Exact Time
                </Checkbox>
            </Flex> */}

            <Flex
                sx={{ marginLeft: '0px!important' }}
                w={isLargerThanSm ? 'auto' : '100%'}
                pt={{ base: '5px', sm: '0' }}>
                <Button
                    h={{ base: '36px', sm: '36px' }}
                    bg="brand.400"
                    _hover={{ bg: 'brand.500' }}
                    _active={{ bg: 'brand.400' }}
                    color="white"
                    mr="10px"
                    alignSelf="flex-end"
                    onClick={() => {
                        setIsSearching(true);
                    }}>
                    Search
                </Button>

                <Button
                    h={{ base: '36px', sm: '36px' }}
                    bg="gray.500"
                    _hover={{ bg: 'gray.600' }}
                    _active={{ bg: 'gray.500' }}
                    color="white"
                    onClick={setWorkPeriod}>
                    Current
                </Button>
            </Flex>
        </HStack>
    );
};

export default DashboardFilter;
