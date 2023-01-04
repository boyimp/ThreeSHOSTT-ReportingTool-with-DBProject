//@ts-check
import { Box, useColorModeValue, useMultiStyleConfig } from '@chakra-ui/react';
import { setHours, setMinutes } from 'date-fns';
import React, { useState } from 'react';
import DatePicker from 'react-datepicker';

import 'react-datepicker/dist/react-datepicker.css';

const CustomDatePicker = ({ value, onChange, isDateOnly = false, isDisabled = false, isInvalid = false, dateFormat = 'MM/dd/yy' }) => {
    const [startDate, setStartDate] = useState(setHours(setMinutes(new Date(), 0), 9));
    const bgMenuColor = useColorModeValue('brandGray.50', 'brandGray.700');
    const textBottomMenu = useColorModeValue('brandGray.700', 'brandGray.50');
    const selectedBg = useColorModeValue(`blue.50`, 'blue.900');
    const selectFontColor = useColorModeValue(`brandGray.800`, 'brandGray.50');
    const inputBaseStyle = useMultiStyleConfig('Input', { variant: 'outline' });

    const isValidDate = (dateObject) =>
        dateObject && new Date(dateObject).toString() !== 'Invalid Date';

    return (
        <Box
            sx={{
                '.react-datepicker': {
                    bg: bgMenuColor,
                    color: textBottomMenu,
                    fontFamily: 'inherit'
                },

                '.react-datepicker__header,.react-datepicker__current-month,.react-datepicker__day-name,.react-datepicker__time,.react-datepicker__day,.react-datepicker-time__header':
                    {
                        bg: bgMenuColor,
                        color: textBottomMenu
                    },
                '.react-datepicker__time-container .react-datepicker__time .react-datepicker__time-box ul.react-datepicker__time-list li.react-datepicker__time-list-item--selected':
                    {
                        bg: selectedBg,
                        color: selectFontColor
                    },
                '.react-datepicker__time-container .react-datepicker__time .react-datepicker__time-box ul.react-datepicker__time-list li.react-datepicker__time-list-item:hover':
                    {
                        bg: 'brandGray.400'
                    },
                '.react-datepicker__day--selected': {
                    bg: selectedBg,
                    color: selectFontColor
                },
                '.react-datepicker__input-container>input': {
                    ...inputBaseStyle.field
                },
                '.react-datepicker__input-container>input:hover': {
                    ...inputBaseStyle.field._hover
                },
                '.react-datepicker__input-container>input:focus': {
                    ...inputBaseStyle.field._focus,
                    background: 'transparent !important'
                },
                '.react-datepicker__input-container>input[disabled]': {
                    ...inputBaseStyle.field._disabled
                },
                '.react-datepicker__input-container>input[readonly]': {
                    ...inputBaseStyle.field._readonly
                }
            }}>
            <DatePicker
                selected={value}
                onChange={(date) => {
                    var sanetizedDate = isValidDate(date) ? new Date(date.toString()) : null;
                    sanetizedDate = isDateOnly && sanetizedDate
                        ? new Date(sanetizedDate.toDateString())
                        : sanetizedDate;
                    onChange(sanetizedDate);
                }}
                showTimeSelect={!isDateOnly}
                dateFormat={isDateOnly ? dateFormat : `${dateFormat} h:mm aa`}
                timeIntervals={5}
                showPopperArrow={false}
                disabled={isDisabled}
                ariaInvalid={isInvalid}
                // customInput={<ExampleCustomInput />}
            />
        </Box>
    );
};

export default CustomDatePicker;
