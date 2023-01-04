import { Text } from '@chakra-ui/react';
import React from 'react';
import { getFormattedDateTime } from '../../services/_utils/formatTime';

const ReportConsideredTime = ({ fromDate, toDate }) => {
    return (
        <Text fontSize="14px" mb="5">
            Work period considered from{' '}
            <strong>{getFormattedDateTime(fromDate || new Date())}</strong> to{' '}
            <strong>{getFormattedDateTime(toDate || new Date())}</strong>
        </Text>
    );
};

export default ReportConsideredTime;
