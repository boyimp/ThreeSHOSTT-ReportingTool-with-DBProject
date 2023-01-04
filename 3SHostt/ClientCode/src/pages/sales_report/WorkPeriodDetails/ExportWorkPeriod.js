import { Flex } from '@chakra-ui/react';
import React from 'react';
import { exportTable } from '../../shared/dataExport/ExportStyle';

const ExportWorkPeriod = () => {
    const TdLine = () => {
        return (
            <tr style={exportTable.tr}>
                <td style={exportTable.td}>Pathao</td>
                <td style={exportTable.td}>13519.05</td>
                <td style={exportTable.td}>%6.72</td>
            </tr>
        );
    };

    return (
        <>
            <Flex height="5px" width="100%"></Flex>

            <tr style={exportTable.tr}>
                <th style={exportTable.th} colSpan="3">
                    Sales Information
                </th>
            </tr>
            <tr style={exportTable.tr}>
                <th style={exportTable.th}>Sales</th>
                <th style={exportTable.th}>Amount</th>
                <th style={exportTable.th}>Perc</th>
            </tr>
            <TdLine />
            <TdLine />
            <tr style={exportTable.tr}>
                <td style={exportTable.td}>Grand Total</td>
                <td style={exportTable.td}>210731.00</td>
                <td style={exportTable.td}></td>
            </tr>

            <Flex height="20px" width="100%"></Flex>

            <tr style={exportTable.tr}>
                <th style={exportTable.th} colSpan="3">
                    Sales Information
                </th>
            </tr>
            <tr style={exportTable.tr}>
                <th style={exportTable.th}>Sales</th>
                <th style={exportTable.th}>Amount</th>
                <th style={exportTable.th}>Perc</th>
            </tr>
            <TdLine />
            <TdLine />
            <tr style={exportTable.tr}>
                <td style={exportTable.td}>Grand Total</td>
                <td style={exportTable.td}>210731.00</td>
                <td style={exportTable.td}></td>
            </tr>

            <Flex height="15px" width="100%"></Flex>
        </>
    );
};

export default ExportWorkPeriod;
