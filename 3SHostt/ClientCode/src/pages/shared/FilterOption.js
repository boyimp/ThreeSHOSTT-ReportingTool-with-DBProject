import {
    Button,
    Flex,
    useColorMode,
    useColorModeValue,
    Input,
    Text,
    Checkbox,
    useMediaQuery,
    HStack,
    useToast
} from '@chakra-ui/react';
import React, { useState, useEffect } from 'react';
import { useQuery } from 'react-query';
import Select from 'react-select';
import {
    onOutletNames,
    onDepartmentNames,
    onAllCategoriesAndMenuItems
} from '../../services/dashboard-service';

const FilterOption = ({
    setIsSearching,
    isApiCalling,
    isExact,
    setIsExact,
    isTicketOpen,
    setIsTicketOpen,
    fromDate,
    setFromDate,
    toDate,
    setToDate,
    setDepartmentIds,
    setOutletIds,
    setCategoriesIds,
    setMenuItemIds,
    setNumberOfYears,
    haveDataYears,
    refetch,
    isMultiOutlet = true,
    isMultiDept = true,
    isMultiCategory = true,
    isMultiMenuItem = true
}) => {
    // chakra
    const [isLargerThanSm] = useMediaQuery('(min-width: 30em)');
    const toast = useToast();
    // color
    const { colorMode } = useColorMode();
    const bgColor = useColorModeValue('white', 'whiteAlpha.50');
    const textColor = useColorModeValue('gray', 'white');
    const brandBgText = useColorModeValue('primary.900', 'blue.100');
    const selectInputBg = useColorModeValue('white', '#232934');

    // use query data
    const { data: outletsData, isLoading: outletLoading } = useQuery('outlets', () =>
        onOutletNames()
    );
    const { data: departmentsData, isLoading: deptLoading } = useQuery('departments', () =>
        onDepartmentNames()
    );
    const { data: categoryMenuItemsData, isLoading: categoryMenuLoading } = useQuery(
        'categoryAndMenuItems',
        () => onAllCategoriesAndMenuItems()
    );
    const outletList = outletsData?.data || [];
    const departmentsList = departmentsData?.data || [];
    const categoryAndMenuList = categoryMenuItemsData?.data || [];
    // data
    const [selectedOutlets, setSelectedOutlets] = useState([]);
    const [selectedDepartments, setSelectedDepartments] = useState([]);
    const [selectedCategories, setSelectedCategories] = useState([]);
    const [menuItemList, setMenuItemList] = useState([]);
    const [selectedMenuItems, setSelectedMenuItems] = useState([]);
    const [selectedYears, setSelectedYears] = useState([]);

    const searchBtnClicked = () => {
        setIsSearching(true);
        refetch && refetch();
    };

    useEffect(() => {
        if (!isMultiOutlet) {
            setOutletIds(selectedOutlets?.value || 0);
        } else {
            selectedOutlets?.length > 0
                ? setOutletIds(selectedOutlets?.map((item) => item.value))
                : setOutletIds && setOutletIds([0]);
        }
        if (!isMultiDept) {
            setDepartmentIds(selectedDepartments?.value || 0);
        } else {
            selectedDepartments?.length > 0
                ? setDepartmentIds(selectedDepartments?.map((item) => item.value))
                : setDepartmentIds && setDepartmentIds([0]);
        }
    }, [selectedOutlets, selectedDepartments]);

    useEffect(() => {
        if (!isMultiCategory) {
            setCategoriesIds(selectedCategories?.label || '');

            // set menu items
            setMenuItemList([]);
            selectedCategories?.value?.forEach((item) => {
                setMenuItemList((prev) => [...prev, item]);
            });
        } else {
            if (selectedCategories.length > 0) {
                setCategoriesIds(selectedCategories?.map((item) => item.label));

                // set menu items
                setMenuItemList([]);
                selectedCategories.forEach((categories) => {
                    categories.value.forEach((item) => {
                        setMenuItemList((prev) => [...prev, item]);
                    });
                });
            } else {
                setCategoriesIds && setCategoriesIds(['']);
                // set menu items
                setMenuItemList([]);
            }
        }
    }, [selectedCategories]);

    useEffect(() => {
        if (!isMultiMenuItem) {
            setMenuItemIds(selectedMenuItems?.value || 0);
        } else {
            selectedMenuItems.length > 0
                ? setMenuItemIds(selectedMenuItems?.map((item) => item.value))
                : setMenuItemIds && setMenuItemIds([0]);
        }
    }, [selectedMenuItems]);

    useEffect(() => {
        setNumberOfYears && setNumberOfYears(selectedYears?.value || 2);
    }, [selectedYears]);

    // select options
    const departmentOptions = departmentsList?.map((item) => ({
        value: item.id,
        label: item.Name
    }));
    const outletOptions = outletList?.map((item) => ({
        value: item.id,
        label: item.Name
    }));
    const categoryOptions = categoryAndMenuList?.map((item) => ({
        value: item.Items,
        label: item.GroupCode
    }));
    const menuItemOptions = menuItemList?.map((item) => ({
        value: item.Id,
        label: item.Name
    }));
    const yearsOptions = haveDataYears?.map((item) => ({
        value: item.year,
        label: item.label
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
            border: colorMode === 'light' ? '1px solid lightgray' : '1px solid gray',
            color: textColor
        }),
        valueContainer: (provided, state) => ({
            ...provided,
            padding: '0 6px',
            color: textColor
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
            height: '34px',
            color: textColor
        }),
        singleValue: (provided, state) => ({
            ...provided,
            color: textColor
        })
    };

    return (
        <>
            <HStack gap="10px" pb="5" flexWrap="wrap" align="flex-end">
                {setNumberOfYears && (
                    <Flex
                        direction="column"
                        w={isLargerThanSm ? '215px' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Text color={textColor} fontSize="15px">
                            Select Years
                        </Text>

                        <Select
                            styles={customStyles}
                            value={selectedYears}
                            onChange={setSelectedYears}
                            options={yearsOptions}
                            className="reactSelectFilterInput"
                            placeholder="Last 2 Years"
                            isSearchable={true}
                        />
                    </Flex>
                )}

                {setFromDate && (
                    <Flex
                        direction="column"
                        w={isLargerThanSm ? '215px' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Text color={textColor} fontSize="15px">
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
                            border={
                                colorMode === 'light' ? '1px solid lightgray' : '1px solid gray'
                            }
                        />
                    </Flex>
                )}

                {setToDate && (
                    <Flex
                        direction="column"
                        w={isLargerThanSm ? '215px' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Text color={textColor} fontSize="15px">
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
                            border={
                                colorMode === 'light' ? '1px solid lightgray' : '1px solid gray'
                            }
                        />
                    </Flex>
                )}

                {setOutletIds && (
                    <Flex
                        direction="column"
                        align="flex-start"
                        w={isLargerThanSm ? '215px' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Text color={textColor} fontSize="15px">
                            Select Outlets
                        </Text>
                        <Select
                            styles={customStyles}
                            closeMenuOnSelect={isMultiOutlet ? false : true}
                            isMulti={isMultiOutlet}
                            isClearable={true}
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
                )}

                {setDepartmentIds && (
                    <Flex
                        direction="column"
                        align="flex-start"
                        w={isLargerThanSm ? '215px' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Text color={textColor} fontSize="15px">
                            Select Department
                        </Text>
                        <Select
                            styles={customStyles}
                            closeMenuOnSelect={isMultiDept ? false : true}
                            isMulti={isMultiDept}
                            isClearable={true}
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
                )}

                {setCategoriesIds && (
                    <Flex
                        direction="column"
                        align="flex-start"
                        w={isLargerThanSm ? '215px' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Text color={textColor} fontSize="15px">
                            Select Categories
                        </Text>
                        <Select
                            styles={customStyles}
                            closeMenuOnSelect={isMultiCategory ? false : true}
                            isMulti={isMultiCategory}
                            isClearable={true}
                            value={selectedCategories}
                            onChange={setSelectedCategories}
                            options={categoryAndMenuList.length > 0 ? categoryOptions : null}
                            className="reactSelectFilterInput"
                            placeholder={
                                !categoryMenuLoading && categoryAndMenuList.length > 0
                                    ? 'All Categories'
                                    : !categoryMenuLoading && categoryAndMenuList.length === 0
                                    ? 'All'
                                    : 'Loading..'
                            }
                        />
                    </Flex>
                )}

                {setMenuItemIds && (
                    <Flex
                        direction="column"
                        align="flex-start"
                        w={isLargerThanSm ? '215px' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Text color={textColor} fontSize="15px">
                            Select Menu Items
                        </Text>
                        <Select
                            onFocus={() => {
                                menuItemList.length === 0 &&
                                    toast({
                                        title: 'Select categories first',
                                        status: 'warning',
                                        duration: 3000,
                                        isClosable: true
                                    });
                            }}
                            styles={customStyles}
                            closeMenuOnSelect={isMultiMenuItem ? false : true}
                            isMulti={isMultiMenuItem}
                            isClearable={true}
                            value={selectedMenuItems}
                            onChange={setSelectedMenuItems}
                            options={menuItemList.length > 0 ? menuItemOptions : []}
                            className="reactSelectFilterInput"
                            placeholder={
                                !categoryMenuLoading && menuItemList.length > 0
                                    ? 'Menu Items'
                                    : !categoryMenuLoading && menuItemList.length === 0
                                    ? 'Menu Items'
                                    : 'Loading..'
                            }
                        />
                    </Flex>
                )}

                {setIsExact && (
                    <Flex
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
                    </Flex>
                )}

                {setIsTicketOpen && (
                    <Flex
                        direction="column"
                        w={isLargerThanSm ? 'auto' : '100%'}
                        sx={{ margin: '0px!important' }}>
                        <Checkbox
                            size="md"
                            isChecked={isTicketOpen}
                            onChange={(e) => setIsTicketOpen(e.target.checked)}
                            color="gray.300"
                            borderColor="gray.300">
                            Only Open Tickets
                        </Checkbox>
                    </Flex>
                )}

                <Flex w={isLargerThanSm ? 'auto' : '100%'} sx={{ margin: '0px!important' }}>
                    <Button
                        onClick={searchBtnClicked}
                        bg="brand.400"
                        _hover={{ bg: 'brand.500' }}
                        _active={{ bg: 'brand.400' }}
                        color="white"
                        disabled={isApiCalling}
                        h={{ base: '36px', sm: '36px' }}>
                        {isApiCalling ? 'Searching' : 'Search'}
                    </Button>
                </Flex>
            </HStack>
        </>
    );
};

export default FilterOption;
