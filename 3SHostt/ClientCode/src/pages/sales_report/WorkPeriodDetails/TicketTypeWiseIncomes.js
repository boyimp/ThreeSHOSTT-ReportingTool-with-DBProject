import { Table, Tbody, Td, Th, Thead, Tr, useColorModeValue } from '@chakra-ui/react';
import React from 'react';
import Loading from '../../shared/Loading';
import { commaFormat } from '../../../services/_utils/index';

const TicketTypeWiseIncomes = ({ data, loading }) => {
    // color
    const tableCaptionBg = useColorModeValue('whitesmoke', 'whiteAlpha.50');

    if (loading) {
        return <Loading line={6} />;
    }

    return (
        <Table variant="simple" w="100%" size="sm" className="tableBorder">
            <Thead>
                <Tr bg={tableCaptionBg}>
                    <Th p="2" colSpan="4" textAlign="center">
                        Ticket Type wise Incomes
                    </Th>
                </Tr>
                <Tr bg="primary.900">
                    <Th p="2" color="white">
                        Type
                    </Th>
                    <Th p="2" color="white">
                        Name
                    </Th>
                    <Th p="2" color="white">
                        Percentage
                    </Th>
                    <Th p="2" color="white">
                        Amount
                    </Th>
                </Tr>
            </Thead>
            <Tbody>
                {data.map((item, i) => (
                    <Tr key={i}>
                        <Td px="2">{item.Info}</Td>
                        <Td px="2">{item.Value}</Td>
                        <Td px="2">{item.Value1}</Td>
                        <Td px="2">{commaFormat(item.Value2)}</Td>
                    </Tr>
                ))}
            </Tbody>
        </Table>
    );
};

export default TicketTypeWiseIncomes;
