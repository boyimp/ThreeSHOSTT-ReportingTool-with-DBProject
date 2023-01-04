import { Table, Tbody, Td, Th, Thead, Tr, useColorModeValue } from '@chakra-ui/react';
import React from 'react';
import Loading from '../../shared/Loading';
import { commaFormat } from '../../../services/_utils/index';

const IncomeInformation = ({ data, loading }) => {
    // color
    const tableCaptionBg = useColorModeValue('whitesmoke', 'whiteAlpha.50');

    if (loading) {
        return <Loading line={6} />;
    }

    return (
        <Table variant="simple" w="100%" size="sm" className="tableBorder">
            <Thead>
                <Tr bg={tableCaptionBg}>
                    <Th p="2" colSpan="3" textAlign="center">
                        Income Information
                    </Th>
                </Tr>
                <Tr bg="primary.900">
                    <Th p="2" color="white">
                        Income Header
                    </Th>
                    <Th p="2" color="white">
                        %
                    </Th>
                    <Th p="2" color="white">
                        Amount
                    </Th>
                </Tr>
            </Thead>
            <Tbody>
                {data.map((item, i) => (
                    <Tr key={i}>
                        <Td px="2">{item.Incomes}</Td>
                        <Td px="2">{item.Perc}</Td>
                        <Td px="2">{commaFormat(item.Amount)}</Td>
                    </Tr>
                ))}
            </Tbody>
        </Table>
    );
};

export default IncomeInformation;
