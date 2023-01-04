import { Table, Tbody, Td, Th, Thead, Tr } from '@chakra-ui/react';
import React from 'react';
import Loading from '../../shared/Loading';
import { commaFormat } from '../../../services/_utils/index';

const ItemSales = ({ data, loading }) => {
    if (loading) {
        return <Loading line={6} />;
    }

    return (
        <Table variant="simple" w="100%" size="sm" className="tableBorder">
            <Thead>
                <Tr bg="primary.900">
                    <Th p="2" colSpan="3" textAlign="center" color="white">
                        Item Sales
                    </Th>
                </Tr>
            </Thead>
            <Tbody>
                {data.map((item, i) => (
                    <Tr key={i}>
                        <Td px="2">{item.ItemSales}</Td>
                        <Td px="2">{item.Perc}</Td>
                        <Td px="2">{commaFormat(item.Amount)}</Td>
                    </Tr>
                ))}
            </Tbody>
        </Table>
    );
};

export default ItemSales;
