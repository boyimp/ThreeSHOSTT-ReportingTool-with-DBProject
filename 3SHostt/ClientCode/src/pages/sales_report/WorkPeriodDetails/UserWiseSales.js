import { Table, Tbody, Td, Th, Thead, Tr } from '@chakra-ui/react';
import React from 'react';
import Loading from '../../shared/Loading';
import { commaFormat } from '../../../services/_utils/index';

const UserWiseSales = ({ data, loading }) => {
    if (loading) {
        return <Loading line={6} />;
    }

    return (
        <Table variant="simple" w="100%" size="sm" className="tableBorder">
            <Thead>
                <Tr bg="primary.900">
                    <Th p="2" colSpan="3" textAlign="center" color="white">
                        User Wise Sales (Orders)
                    </Th>
                </Tr>
            </Thead>
            <Tbody>
                {data.map((item, i) => (
                    <Tr key={i}>
                        <Td px="2">{item.UserWiseSalesOrders}</Td>
                        <Td px="2">{commaFormat(item.Value)}</Td>
                        <Td px="2">{item.Perc}</Td>
                    </Tr>
                ))}
            </Tbody>
        </Table>
    );
};

export default UserWiseSales;
