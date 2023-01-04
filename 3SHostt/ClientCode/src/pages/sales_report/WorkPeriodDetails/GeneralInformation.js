import { Table, Tbody, Td, Th, Thead, Tr } from '@chakra-ui/react';
import React from 'react';
import Loading from '../../shared/Loading';
import { commaFormat } from '../../../services/_utils/index';

const GeneralInformation = ({ data, loading }) => {
    if (loading) {
        return <Loading line={6} />;
    }

    return (
        <Table variant="simple" w="100%" size="sm" className="tableBorder">
            <Thead>
                <Tr bg="primary.900">
                    <Th p="2" colSpan="2" textAlign="center" color="white">
                        General Information
                    </Th>
                </Tr>
            </Thead>
            <Tbody>
                {data.map((item, i) => (
                    <Tr key={i}>
                        <Td px="2">{item.Info}</Td>
                        <Td px="2">{commaFormat(item.Value)}</Td>
                    </Tr>
                ))}
            </Tbody>
        </Table>
    );
};

export default GeneralInformation;
